import os
import copy

def part_one(flip_tiles):
    flipped_tiles = []

    # For each tile
    for tile in flip_tiles:
        position = (0, 0, 0)

        # For each direction
        for direction in tile:
            # Switch on direction
            position = {
                'ne': lambda p: (p[0]+1, p[1], p[2]-1),
                'e': lambda p: (p[0]+1, p[1]-1, p[2]),
                'se': lambda p: (p[0], p[1]-1, p[2]+1),
                'sw': lambda p: (p[0]-1, p[1], p[2]+1),
                'w': lambda p: (p[0]-1, p[1]+1, p[2]),
                'nw': lambda p: (p[0], p[1]+1, p[2]-1),
            }[direction](position)
        
        # Check if position already flipped
        if position in flipped_tiles:
            # Flip back
            flipped_tiles.remove(position)
        else:
            # Flip
            flipped_tiles.append(position)
        
    return flipped_tiles

def part_two_helper(tile):
    return [
        (tile[0]+1, tile[1], tile[2]-1),    # ne
        (tile[0]+1, tile[1]-1, tile[2]),    # e
        (tile[0], tile[1]-1, tile[2]+1),    # se
        (tile[0]-1, tile[1], tile[2]+1),    # sw
        (tile[0]-1, tile[1]+1, tile[2]),    # w
        (tile[0], tile[1]+1, tile[2]-1),    # nw
    ]

def part_two(flip_tiles):
    black_tiles = part_one(flip_tiles)

    # Iterate n times
    for x in range(100):
        print(x, len(black_tiles))
        new_black_tiles = copy.deepcopy(black_tiles)

        # Generate list of white tiles
        white_tiles = set()
        for tile in black_tiles:
            for surround in part_two_helper(tile):
                if surround not in black_tiles:
                    white_tiles.add(surround)

        # Calculate new white tiles
        for tile in black_tiles:
            black_count = 0
            for surround in part_two_helper(tile):
                if surround in black_tiles:
                    black_count = black_count + 1
            if black_count == 0 or black_count > 2:
                new_black_tiles.remove(tile)
        
        # Calculate new black tiles
        for tile in white_tiles:
            black_count = 0
            for surround in part_two_helper(tile):
                if surround in black_tiles:
                    black_count = black_count + 1
            if black_count == 2:
                new_black_tiles.append(tile)

        # Set black tiles to the new list
        black_tiles = new_black_tiles

    return black_tiles

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 24\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        index = 0
        directions = []
        while index < len(entry.rstrip()):
            if entry.rstrip()[index] == 'e':
                directions.append('e')
                index = index + 1
            elif entry.rstrip()[index] == 'w':
                directions.append('w')
                index = index + 1
            elif entry.rstrip()[index] == 'n':
                if entry.rstrip()[index + 1] == 'e':
                    directions.append('ne')
                    index = index + 2
                else:
                    directions.append('nw')
                    index = index + 2
            elif entry.rstrip()[index] == 's':
                if entry.rstrip()[index + 1] == 'e':
                    directions.append('se')
                    index = index + 2
                else:
                    directions.append('sw')
                    index = index + 2
        entries.append(directions)
    
    # Part one
    print(len(part_one(entries)))

    # Part two
    print(len(part_two(entries)))
