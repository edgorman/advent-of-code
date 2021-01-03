import os
from collections import deque

def part_one_parenth_index(equation):
    count = 0
    for index in range(len(equation)):
        if equation[index] == '(':
            count = count + 1
        elif equation[index] == ')':
            count = count - 1
        if count == 0:
            return index
    return None

def part_one_helper(equation):
    # If equation is a number
    if len(equation) == 1 and equation[0].isdigit():
        return int(equation[0])
    
    # If LHS is not evaluated
    if equation[0] == '(':
        # Evaluate lhs
        idx = part_one_parenth_index(equation)
        lhs = part_one_helper(equation[1:idx])

        # Evaluate remaining
        remain = equation[idx+1:]
        remain.insert(0, str(lhs))
        return part_one_helper(remain)
    
    # If LHS starts with a number
    if equation[0].isdigit():
        lhs = int(equation[0])

        # If RHS starts with a number
        if equation[2].isdigit():
            # Evaluate lhs
            rhs = int(equation[2])

            # If operator is +
            if equation[1] == '+':
                lhs = lhs + rhs
            # Else if operator is *
            elif equation[1] == '*':
                lhs = lhs * rhs
            
            # Evaluate remaining
            remain = equation[3:]
            remain.insert(0, str(lhs))
            return part_one_helper(remain)
        # Else RHS is not evaluated
        else:
            # Evaluate rhs
            idx = part_one_parenth_index(equation[2:])
            rhs = part_one_helper(equation[3:idx+2])

            # If operator is +
            if equation[1] == '+':
                lhs = lhs + rhs
            # Else if operator is *
            elif equation[1] == '*':
                lhs = lhs * rhs

            # Evaluate remaining
            remain = equation[idx+3:]
            remain.insert(0, str(lhs))
            return part_one_helper(remain)

    return None

def part_one(equations):
    values = []

    # For each equation
    for equation in equations:
        values.append(part_one_helper(equation))
    
    # Return sum of values
    return sum(values)

def part_two_helper(equation):
    multiplier  = 1
    accumulator = 0
    
    # While equation is not empty
    while len(equation) > 0:
        # Get leftmost token
        tok = equation.popleft()

        # If its a digit
        if tok.isdigit():
            # Update accumulator using value and multiplier
            val = int(tok)
            accumulator += val * multiplier
        elif tok == '*':
            # Update multiplier with accumulator
            multiplier = accumulator
            accumulator = 0
        elif tok == '(':
            # Recurse on equation between parentheses
            val = part_two_helper(equation)
            accumulator += val * multiplier
        elif tok == ')':
            break

    return accumulator

def part_two(equations):
    values = []

    # For each equation
    for equation in equations:
        values.append(part_two_helper(deque(equation)))
    
    # Return sum of values
    return sum(values)

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 18\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        tokens = []
        for letter in entry.rstrip():
            if letter != ' ':
                tokens.append(letter)
        entries.append(tokens)

    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
