#include <iostream>
#include <fstream>
#include <string>
#include <vector>

std::string PartOne(std::vector<int> input)
{
    return "";
}

std::string PartTwo(std::vector<int> input)
{

    return "";
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
