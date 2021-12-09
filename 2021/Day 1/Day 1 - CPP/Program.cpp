#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <numeric>

std::string PartOne(std::vector<int> input)
{
    uint32_t numberOfIncreases = 0;

    for (int i = 1; i < input.size(); i++)
    {
        if (input[i] > input[i - 1])
        {
            numberOfIncreases++;
        }
    }

    return std::to_string(numberOfIncreases);
}

std::string PartTwo(std::vector<int> input)
{
    uint32_t numberOfIncreases = 0;

    for (int i = 1; i < input.size() - 2; i++)
    {
        uint32_t prevWindow = std::accumulate(input.begin() + i - 1, input.begin() + i - 1 + 3, 0);
        uint32_t nextWindow = std::accumulate(input.begin() + i, input.begin() + i + 3, 0);

        if (nextWindow > prevWindow)
        {
            numberOfIncreases++;
        }
    }

    return std::to_string(numberOfIncreases);
}

int main()
{
    // Get input from txt file
    std::string line;
    std::vector<std::string> lines;
    std::ifstream file("input.txt");
    while (std::getline(file, line))
    {
        lines.push_back(line);
    }

    // Clean input
    std::vector<int> input;
    for (int i = 0; i < lines.size(); i++)
    {
        input.push_back(stoi(lines[i]));
    }
    
    // Part 1
    std::cout << PartOne(input) << "\n";

    // Part 2
    std::cout << PartTwo(input) << "\n";
}
