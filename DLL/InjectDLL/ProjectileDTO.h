#pragma once

#include "Platform.h"
#include "Vec3f.h"
#include "Quaternion.h"

namespace DataTypes
{
	class ProjectileDTO
	{
	public:
		int Id = 0;
		byte Type = 0;
		bool Active = false;
		Vec3f Position;
		Quaternion Rotation;
	};
}
