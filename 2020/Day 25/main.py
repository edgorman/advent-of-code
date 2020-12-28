import os

def part_one(pub_keys):
    subject_number = 7

    # Iterate until found
    while True:
        loop_count = 0
        curr_value = 1
        # Loop until value greater than max pub key
        while not loop_count > 20201227:
            curr_value = curr_value * subject_number
            curr_value = curr_value % 20201227
            loop_count = loop_count + 1
            
            # If found a pub key
            if curr_value in pub_keys:
                break

        # If found all pub keys
        if curr_value in pub_keys:
            break
        # Else try new subject number
        else:
            subject_number = subject_number + 1
    
    # Calculate encryption key
    encr_value = 1
    oppos_key = len(pub_keys)-1-pub_keys.index(curr_value)
    for _ in range(loop_count):
        encr_value = encr_value * pub_keys[oppos_key]
        encr_value = encr_value % 20201227
    return encr_value

def part_two(entries):
    pass

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 25\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(int(entry.rstrip()))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
