import os
from copy import deepcopy

def part_one_helper(intcode, position, mode):
    if mode == "1": return intcode[position]
    else: return intcode[intcode[position]]

def part_one(intcode, input_):
    index = 0
    output_ = 0

    # Define custom op code functions
    def add(a, b):
        return a + b
    
    def mult(a, b):
        return a * b
    
    def store(v, p):
        intcode[p] = v
    
    def output(p):
        return intcode[p]

    # Iterate until reach the stop code
    while intcode[index] != 99:
        opcode = int(str(intcode[index])[-2:])
        modes = str(intcode[index])[:-2][::-1] + '000'

        # Addition
        if opcode == 1:
            intcode[intcode[index + 3]] = add(
                part_one_helper(intcode, index + 1, modes[0]), 
                part_one_helper(intcode, index + 2, modes[1])
            )
            index += 4
        # Multiplication
        elif opcode == 2:
            intcode[intcode[index + 3]] = mult(
                part_one_helper(intcode, index + 1, modes[0]), 
                part_one_helper(intcode, index + 2, modes[1])
            )
            index += 4
        # Storage
        elif opcode == 3:
            store(input_, intcode[index + 1])
            index += 2
        # Output
        elif opcode == 4:
            output_ = output(intcode[index + 1])
            index += 2
        # Unknown
        else:
            raise Exception("Unknown op code")

    return output_

def part_two(intcode, input_):
    index = 0
    output_ = 0

    # Define custom op code functions
    def add(a, b):
        return a + b
    
    def mult(a, b):
        return a * b
    
    def store(v, p):
        intcode[p] = v
    
    def output(p):
        return intcode[p]
    
    def lessthan(a, b):
        return a < b

    def equals(a, b):
        return a == b

    # Iterate until reach the stop code
    while intcode[index] != 99:
        opcode = int(str(intcode[index])[-2:])
        modes = str(intcode[index])[:-2][::-1] + '000'

        # Addition
        if opcode == 1:
            intcode[intcode[index + 3]] = add(
                part_one_helper(intcode, index + 1, modes[0]), 
                part_one_helper(intcode, index + 2, modes[1])
            )
            index += 4
        # Multiplication
        elif opcode == 2:
            intcode[intcode[index + 3]] = mult(
                part_one_helper(intcode, index + 1, modes[0]), 
                part_one_helper(intcode, index + 2, modes[1])
            )
            index += 4
        # Storage
        elif opcode == 3:
            store(input_, intcode[index + 1])
            index += 2
        # Output
        elif opcode == 4:
            output_ = output(intcode[index + 1])
            index += 2
        # Jump if true
        elif opcode == 5:
            if part_one_helper(intcode, index + 1, modes[0]) != 0:
                index = part_one_helper(intcode, index + 2, modes[1])
            else:
                index += 3
        # Jump if false
        elif opcode == 6:
            if part_one_helper(intcode, index + 1, modes[0]) == 0:
                index = part_one_helper(intcode, index + 2, modes[1])
            else:
                index += 3
        # Less than
        elif opcode == 7:
            value = 0
            position = intcode[index + 3]
            if lessthan(
                part_one_helper(intcode, index + 1, modes[0]), 
                part_one_helper(intcode, index + 2, modes[1])
            ):
                value = 1
            store(value, position)
            index += 4
        # Equals
        elif opcode == 8:
            value = 0
            position = intcode[index + 3]
            if equals(
                part_one_helper(intcode, index + 1, modes[0]), 
                part_one_helper(intcode, index + 2, modes[1])
            ):
                value = 1
            store(value, position)
            index += 4
        # Unknown
        else:
            raise Exception("Unknown op code")

    return output_

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 5\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        for number in entry.rstrip().split(','):
            entries.append(int(number))
    
    # Part one
    print(part_one(deepcopy(entries), 1))

    # Part two
    print(part_two(deepcopy(entries), 5))
