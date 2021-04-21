import os

def part_one(entries):
    password_count = 0
    lower = int(entries[0])
    upper = int(entries[1])

    # For each possible digit
    for a in range(int(entries[0][0]), 10):
        for b in range(a, 10):
            for c in range(b, 10):
                for d in range(c, 10):
                    for e in range(d, 10):
                        for f in range(e, 10):
                            # Get next value
                            value = int(str(a) + str(b) + str(c) + str(d) + str(e) + str(f))

                            # Check value is above range
                            if value > upper:
                                return password_count
                            
                            # Check value is below range
                            if value < lower:
                                continue
                                
                            # Check for no adjacent digits
                            if not (a == b or b == c or c == d or d == e or e == f):
                                continue
                            
                            password_count += 1
                                
    # Return number of possible passwords
    return password_count

def part_two(entries):
    pass

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 4\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        for range_ in entry.rstrip().split("-"):
            entries.append(range_)
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
