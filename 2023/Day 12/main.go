package main

import (
	"fmt"
	"os"
	"reflect"
	"strconv"
	"strings"
)

func main() {
	// Read input from local file
	input, err := os.ReadFile("test.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse input
	record_list := parse_input(input)

	// Part 1
	part_1_result := part_1(record_list)
	fmt.Println("Part 1", part_1_result)

	// Update input
	record_list = expand_records(record_list, 5)

	// Part 2
	part_2_result := part_1(record_list)
	fmt.Println("Part 2", part_2_result)
}

type record struct {
	value_list          []string
	damaged_spring_list []int
}

func parse_input(input []byte) []record {
	// Parse list of records
	// e.g. #.#.### 1,1,3
	input_strings_list := strings.Split(string(input), "\n")

	// Convert each line to list of records
	record_list := make([]record, len(input_strings_list))
	for index, input_string := range input_strings_list {
		// Extract values and damaged springs
		value_string := strings.Split(input_string, " ")[0]
		damaged_spring_string := strings.Split(input_string, " ")[1]

		// Convert both to lists
		value_list := strings.Split(value_string, "")
		damaged_spring_list := make([]int, len(strings.Split(damaged_spring_string, ",")))
		for jndex, damaged_spring := range strings.Split(damaged_spring_string, ",") {
			damaged_spring_list[jndex], _ = strconv.Atoi(damaged_spring)
		}

		// Insert new record instance to list
		record_list[index] = record{
			value_list:          value_list,
			damaged_spring_list: damaged_spring_list,
		}
	}

	return record_list
}

func expand_records(input_list []record, repeat_count int) []record {
	new_input_list := make([]record, len(input_list))

	// For each existing record input
	for index, input := range input_list {

		// Create a new version where each value is repeated n times
		new_record := record{}
		for jndex := 0; jndex < repeat_count; jndex += 1 {
			new_record.value_list = append(new_record.value_list, input.value_list...)
			new_record.damaged_spring_list = append(new_record.damaged_spring_list, input.damaged_spring_list...)
		}
		new_input_list[index] = new_record

	}

	return new_input_list
}

func count_valid_arrangements(input record) int {
	// Check if this is a valid arrangement
	last_damaged_spring_index := -1
	damaged_spring_list := make([]int, 0)
	for index, value := range input.value_list {
		if value == "#" {
			if last_damaged_spring_index == -1 {
				last_damaged_spring_index = index
			}
		} else if last_damaged_spring_index != -1 {
			damaged_spring_list = append(damaged_spring_list, index-last_damaged_spring_index)
			last_damaged_spring_index = -1
		}
	}
	if last_damaged_spring_index != -1 {
		damaged_spring_list = append(damaged_spring_list, len(input.value_list)-last_damaged_spring_index)
		last_damaged_spring_index = -1
	}

	if reflect.DeepEqual(damaged_spring_list, input.damaged_spring_list) {
		return 1
	}

	// Check if input contains unknown
	first_unknown_index := -1
	for index, value := range input.value_list {
		if value == "?" {
			first_unknown_index = index
			break
		}
	}

	if first_unknown_index == -1 {
		return 0
	}

	// Else recurse on the next unknown index
	input_a := record{}
	input_a.value_list = append(input_a.value_list, input.value_list...)
	input_a.damaged_spring_list = append(input_a.damaged_spring_list, input.damaged_spring_list...)
	input_a.value_list[first_unknown_index] = "."

	input_b := record{}
	input_b.value_list = append(input_b.value_list, input.value_list...)
	input_b.damaged_spring_list = append(input_b.damaged_spring_list, input.damaged_spring_list...)
	input_b.value_list[first_unknown_index] = "#"

	return count_valid_arrangements(input_a) + count_valid_arrangements(input_b)
}

func part_1(record_list []record) int {
	// Store sum of valid arrangements
	valid_arrangements_sum := 0

	// For each record, check each arrangement is valid
	for _, record := range record_list {
		valid_arrangements_sum += count_valid_arrangements(record)
	}

	// Return the sum of valid arrangements
	return valid_arrangements_sum
}
