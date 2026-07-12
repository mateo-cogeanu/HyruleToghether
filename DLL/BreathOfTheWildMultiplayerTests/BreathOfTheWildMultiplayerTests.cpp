#include "pch.h"
#include "CppUnitTest.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace BreathOfTheWildMultiplayerTests
{
	TEST_CLASS(BreathOfTheWildMultiplayerTests)
	{
	public:
		
		TEST_METHOD(TestMethod1)
		{

			std::string ExpectedValue = "5.5555";

			std::string ActualValue = to_string_precision(5.5555555555, 4);
			Assert::AreEqual(ExpectedValue, ActualValue);

		}

		std::string to_string_precision(float number, int precision)
		{
            std::string num = std::to_string(number);
            std::string res = "";

            bool decimal = false;
            int numberOfDecimals = 0;

            for (int i = 0; i < num.size(); i++)
            {
                if (!decimal)
                {

                    if (num[i] != '.')
                    {
                        res += num[i];
                    }
                    else
                    {
                        res += num[i];
                        decimal = true;
                    }

                }
                else
                {

                    res += num[i];
                    numberOfDecimals += 1;

                    if (numberOfDecimals == precision)
                    {

                        break;

                    }

                }
            }

            return res;
		}
	};
}
