#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <tuple>

std::string PartOne(std::vector<std::tuple<std::string, std::int32_t>> input)
{
    uint32_t horizontal = 0;
    uint32_t depth = 0;

    for (int i = 0; i < input.size(); i++)
    {
        std::string direction = std::get<0>(input[i]);
        std::int32_t value = std::get<1>(input[i]);

        if (direction == "forward")
        {
            horizontal += value;
        }
        else if (direction == "down")
        {
            depth += value;
        }
        else if (direction == "up")
        {
            depth -= value;
        }
    }

    uint32_t finalPosition = horizontal * depth;

    return std::to_string(finalPosition);
}

std::string PartTwo(std::vector<std::tuple<std::string, std::int32_t>> input)
{
    uint32_t horizontal = 0;
    uint32_t depth = 0;
    uint32_t aim = 0;

    for (int i = 0; i < input.size(); i++)
    {
        std::string direction = std::get<0>(input[i]);
        std::int32_t value = std::get<1>(input[i]);

        if (direction == "forward")
        {
            horizontal += value;
            depth += value * aim;
        }
        else if (direction == "down")
        {
            aim += value;
        }
        else if (direction == "up")
        {
            aim -= value;
        }
    }

    uint32_t finalPosition = horizontal * depth;

    return std::to_string(finalPosition);
}

std::vector<std::string> Split(std::string input, std::string delimiter)
{
    std::vector<std::string> tokens;

    size_t pos = 0;
    while ((pos = input.find(delimiter)) != std::string::npos) {
        tokens.push_back(input.substr(0, pos));
        input.erase(0, pos + delimiter.length());
    }
    tokens.push_back(input);

    return tokens;
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
    std::vector<std::tuple<std::string, std::int32_t>> input;
    for (int i = 0; i < lines.size(); i++)
    {
        std::vector<std::string> tokens = Split(lines[i], " ");
        input.push_back(std::make_tuple(tokens[0], stoi(tokens[1])));
    }

    // Part 1
    std::cout << PartOne(input) << "\n";

    // Part 2
    std::cout << PartTwo(input) << "\n";
}
