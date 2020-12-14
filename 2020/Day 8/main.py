import os

def part_one(entries):
    # Computer variables
    accumulator = 0
    instr_id = 0

    # List of instructions visited
    visited_instr = []

    # Iterate until revisit instruction or finish
    while not (instr_id in visited_instr or instr_id >= len(entries)):
        visited_instr.append(instr_id)
        instr, value = entries[instr_id]
        
        # Execute instruction
        if instr == 'acc':
            accumulator = accumulator + value
            instr_id = instr_id + 1
        elif instr == 'jmp':
            instr_id = instr_id + value
        elif instr == 'nop':
            instr_id = instr_id + 1

    return instr_id in visited_instr, accumulator

def part_two(entries):
    # List of instructions switched
    switched_instr = []

    # Continue until instr found
    while True:
        entries_copy = entries.copy()

        # For each instr
        for i in range(len(entries)):
            # Ignore already switched instructions
            if i in switched_instr:
                continue

            instr, value = entries[i]

            # Switch instructions
            if instr == 'jmp':
                entries_copy[i] = ('nop', value)
                switched_instr.append(i)
                break
            elif instr == 'nop':
                entries_copy[i] = ('jmp', value)
                switched_instr.append(i)
                break
        
        # Execute instructions
        revist_instr, accumulator = part_one(entries_copy)

        # If didn't repeat instruction
        if not revist_instr:
            return accumulator


if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 8\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        tokens = entry.split(' ')
        entries.append((
            tokens[0],
            int(tokens[1].rstrip())
        ))
    
    # Part one
    print(part_one(entries)[1])

    # Part two
    print(part_two(entries))
