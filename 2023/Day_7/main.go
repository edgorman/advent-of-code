package main

import (
	"container/list"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	// Read input from local file
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse input
	hand_list := parse(input)

	// Part 1
	face_rankings := map[string]int{"A": 0, "K": 1, "Q": 2, "J": 3, "T": 4, "9": 5, "8": 6, "7": 7, "6": 8, "5": 9, "4": 10, "3": 11, "2": 12}
	part_1 := part1(hand_list, face_rankings)
	fmt.Println("Part 1", part_1)

	// Update the hand ranking with jokers
	hand_list = update_rankings(hand_list)

	// Part 2
	face_rankings["J"] = 13
	part_2 := part1(hand_list, face_rankings)
	fmt.Println("Part 2", part_2)
}

const (
	// Hand types ordered by importance
	// where 0 > 1 > ...
	FIVE_OF_A_KIND  int = 0
	FOUR_OF_A_KIND  int = 1
	FULL_HOUSE      int = 2
	THREE_OF_A_KIND int = 3
	TWO_PAIR        int = 4
	ONE_PAIR        int = 5
	HIGH_CARD       int = 6
)

type hand struct {
	value string
	type_ int
	bid   int
}

func parse(input []byte) []hand {
	// Convert byte data to list of input lines
	input_lines := strings.Split(string(input), "\n")

	// Store hands in a slice
	hand_list := make([]hand, 0)

	// Extract hand from lines
	for _, line := range input_lines {
		line_values := strings.Split(line, " ")

		value := line_values[0]
		type_ := parse_type(value)
		bid, _ := strconv.Atoi(line_values[1])

		hand_list = append(
			hand_list,
			hand{
				value: value,
				type_: type_,
				bid:   bid,
			},
		)
	}

	return hand_list
}

func parse_type(hand_value string) int {
	faces_mapping := make(map[string]int)
	type_mappings := make(map[int]int)

	// For each face in the hand value, store value in map
	for _, face := range strings.Split(hand_value, "") {
		faces_mapping[face] += 1
	}

	// For each face mapping, store type in map
	for _, count := range faces_mapping {
		type_mappings[count] += 1
	}

	// Check for hand types in order of importance
	if _, ok := type_mappings[5]; ok {
		return FIVE_OF_A_KIND
	}
	if _, ok := type_mappings[4]; ok {
		return FOUR_OF_A_KIND
	}
	if _, ok := type_mappings[3]; ok {
		if _, ok := type_mappings[2]; ok {
			return FULL_HOUSE
		}
	}
	if _, ok := type_mappings[3]; ok {
		return THREE_OF_A_KIND
	}
	if _, ok := type_mappings[2]; ok {
		if type_mappings[2] == 2 {
			return TWO_PAIR
		}
		return ONE_PAIR
	}

	return HIGH_CARD
}

func is_hand_better(hand hand, other_hand hand, face_rankings map[string]int) bool {
	// If either hand is better, return true/false
	if hand.type_ != other_hand.type_ {
		return hand.type_ < other_hand.type_
	}

	// Both hand types are equal, return which has highest first card
	for index := 0; index < len(hand.value); index++ {
		hand_face_ranking := face_rankings[string(hand.value[index])]
		other_hand_face_ranking := face_rankings[string(other_hand.value[index])]

		if hand_face_ranking == other_hand_face_ranking {
			continue
		}

		return hand_face_ranking < other_hand_face_ranking
	}

	fmt.Println("SHOULD NOT BE HERE", hand.value, other_hand.value)
	return false
}

func update_rankings(hand_list []hand) []hand {
	type key struct {
		card_type   int
		joker_count int
	}

	// Mapping of type+joker count -> new type
	rankings_mapping := map[key]int{
		{1, 1}: 0, // four of a kind
		{1, 4}: 0,
		{2, 1}: 1, // full house
		{2, 2}: 0,
		{2, 3}: 0,
		{3, 1}: 1, // three of a kind
		{3, 3}: 1,
		{4, 2}: 1, // two pair
		{4, 1}: 2,
		{5, 1}: 3, // one pair
		{5, 2}: 3,
		{6, 1}: 5, // high card
	}

	// Update each hand
	for index := 0; index < len(hand_list); index++ {

		// Count number of jokers in hand
		joker_count := 0
		for _, face := range hand_list[index].value {
			if string(face) == "J" {
				joker_count += 1
			}
		}

		// If key exists in rankings mapping, update type
		k := key{card_type: hand_list[index].type_, joker_count: joker_count}
		if _, ok := rankings_mapping[k]; ok {
			hand_list[index].type_ = rankings_mapping[k]
		}

	}

	return hand_list
}

func part1(hand_list []hand, face_rankings map[string]int) int {
	// Store hand ranks in a mapping, starting with first hand in hand_list
	hand_linked_list := list.New()

	// For each hand
	for _, current_hand := range hand_list {

		has_inserted := false

		// For each other hand in hand_rank_mapping
		for next_hand := hand_linked_list.Front(); next_hand != nil; next_hand = next_hand.Next() {

			// If current hand is better than next hand, insert before
			next_hand_value := next_hand.Value.(hand)
			if is_hand_better(current_hand, next_hand_value, face_rankings) {
				hand_linked_list.InsertBefore(current_hand, next_hand)
				has_inserted = true
				break
			}

		}

		// Else insert current hand at end of the list
		if !has_inserted {
			hand_linked_list.PushBack(current_hand)
		}

	}

	// Calculate sum of winnings from hand bids
	winnings_sum := 0
	rank_index := len(hand_list)

	// For each hand, multiply by rank index and decrement rank index
	for next_hand := hand_linked_list.Front(); next_hand != nil; next_hand = next_hand.Next() {
		winnings_sum += next_hand.Value.(hand).bid * rank_index
		rank_index -= 1
	}

	return winnings_sum
}
