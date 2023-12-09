package main

import (
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
	history_list := parse_input(input)

	// Part 1
	part_1_result := part_1(history_list)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	part_2_result := part_2(history_list)
	fmt.Println("Part 2", part_2_result)
}

func parse_input(input []byte) [][]int {
	// Parse a list of histories
	// e.g. 0 3 6 9 12 15
	input_strings_list := strings.Split(string(input), "\n")

	// Extract each history to a slice
	history_list := make([][]int, len(input_strings_list))

	// For each history
	for index, input_string := range input_strings_list {

		// Convert each value to int and store in slice
		history_int_list := make([]int, len(strings.Split(input_string, " ")))
		for jndex, input_value := range strings.Split(input_string, " ") {
			history_int_list[jndex], _ = strconv.Atoi(input_value)
		}
		history_list[index] = history_int_list

	}

	return history_list
}

const (
	EXTRAPOLATE_FORWARDS  int = 0
	EXTRAPOLATE_BACKWARDS int = 1
)

func extrapolate_history(history []int, extrapolate_direction int) int {
	differences := make([]int, len(history)-1)
	contains_non_zero_value := false

	// Calculate differences between history values
	for index := 0; index < len(differences); index++ {
		difference := history[index+1] - history[index]
		differences[index] = difference

		if difference != 0 {
			contains_non_zero_value = true
		}
	}

	// If differences are not all 0
	if !contains_non_zero_value {
		return 0
	}

	// Calculate the extrapolated value for history
	if extrapolate_direction == EXTRAPOLATE_FORWARDS {
		return differences[len(differences)-1] + extrapolate_history(differences, extrapolate_direction)
	} else {
		return differences[0] - extrapolate_history(differences, extrapolate_direction)
	}
}

func part_1(history_list [][]int) int {
	extrapolated_values_sum := 0

	// For each history, recursively extrapolate and add value to sum
	for _, history := range history_list {
		extrapolated_values_sum += history[len(history)-1] + extrapolate_history(history, EXTRAPOLATE_FORWARDS)
	}

	return extrapolated_values_sum
}

func part_2(history_list [][]int) int {
	extrapolated_values_sum := 0

	// For each history, recursively extrapolate and add value to sum
	for _, history := range history_list {
		extrapolated_values_sum += history[0] - extrapolate_history(history, EXTRAPOLATE_BACKWARDS)
	}

	return extrapolated_values_sum
}
