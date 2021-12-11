#include "Conversion.h"
#include <bitset>

std::uint32_t BitStringToInt(std::string input)
{
    int value = 0;

    for (int i = 0; i < input.size(); i++) {
        value <<= 1;
        value |= input[i] - '0';
    }

    return value;
}