#pragma once

#include <atomic>
#include <mutex>
#include <string>

#include "ProjectileDTO.h"
#include "Vec3fBE.h"
#include "QuaternionBE.h"

namespace DataTypes
{
	class ProjectileAccess
	{
	public:
		enum class State : byte { Inactive, Processing, Active, Cancelled };

		ProjectileDTO Get(const char* caller)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			ProjectileDTO result;
			result.Id = Id;
			result.Type = Type;
			result.Active = CurrentState == State::Active && BaseAddr != 0;
			if (result.Active)
			{
				result.Position = Position.get(caller);
				result.Rotation = Rotation.get(caller);
			}
			return result;
		}

		bool BeginLocal(uint64_t actorAddress, byte type, const std::string& actorName,
			const char* caller)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			if (CurrentState == State::Active && BaseAddr == actorAddress)
				return false;
			// Track the newest shot. Older local arrows remain normal BOTW actors;
			// their remote replicas likewise continue under BOTW physics after the
			// next generation takes over transform streaming.
			SetAddressLocked(actorAddress, caller);
			if (BaseAddr == 0)
				return false;
			Id = NextLocalId.fetch_add(1, std::memory_order_relaxed);
			if (Id <= 0)
			{
				NextLocalId.store(2, std::memory_order_relaxed);
				Id = 1;
			}
			Type = type;
			ExpectedName = actorName;
			CurrentState = State::Active;
			return true;
		}

		void EndLocal(uint64_t actorAddress)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			if (BaseAddr != actorAddress)
				return;
			SetAddressLocked(0, __FUNCTION__);
			CurrentState = State::Inactive;
		}

		void EndRemote(uint64_t actorAddress)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			if (BaseAddr != actorAddress)
				return;
			RetiredRemoteId = Id;
			SetAddressLocked(0, __FUNCTION__);
			CurrentState = State::Inactive;
		}

		bool PrepareRemote(const ProjectileDTO& projectile, const std::string& actorName)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			if (projectile.Id <= 0 || !projectile.Active)
				return false;
			if (projectile.Id == RetiredRemoteId)
				return false;
			if (Id == projectile.Id &&
				(CurrentState == State::Processing || CurrentState == State::Active))
				return false;
			Id = projectile.Id;
			Type = projectile.Type;
			ExpectedName = actorName;
			PendingPosition = projectile.Position;
			PendingRotation = projectile.Rotation;
			SetAddressLocked(0, __FUNCTION__);
			CurrentState = State::Processing;
			return true;
		}

		bool TryAssignRemote(const std::string& actorName, int generation,
			uint64_t actorAddress, const char* caller, bool& stale)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			stale = false;
			if (Id != generation || ExpectedName != actorName ||
				(CurrentState != State::Processing && CurrentState != State::Cancelled))
			{
				stale = true;
				return true;
			}
			if (CurrentState == State::Cancelled)
			{
				stale = true;
				ExpectedName.clear();
				return true;
			}
			SetAddressLocked(actorAddress, caller);
			if (BaseAddr == 0)
			{
				CurrentState = State::Cancelled;
				stale = true;
				return true;
			}
			Position.set(PendingPosition, caller);
			Rotation.set(PendingRotation, caller);
			CurrentState = State::Active;
			ExpectedName.clear();
			return true;
		}

		void UpdateRemote(const ProjectileDTO& projectile, const char* caller)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			if (projectile.Id != Id)
				return;
			if (!projectile.Active)
			{
				if (CurrentState == State::Processing)
					CurrentState = State::Cancelled;
				else if (CurrentState == State::Active)
				{
					Position.set(Vec3f(0.0f, -10000.0f, 0.0f), caller);
					SetAddressLocked(0, caller);
					CurrentState = State::Inactive;
				}
				return;
			}
			PendingPosition = projectile.Position;
			PendingRotation = projectile.Rotation;
			if (CurrentState == State::Active && BaseAddr != 0)
			{
				Position.set(projectile.Position, caller);
				Rotation.set(projectile.Rotation, caller);
			}
		}

		uint64_t Address() const
		{
			std::lock_guard<std::mutex> lock(Mutex);
			return BaseAddr;
		}

		int Generation() const
		{
			std::lock_guard<std::mutex> lock(Mutex);
			return Id;
		}

		void Reset(const char* caller, bool hideActor)
		{
			std::lock_guard<std::mutex> lock(Mutex);
			if (hideActor && CurrentState == State::Active && BaseAddr != 0)
				Position.set(Vec3f(0.0f, -10000.0f, 0.0f), caller);
			SetAddressLocked(0, caller);
			Id = 0;
			RetiredRemoteId = 0;
			Type = 0;
			ExpectedName.clear();
			CurrentState = State::Inactive;
		}

	private:
		void SetAddressLocked(uint64_t addr, const char* caller)
		{
			BaseAddr = 0;
			if (addr == 0)
			{
				Position.setAddress(0, caller);
				Rotation.setAddress(0, caller);
				return;
			}
			const uint64_t positionAddress =
				Memory::ReadPointers(addr, {0x3A0, 0x50, 0x4, 0x80, 0x0, 0x5C, 0x18}, true) + 0x50;
			if (positionAddress < 30000)
				return;
			Position.setAddress(positionAddress, caller, true);
			Rotation.setAddress(positionAddress - 0x30, caller, false);
			BaseAddr = addr;
		}

		mutable std::mutex Mutex;
		Vec3fBE Position{0, "ProjectilePosition"};
		QuaternionBE Rotation{0, "ProjectileRotation"};
		Vec3f PendingPosition;
		Quaternion PendingRotation;
		uint64_t BaseAddr = 0;
		int Id = 0;
		int RetiredRemoteId = 0;
		byte Type = 0;
		State CurrentState = State::Inactive;
		std::string ExpectedName;
		inline static std::atomic<int> NextLocalId{1};
	};
}
