#pragma once
#include "CharacterEquipment.h"
#include "Memory.h"

namespace DataTypes
{
	class EquipmentAccess
	{
	public:
		bool Changed = false;
		bool SetupFailed = false;
		bool NeedsActorRefresh = false;
		CharacterEquipment* LastKnown = new CharacterEquipment{};
		std::string lastBase = "Jugador1ModelNameLongForASpecificReasonHead";
		int PlayerNumber = 0;

		void SetPlayerNumber(int playerNumber)
		{
			PlayerNumber = playerNumber;
		}

		bool Compare(CharacterEquipment newEquipment)
		{
			std::unique_lock<std::shared_mutex> lock(Mutex);
			bool changed = false;

			if (LastKnown->WType != newEquipment.WType)
			{
				changed = true;
				LastKnown->WType = newEquipment.WType;
			}
			if (LastKnown->Sword != newEquipment.Sword)
			{
				changed = true;
				LastKnown->Sword = newEquipment.Sword;
			}
			if (LastKnown->Shield != newEquipment.Shield)
			{
				changed = true;
				LastKnown->Shield = newEquipment.Shield;
			}
			if (LastKnown->Bow != newEquipment.Bow)
			{
				changed = true;
				LastKnown->Bow = newEquipment.Bow;
			}
			if (LastKnown->Head != newEquipment.Head)
			{
				changed = true;
				LastKnown->Head = newEquipment.Head;
			}
			if (LastKnown->Upper != newEquipment.Upper)
			{
				changed = true;
				LastKnown->Upper = newEquipment.Upper;
			}
			if (LastKnown->Lower != newEquipment.Lower)
			{
				changed = true;
				LastKnown->Lower = newEquipment.Lower;
			}

			Changed = changed;
			if (changed)
				NeedsActorRefresh = true;
			return changed;
		}

		bool ConsumeActorRefresh()
		{
			std::unique_lock<std::shared_mutex> lock(Mutex);
			if (!NeedsActorRefresh)
				return false;
			NeedsActorRefresh = false;
			Changed = false;
			return true;
		}

		void SetWeapons(uint64_t baseAddr)
		{
			const std::string playerPrefix = "Jugador" + std::to_string(PlayerNumber);
			const std::string RIGHT_DEFAULT = playerPrefix + "RightHandWeaponLongName";
			const std::string LEFT_DEFAULT = playerPrefix + "LeftHandWeaponLongName";
			const std::string BOW_DEFAULT = playerPrefix + "BowWeaponLongName";
			std::unique_lock<std::shared_mutex> lock(Mutex);

			FindWeaponAddr(baseAddr);
			RemoveActorLocalTemplateAddresses();
			FindWeaponTemplateAddrs(RIGHT_DEFAULT, LEFT_DEFAULT, BOW_DEFAULT);

			uint64_t OldAddress = ArmorAddrs.Face;

			FindArmorAddr(baseAddr);

			std::string RightHandWeapon;
			std::string LeftHandWeapon;
			std::string BowWeapon;

			if (LastKnown->Sword != 0)
			{
				switch (LastKnown->WType)
				{
				case 1:
					RightHandWeapon = "Weapon_Sword_" + NumToStr(LastKnown->Sword);
					break;
				case 2:
					RightHandWeapon = "Weapon_Lsword_" + NumToStr(LastKnown->Sword);
					break;
				case 3:
					RightHandWeapon = "Weapon_Spear_" + NumToStr(LastKnown->Sword);
					break;
				}
			}
			if (LastKnown->Shield != 0)
				LeftHandWeapon = "Weapon_Shield_" + NumToStr(LastKnown->Shield);
			if (LastKnown->Bow != 0)
				BowWeapon = "Weapon_Bow_" + NumToStr(LastKnown->Bow);

			WriteWeaponResource(
				WeaponTemplateAddrs.Right, WeaponAddrs.Right,
				RightHandWeapon, RIGHT_DEFAULT.size() + 1);
			WriteWeaponResource(
				WeaponTemplateAddrs.Left, WeaponAddrs.Left,
				LeftHandWeapon, LEFT_DEFAULT.size() + 1);
			WriteWeaponResource(
				WeaponTemplateAddrs.Bow, WeaponAddrs.Bow,
				BowWeapon, BOW_DEFAULT.size() + 1);
			Logging::LoggerService::LogInformation(
				"Equipment setup: weapon=" + (RightHandWeapon.empty() ? "none" : RightHandWeapon) +
				", shield=" + (LeftHandWeapon.empty() ? "none" : LeftHandWeapon) +
				", bow=" + (BowWeapon.empty() ? "none" : BowWeapon) + ".",
				__FUNCTION__);

			if (OldAddress == ArmorAddrs.Face)
				Changed = false;
		}

