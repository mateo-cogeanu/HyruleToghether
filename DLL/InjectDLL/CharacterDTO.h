#pragma once

#include <vector>
#include <string>
#include "Vec3f.h"
#include "Quaternion.h"
#include "CharacterLocation.h"
#include "CharacterEquipment.h"

using namespace DataTypes;

namespace DTO
{

	class ClientCharacterDTO
	{
	public:
		Vec3f Position;
		Quaternion Rotation1;
		Quaternion Rotation2;
		Quaternion Rotation3;
		Quaternion Rotation4;
		int Animation;
		int Health;
		float AtkUp;
		// Full raw byte from Link's verified equipment-controller field. Zero is
		// sheathed and nonzero is held; retaining the remaining bits lets matching
		// builds diagnose shield/bow submodes without changing the wire size.
		byte EquipmentState;
		CharacterEquipment Equipment;
		CharacterLocation Location;
		Vec3f Bomb;
		Vec3f Bomb2;
		Vec3f BombCube;
		Vec3f BombCube2;
	};

	class CloseCharacterDTO
	{
	public:
		byte PlayerNumber;
		byte Status;
		bool Updated;
		Vec3f Position;
		Quaternion Rotation1;
		Quaternion Rotation2;
		Quaternion Rotation3;
		Quaternion Rotation4;
		int Animation;
		int Health;
		float AtkUp;
		byte EquipmentState;
		CharacterEquipment Equipment;
		CharacterLocation Location;
		Vec3f Bomb;
		Vec3f Bomb2;
		Vec3f BombCube;
		Vec3f BombCube2;
	};

	class FarCharacterDTO
	{
	public:
		byte PlayerNumber;
		byte Status;
		bool Updated;
		Vec3f Position;
		CharacterLocation Location;
	};

}
