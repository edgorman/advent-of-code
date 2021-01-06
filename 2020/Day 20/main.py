import os
import re
import math
from collections import defaultdict

def part_one_helper(side, remove_ids, entries):
    for remove in remove_ids:
        entries.pop(remove, None)

    # Check rotation
    for tile_id, tile_contents in entries.items():
        if side in tile_contents.values():
            return tile_id
    
    # Check flipping
    side = [(9-s) for s in side[::-1]]
    for tile_id, tile_contents in entries.items():
        if side in tile_contents.values():
            return tile_id

    return None

def part_one(tiles):
    corner_ids = []
    directions = ['top', 'right', 'bottom', 'left']

    # For each tile
    for tile_id, tile_contents in tiles.items():
        match_tiles = []

        # For each side of tile
        for d in directions:
            # Get matching tile
            match = part_one_helper(tile_contents[d], match_tiles+[tile_id], tiles.copy())
            
            # If found a matching tile
            if match is not None:
                match_tiles.append(match)
            
            # If number of matching tiles means not a corner
            if len(match_tiles) > 2:
                break
        
        # If tile is a corner
        if len(match_tiles) == 2:
            corner_ids.append(tile_id)

        # If all corners found
        if len(corner_ids) == 4:
            break
    
    return math.prod(corner_ids)

def part_two(tiles):
    # Set up tile grid
    n = int(math.sqrt(len(tiles)))
    tile_grid = [[] for _ in range(n)]
    corner_id = 0

    # Find corner tile
    directions = ['top', 'right', 'bottom', 'left']
    for tile_id, tile_contents in tiles.items():
        match_tiles = []
        match_directions = []

        # For each side of tile
        for d in directions:
            match = part_one_helper(tile_contents[d], match_tiles+[tile_id], tiles.copy())
            if match is not None:
                match_tiles.append(match)
                match_directions.append(d)
        
        # If tile is a corner
        if len(match_tiles) == 2:
            corner_id = tile_id
            break
    
    # Get matching sides of corner tile
    corner_tile = {
        'bottom': tiles[corner_id][match_directions[0]],
        'right': tiles[corner_id][match_directions[1]]
    }
    
    # Tile grid set up
    tile_grid[0].append(corner_id)
    remaining_tiles = list(tiles.keys())
    remaining_tiles.remove(corner_id)

    # Iterate until all tiles visited
    while len(remaining_tiles) != 0:
        valid_tile = True
        next_tile = remaining_tiles.pop()
        
        # Calculate position of next tile
        tile_n = len(tiles) - len(remaining_tiles)
        tile_x, tile_y = tile_n % n, math.floor(tile_n / n)

        # If not on top row, check top side of next tile
        if tile_y >= 1:
            pass

        # If not on left col, check left side of next tile
        if tile_x != 0:
            pass

        # If next tile is valid        
        if valid_tile:
            tile_grid[tile_y].append(next_tile)
        # else next tile not found, add to end of list
        else:
            remaining_tiles.append(next_tile)

    return tile_grid

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 20\\test.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    tiles = {}
    tile_id = None
    tile_index = 0
    tile_contents = defaultdict(list)
    for entry in file_input:
        # If entry is new  tile
        if entry.rstrip() == "":
            tile_index = 0
            continue

        # If tile index is 0
        if tile_index == 0:
            tile_id = int(re.findall(r'\d+', entry)[0])
            tile_contents = defaultdict(list)
        # Else if tile index is at top
        else:
            # Left and right
            if entry.startswith('#'):
                tile_contents['left'].append(tile_index - 1)
            if entry.rstrip().endswith('#'):
                tile_contents['right'].append(tile_index - 1)
            
            # If tile index is at top
            if tile_index == 1:
                tile_contents['top'] = [i for i in range(len(entry.rstrip())) if entry.startswith('#', i)]
            # Else if tile index is at bottom
            elif tile_index == 10:
                tile_contents['bottom'] = [i for i in range(len(entry.rstrip())) if entry.startswith('#', i)]
                tiles[tile_id] = tile_contents

        # Increment tile index
        tile_index = tile_index + 1
    
    # Part one
    print(part_one(tiles))

    # Part two
    print(part_two(tiles))
