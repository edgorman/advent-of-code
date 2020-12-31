import os
import copy

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

def part_two(cups):
    CUP_COUNT = 1000000
    ITERATIONS = 10000000

    # Update cup list with extra nums
    cups = cups + [n for n in range(max(cups) + 1, CUP_COUNT + 1)]

    # Convert to linked list
    linked_list = {}
    for a, b in zip(cups, (cups[1:] + [cups[0]])):
        linked_list[a] = b
    # linked_list = {number: cups[(idx + 1) % len(cups)] for idx, number in enumerate(cups)}

    # Get first value
    curr = cups[0]

    # Iterate n times
    for _ in range(ITERATIONS):
        # Get next 3 cups
        cup1 = linked_list[curr]
        cup2 = linked_list[cup1]
        cup3 = linked_list[cup2]
        next_cup = linked_list[cup3]
        curr_cups = [cup1, cup2, cup3]

        # Get destination cup
        dest = curr - 1
        while True:
            if dest not in curr_cups and dest >= 1:
                break
            else:
                if dest in curr_cups: 
                    dest -= 1
                if dest < 1: 
                    dest = max(cups)

        # Update links
        linked_list[curr], linked_list[dest], linked_list[cup3] = linked_list[cup3], cup1, linked_list[dest]
	
	    # Update current to next
        curr = next_cup

    return linked_list[1] * linked_list[linked_list[1]]

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
    print(part_one(copy.deepcopy(entries)))

    # Part two
    print(part_two(copy.deepcopy(entries)))
