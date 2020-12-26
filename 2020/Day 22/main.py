import os
import copy

def part_one(players):
    # Iterate until one player has zero cards
    while len(players[0]) > 0 and len(players[1]) > 0:
        # Players draw top card
        card1 = players[0].pop(0)
        card2 = players[1].pop(0)

        # If player 1 has highest card
        if card1 > card2:
            players[0].append(card1)
            players[0].append(card2)
        # Else player 2 has highest card
        else:
            players[1].append(card2)
            players[1].append(card1)
    
    # Get winning player cards
    winning_cards = []
    if len(players[0]) > 0:
        winning_cards = players[0]
    else:
        winning_cards = players[1]
    
    # Calculate winning players score
    score = sum((c*m) for c, m in zip(winning_cards, range(len(winning_cards), 0, -1)))
    return score

def part_two_helper(players, seen_before):
    # Iterate until one player has zero cards
    while len(players[0]) > 0 and len(players[1]) > 0:
        # Generate hash of current hands
        players_hash = hash((tuple(players[0]), tuple(players[1])))

        # If hash has been seen before
        if players_hash in seen_before:
            return players
        else:
            seen_before.append(players_hash)
        
        # Players draw top card
        card1 = players[0].pop(0)
        card2 = players[1].pop(0)

        # Check if card values greater than player deck sizes
        if card1 < len(players[0])+1 and card2 < len(players[1])+1:
            # New game of recursive combat
            sub_players = [
                copy.deepcopy(players[0])[:card1],
                copy.deepcopy(players[1])[:card2]
            ]
            result = part_two_helper(sub_players, [])
            
            if len(result[0]) == 0:
                # Player 2 won
                players[1].append(card2)
                players[1].append(card1)
            else:
                # Player 1 won
                players[0].append(card1)
                players[0].append(card2)
        # Else if player 1 has higher card
        elif card1 > card2:
            players[0].append(card1)
            players[0].append(card2)
        # Else player 2 has higher card
        else:
            players[1].append(card2)
            players[1].append(card1)

    return players

def part_two(players):
    # Start recursive function
    result = part_two_helper(players, [])

    # Get winning player cards
    winning_cards = []
    if len(result[0]) == 0:
        winning_cards = result[1]
    else:
        winning_cards = result[0]
    
    # Calculate winning players score
    score = sum((c*m) for c, m in zip(winning_cards, range(len(winning_cards), 0, -1)))
    return score

if __name__ == "__main__":
    # Get input from txt file
    with open(os.getcwd() + '\\2020\\Day 22\\input.txt', 'r') as file_obj:
        file_input = file_obj.readlines()
    
    # Clean input
    entries = []
    cards = []
    for entry in file_input:
        if "Player" in entry:
            if len(cards) > 1:
                entries.append(cards)
            cards = []
        elif entry.rstrip().isdigit():
            cards.append(int(entry.rstrip()))
    entries.append(cards)

    # Part one
    print(part_one(copy.deepcopy(entries)))

    # Part two
    print(part_two(copy.deepcopy(entries)))
