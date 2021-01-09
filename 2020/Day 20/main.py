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
    
    def get_nth_row(self, n):
        return self.contents[n]
    
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
    
    def find_monsters(self):
        """ Monster:
                              # 
            #    ##    ##    ###
             #  #  #  #  #  #   
        """
        count = 0

        for y in range(1, self.length-1):
            for x in range(self.length-19):
                if self.contents[y-1][x+18] == '.': continue

                if self.contents[y][x] == '.': continue
                if self.contents[y][x+5] == '.': continue
                if self.contents[y][x+6] == '.': continue
                if self.contents[y][x+11] == '.': continue
                if self.contents[y][x+12] == '.': continue
                if self.contents[y][x+17] == '.': continue
                if self.contents[y][x+18] == '.': continue
                if self.contents[y][x+19] == '.': continue

                if self.contents[y+1][x+1] == '.': continue
                if self.contents[y+1][x+4] == '.': continue
                if self.contents[y+1][x+7] == '.': continue
                if self.contents[y+1][x+10] == '.': continue
                if self.contents[y+1][x+13] == '.': continue
                if self.contents[y+1][x+16] == '.': continue

                count = count + 1
        
        return count
    
    def get_rough_water(self):
        count = 0

        for y in range(self.length):
            count = count + self.contents[y].count('#')
        
        return count

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
    
    # TODO: corner orientation matters alot, to generate solution requires extra rotations
    # input.txt -> c.flip()
    # test.txt -> c.rotate() and c.flip()
    # Uncomment as needed below:
    # c.rotate()
    c.flip()
    
    # Arrange grid
    size = int(math.sqrt(len(tiles)))
    grid = [[] for _ in range(size)]
    grid[0].append(c)
    index = 1

    # Iterate until all tiles are placed
    while index != size ** 2:
        n = tiles.pop(0)
        x = index % size
        y = math.floor(index / size)
        tileFound = False

        # For each orientation
        for reorientate in [n.rotate, n.rotate, n.rotate, n.flip, n.rotate, n.rotate, n.rotate]:
            # If can check top and left
            if x > 0 and y > 0:
                t = grid[y-1][x]
                l = grid[y][x-1]

                # Check edges match
                if n.get_top() == t.get_bottom() and n.get_left() == l.get_right():
                    tileFound = True
            # Else if can check top
            elif y > 0:
                t = grid[y-1][x]
                # Check edges match
                if n.get_top() == t.get_bottom():
                    tileFound = True
            # Else if can check left
            elif x > 0:
                l = grid[y][x-1]
                # Check edges match
                if n.get_left() == l.get_right():
                    tileFound = True
            
            # If next tile is found
            if tileFound:
                break
            
            # Reorientate tile
            reorientate()

        # If next tile fits
        if tileFound:
            grid[y].append(n)
            index = index + 1
        # Else
        else:
            tiles.append(n)

    # Construct image
    image = ""

    # For each row of ids
    for row in grid:
        # For each inner row
        for i in range(1, row[0].get_length()-1):
            # For each tile in row
            for tile in row:
                image = image + tile.get_nth_row(i)[1:-1]
            image = image + "\n"
    
    image = image[:-1]
    o = Tile("1", image.split("\n"))

    # Count number of sea monsters
    monster_count = 0
    for reorientate in [o.rotate, o.rotate, o.rotate, o.flip, o.rotate, o.rotate, o.rotate]:
        # Find monsters
        monster_count = monster_count + o.find_monsters()

        # Reorientate
        reorientate()
    
    # Return number of rough monsters that aren't monsters
    return o.get_rough_water() - (monster_count * 15)

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 20\\input.txt', 'r') as file_obj:
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
