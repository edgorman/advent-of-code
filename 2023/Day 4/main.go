package main

import (
	"fmt"
	"math"
	"os"
	"regexp"
	"strconv"
	"strings"
)

func main() {
	// Read document from local file
	document, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read document")
	}
	card_lines_list := strings.Split(string(document), "\n")
	cards_list := parse_cards(card_lines_list)

	// Part 1
	part_1_result := part_1(cards_list)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	part_2_result := part_2(cards_list)
	fmt.Println("Part 2", part_2_result)
}

type card struct {
	index           int
	count           int
	winning_numbers map[int]bool
	elf_numbers     map[int]bool
}

func parse_cards(card_lines_list []string) []card {
	// Parse card information
	// e.g. Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
	card_list := make([]card, len(card_lines_list))

	// Define regexps
	card_index_regexp := regexp.MustCompile(`Card\s+(\d+):`)
	winning_numbers_regexp := regexp.MustCompile(`:\s+(.*?)\s+\|`)
	elf_numbers_regexp := regexp.MustCompile(`\|\s+(.*?)\s*$`)

	// Parse each card line and store in card_list
	for _, card_line := range card_lines_list {
		// Extract card index, winning numbers and elf numbers
		card_index, _ := strconv.Atoi(card_index_regexp.FindStringSubmatch(card_line)[1])
		winning_numbers_text := winning_numbers_regexp.FindStringSubmatch(card_line)[1]
		elf_numbers_text := elf_numbers_regexp.FindStringSubmatch(card_line)[1]

		// Clean up text by removing extra spaces
		winning_number_strings := strings.Split(strings.Join(strings.Fields(winning_numbers_text), " "), " ")
		elf_number_strings := strings.Split(strings.Join(strings.Fields(elf_numbers_text), " "), " ")

		// Extract winning numbers and elf numbers to slices
		winning_numbers := make(map[int]bool)
		elf_numbers := make(map[int]bool)

		for _, winning_number := range winning_number_strings {
			value, _ := strconv.Atoi(winning_number)
			winning_numbers[value] = true
		}
		for _, elf_number := range elf_number_strings {
			value, _ := strconv.Atoi(elf_number)
			elf_numbers[value] = true
		}

		// Add new card to list
		card_list[card_index-1] = card{
			index:           card_index,
			count:           1,
			winning_numbers: winning_numbers,
			elf_numbers:     elf_numbers,
		}
	}

	return card_list
}

func part_1(cards_list []card) int {
	card_points_sum := 0

	// For each card, calculate points
	for _, card := range cards_list {
		winning_cards_sum := -1

		// For each winning card, check if it's in the elfs cards
		// and if so, increment winning_cards_sum by 1
		for winning_card := range card.winning_numbers {
			if _, ok := card.elf_numbers[winning_card]; ok {
				winning_cards_sum += 1
			}
		}

		// Add 2**winning_cards_sum to the card_points_sum
		card_points_sum += int(math.Pow(2, float64(winning_cards_sum)))
	}

	return card_points_sum
}

func part_2(cards_list []card) int {
	card_counts_sum := 0

	// For each card, calculate points
	for index, card := range cards_list {
		winning_cards_sum := -1

		// For each winning card, check if it's in the elf's cards
		// and if so, increment winning_cards_sum by 1
		for winning_card := range card.winning_numbers {
			if _, ok := card.elf_numbers[winning_card]; ok {
				winning_cards_sum += 1
			}
		}

		// For each card starting at `index` and ending at `index + winning_cards_sum`
		// increment the card count of each by the count of the current card
		for increment_index := index + 1; increment_index <= index+1+winning_cards_sum; increment_index++ {
			cards_list[increment_index].count += card.count
		}

		// Increment the card counts sum by this card
		card_counts_sum += card.count
	}

	return card_counts_sum
}
