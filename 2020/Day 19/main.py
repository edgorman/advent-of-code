import os
import re
import string

def part_one_helper(rules, index):
    rule = rules[index]

    # If next rule is letter (e.g. a)
    if len(rule) == 1 and not rule[0][0].isdigit():
        return rule[0][0]
    
    values_list = []
    # For each subrule (e.g. [4, 1, 5])
    for subrule in rule:
        values = []
        # For each rule (e.g. 4, then 1, then 5)
        for s in subrule:
            # Update values with result
            result = part_one_helper(rules, s)

            if len(values) == 0:
                values = result
            else:
                values = [a+b for a in values for b in result]
        # Append values to return list
        values_list.extend(values)

    return values_list

def part_one(rules, messages):
    start_index = "0"
    message_count = 0

    # For each sub rule
    valid_messages = part_one_helper(rules, start_index)

    # For each message
    for message in messages:
        # If message is valid
        if message in valid_messages:
            message_count = message_count + 1
    
    return message_count

def part_two_helper(rules, index):
    rule = rules[index]

    # If next rule is letter (e.g. a)
    if len(rule) == 1 and not rule[0][0].isdigit():
        return rule[0][0]
    
    ret_value = ""
    # For each subrule (e.g. [4, 1, 5])
    for subrule in rule:
        # For each rule (e.g. 4, then 1, then 5)
        for s in subrule:
            # Get result
            result = part_two_helper(rules, s)
            if "|" in result:
                result = f"({result})"
            ret_value += result
        ret_value += "|"
    ret_value = ret_value[:-1]

    return ret_value

def part_two(rules, messages):
    # Find recursive rules
    # 8 -> 42 | 42 8
    # 11 -> 42 31 | 41 11 31

    # Generate rules
    rule42 = part_two_helper(rules, "42")
    rule31 = part_two_helper(rules, "31")

    # Generate regex pattern
    pattern = (
        f"^({rule42})+"
        "("
        f"({rule42}){{1}}({rule31}){{1}}|"
        f"({rule42}){{2}}({rule31}){{2}}|"
        f"({rule42}){{3}}({rule31}){{3}}|"
        f"({rule42}){{4}}({rule31}){{4}}"
        ")$"
    )
    
    message_count = 0

    # For each message
    for message in messages:
        # If message matches regex
        if re.match(pattern, message):
            message_count = message_count + 1
    
    return message_count

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 19\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    rules = {}
    messages = []
    for entry in file_input:
        # If entry is rule
        if ':' in entry:
            key = entry[0:entry.index(':')]
            rules[key] = []

            # For each sub rule
            for subrule in entry[len(key)+2:].split(' | '):
                rules[key].append(subrule.rstrip().replace('"', '').split(' '))

        # Else entry is message
        else:
            # Ignore empty strings
            if entry.rstrip() != '':
                messages.append(entry.rstrip())

    # Part one
    print(part_one(rules, messages))

    # Update rules for part two
    rules['8'] = [['42'], ['42', '8']]
    rules['11'] = [['42', '31'], ['42', '11', '31']]

    # Part two
    print(part_two(rules, messages))
