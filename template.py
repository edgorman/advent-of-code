"""
--- Day X: Template ---
Challenge description here
"""

import os

def part_one(entries):
    pass

def part_two(entries):
    pass

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\YEAR\\Day X\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(entry.rstrip())
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
