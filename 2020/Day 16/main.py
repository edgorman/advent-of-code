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

    # Find valid fields
    valid_fields = {}

    # For each ticket index
    for ticket_index in range(len(my_ticket)):
        fields_list = []
        # For each rule
        for field, (lower, upper) in rules.items():
            in_range = True

            # For each ticket
            for ticket in valid_tickets:
                # Check if value not in bounds
                if not (lower[1] >= ticket[ticket_index] >= lower[0] or upper[1] >= ticket[ticket_index] >= upper[0]):
                    in_range = False
                    break
            
            # If field in range for index
            if in_range:
                fields_list.append(field)

        # Append list of possible fields to valid fields
        valid_fields[ticket_index] = fields_list
    
    # Find final fields
    final_fields = {}

    # For each index of the sorted valid fields
    for position in sorted(valid_fields, key=lambda f: len(valid_fields[f]), reverse=False):
        fields = valid_fields[position]
        # For each field at position
        for field in fields:
            # If not in final fields
            if field not in final_fields.values():
                final_fields[position] = field
                break
    
    # Multiply all departure values
    departure_mult = 1

    # For each field
    for position, name in final_fields.items():
        # If name contains departure
        if "departure" in name:
            departure_mult = departure_mult * my_ticket[position]
    
    return departure_mult

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
