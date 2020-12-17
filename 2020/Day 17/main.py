import os
import copy

def part_one_helper(pos, cubes):
    surrounding = []
    px, py, pz = pos
    for z in range(-1, 2, 1):
        for y in range(-1, 2, 1):
            for x in range(-1, 2, 1):
                if (x == y == z == 0):
                    # Ignore self
                    continue
                # Add to surrounding
                surrounding.append((
                    (x + px, y + py, z + pz),
                    (x + px, y + py, z + pz) in cubes
                ))
    return surrounding

def part_one(active_cubes, helper_method):
    # For each cycle
    for _ in range(6):
        next_cubes = []
        check_cubes = []
        visited_cubes = []

        # Add known active cubes to check list
        for pos in active_cubes:
            check_cubes.append((pos, True))

        # While cubes to check is not empty
        while len(check_cubes) > 0:
            cube_pos, is_active = check_cubes.pop()
            visited_cubes.append(cube_pos)

            # Get surrounding cubes
            surrounding = helper_method(cube_pos, active_cubes)

            # Count surrounding active cubes
            active_count = sum([1 if active else 0 for (pos, active) in surrounding])

            # If cube is active
            if is_active:
                # Add surrounding cubes
                for pos, active in surrounding:
                    if not active and pos not in visited_cubes:
                        check_cubes.append((pos, active))
                
                # If cube stays active
                if active_count == 2 or active_count == 3:
                    next_cubes.append(cube_pos)
            # Else cube is not active:
            else:
                # If cube becomes active
                if active_count == 3:
                    next_cubes.append(cube_pos)

        # Update current state
        active_cubes = copy.deepcopy(next_cubes)
    
    # Return active cube count
    return len(active_cubes)

def part_two_helper(pos, cubes):
    surrounding = []
    px, py, pz, pw = pos
    for w in range(-1, 2, 1):
        for z in range(-1, 2, 1):
            for y in range(-1, 2, 1):
                for x in range(-1, 2, 1):
                    if (x == y == z == w == 0):
                        # Ignore self
                        continue
                    # Add to surrounding
                    surrounding.append((
                        (x + px, y + py, z + pz, w + pw),
                        (x + px, y + py, z + pz, w + pw) in cubes
                    ))
    return surrounding

def part_two(active_cubes):
    return active_cubes

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 17\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    zed_index = 0
    for row_index, row in enumerate(file_input):
        for col_index, value in enumerate(row):
            if value == '#':
                entries.append((col_index, row_index, zed_index))
    
    # Part one
    print(part_one(entries, part_one_helper))

    entries = []
    dbu_index = 0
    zed_index = 0
    for row_index, row in enumerate(file_input):
        for col_index, value in enumerate(row):
            if value == '#':
                entries.append((col_index, row_index, zed_index, dbu_index))

    # Part two
    print(part_one(entries, part_two_helper))
