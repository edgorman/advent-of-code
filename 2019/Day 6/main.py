import os

def part_one(orbits):
    # Count the number of direct orbits
    direct_count = len(orbits.keys())

    # Count the number of indirect orbits
    indirect_count = 0

    for obj in orbits.keys():
        parent = orbits[obj]

        # Count how many orbits until COM
        while parent != "COM":
            parent = orbits[parent]
            indirect_count += 1
        
    # Return total number of orbits
    return direct_count + indirect_count

def part_two(orbits):
    # Generate list of parent objects for YOU and SAN
    you_parents = [orbits["YOU"]]
    while you_parents[0] != "COM": you_parents.insert(0, orbits[you_parents[0]])
    san_parents = [orbits["SAN"]]
    while san_parents[0] != "COM": san_parents.insert(0, orbits[san_parents[0]])

    # Find the closest parent for both YOU and SAN
    closest_parent = "COM"
    for yp, sp in zip(you_parents, san_parents):
        if yp != sp:
            break
        closest_parent = yp
    
    # Calculate how many transfers to closest parent
    you_transfers = you_parents.index(closest_parent) + 1
    san_transfers = san_parents.index(closest_parent) + 1

    # Return minimum number of transfers    
    return (len(you_parents) - you_transfers) + (len(san_parents) - san_transfers)

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 6\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = {}
    for entry in file_input:
        obj, orb = tuple(entry.rstrip().split(')'))
        entries[orb] = obj
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
