import os
from collections import defaultdict

def part_one(foods):
    temp = defaultdict(dict)

    for ingredients, allergens in foods:
        for a in allergens:
            for i in ingredients:
                if i in temp[a].keys():
                    temp[a][i] = temp[a][i] + 1
                else:
                    temp[a][i] = 1

    return temp

def part_two(entries):
    pass

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 21\\test.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    for entry in file_input:
        food = entry.rstrip().split(' (contains ')
        ingredients = food[0].split(' ')
        allergens = food[1][:-1].split(', ')

        entries.append((
            ingredients,
            allergens
        ))
    
    # Part one
    print(part_one(entries))

    # Part two
    print(part_two(entries))
