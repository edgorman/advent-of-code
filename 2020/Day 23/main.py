import os

def part_one(cups):
    # Iterate n times
    for _ in range(100):
        # Get current cup value
        curr_value = cups[0]

        # Get next 3 cups
        cup1 = cups.pop(1)
        cup2 = cups.pop(1)
        cup3 = cups.pop(1)

        # Get destination cup
        dest_value = curr_value - 1
        while dest_value not in cups:
            if dest_value < min(cups):
                dest_value = max(cups)
            else:
                dest_value = dest_value - 1

        # Cacluate index
        dest_index = cups.index(dest_value)

        # Add after dest value
        cups.insert(dest_index + 1, cup1)
        cups.insert(dest_index + 2, cup2)
        cups.insert(dest_index + 3, cup3)

        # Move first number to back
        cups.append(cups.pop(0))
    
    # Find cup order starting at 1
    oi = cups.index(1)
    return ''.join(map(str, cups[oi+1:] + cups[:oi]))

def part_two(entries):
    pass

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 23\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        for e in entry.rstrip():
            entries.append(int(e))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
