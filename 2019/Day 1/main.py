import os
import math

def part_one(modules):
    mass_list = []

    # For each module
    for m in modules:
        # Calculate mass and append to list
        mass_list.append(math.floor(m / 3) - 2)
    
    # Return sum of list
    return sum(mass_list)

def part_two(modules):
    mass_list = []

    # For each module
    for m in modules:
        # Calculate mass and append to list
        mass = math.floor(m / 3) - 2

        # Iterate until mass is negative
        while mass > 0:
            mass_list.append(mass)
            mass = math.floor(mass / 3) - 2
    
    # Return sum of list
    return sum(mass_list)

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 1\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(int(entry.rstrip()))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
