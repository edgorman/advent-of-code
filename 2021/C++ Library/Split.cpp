#include "Split.h"

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