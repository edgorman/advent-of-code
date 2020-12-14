import os
from collections import defaultdict

def part_one(entries):
    yes_count = 0

    # For each group
    for group in entries:
        # List of yes answers
        yes_answers = set()

        # For person in group
        for person in group:
            # For answer in person
            for answer in person:
                yes_answers.add(answer)
        
        # Aggregate answers to yes_count
        yes_count = yes_count + len(yes_answers)

    return yes_count

def part_two(entries):
    yes_count = 0

    # For each group
    for group in entries:
        # Dict of yes answers
        yes_answers = defaultdict(int)

        # For person in group
        for person in group:
            # For answer in person
            for answer in person:
                yes_answers[answer] = yes_answers[answer] + 1
        
        # For each answer result
        for answer, count in yes_answers.items():
            # Check if count equal group size
            if count == len(group):
                # Increment yes_count
                yes_count = yes_count + 1
    
    return yes_count

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 6\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    group = []
    for entry in file_input:
        # If starting a new group
        if entry == "\n":
            entries.append(group)
            group = []
            continue
        
        # Get answers for people in group
        group.append(entry.rstrip())
    # Add the final group
    entries.append(group)
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
