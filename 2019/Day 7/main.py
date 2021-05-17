import os
from copy import deepcopy
from itertools import permutations 

from intcode import IntCode

def part_one(intcode):
    max_value = 0

    # For each permutation
    for perm in permutations([0, 1, 2, 3, 4]):
        result = 0

        # Calculate thruster output
        for input_ in perm:
            amp = IntCode(deepcopy(intcode))
            result = amp.run([input_, result])
        
        if result > max_value:
            max_value = result

    # Return max thruster output
    return max_value

def part_two(entries):
    pass

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 7\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        for number in entry.rstrip().split(','):
            entries.append(int(number))
    
    # Part one
    print(part_one(deepcopy(entries)))

    # Part two
    print(part_two(entries))
