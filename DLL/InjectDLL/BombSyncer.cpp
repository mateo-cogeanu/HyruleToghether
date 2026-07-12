#include "Memory.h"

using namespace Memory;

void BombSyncer::FindBombs()
{

	std::vector<int> sig = { 0x40, 0x52, 0x65, 0x6D, 0x6F, 0x74, -1, 0x42, 0x6F, 0x6D, 0x62 };
	std::vector<uint64_t> surroundingBombs = PatternScanMultiple(sig, getBaseAddress(), 8, 0x6A000000, false, 0x70000000);
	std::vector<BYTE> BombSignature = { 0x01, 0x00, 0x0A, 0x00, 0x17, 0xFF };
	std::vector<std::string> FoundBombTypes;

	for (uint64_t item : surroundingBombs)
	{

		//if (read_bytes(item + 0x41A, 6) != BombSignature) continue;

		if (!Memory::CompareSignatures(read_bytes(item + 0x41A, 6, __FUNCTION__), BombSignature, { 2 })) continue;

		FoundBombTypes.push_back(read_string(item + 1, 50, __FUNCTION__));

		BombMutex.lock();
		Bombs[read_string(item + 1)] = item + 0x41A - 0x19D;
		BombMutex.unlock();

	}

	for (auto const& pair : Bombs)
	{
		std::string Type = pair.first;
		uint64_t address = pair.second;

		if (std::find(FoundBombTypes.begin(), FoundBombTypes.end(), Type) == FoundBombTypes.end())
		{
			BombMutex.lock();
			Bombs[Type] = 0;
			BombMutex.unlock();
		}
	}

}

std::tuple<std::vector<std::string>, std::vector<std::any>, std::vector<std::string>> BombSyncer::GetBombPositions()
{

	std::tuple<std::vector<std::string>, std::vector<std::any>, std::vector<std::string>> BombPositions;

	std::vector<std::string> BombNames;
	std::vector<std::any> BombValues;
	std::vector<std::string> BombTypes;

	std::vector<std::string> Axis = { "x", "y", "z" };

	for (auto const& pair : Bombs)
	{

		BombNames.push_back(std::regex_replace(std::regex_replace(pair.first, std::regex("RemoteBomb"), "RB"), std::regex("Cube"), "C"));

		//std::vector<std::string> Positions;
		std::map<std::string, std::string> Positions;

		for (int i = 0; i < 3; i++)
		{

			if (pair.second == 0)
			{
				//Positions.push_back(to_string_precision(0, 4));
				Positions[Axis[i]] = "0.0";
			}
			else if (pair.second == -1)
			{
				Positions[Axis[i]] = "-1.0";
			}
			else
			{
				float ReadPosition = Memory::read_bigEndianFloat(pair.second + i * 4, __FUNCTION__);

				if (ReadPosition > 5000 || ReadPosition < -5000)
				{
					Positions[Axis[i]] = "-1.0";
				}
				else
				{
					Positions[Axis[i]] = to_string_precision(ReadPosition, 4);
				}
			}

		}

		BombValues.push_back(Positions);
		BombTypes.push_back("float");

		//BombPositions.push_back(std::make_tuple(BombNames, BombValues, BombTypes));

	}

	return std::make_tuple(BombNames, BombValues, BombTypes);

}

uint64_t BombSyncer::FindBombPosAddr(uint64_t baseAddr)
{

	uint64_t PosAddr = Memory::ReadPointers(baseAddr, { 0x3A0, 0x50 - 0x4, 0x4 + 0x8, 0x80, 0x0, 0x5C, 0x18 }, true);

	if (PosAddr == 0) return 0;

	return PosAddr + 0x50;

}

std::string BombSyncer::to_string_precision(float number, int precision)
{
	std::stringstream stream;
	stream << std::fixed << std::setprecision(precision) << number;
	return stream.str();
}