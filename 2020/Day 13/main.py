"""
--- Day X: Template ---
Challenge description here
"""

import os

def part_one(entries):
    # Get earliest depart time
    earliest_time = int(entries[0])

    # Parse ids from list
    bus_list = []
    for bus_id in entries[1].split(','):
        if bus_id == 'x':
            continue
        else:
            bus_list.append((int(bus_id), 0))
    
    # Iterate until value found
    while True:
        # Get the bus at the front of list
        bus_id, time = bus_list.pop(0)

        # Check if time is greater than earliest time
        if time >= earliest_time:
            return (time - earliest_time) * bus_id
        
        # Add bus back to list
        bus_list.append((bus_id, time + bus_id))

        # Sort bus list by time
        bus_list.sort(key=lambda tup: tup[1])

def part_two_helper(bus_list, timestamp):
    for bus_id, offset in bus_list:
        if (timestamp + offset) % bus_id == 0:
            continue
        return False
    return True

def part_two(entries):
    id_tokens = entries[1].split(',')

    # Parse ids from list
    bus_list = []
    for index in range(len(id_tokens)):
        if id_tokens[index] == 'x':
            continue
        else:
            bus_list.append((int(id_tokens[index]), index))
    
    timestamp = 0
    # Iterate until value found
    while True:
        print(timestamp)

        # If timestamp is valid
        if part_two_helper(bus_list, timestamp):
            return timestamp
        
        # Increment timestamp value
        timestamp = timestamp + bus_list[0][0]
    
    return None

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 13\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(entry.rstrip())
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
