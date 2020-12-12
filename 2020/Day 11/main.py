"""
--- Day 11: Seating System ---
Your plane lands with plenty of time to spare. The final leg of your journey is a ferry that goes directly to the tropical island where you can finally start your vacation. As you reach the waiting area to board the ferry, you realize you're so early, nobody else has even arrived yet!

By modeling the process people use to choose (or abandon) their seat in the waiting area, you're pretty sure you can predict the best place to sit. You make a quick map of the seat layout (your puzzle input).

The seat layout fits neatly on a grid. Each position is either floor (.), an empty seat (L), or an occupied seat (#). For example, the initial seat layout might look like this:

L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL
Now, you just need to model the people who will be arriving shortly. Fortunately, people are entirely predictable and always follow a simple set of rules. All decisions are based on the number of occupied seats adjacent to a given seat (one of the eight positions immediately up, down, left, right, or diagonal from the seat). The following rules are applied to every seat simultaneously:

If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
Otherwise, the seat's state does not change.
Floor (.) never changes; seats don't move, and nobody sits on the floor.

After one round of these rules, every seat in the example layout becomes occupied:

#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##
After a second round, the seats with four or more occupied adjacent seats become empty again:

#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##
This process continues for three more rounds:

#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##
#.#L.L#.##
#LLL#LL.L#
L.L.L..#..
#LLL.##.L#
#.LL.LL.LL
#.LL#L#.##
..L.L.....
#L#LLLL#L#
#.LLLLLL.L
#.#L#L#.##
#.#L.L#.##
#LLL#LL.L#
L.#.L..#..
#L##.##.L#
#.#L.LL.LL
#.#L#L#.##
..L.L.....
#L#L##L#L#
#.LLLLLL.L
#.#L#L#.##
At this point, something interesting happens: the chaos stabilizes and further applications of these rules cause no seats to change state! Once people stop moving around, you count 37 occupied seats.

Simulate your seating area by applying the seating rules repeatedly until no seats change state. How many seats end up occupied?

Your puzzle answer was 2441.

--- Part Two ---
As soon as people start to arrive, you realize your mistake. People don't just care about adjacent seats - they care about the first seat they can see in each of those eight directions!

Now, instead of considering just the eight immediately adjacent seats, consider the first seat in each of those eight directions. For example, the empty seat below would see eight occupied seats:

.......#.
...#.....
.#.......
.........
..#L....#
....#....
.........
#........
...#.....
The leftmost empty seat below would only see one empty seat, but cannot see any of the occupied ones:

.............
.L.L.#.#.#.#.
.............
The empty seat below would see no occupied seats:

.##.##.
#.#.#.#
##...##
...L...
##...##
#.#.#.#
.##.##.
Also, people seem to be more tolerant than you expected: it now takes five or more visible occupied seats for an occupied seat to become empty (rather than four or more from the previous rules). The other rules still apply: empty seats that see no occupied seats become occupied, seats matching no rule don't change, and floor never changes.

Given the same starting layout as above, these new rules cause the seating area to shift around as follows:

L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL
#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##
#.LL.LL.L#
#LLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLLL.L
#.LLLLL.L#
#.L#.##.L#
#L#####.LL
L.#.#..#..
##L#.##.##
#.##.#L.##
#.#####.#L
..#.#.....
LLL####LL#
#.L#####.L
#.L####.L#
#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##LL.LL.L#
L.LL.LL.L#
#.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLL#.L
#.L#LL#.L#
#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.#L.L#
#.L####.LL
..#.#.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#
#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.LL.L#
#.LLLL#.LL
..#.L.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#
Again, at this point, people stop shifting around and the seating area reaches equilibrium. Once this occurs, you count 26 occupied seats.

Given the new visibility method and the rule change for occupied seats becoming empty, once equilibrium is reached, how many seats end up occupied?

Your puzzle answer was 2190.
"""

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
