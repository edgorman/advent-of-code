import os
from collections import defaultdict

def part_one_helper(foods):
    bad_ingredients_dict = {}
    allergen_dict = defaultdict(dict)

    # For each food
    for ingredients, allergens in foods:
        # For each allergen
        for a in allergens:
            # For each ingredient
            for i in ingredients:
                # Add ingredient to allergen dict
                if i in allergen_dict[a].keys():
                    allergen_dict[a][i] = allergen_dict[a][i] + 1
                else:
                    allergen_dict[a][i] = 1
    
    allergen_list = list(allergen_dict.keys())

    # Iterate until all allergens found
    while len(allergen_list) != 0:
        # Set bad ingredient to none
        bad_ingredients = []

        # Get next allergen
        next_allergen = allergen_list.pop(0)
        next_ingredients = allergen_dict[next_allergen]

        # Check if is last ingredient
        if len(next_ingredients) == 1:
            bad_ingredients.append(list(next_ingredients.keys())[0])
        # Else there is more than one ingredient
        else:
            # Find single ingredient that appears the most
            sort_ingredients = sorted(next_ingredients.items(), key=lambda i: i[1], reverse=True)
            if not sort_ingredients[0][1] == sort_ingredients[1][1]:
                bad_ingredients.append(sort_ingredients[0][0])

        # If found bad ingredient
        if len(bad_ingredients) == 1:
            bad_ingredients_dict[next_allergen] = bad_ingredients[0]
            # Remove entries from allergen dict
            for allergen in allergen_dict.keys():
                if bad_ingredients[0] in allergen_dict[allergen]:
                    allergen_dict[allergen].pop(bad_ingredients[0])
        # Else no bad ingredient
        else:
            allergen_list.append(next_allergen)
    
    return bad_ingredients_dict

def part_one(foods, bad_ingredients):
    ingred_count = 0

    # For each food
    for ingredients, _ in foods:
        # For each ingredient
        for i in ingredients:
            # If ingredient not in bad list
            if i not in bad_ingredients.values():
                # Increment ingredient count
                ingred_count = ingred_count + 1

    return ingred_count

def part_two(bad_ingredients):
    canon_string = ""
    for allergen in sorted(bad_ingredients.keys(), key=lambda x:x.lower()):
        ingredient = bad_ingredients[allergen]
        canon_string = canon_string + ingredient + ","
    return canon_string[:-1]

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 21\\input.txt', 'r') as file_obj:
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
    
    # Get bad ingredients
    bad_ingredients = part_one_helper(entries)
    print(bad_ingredients)

    # Part one
    print(part_one(entries, bad_ingredients))

    # Part two
    print(part_two(bad_ingredients))
