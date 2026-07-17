#pragma once

#include <string>

#include "Platform.h"

namespace DataTypes
{
	enum EquipmentMode : byte
	{
		EquipmentSheathed = 0,
		EquipmentLegacyHeld = 1,
		EquipmentMelee = 2,
		EquipmentBow = 3,
		EquipmentShield = 4
	};

	inline byte ParseEquipmentMode(const std::string& controllerType)
	{
		if (controllerType.empty())
			return EquipmentSheathed;
		if (controllerType == "WeaponBow")
			return EquipmentBow;
		if (controllerType == "WeaponShield")
			return EquipmentShield;
		if (controllerType == "WeaponSmallSword" ||
			controllerType == "WeaponLargeSword" ||
			controllerType == "WeaponSpear")
			return EquipmentMelee;
		return EquipmentLegacyHeld;
	}

	inline bool IsEquipmentHeld(byte mode)
	{
		return mode != EquipmentSheathed;
	}

	inline const char* EquipmentModeName(byte mode)
	{
		switch (mode)
		{
		case EquipmentSheathed: return "sheathed";
		case EquipmentLegacyHeld: return "legacy-held";
		case EquipmentMelee: return "melee";
		case EquipmentBow: return "bow";
		case EquipmentShield: return "shield";
		default: return "unknown-held";
		}
	}
}
