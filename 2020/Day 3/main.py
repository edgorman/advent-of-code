import os

def part_one(entries, sx, sy):
    trees_hit = 0
    deepest_y = len(entries) - 1

    # Starting position
    curr_x = 0
    curr_y = 0
    
    # Step values
    step_x = sx
    step_y = sy

    # Traverse until hit the bottom
    while curr_y <= deepest_y:
        # Check if x beyond input
        if curr_x >= len(entries[0]):
            curr_x = curr_x - len(entries[0])
        
        # Check if spot has a tree
        if entries[curr_y][curr_x] == "#":
            trees_hit = trees_hit + 1
        
        # Traverse down hill
        curr_x = curr_x + step_x
        curr_y = curr_y + step_y
    
    return trees_hit


def part_two(entries):
    tree_mult = 1
    step_coords = [
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2)
    ]

    # For each slopes descent profile
    for s in step_coords:
        # Calculate tree hit using part_one
        tree_mult = tree_mult * part_one(entries, s[0], s[1])
    
    return tree_mult

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 3\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(entry.rstrip())
    
    # Part one
    print(part_one(entries, 3, 1))

    # Part two
    print(part_two(entries))