		void SetArmor()
		{
			std::string BASE_FOLDER = "Jugador1ModelNameLongForASpecificReason";
			std::string BASE_DEFAULT = "Jugador1ModelNameLongForASpecificReasonHead";
			std::string CHEST_DEFAULT = "Jugador1ModelNameLongForASpecificReasonChest";
			std::string UPPER_DEFAULT = "Jugador1ModelNameLongForASpecificReasonHelmet";
			std::string LOWER_DEFAULT = "Jugador1ModelNameLongForASpecificReasonLower";
			std::string HEAD_DEFAULT = "Jugador1ModelNameLongForASpecificReasonUpper";

			std::string UPPER_DEFAULT_ARMOR = "MP_Armor_Default_Upper";
			std::string LOWER_DEFAULT_ARMOR = "MP_Armor_Default_Lower";
			std::string HEAD_DEFAULT_ARMOR = "MP_Armor_Default_Head";

			std::string BaseToWrite = BASE_DEFAULT;
			std::string UpperToWrite = LastKnown->Upper == 0 ? UPPER_DEFAULT_ARMOR : "MP_Armor_" + NumToStr(LastKnown->Upper) + "_Upper";
			std::string LowerToWrite = LastKnown->Lower == 0 ? LOWER_DEFAULT_ARMOR : "MP_Armor_" + NumToStr(LastKnown->Lower) + "_Lower";
			std::string HeadToWrite = LastKnown->Head == 0 ? HEAD_DEFAULT_ARMOR : "MP_Armor_" + NumToStr(LastKnown->Head) + "_Head";
			std::string ChestToWrite = CHEST_DEFAULT;

			if (UpperToWrite != "" && UpperToWrite != UPPER_DEFAULT_ARMOR)
				ChestToWrite = "EmptyModel";

			if (HeadToWrite == "MP_Armor_180_Head" || HeadToWrite == "MP_Armor_160_Head" || HeadToWrite == "MP_Armor_171_Head")
				BaseToWrite = "EmptyModel";

			if(ArmorAddrs.Face != 0)
			{
				try {
					std::string HeadStr = Memory::read_string(ArmorAddrs.Face, BASE_DEFAULT.size() + 2, __FUNCTION__);
					if (HeadStr != lastBase && HeadStr != "EmptyModel")
					{
						ArmorAddrs.Face = 0;
						Logging::LoggerService::LogInformation("Failed to set up armor. Trying again...", __FUNCTION__);
						this->SetupFailed = true;
						return;
					}

					Memory::write_string(ArmorAddrs.Folder, BASE_FOLDER, BASE_FOLDER.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Face, BaseToWrite, BASE_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Chest, ChestToWrite, CHEST_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Head, HeadToWrite, HEAD_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Lower, LowerToWrite, LOWER_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Upper, UpperToWrite, UPPER_DEFAULT.size() + 2, __FUNCTION__);

					lastBase = BaseToWrite;

					this->SetupFailed = false;
					this->Changed = false;
					Logging::LoggerService::LogInformation(
						"Armor setup: head=" + HeadToWrite +
						", upper=" + UpperToWrite +
						", lower=" + LowerToWrite + ".",
						__FUNCTION__);
				}
				catch (...)
				{
					this->SetupFailed = true;
					Logging::LoggerService::LogInformation("Failed to setup armor.", __FUNCTION__);
				}
			}
			else 
			{
				Logging::LoggerService::LogInformation("Couldn't find armor address.", __FUNCTION__);
				this->SetupFailed = true;
			}
		}

		void SetModel(std::string model)
		{
			//model = "Npc_Zora_Prince:Npc_Zora_Prince";
			std::stringstream stream(model);
			std::string segment;
			std::vector<std::string> splitModel;

			while (std::getline(stream, segment, ':'))
			{
				splitModel.push_back(segment);
			}

			std::string BASE_FOLDER = "Jugador1ModelNameLongForASpecificReason";
			std::string BASE_DEFAULT = "Jugador1ModelNameLongForASpecificReasonHead";
			std::string CHEST_DEFAULT = "Jugador1ModelNameLongForASpecificReasonChest";
			std::string UPPER_DEFAULT = "Jugador1ModelNameLongForASpecificReasonHelmet";
			std::string LOWER_DEFAULT = "Jugador1ModelNameLongForASpecificReasonLower";
			std::string HEAD_DEFAULT = "Jugador1ModelNameLongForASpecificReasonUpper";

			if (ArmorAddrs.Face != 0)
			{
				try {
					Memory::write_string(ArmorAddrs.Folder, splitModel[0], BASE_FOLDER.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Face, splitModel[1], BASE_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Chest, splitModel[1], CHEST_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Head, splitModel[1], HEAD_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Lower, splitModel[1], LOWER_DEFAULT.size() + 2, __FUNCTION__);
					Memory::write_string(ArmorAddrs.Upper, splitModel[1], UPPER_DEFAULT.size() + 2, __FUNCTION__);

					lastBase = splitModel[1];

					this->SetupFailed = false;
					this->Changed = false;
					Logging::LoggerService::LogInformation("Model setup.", __FUNCTION__);
				}
				catch (...)
				{
					Logging::LoggerService::LogInformation("Failed to setup model.", __FUNCTION__);
					this->SetupFailed = true;
				}
			}
			else
			{
				Logging::LoggerService::LogInformation("Could not find model address. Trying again...", __FUNCTION__);
				this->SetupFailed = true;
			}
		}

