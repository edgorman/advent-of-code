"""
--- Day 6: Custom Customs ---
As your flight approaches the regional airport where you'll switch to a much larger plane, customs declaration forms are distributed to the passengers.

The form asks a series of 26 yes-or-no questions marked a through z. All you need to do is identify the questions for which anyone in your group answers "yes". Since your group is just you, this doesn't take very long.

However, the person sitting next to you seems to be experiencing a language barrier and asks if you can help. For each of the people in their group, you write down the questions for which they answer "yes", one per line. For example:

abcx
abcy
abcz
In this group, there are 6 questions to which anyone answered "yes": a, b, c, x, y, and z. (Duplicate answers to the same question don't count extra; each question counts at most once.)

Another group asks for your help, then another, and eventually you've collected answers from every group on the plane (your puzzle input). Each group's answers are separated by a blank line, and within each group, each person's answers are on a single line. For example:

abc

a
b
c

ab
ac

a
a
a
a

b
This list represents answers from five groups:

The first group contains one person who answered "yes" to 3 questions: a, b, and c.
The second group contains three people; combined, they answered "yes" to 3 questions: a, b, and c.
The third group contains two people; combined, they answered "yes" to 3 questions: a, b, and c.
The fourth group contains four people; combined, they answered "yes" to only 1 question, a.
The last group contains one person who answered "yes" to only 1 question, b.
In this example, the sum of these counts is 3 + 3 + 3 + 1 + 1 = 11.

For each group, count the number of questions to which anyone answered "yes". What is the sum of those counts?

Your puzzle answer was 6633.

--- Part Two ---
As you finish the last group's customs declaration, you notice that you misread one word in the instructions:

You don't need to identify the questions to which anyone answered "yes"; you need to identify the questions to which everyone answered "yes"!

Using the same example as above:

abc

a
b
c

ab
ac

a
a
a
a

b
This list represents answers from five groups:

In the first group, everyone (all 1 person) answered "yes" to 3 questions: a, b, and c.
In the second group, there is no question to which everyone answered "yes".
In the third group, everyone answered yes to only 1 question, a. Since some people did not answer "yes" to b or c, they don't count.
In the fourth group, everyone answered yes to only 1 question, a.
In the fifth group, everyone (all 1 person) answered "yes" to 1 question, b.
In this example, the sum of these counts is 3 + 0 + 1 + 1 + 1 = 6.

For each group, count the number of questions to which everyone answered "yes". What is the sum of those counts?

Your puzzle answer was 3202.
"""

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
