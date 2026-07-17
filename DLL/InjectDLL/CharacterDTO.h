#pragma once

#include <vector>
#include <string>
#include "Vec3f.h"
#include "Quaternion.h"
#include "CharacterLocation.h"
#include "CharacterEquipment.h"
#include "ProjectileDTO.h"

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
		// Compact mode derived from Link's verified controller profile string.
		// Zero remains sheathed and every active mode remains nonzero for legacy
		// one-byte boolean compatibility.
		byte EquipmentState;
		CharacterEquipment Equipment;
		CharacterLocation Location;
		Vec3f Bomb;
		Vec3f Bomb2;
		Vec3f BombCube;
		Vec3f BombCube2;
		ProjectileDTO Arrow;
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
		ProjectileDTO Arrow;
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
