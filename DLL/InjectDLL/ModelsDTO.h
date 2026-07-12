#pragma once
#include <map>
#include <string>
#include "Platform.h"
#include "ModelData.h"

using namespace DataTypes;

namespace DTO
{
	class ModelsDTO
	{
	public:
		std::map<byte, ModelData> Models;
	};
}
