import os

def part_one(entries, nth):
    numbers = entries.copy()
    word_count = len(numbers) + 1
    last_number = numbers[len(numbers) - 1]

    # Iterate until nth number spoken
    while word_count <= nth:
        # If last number said before
        if last_number in numbers[:-1]:
            last_number = numbers[:-1][::-1].index(last_number) + 1
        # Else number is new
        else:
            last_number = 0
        
        # Add last number to list
        numbers.append(last_number)

        # Increment words spoken
        word_count = word_count + 1

    return last_number

def part_two(entries, nth):
    spoken_dict = {}
    spoken = 0
    last_spoken = None
    word_count = 1

    # Add initial values to dict
    for e in entries:
        spoken = e
        if last_spoken is not None:
            spoken_dict[last_spoken] = word_count - 1
        word_count = word_count + 1
        last_spoken = spoken
    spoken_dict[last_spoken] = word_count - 1

    # Iterate until nth number spoken
    while word_count <= nth:
        # If last number said before
        if last_spoken in spoken_dict:
            spoken = word_count - 1 - spoken_dict[last_spoken]
        # Else number is new
        else:
            spoken = 0
        
        # Update values
        spoken_dict[last_spoken] = word_count - 1
        word_count = word_count + 1
        last_spoken = spoken

    return last_spoken

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 15\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        for e in entry.split(','):
            entries.append(int(e.rstrip()))
    
    # Part one
    print(part_one(entries, 2020))

    # Part two
    print(part_two(entries, 30000000))
