import os

def part_one(entries):
    # For each input
    for a in entries:
        # For each other input:
        for b in entries:
            # Check if sums to 2020
            if sum([int(a), int(b)]) == 2020:
                # Output a*b and exit
                return int(a) * int(b)

def part_two(entries):
    # Create all combinations
    combinations = [(a, b, c) for a in entries for b in entries for c in entries]

    # For each combination
    for c in combinations:
        # Check if sums to 2020
        if sum(c) == 2020:
            # Return a*b*c
            return c[0] * c[1] * c[2]

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 1\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(int(entry.rstrip()))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
