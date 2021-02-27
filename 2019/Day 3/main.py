import os

def part_one_helper(position, direction, value):
    if direction == 'U':
        return (position[0] - value, position[1])
    if direction == 'R':
        return (position[0], position[1] + value)
    if direction == 'D':
        return (position[0] + value, position[1])
    if direction == 'L':
        return (position[0], position[1] - value)

def part_one(wire_paths):
    # Find list of vertexes per wire
    vertexes_list = []
    
    # For each wire's path
    for path in wire_paths:
        # Initialise wire
        position = (0, 0)
        vertexes = []

        # For each direction in path
        for direction, value in path:
            # Calculate new position
            position = part_one_helper(
                position,
                direction,
                value
            )

            # Add line to this list
            vertexes.append(position)

        # Append lines to the list
        vertexes_list.append(vertexes)
    
    # Find the crossing points of the wire
    crossing_points = set()
    crossing_points.add((0, 0))

    u_last = (0, 0)
    # For each vertex in the first wire
    for u in vertexes_list[0]:

        v_last = (0, 0)
        # For each vertex in the second wire
        for v in vertexes_list[1]:
            # If u is vertical
            if u[0] == u_last[0]:
                # If v is horizontal
                if v[1] == v_last[1]:
                    # Check v within y bounds of u
                    if u[1] <= v[1] <= u_last[1] or u_last[1] <= v[1] <= u[1]:
                        # Check u within x bounds of v
                        if v[0] <= u[0] <= v_last[0] or v_last[0] <= u[0] <= v[0]:
                            # Crossing at ux, vy
                            crossing_points.add((u[0], v[1]))

            # Else u is horizontal
            else:
                # If v is vertical
                if v[0] == v_last[0]:
                    # Check v within x bounds of u
                    if u[0] <= v[0] <= u_last[0] or u_last[0] <= v[0] <= u[0]:
                        # Check u within y bounds of v
                        if v[1] <= u[1] <= v_last[1] or v_last[1] <= u[1] <= v[1]:
                            # Crossing at vx, uy
                            crossing_points.add((v[0], u[1]))
            
            # Update last u and v positions
            v_last = v
        u_last = u

    # Remove 0,0 crossing point
    crossing_points.remove((0, 0))

    # Return shortest crossing point from origin using manhatten distance
    return sorted([abs(x) + abs(y) for x, y in crossing_points])[0]

def part_two_helper(a, a_last):
    return abs(a[0] - a_last[0]) + abs(a[1] - a_last[1])

def part_two(wire_paths):
     # Find list of vertexes per wire
    vertexes_list = []
    
    # For each wire's path
    for path in wire_paths:
        # Initialise wire
        position = (0, 0)
        vertexes = []

        # For each direction in path
        for direction, value in path:
            # Calculate new position
            position = part_one_helper(
                position,
                direction,
                value
            )

            # Add line to this list
            vertexes.append(position)

        # Append lines to the list
        vertexes_list.append(vertexes)
    
    # Find the crossing points of the wire
    crossing_points = list()

    u_last = (0, 0)
    u_hist = []
    # For each vertex in the first wire
    for u in vertexes_list[0]:
        # Add value of direction to history
        u_hist.append(part_two_helper(u, u_last))

        v_last = (0, 0)
        v_hist = []
        # For each vertex in the second wire
        for v in vertexes_list[1]:
            # Add value of direction to history
            v_hist.append(part_two_helper(v, v_last))

            # If u is vertical
            if u[0] == u_last[0]:
                # If v is horizontal
                if v[1] == v_last[1]:
                    # Check v within y bounds of u
                    if u[1] <= v[1] <= u_last[1] or u_last[1] <= v[1] <= u[1]:
                        # Check u within x bounds of v
                        if v[0] <= u[0] <= v_last[0] or v_last[0] <= u[0] <= v[0]:
                            # Crossing at ux, vy
                            crossing_points.append(
                                (
                                    u_hist[:-1] + [part_two_helper((u[0], v[1]), u_last)], 
                                    v_hist[:-1] + [part_two_helper((u[0], v[1]), v_last)]
                                )
                            )

            # Else u is horizontal
            else:
                # If v is vertical
                if v[0] == v_last[0]:
                    # Check v within x bounds of u
                    if u[0] <= v[0] <= u_last[0] or u_last[0] <= v[0] <= u[0]:
                        # Check u within y bounds of v
                        if v[1] <= u[1] <= v_last[1] or v_last[1] <= u[1] <= v[1]:
                            # Crossing at vx, uy
                            crossing_points.append(
                                (
                                    u_hist[:-1] + [part_two_helper((v[0], u[1]), u_last)], 
                                    v_hist[:-1] + [part_two_helper((v[0], u[1]), v_last)]
                                )
                            )

            # Update last u and v positions
            v_last = v
        u_last = u
    
    # Find shortest crossing point from origin using length of wires to that point
    crossing_distances = sorted([sum(x) + sum(y) for x, y in crossing_points])

    # Remove 0,0 crossing point
    if 0 in crossing_distances:
        crossing_distances.remove(0)
    
    return crossing_distances[0]

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 3\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        moves = []
        for move in entry.rstrip().split(','):
            moves.append((move[0], int(move[1:])))

        entries.append(moves)
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
