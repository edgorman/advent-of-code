package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"

	"golang.org/x/exp/maps"
)

func main() {
	// Read document from local file
	document, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read document")
	}
	schematic_lines_list := strings.Split(string(document), "\n")

	// Part 1
	part1 := part1(schematic_lines_list)
	fmt.Println("Part 1", part1)

	// Part 2
	part2 := part2(schematic_lines_list)
	fmt.Println("Part 2", part2)
}

type position struct {
	x int
	y int
}

func is_number(character byte) bool {
	if _, err := strconv.Atoi(string(character)); err == nil {
		return true
	}
	return false
}

func part1(schematic_lines_list []string) int {
	// Store positions of symbols in slice
	symbol_regexp := regexp.MustCompile(`[^a-zA-Z0-9.\n]+`)
	symbol_positions := make([]position, 0)

	// Iterate over every line in schematic and search for symbols
	for y, schematic_line := range schematic_lines_list {
		symbols_indexes := symbol_regexp.FindAllStringSubmatchIndex(schematic_line, -1)

		for _, x := range symbols_indexes {
			symbol_positions = append(symbol_positions, position{x: x[0], y: y})
		}
	}

	// Store numbers by their position (to avoid duplicates)
	var part_numbers_map map[position]int = make(map[position]int)
	max_position_y, max_position_x := len(schematic_lines_list), len(schematic_lines_list[0])

	// Iterate over every symbol to search for neighbouring numbers
	for _, symbol_position := range symbol_positions {

		for y := symbol_position.y - 1; y <= symbol_position.y+1; y++ {
			for x := symbol_position.x - 1; x <= symbol_position.x+1; x++ {
				// Check if x, y is in bounds
				if !(y >= 0 && y < max_position_y && x >= 0 && x < max_position_x) {
					continue
				}

				// Check if position is not a number
				if !is_number(schematic_lines_list[y][x]) {
					continue
				}

				// Shift x to the left until start of number is reached
				x_start := x
				for x_start-1 >= 0 && is_number(schematic_lines_list[y][x_start-1]) {
					x_start -= 1
				}

				// Shift x to the right until end of number is reached
				x_end := x
				for x_end < max_position_x && is_number(schematic_lines_list[y][x_end]) {
					x_end += 1
				}

				// Add position and number to mapping
				part_numbers_map[position{x: x_start, y: y}], _ = strconv.Atoi(schematic_lines_list[y][x_start:x_end])
			}
		}
	}

	// Sum the part numbers together
	part_numbers_sum := 0
	for _, part_number := range part_numbers_map {
		part_numbers_sum += part_number
	}
	return part_numbers_sum
}

func part2(schematic_lines_list []string) int {
	// Store positions of gears in slice
	symbol_regexp := regexp.MustCompile(`(\*)`)
	symbol_positions := make([]position, 0)

	// Iterate over every line in schematic and search for symbols
	for y, schematic_line := range schematic_lines_list {
		symbols_indexes := symbol_regexp.FindAllStringSubmatchIndex(schematic_line, -1)

		for _, x := range symbols_indexes {
			symbol_positions = append(symbol_positions, position{x: x[0], y: y})
		}
	}

	// Store the sum of valid gear value ratios
	gear_values_sum := 0
	max_position_y, max_position_x := len(schematic_lines_list), len(schematic_lines_list[0])

	// Iterate over every symbol to search for neighbouring numbers
	for _, symbol_position := range symbol_positions {

		// Store gear values found in a set
		gear_values_map := make(map[int]bool)

		for y := symbol_position.y - 1; y <= symbol_position.y+1; y++ {
			for x := symbol_position.x - 1; x <= symbol_position.x+1; x++ {
				// Check if x, y is in bounds
				if !(y >= 0 && y < max_position_y && x >= 0 && x < max_position_x) {
					continue
				}

				// Check if position is not a number
				if !is_number(schematic_lines_list[y][x]) {
					continue
				}

				// Shift x to the left until start of number is reached
				x_start := x
				for x_start-1 >= 0 && is_number(schematic_lines_list[y][x_start-1]) {
					x_start -= 1
				}

				// Shift x to the right until end of number is reached
				x_end := x
				for x_end < max_position_x && is_number(schematic_lines_list[y][x_end]) {
					x_end += 1
				}

				// Add position and number to mapping
				gear_value, _ := strconv.Atoi(schematic_lines_list[y][x_start:x_end])
				gear_values_map[gear_value] = true
			}
		}

		gear_values_keys := maps.Keys(gear_values_map)

		// Only multiply gear values if there are two parts connecting it
		if len(gear_values_keys) == 2 {
			gear_values_sum += gear_values_keys[0] * gear_values_keys[1]
		}
	}

	return gear_values_sum
}