		std::string NumToStr(int number)
		{
			std::string result = std::to_string(number);

			int numberOfZeros = 3 - result.size();

			for (int i = 0; i < numberOfZeros; i++)
				result = "0" + result;

			return result;
		}

		void FindWeaponAddr(uint64_t baseAddr)
		{
			WeaponAddrs = {};
			uint64_t addr = Memory::ReadPointers(baseAddr, { 0x39C, 0x78, 0x244 }, false);

			bool Found = false;
			int iterator = 0;

			while (!Found)
			{
				uint64_t temp = Memory::read_bigEndian4BytesOffset(addr + (iterator * 0x4), __FUNCTION__);

				if (int(3518303375) == Memory::read_bigEndian4BytesOffset(temp + 0x10, __FUNCTION__))
				{
					Found = true;
					addr = Memory::read_bigEndian4BytesOffset(temp + 0x4, __FUNCTION__);
					this->WeaponAddrs.Right = Memory::read_bigEndian4BytesOffset(addr + 0x1C, __FUNCTION__) + Memory::getBaseAddress();
					this->WeaponAddrs.Left = Memory::read_bigEndian4BytesOffset(addr + 0xA0, __FUNCTION__) + Memory::getBaseAddress();
					uint32_t bowAddress = 0;
					if (Memory::TryReadBigEndian4BytesOffset(addr + 0x124, bowAddress) && bowAddress != 0)
						this->WeaponAddrs.Bow = bowAddress + Memory::getBaseAddress();
				}

				iterator++;
			}
		}

		void FindArmorAddr(uint64_t baseAddr)
		{

			if (baseAddr == 0)
			{
				return;
			}

			uint64_t addr = Memory::ReadPointers(baseAddr, { 0x39C, 0x6C });

			this->ArmorAddrs.Folder = Memory::ReadPointers(addr, { 0x48C, 0x1C }, true) + 0xC;

			addr = Memory::ReadPointers(addr, { 0x4B0, 0x38 });
			this->ArmorAddrs.Face = Memory::ReadPointers(addr, { 0x0, 0xC }, true);
			addr = Memory::read_bigEndian4BytesOffset(addr + 0x10, __FUNCTION__);
			this->ArmorAddrs.Chest = Memory::ReadPointers(addr, { 0x0, 0xC }, true);
			addr = Memory::read_bigEndian4BytesOffset(addr + 0x10, __FUNCTION__);
			this->ArmorAddrs.Head = Memory::ReadPointers(addr, { 0x0, 0xC }, true);
			addr = Memory::read_bigEndian4BytesOffset(addr + 0x10, __FUNCTION__);
			this->ArmorAddrs.Lower = Memory::ReadPointers(addr, { 0x0, 0xC }, true);
			addr = Memory::read_bigEndian4BytesOffset(addr + 0x10, __FUNCTION__);
			this->ArmorAddrs.Upper = Memory::ReadPointers(addr, { 0x0, 0xC }, true);

			this->Changed = true;
		}

	private:
		std::shared_mutex Mutex;
		bool WeaponTemplateScanComplete = false;

		bool IsActorLocalWeaponAddress(uint64_t address) const
		{
			return address != 0 &&
				(address == WeaponAddrs.Right || address == WeaponAddrs.Left || address == WeaponAddrs.Bow);
		}

		void RemoveActorLocalTemplateAddresses()
		{
			auto removeActorAddress = [this](std::vector<uint64_t>& addresses) {
				addresses.erase(
					std::remove_if(
						addresses.begin(), addresses.end(),
						[this](uint64_t address) { return IsActorLocalWeaponAddress(address); }),
					addresses.end());
			};
			removeActorAddress(WeaponTemplateAddrs.Right);
			removeActorAddress(WeaponTemplateAddrs.Left);
			removeActorAddress(WeaponTemplateAddrs.Bow);
		}

