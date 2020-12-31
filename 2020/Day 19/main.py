import os
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
        return rule[0][0], []
    
    values_list = []
    recurs_list = []
    # For each subrule (e.g. [4, 1, 5])
    for subrule in rule:
        values = []
        recurs = []

        # Check if recursive rule
        if index in subrule:
            # For each rule (e.g. 4, then 1, then 5)
            for s in subrule:
                # If s is the recursive rule
                if s == index:
                    recurs = [r+"*" for r in recurs]
                # Else its a normal rule
                else:
                    val, _ = part_two_helper(rules, s)

                    if len(values) == 0:
                        values = val
                        recurs = val
                    else:
                        values = [''.join((a, b)) for a in values for b in val]
                        recurs = [''.join((a, b)) for a in recurs for b in val]
        # Else is normal value
        else:
            # For each rule (e.g. 4, then 1, then 5)
            for s in subrule:
                # Update values with result
                result, recurs = part_two_helper(rules, s)

                if len(values) == 0:
                    values = result
                else:
                    values = [''.join((a, b)) for a in values for b in result]

        # Append values and recurs to return list
        values_list.extend(values)
        recurs_list.extend(recurs)
    
    return values_list, recurs_list


def part_two(rules, messages):
    start_index = "0"
    message_count = 0

    # For each sub rule
    valid_messages, recurs_messages = part_two_helper(rules, start_index)

    # For each message
    for message in messages:
        # If message is valid
        if message in valid_messages:
            message_count = message_count + 1
            continue
        
        # For each recurs_message:
        for recurs in recurs_messages:
            idx = recurs.index('*')
            if message.startswith(recurs[0:idx]) and message.endswith(recurs[idx+1:]):
                message_count = message_count + 1
                break
    
    return message_count

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 19\\test.txt', 'r') as file_obj:
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
