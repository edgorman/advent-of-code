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

def part_one(entries):
    corner_ids = []
    directions = ['top', 'right', 'bottom', 'left']

    # For each tile
    for tile_id, tile_contents in entries.items():
        match_tiles = [tile_id]

        # For each side of tile
        for d in directions:
            # Get matching tile
            match = part_one_helper(tile_contents[d], match_tiles, entries.copy())
            
            # If found a matching tile
            if match is not None:
                match_tiles.append(match)
            
            # If number of matching tiles means not a corner
            if len(match_tiles) > 3:
                break
        
        match_tiles.remove(tile_id)

        # If tile is a corner
        if len(match_tiles) == 2:
            corner_ids.append(tile_id)

        # If all corners found
        if len(corner_ids) == 4:
            break
    
    return math.prod(corner_ids)

def part_two(entries):
    pass

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 20\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = {}
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
                entries[tile_id] = tile_contents

        # Increment tile index
        tile_index = tile_index + 1
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
