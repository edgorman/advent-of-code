"""
--- Day X: Template ---
Challenge description here
"""

import os
import re

def part_one(entries):
    valid_passports = []
    all_keys = ['byr', 'iyr', 'eyr', 'hgt', 'hcl', 'ecl', 'pid', 'cid']
    req_keys = all_keys.copy()
    req_keys.remove('cid')

    # For each passport
    for e in entries:
        passport_keys = e.keys()

        # If passport has all keys
        if len(passport_keys) == len(all_keys):
            valid_passports.append(e)
            continue
        
        # If passport has required keys and not optional keys
        if len(passport_keys) == len(req_keys) and 'cid' not in passport_keys:
            valid_passports.append(e)
            continue

    return valid_passports

def part_two_helper(p):
    # Return True if valid, False if not
    for key, value in p.items():
        if key == 'byr':
            # Length must be four digits
            if len(value) != 4:
                return False
            # Must be between 1920 and 2002
            if not (2002 >= int(value) >= 1920):
                return False
        elif key == 'iyr':
            # Length must be four digits
            if len(value) != 4:
                return False
            # Must be between 2010 and 2020
            if not (2020 >= int(value) >= 2010):
                return False
        elif key == 'eyr':
            # Length must be four digits
            if len(value) != 4:
                return False
            # Must be between 2020 and 2030
            if not (2030 >= int(value) >= 2020):
                return False
        elif key == 'hgt':
            # If contains cm
            if 'cm' in value:
                # Must be between 150 and 193
                if not (193 >= int(value.replace('cm', '')) >= 150):
                    return False
            elif 'in' in value:
                # Must be between 59 and 76
                if not (76 >= int(value.replace('in', '')) >= 59):
                    return False
            # Must contain cm/in
            else:
                return False
        elif key == 'hcl':
            # Must match reg exp
            if not re.match('^#[A-Fa-f0-9]{6}$', value):
                return False
        elif key == 'ecl':
            # Must be in list of colours
            if not value in ['amb', 'blu', 'brn', 'gry', 'grn', 'hzl', 'oth']:
                return False
        elif key == 'pid':
            # Length must be 9 digits
            if len(value) != 9:
                return False
    
    return True

def part_two(entries):
    valid_count = 0

    # Use part_one so we know all entries have required keys
    passports = part_one(entries)

    # For each passport
    for p in passports:
        # Check if passport is valid
        if part_two_helper(p):
            # Increment valid count
            valid_count = valid_count + 1
    
    return valid_count

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 4\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    passport = dict()
    for entry in file_input:
        # If starting a new passport
        if entry == "\n":
            entries.append(passport)
            passport = dict()
            continue
        
        # Get key value information for passport
        tokens = entry.split(' ')
        for t in tokens:
            key = t.split(':')[0].rstrip()
            value = t.split(':')[1].rstrip()
            passport[key] = value
    # Add the final passport
    entries.append(passport)
    
    # Part one
    print(len(part_one(entries)))

    # Part two
    print(part_two(entries))
