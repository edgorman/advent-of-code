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

def part_two(intcode):
    max_value = 0

    # For each permutation
    for perm in permutations([5, 6, 7, 8, 9]):
        result = 0
        amp_list = [IntCode(deepcopy(intcode))] * 5
        input_list = [0] + list(perm)

        # Calculate thruster output
        while len(input_list) > 0:
            input_ = input_list.pop(0)
            amp = amp_list.pop(0)

            print("amp input", input_, result)
            result = amp.run([input_, result])
            print("amp result", result)

            amp_list.append(amp)
            input_list.append(result)
        
        if result > max_value:
            max_value = result
    
    return max_value

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 7\\test.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        for number in entry.rstrip().split(','):
            entries.append(int(number))
    
    # Part one
    # print(part_one(deepcopy(entries)))

    # Part two
    print(part_two(deepcopy(entries)))
