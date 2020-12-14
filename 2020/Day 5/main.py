import os

def part_one(entries):
    highest_id = 0

    # For each boarding pass
    for seat in entries:
        bin_seat = seat

        # Convert to binary encoding
        bin_seat = bin_seat.replace('B', '1')
        bin_seat = bin_seat.replace('F', '0')
        bin_seat = bin_seat.replace('R', '1')
        bin_seat = bin_seat.replace('L', '0')

        # Calculate row, col and id
        row = int(bin_seat[0:7].encode('ascii'), 2)
        col = int(bin_seat[7:10].encode('ascii'), 2)
        seat_id = (row * 8) + col

        # Check if seat_id is new highest_id
        if seat_id > highest_id:
            highest_id = seat_id

    return highest_id

def part_two(entries):
    id_list = []

    # For each boarding pass
    for seat in entries:
        bin_seat = seat

        # Convert to binary encoding
        bin_seat = bin_seat.replace('B', '1')
        bin_seat = bin_seat.replace('F', '0')
        bin_seat = bin_seat.replace('R', '1')
        bin_seat = bin_seat.replace('L', '0')

        # Calculate row, col and id
        row = int(bin_seat[0:7].encode('ascii'), 2)
        col = int(bin_seat[7:10].encode('ascii'), 2)
        id_list.append((row * 8) + col)
    
    # Sort id list array
    id_sorted = sorted(id_list)

    # Iterate through list
    i = 0
    while i + 2 <= len(id_sorted):
        # Get ids
        left_id = id_sorted[i]
        seat_id = id_sorted[i + 1]

        # seat_id must follow left_id
        if not left_id + 1 == seat_id:
            return left_id + 1

        i = i + 1
    return None

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 5\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        entries.append(entry.rstrip())
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
