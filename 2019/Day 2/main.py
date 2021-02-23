import os
from copy import deepcopy

def part_one(intcode):
    index = 0

    # Modify intcode
    intcode[1] = 12
    intcode[2] = 2

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

def part_two(entries):
    pass

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
    print(part_one(deepcopy(entries)))

    # Part two
    print(part_two(deepcopy(entries)))
