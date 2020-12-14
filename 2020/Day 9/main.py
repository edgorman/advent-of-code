import os

def part_one(entries, preamble_len):
    index = preamble_len

    # Iterate until index reaches end
    while index < len(entries):
        # Get preamble numbers and target value
        preamble = entries[index - preamble_len:index]
        target = entries[index]
        
        # Generate preamble combinations
        combos = [(a, b) for a in preamble for b in preamble if a != b]
        combo_found = False
        
        # For each combination
        for c in combos:
            # If sum equals target
            if sum(c) == target:
                combo_found = True
                break
        
        # If no combo value equals target
        if not combo_found:
            return target

        # Increment index
        index = index + 1

    return None

def part_two(entries, target):
    start = 0
    
    # Iterate until start reaches last entry
    while start < len(entries):
        end = start + 2
        value = 0

        # Iterate until end reaches last entry or value exceeds target
        while end < len(entries) and value < target:
            # Calculate value from contigious entries
            value = sum(entries[start:end])

            # If value equals target
            if value == target:
                # Return smallest and largest number
                sort = sorted(entries[start:end])
                return sort[0] + sort[len(sort)-1]

            # Increment end
            end = end + 1
        
        # Increment start
        start = start + 1

    return None

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 9\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(int(entry.rstrip()))
    
    # Part one
    print(part_one(entries, 25))

    # Part two
    print(part_two(entries, 144381670))
