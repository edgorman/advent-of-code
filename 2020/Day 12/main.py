import os
import math
import numpy as np

def part_one_helper(angle, change):
    change = math.radians(change)
    angle = angle + change

    if angle > 2 * math.pi:
        angle = angle - (2 * math.pi)
    elif angle < 0:
        angle = angle + (2 * math.pi)
    
    return angle

def part_one(entries):
    # Ship properties
    x, y = (0, 0)
    a = math.pi / 2

    # For each instruction
    for instr, value in entries:
        # Switch on instruction
        if instr == 'N':
            y = y + value
        elif instr == 'S':
            y = y - value
        elif instr == 'E':
            x = x + value
        elif instr == 'W':
            x = x - value
        elif instr == 'L':
            a = part_one_helper(a, -1 * value)
        elif instr == 'R':
            a = part_one_helper(a, value)
        elif instr == 'F':
            x = x + round((math.sin(a) * value), 3)
            y = y + round((math.cos(a) * value), 3)
    
    return int(abs(x) + abs(y))

def part_two_helper(x, y, change):
    return part_one_helper(math.atan2(x, y), change)

def part_two(entries):
    # Ship properties
    x, y = (0, 0)
    wx, wy = (10, 1)

    # For each instruction
    for instr, value in entries:
        # Switch on instruction
        if instr == 'N':
            wy = wy + value
        elif instr == 'S':
            wy = wy - value
        elif instr == 'E':
            wx = wx + value
        elif instr == 'W':
            wx = wx - value
        elif instr == 'L':
            wa = part_two_helper(wx, wy, -1 * value)
            radius = math.sqrt(wx ** 2 + wy ** 2)
            wx = round(math.sin(wa) * radius, 3)
            wy = round(math.cos(wa) * radius, 3)
        elif instr == 'R':
            wa = part_two_helper(wx, wy, value)
            radius = math.sqrt(wx ** 2 + wy ** 2)
            wx = round(math.sin(wa) * radius, 3)
            wy = round(math.cos(wa) * radius, 3)
        elif instr == 'F':
            for _ in range(value):
                x = x + wx
                y = y + wy

    return int(abs(x) + abs(y))

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 12\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append((
            entry[0],
            int(entry[1:len(entry)].rstrip())
        ))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
