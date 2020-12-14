import os

def part_one(entries):
    valid_count = 0

    # For each entry
    for e in entries:
        # Check if valid
        if e[1] >= e[3].count(e[2]) >= e[0]:
            # Increment valid count
            valid_count = valid_count + 1
    
    return valid_count

def part_two(entries):
    valid_count = 0

    # For each entry
    for e in entries:
        # Check if valid
        if bool(e[3][e[0]-1] == e[2]) != bool(e[3][e[1]-1] == e[2]):
            # Increment valid count
            valid_count = valid_count + 1

    return valid_count

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 2\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        tokens = entry.split(' ')
        entries.append((
            int(tokens[0].split('-')[0]),
            int(tokens[0].split('-')[1]),
            tokens[1][0],
            tokens[2].rstrip()
        ))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
