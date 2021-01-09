import os
import re
import math

class Tile:
    def __init__(self, i, c):
        self.id = int(i)
        self.contents = c
        self.length = len(c)

    def __str__(self):
        return '\n'.join([row for row in self.contents])

    def get_id(self):
        return self.id
    
    def get_length(self):
        return self.length
    
    def get_top(self):
        return int(self.contents[0].replace('#','1').replace('.','0'), 2)
    
    def get_right(self):
        return int(''.join([row[-1] for row in self.contents]).replace('#','1').replace('.','0'), 2)
    
    def get_bottom(self):
        return int(self.contents[self.length-1].replace('#','1').replace('.','0'), 2)
    
    def get_left(self):
        return int(''.join([row[0] for row in self.contents]).replace('#','1').replace('.','0'), 2)
    
    def get_edges(self):
        return [
            self.get_top(),
            self.get_right(),
            self.get_bottom(),
            self.get_left()
        ]
    
    def get_all_edges(self):
        orig = self.get_edges()
        flip = [int('{:0{l}b}'.format(o, l=self.length)[::-1], 2) for o in orig]
        return [
            [orig[0], orig[1], orig[2], orig[3]],   # original
            [orig[3], orig[0], orig[1], orig[2]],   # 90 deg
            [orig[2], orig[3], orig[0], orig[1]],   # 180 deg
            [orig[1], orig[2], orig[3], orig[0]],   # 270 deg
            [flip[0], flip[1], flip[2], flip[3]],   # flip
            [flip[3], flip[0], flip[1], flip[2]],   # 90 deg
            [flip[2], flip[3], flip[0], flip[1]],   # 180 deg
            [flip[1], flip[2], flip[3], flip[0]],   # 270 deg
        ]
    
    def rotate(self):
        self.contents = [''.join(list(row)) for row in zip(*self.contents[::-1])]
    
    def flip(self):
        self.contents = [row[::-1] for row in self.contents]
    
    def strip_edges(self):
        return [row[1:self.length-1] for row in self.contents]

def part_one(tiles):
    corners = []

    # For each tile
    for tile in tiles:
        match_count = 0

        # For each edge in tile
        for edge in tile.get_edges():
            # Check against all other tiles
            for other_tile in tiles:
                # Ignore if tile == other tile
                if tile.get_id() == other_tile.get_id():
                    continue

                # For each variation of other
                for other_edge in other_tile.get_all_edges():
                    # If edges match
                    if edge in other_edge:
                        match_count = match_count + 1
                        break

        # If tile is a corner
        if match_count == 2:
            corners.append(tile)
    
    return corners

def part_two(tiles, corners):
    # Orientate corner tile
    c = corners[0]
    
    # For each orientation
    for reorientate in [c.rotate, c.rotate, c.rotate, c.flip, c.rotate, c.rotate, c.rotate]:
        edge1Found = False
        edge2Found = False
        edges = c.get_edges()

        # Check against all other tiles
        for other_tile in tiles:
            # Ignore if tile == other tile
            if c.get_id() == other_tile.get_id():
                continue

            # For each variation of other
            for other_edge in other_tile.get_all_edges():
                # If right edge matches left edge
                if edges[1] == other_edge[3]:
                    edge1Found = True
                
                # If bottom edge matches top edge
                if edges[2] == other_edge[0]:
                    edge2Found = True

        # If right and bottom edge are found
        if edge1Found and edge2Found:
            break
        
        # Reorientate tile
        reorientate()
    
    # Arrange grid
    size = int(math.sqrt(len(tiles)))
    grid = [[] for _ in range(size)]
    grid[0].append(c)
    index = 1

    # Iterate until all tiles are placed
    while index != size ** 2:
        n = tiles.pop(0)
        tileFound = False

        # For each orientation
        for reorientate in [n.rotate, n.rotate, n.rotate, n.flip, n.rotate, n.rotate, n.rotate]:
            # If next tile is on top row
            if index < size:
                p = grid[0][index-1]
                # If left edge matches right edge
                if n.get_left() == p.get_right():
                    tileFound = True
            # Else next tile is on lower row
            else:
                p = grid[math.floor(index / size)-1][index % size]
                # If top edge matches bottom edge
                if n.get_top() == p.get_bottom():
                    tileFound = True
            
            # If next tile is found
            if tileFound:
                break
            
            # Reorientate tile
            reorientate()

        # If next tile fits
        if tileFound:
            grid[math.floor(index / size)].append(n)
            index = index + 1
        # Else
        else:
            tiles.append(n)

    # Construct grid

    # Count number of sea monsters
    
    return None

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 20\\test.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    tiles = []
    for entry in file_input:
        if entry.rstrip() == "":
            tiles.append(Tile(tile_id, tile_contents))
        elif "Tile" in entry.rstrip():
            tile_id = re.findall(r'\d+', entry)[0]
            tile_contents = []
        else:
            tile_contents.append(entry.rstrip())
    
    # Part one
    corners = part_one(tiles)
    print(math.prod(c.get_id() for c in corners))

    # Part two
    print(part_two(tiles, corners))
