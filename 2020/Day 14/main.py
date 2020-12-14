import os
import re

def part_one(entries):
    memory = {}

    # For each mask and list of memory locations
    for mask, memories in entries:
        # For each location and value
        for pos, value in memories:
            bin_value = ''.join([b if b != 'X' else a for (a, b) in zip(value, mask)])
            
            # Update memory at position
            dec_value = int(bin_value, 2)
            dec_pos = int(pos, 2)
            memory[dec_pos] = dec_value

    # Return sum of values in memory
    return sum(memory.values())

def part_two(entries):
    memory = {}

    # For each mask and list of memory locations
    for mask, memories in entries:
        # Add mask to each memory
        for i in range(len(memories)):
            pos, value = memories[i]
            bin_pos = ''.join([b if b != '0' else a for (a, b) in zip(pos, mask)])
            memories[i] = (bin_pos, value)
        
        # Iterate until no memories remain
        while len(memories) > 0:
            pos, value = memories.pop(0)

            # If value contains 'X'
            if 'X' in pos:
                # Replace next 'X' with 0 and 1
                memories.extend([
                    (pos.replace('X', '0', 1), value), 
                    (pos.replace('X', '1', 1), value)
                ])
                continue

            # Update memory at position
            dec_value = int(value, 2)
            dec_pos = int(pos, 2)
            memory[dec_pos] = dec_value
    
    # Return sum of values in memory
    return sum(memory.values())

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 14\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    mask = ""
    mems = []
    for entry in file_input:
        tokens = entry.split(' ')

        # If starting a new mask
        if "mask" in entry:
            if len(mems) > 0:
                entries.append((mask, mems))
            mask = tokens[2].rstrip()
            mems = []
            continue
        
        # Add memory address to list
        mems.append((
            format(int(''.join(filter(str.isdigit, tokens[0]))), '#038b')[2:],
            format(int(tokens[2].rstrip()), '#038b')[2:]
        ))
    entries.append((mask, mems))

    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
