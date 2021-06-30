import os

def part_one(input_, width, height):
    layer_length = width * height
    lowest_zero_score = 0
    lowest_zero_count = layer_length

    # For each layer in input
    for index in range(0, len(input_), layer_length):
        # Ignore layers which are incomplete
        if index + layer_length > len(input_):
            continue
        layer = input_[index:index + layer_length]
        zero_count = layer.count("0")

        # If zero digits is the lowest we've seen
        if zero_count < lowest_zero_count:
            lowest_zero_count = zero_count

            # Update score
            lowest_zero_score = layer.count("1") * layer.count("2")

    return lowest_zero_score

def part_two(input_, width, height):
    layer_length = width * height
    output = list(input_[0:layer_length])

    # For each layer in input
    for index in range(0, len(input_), layer_length):
        # Ignore layers which are incomplete
        if index + layer_length > len(input_):
            continue
        layer = input_[index:index + layer_length]

        # Update output pixels
        for jndex in range(0, len(output)):
            new_pixel = layer[jndex]
            old_pixel = output[jndex]

            if old_pixel == "2":
                output[jndex] = new_pixel
    output = "".join(output)

    # Format output according to width
    formatted_output = "\n".join(
        [output[i:i+width] for i in range(0, len(output), width)]
    )
    return formatted_output

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2019\\Day 8\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    input_ = str(file_input[0])
    
    # Part one
    print(part_one(input_, 25, 6))

    # Part two
    print(part_two(input_, 25, 6))
    
