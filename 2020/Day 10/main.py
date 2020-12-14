import os

def part_one(entries):
    curr_adapter = 0
    next_adapter = 0

    # Sort the adapters in ascending order
    adapters = sorted(entries)
    final_adapter = adapters[len(adapters) - 1]

    # Keep count of one jolt and three jolt
    one_jolt_count = 0
    three_jolt_count = 1

    # Iterate until current adapter reaches final adapter
    while not curr_adapter == final_adapter:
        # Assign the next adapter
        if curr_adapter == 0:
            next_adapter = adapters[0]
        else:
            next_adapter = adapters[adapters.index(curr_adapter) + 1]
        
        # Check if next adapter is within 1, 2 or 3 jumps
        if next_adapter - curr_adapter == 1:
            one_jolt_count = one_jolt_count + 1
            curr_adapter = next_adapter
        elif next_adapter - curr_adapter == 2:
            curr_adapter = next_adapter
        elif next_adapter - curr_adapter == 3:
            three_jolt_count = three_jolt_count + 1
            curr_adapter = next_adapter
        # Else not possible to visit all adapters
        else:
            return None

    return one_jolt_count * three_jolt_count

def part_two(entries):
    combo_count = 0

    # Store combinations and start point
    # (adaptaver value, path length, identical situation count)
    combinations = []
    combinations.append((0, 0, 1))

    # Sort the adapters in ascending order
    adapters = sorted(entries)
    final_adapter = adapters[len(adapters) - 1]

    # Iterate until combinations is empty
    while not len(combinations) == 0:
        curr_adapter, path_len, ident_count = combinations.pop(0)

        # Check if adapter has reached the end
        if curr_adapter == final_adapter:
            combo_count = combo_count + ident_count
            continue
        
        # Assign the next adapter
        if curr_adapter == 0:
            next_adapter = adapters[0]
        else:
            next_adapter = adapters[adapters.index(curr_adapter) + 1]

        # Find available moves from adapter
        while next_adapter - curr_adapter <= 3:
            combo = (next_adapter, path_len + 1, ident_count)

            # Check if combination exists
            if (combo[0], combo[1]) in [(a,b) for a, b, _ in combinations]:
                _, _, c3 = combinations.pop([(a,b) for a, b, _ in combinations].index((combo[0], combo[1])))
                combo = (combo[0], combo[1], combo[2] + c3)

            # Add combination
            combinations.append(combo)

            # Check if adapter has reached the end
            if next_adapter == final_adapter:
                break

            # Assign next adapter
            next_adapter = adapters[adapters.index(next_adapter) + 1]
    
    return combo_count

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 10\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(int(entry.rstrip()))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
