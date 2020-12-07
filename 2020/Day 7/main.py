"""
--- Day X: Template ---
Challenge description here
"""

import os
from collections import defaultdict

def part_one(entries):
    gold_bag_list = []

    # Repeat n times
    for _ in range(5):
        # For each bag
        for parent_bag, parent_list in entries:
            # Ignore bags which contain nothing
            if len(parent_list) == 0:
                continue

            # Ignore bags already in gold_bag_list
            if parent_bag in gold_bag_list:
                continue

            # For each bag in parent
            for _, child_bag in parent_list:
                # Check if bag contains "shiny gold"
                if child_bag == "shiny gold":
                    gold_bag_list.append(parent_bag)
                    break
                
                # Check if bag is in gold_bag_list
                if child_bag in gold_bag_list:
                    gold_bag_list.append(parent_bag)
                    break

    return len(gold_bag_list)

def part_two(entries):
    pass    

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 7\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        tokens = entry.split(' ')

        # Get bag and bag list info
        bag = ' '.join(tokens[0:2])
        bag_list = []
        index = 4
        while index < len(tokens):
            # If bag contains no other bags
            if tokens[index] == 'no':
                break
            # Else contains other bags
            else:
                bag_list.append((tokens[index], ' '.join(tokens[index + 1:index + 3])))
                index = index + 4

        entries.append((bag, bag_list))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
