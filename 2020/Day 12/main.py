"""
--- Day 12: Rain Risk ---
Your ferry made decent progress toward the island, but the storm came in faster than anyone expected. The ferry needs to take evasive actions!

Unfortunately, the ship's navigation computer seems to be malfunctioning; rather than giving a route directly to safety, it produced extremely circuitous instructions. When the captain uses the PA system to ask if anyone can help, you quickly volunteer.

The navigation instructions (your puzzle input) consists of a sequence of single-character actions paired with integer input values. After staring at them for a few minutes, you work out what they probably mean:

Action N means to move north by the given value.
Action S means to move south by the given value.
Action E means to move east by the given value.
Action W means to move west by the given value.
Action L means to turn left the given number of degrees.
Action R means to turn right the given number of degrees.
Action F means to move forward by the given value in the direction the ship is currently facing.
The ship starts by facing east. Only the L and R actions change the direction the ship is facing. (That is, if the ship is facing east and the next instruction is N10, the ship would move north 10 units, but would still move east if the following action were F.)

For example:

F10
N3
F7
R90
F11
These instructions would be handled as follows:

F10 would move the ship 10 units east (because the ship starts by facing east) to east 10, north 0.
N3 would move the ship 3 units north to east 10, north 3.
F7 would move the ship another 7 units east (because the ship is still facing east) to east 17, north 3.
R90 would cause the ship to turn right by 90 degrees and face south; it remains at east 17, north 3.
F11 would move the ship 11 units south to east 17, south 8.
At the end of these instructions, the ship's Manhattan distance (sum of the absolute values of its east/west position and its north/south position) from its starting position is 17 + 8 = 25.

Figure out where the navigation instructions lead. What is the Manhattan distance between that location and the ship's starting position?

Your puzzle answer was 1152.

--- Part Two ---
Before you can give the destination to the captain, you realize that the actual action meanings were printed on the back of the instructions the whole time.

Almost all of the actions indicate how to move a waypoint which is relative to the ship's position:

Action N means to move the waypoint north by the given value.
Action S means to move the waypoint south by the given value.
Action E means to move the waypoint east by the given value.
Action W means to move the waypoint west by the given value.
Action L means to rotate the waypoint around the ship left (counter-clockwise) the given number of degrees.
Action R means to rotate the waypoint around the ship right (clockwise) the given number of degrees.
Action F means to move forward to the waypoint a number of times equal to the given value.
The waypoint starts 10 units east and 1 unit north relative to the ship. The waypoint is relative to the ship; that is, if the ship moves, the waypoint moves with it.

For example, using the same instructions as above:

F10 moves the ship to the waypoint 10 times (a total of 100 units east and 10 units north), leaving the ship at east 100, north 10. The waypoint stays 10 units east and 1 unit north of the ship.
N3 moves the waypoint 3 units north to 10 units east and 4 units north of the ship. The ship remains at east 100, north 10.
F7 moves the ship to the waypoint 7 times (a total of 70 units east and 28 units north), leaving the ship at east 170, north 38. The waypoint stays 10 units east and 4 units north of the ship.
R90 rotates the waypoint around the ship clockwise 90 degrees, moving it to 4 units east and 10 units south of the ship. The ship remains at east 170, north 38.
F11 moves the ship to the waypoint 11 times (a total of 44 units east and 110 units south), leaving the ship at east 214, south 72. The waypoint stays 4 units east and 10 units south of the ship.
After these operations, the ship's Manhattan distance from its starting position is 214 + 72 = 286.

Figure out where the navigation instructions actually lead. What is the Manhattan distance between that location and the ship's starting position?

Your puzzle answer was 58637.
"""

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