		bool IsMappedWriteRange(uint64_t address, size_t capacity) const
		{
			if (address == 0 || capacity == 0 ||
				address > std::numeric_limits<uint64_t>::max() - capacity)
				return false;

			const uint64_t memoryBase = Memory::getBaseAddress();
			if (memoryBase == 0 || address < memoryBase ||
				address - memoryBase > std::numeric_limits<uint32_t>::max() - capacity)
				return false;

			MEMORY_BASIC_INFORMATION memoryInfo{};
			const DWORD rejectedProtection = PAGE_GUARD | PAGE_NOCACHE | PAGE_NOACCESS;
			if (VirtualQuery(reinterpret_cast<LPCVOID>(address), &memoryInfo, sizeof(memoryInfo)) == 0 ||
				!(memoryInfo.State & MEM_COMMIT) || (memoryInfo.Protect & rejectedProtection))
				return false;

			const uint64_t regionStart = reinterpret_cast<uint64_t>(memoryInfo.BaseAddress);
			return address >= regionStart && memoryInfo.RegionSize >= capacity &&
				address - regionStart <= memoryInfo.RegionSize - capacity;
		}

		void FindWeaponTemplateAddrs(
			const std::string& rightPlaceholder,
			const std::string& leftPlaceholder,
			const std::string& bowPlaceholder)
		{
			if (WeaponTemplateScanComplete || PlayerNumber <= 0)
				return;

			const std::string prefix = "Jugador" + std::to_string(PlayerNumber);
			std::vector<int> signature(prefix.begin(), prefix.end());
			for (uint64_t address : Memory::PatternScanMultiple(
				signature, Memory::getBaseAddress(), 8, 0, false, 0, 0))
			{
				if (IsActorLocalWeaponAddress(address))
					continue;
				std::string value = Memory::read_string(address, 64, __FUNCTION__);
				if (value == rightPlaceholder &&
					std::find(WeaponTemplateAddrs.Right.begin(), WeaponTemplateAddrs.Right.end(), address) ==
						WeaponTemplateAddrs.Right.end())
					WeaponTemplateAddrs.Right.push_back(address);
				else if (value == leftPlaceholder &&
					std::find(WeaponTemplateAddrs.Left.begin(), WeaponTemplateAddrs.Left.end(), address) ==
						WeaponTemplateAddrs.Left.end())
					WeaponTemplateAddrs.Left.push_back(address);
				else if (value == bowPlaceholder &&
					std::find(WeaponTemplateAddrs.Bow.begin(), WeaponTemplateAddrs.Bow.end(), address) ==
						WeaponTemplateAddrs.Bow.end())
					WeaponTemplateAddrs.Bow.push_back(address);
			}
			WeaponTemplateScanComplete =
				!WeaponTemplateAddrs.Right.empty() &&
				!WeaponTemplateAddrs.Left.empty() &&
				!WeaponTemplateAddrs.Bow.empty();

			Logging::LoggerService::LogInformation(
				"Resolved player " + std::to_string(PlayerNumber) +
				" persistent equipment templates: weapon=" +
				std::to_string(WeaponTemplateAddrs.Right.size()) +
				", shield=" + std::to_string(WeaponTemplateAddrs.Left.size()) +
				", bow=" + std::to_string(WeaponTemplateAddrs.Bow.size()) + ".",
				__FUNCTION__);
		}

		void WriteWeaponResource(
			std::vector<uint64_t>& templateAddresses,
			uint64_t actorAddress,
			const std::string& resource,
			size_t capacity)
		{
			templateAddresses.erase(
				std::remove_if(
					templateAddresses.begin(), templateAddresses.end(),
					[this, &resource, capacity](uint64_t address) {
						if (!IsMappedWriteRange(address, capacity))
							return true;
						Memory::write_string(address, resource, static_cast<int>(capacity), __FUNCTION__);
						return false;
					}),
				templateAddresses.end());
			if (IsMappedWriteRange(actorAddress, capacity) &&
				std::find(templateAddresses.begin(), templateAddresses.end(), actorAddress) == templateAddresses.end())
			{
				Memory::write_string(actorAddress, resource, static_cast<int>(capacity), __FUNCTION__);
			}
			else if (actorAddress != 0 && !IsMappedWriteRange(actorAddress, capacity))
			{
				Logging::LoggerService::LogWarning(
					"Skipped stale actor-local equipment address for player " +
					std::to_string(PlayerNumber) + ".",
					__FUNCTION__);
			}
		}

		struct WeaponAddr 
		{
			uint64_t Left = 0;
			uint64_t Right = 0;
			uint64_t Bow = 0;
		} WeaponAddrs;

		struct WeaponTemplateAddr
		{
			std::vector<uint64_t> Left;
			std::vector<uint64_t> Right;
			std::vector<uint64_t> Bow;
		} WeaponTemplateAddrs;

		struct ArmorAddr
		{
			uint64_t Folder = 0;
			uint64_t Face = 0;
			uint64_t Chest = 0;
			uint64_t Head = 0;
			uint64_t Upper = 0;
			uint64_t Lower = 0;
		} ArmorAddrs;

	};
}
