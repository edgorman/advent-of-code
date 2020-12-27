import os

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
        
    return len(flipped_tiles)

def part_two(entries):
    pass

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
    print(part_one(entries))

    # Part two
    print(part_two(entries))
