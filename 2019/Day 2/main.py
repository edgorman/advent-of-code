import os
from copy import deepcopy

def part_one(intcode, noun, verb):
    index = 0

    # Modify intcode
    intcode[1] = noun
    intcode[2] = verb

    # Define custom op code functions
    def add(a, b):
        return a+b
    
    def mult(a, b):
        return a*b
    
    # Iterate until reach the stop code
    while intcode[index] != 99:
        # Addition
        if intcode[index] == 1:
            intcode[intcode[index + 3]] = add(
                intcode[intcode[index + 1]],
                intcode[intcode[index + 2]]
            )
        # Multiplication
        elif intcode[index] == 2:
            intcode[intcode[index + 3]] = mult(
                intcode[intcode[index + 1]],
                intcode[intcode[index + 2]]
            )
        # Else unknown
        else:
            raise Exception("Unknown op code")
        
        # Increment index
        index += 4

    return intcode[0]

def part_two(entries, value):
    # For each combination of noun and verb
    for noun in range(1, 100):
        for verb in range(1, 100):
            # Calculate result of execution
            result = part_one(deepcopy(entries), noun, verb)

            # Check if result equals value
            if result == value:
                return (100 * noun) + verb
    
    # Couldn't find number
    return 0

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 2\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        for number in entry.rstrip().split(','):
            entries.append(int(number))
    
    # Part one
    print(part_one(deepcopy(entries), 12, 2))

    # Part two
    print(part_two(deepcopy(entries), 19690720))
