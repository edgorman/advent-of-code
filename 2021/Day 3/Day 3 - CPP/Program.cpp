#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <bitset>

#include "Conversion.h"

std::string PartOne(std::vector<std::vector<int>> input)
{
    std::string gammaString;
    std::string epsilonString;

    for (int x = 0; x < input[0].size(); x++)
    {
        int count = 0;

        for (int y = 0; y < input.size(); y++)
        {
            if (input[y][x] == 1)
            {
                count++;
            }
        }

        if (count >= ceil(input.size() / 2))
        {
            gammaString += "1";
            epsilonString += "0";
        }
        else
        {
            gammaString += "0";
            epsilonString += "1";
        }
    }

    std::uint32_t gammaValue = BitStringToInt(gammaString);
    std::uint32_t epsiolonValue = BitStringToInt(epsilonString);
    std::uint32_t powerConsumption = gammaValue * epsiolonValue;

    return std::to_string(powerConsumption);
}

std::string PartTwo(std::vector<std::vector<int>> input)
{
    return "";
}

int main()
{
    // Get input from txt file
    std::string line;
    std::vector<std::string> lines;
    std::ifstream file("test.txt");
    while (std::getline(file, line))
    {
        lines.push_back(line);
    }

    // Clean input
    std::vector<std::vector<int>> input;
    for (int i = 0; i < lines.size(); i++)
    {
        std::vector<int> numbers;
        for (auto c : lines[i])
        {
            numbers.push_back(c - '0');
        }
        input.push_back(numbers);
    }

    // Part 1
    std::cout << PartOne(input) << "\n";

    // Part 2
    std::cout << PartTwo(input) << "\n";
}
