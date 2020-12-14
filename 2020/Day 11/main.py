import os
import copy

def part_one_output(state):
    output = ""

    for y in range(len(state)):
        for x in range(len(state[0])):
            output = output + str(state[y][x])
        output = output + "\n"

    return output


def part_one_helper(state, m, n):
    count = 0

    # Check all surrounding positions
    for y in range(n-1, n+2):
        # Check y within bounds
        if len(state) > y >= 0:
            for x in range(m-1, m+2):
                # Check x within bounds
                if len(state[0]) > x >= 0:
                    # Check x,y not same as m,n
                    if x == m and y == n:
                        # Skip current state
                        continue
                    else:
                        # If position is occupied
                        if state[y][x] == '#':
                            count = count + 1
    
    return count

def part_one(entries, empty_seat_thresh, count_method):
    curr_state = entries
    state_stable = False

    # While state is not stable (assumes not stable)
    while not state_stable:
        # Update state variables
        state_stable = True
        next_state = copy.deepcopy(curr_state)

        # Apply rules to each point
        for y in range(len(curr_state)):
            for x in range(len(curr_state[0])):

                # If seat is currently empty
                if curr_state[y][x] == 'L':
                    # Helper counts surrounding occupied seats
                    surroud_count = count_method(curr_state, x, y)

                    if surroud_count == 0:
                        next_state[y][x] = '#'
                        state_stable = False
                # Else if occupied
                elif curr_state[y][x] == '#':
                    # Helper counts surrounding occupied seats
                    surroud_count = count_method(curr_state, x, y)

                    if surroud_count >= empty_seat_thresh:
                        next_state[y][x] = 'L'
                        state_stable = False
        
        # Update current state
        curr_state = copy.deepcopy(next_state)

    # Count occupied seats
    occupied_seats = 0
    for y in range(len(curr_state)):
        for x in range(len(curr_state[0])):
            if curr_state[y][x] == '#':
                occupied_seats = occupied_seats + 1

    return occupied_seats

def part_two_helper(state, m, n):
    count = 0

    # positive x, constant y
    for x in range(m, len(state[0]), 1):
        if x == m:
            continue
        else:
            # If position is free
            if state[n][x] == 'L':
                break
            # Else if position is occupied
            elif state[n][x] == '#':
                count = count + 1
                break
    
    # negative x, constant y
    for x in range(m, -1, -1):
        if x == m:
            continue
        else:
           # If position is free
            if state[n][x] == 'L':
                break
            # Else if position is occupied
            elif state[n][x] == '#':
                count = count + 1
                break
    
    # positive y, constant x
    for y in range(n, len(state), 1):
        if y == n:
            continue
        else:
            # If position is free
            if state[y][m] == 'L':
                break
            # Else if position is occupied
            elif state[y][m] == '#':
                count = count + 1
                break
    
    # negative y, constant x
    for y in range(n, -1, -1):
        if y == n:
            continue
        else:
            # If position is free
            if state[y][m] == 'L':
                break
            # Else if position is occupied
            elif state[y][m] == '#':
                count = count + 1
                break
    
    # TODO:
    # positive x, positive y
    for i in range(min(len(state[0])-m-1, len(state)-n-1)):
        x, y = (m+1+i, n+1+i)
        if x == m and y == n:
            continue
        else:
            # If position is free
            if state[y][x] == 'L':
                break
            # Else if position is occupied
            elif state[y][x] == '#':
                count = count + 1
                break

    # positive x, negative y
    for i in range(min(len(state[0])-m-1, n-0)):
        x, y = (m+1+i, n-1-i)
        if x == m and y == n:
            continue
        else:
            # If position is free
            if state[y][x] == 'L':
                break
            # Else if position is occupied
            elif state[y][x] == '#':
                count = count + 1
                break

    # negative x, positive y
    for i in range(min(m-0, len(state)-n-1)):
        x, y = (m-1-i, n+1+i)
        if x == m and y == n:
            continue
        else:
            # If position is free
            if state[y][x] == 'L':
                break
            # Else if position is occupied
            elif state[y][x] == '#':
                count = count + 1
                break

    # negative x, negative y
    for i in range(min(m-0, n-0)):
        x, y = (m-1-i, n-1-i)
        if x == m and y == n:
            continue
        else:
            # If position is free
            if state[y][x] == 'L':
                break
            # Else if position is occupied
            elif state[y][x] == '#':
                count = count + 1
                break

    return count

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 11\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(list(entry.rstrip()))
    
    # Part one
    print(part_one(entries, 4, part_one_helper))

    # Part two
    print(part_one(entries, 5, part_two_helper))
