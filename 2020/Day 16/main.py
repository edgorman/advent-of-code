import os
import re
from collections import defaultdict

def part_one(rules, nearby_tickets):
    error_rate = 0

    # For each ticket
    for ticket in nearby_tickets:
        # For each number
        for number in ticket:
            in_range = False
            # For each range in rules
            for rmin, rmax in [r for rules in rules.values() for r in rules]:
                # Check number in range
                if rmax >= number >= rmin:
                    in_range = True
                    break
            
            # If not in range
            if not in_range:
                error_rate = error_rate + number
                break

    # Return error rate
    return error_rate

def part_two(rules, my_ticket, nearby_tickets):
    # Find valid tickets
    valid_tickets = nearby_tickets.copy()

    # For each ticket
    for ticket in list(nearby_tickets):
        # For each number
        for number in ticket:
            in_range = False

            # For each range in rules
            for rmin, rmax in [r for rules in rules.values() for r in rules]:
                # Check number in range
                if rmax >= number >= rmin:
                    in_range = True
                    break
            
            # If not in range
            if not in_range:
                valid_tickets.remove(ticket)
                break
    valid_tickets.append(my_ticket)

    # Find valid field positions
    valid_fields = defaultdict(list)

    # For each rule
    for name, rang in rules.items():
        lower_min, lower_max = rang[0]
        upper_min, upper_max = rang[1]

        # For each possible ticket index
        for ticket_index in range(len(rules)):
            in_range = True

            # For each ticket
            for ticket in valid_tickets:
                # Get value at ticket index
                number = ticket[ticket_index]

                # Check number not in range
                if not(lower_max >= number >= lower_min or upper_max >= number >= upper_min):
                    in_range = False
                    break
            
            # If ticket index in range for all tickets
            if in_range:
                valid_fields[ticket_index].append(name)

    # Find final field positions
    final_fields = []

    # For each index in order of valid field counts
    for index in sorted(valid_fields, key=lambda k: len(valid_fields[k])):
        next_field = [f for f in valid_fields[index] if f not in final_fields][0]
        final_fields.append(next_field)
    
    # Calculate all departure values multiplied
    departure_mults = 1

    # For each field
    for index in range(len(final_fields)):
        # If index is a departure field
        if "departure" in final_fields[index]:
            departure_mults = departure_mults * my_ticket[index]
    
    # 5738476634257
    # 910339449193
    return departure_mults

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 16\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    rules = {}
    my_ticket = []
    my_ticket_next = False
    nearby_tickets = []
    nearby_ticket_next = False
    for entry in file_input:
        # Skip empty lines
        if len(entry.rstrip()) == 0:
            continue

        # If a rule line
        if "or" in entry:
            field = entry[0:entry.index(':')]
            values = [int(s) for s in re.findall(r'\d+', entry)]
            rules[field] = [
                (values[0], values[1]),
                (values[2], values[3])
            ]
        # Else if my ticket
        elif "your" in entry:
            my_ticket_next = True
        elif my_ticket_next:
            my_ticket_next = False
            values = [int(s) for s in re.findall(r'\d+', entry)]
            my_ticket.extend(values)
        # Else if near ticket
        elif "nearby" in entry:
            nearby_ticket_next = True
        elif nearby_ticket_next:
            values = [int(s) for s in re.findall(r'\d+', entry)]
            nearby_tickets.append(values)
    
    # Part one
    print(part_one(rules, nearby_tickets))

    # Part two
    print(part_two(rules, my_ticket, nearby_tickets))
