package main

import (
	"fmt"
	"os"
	"reflect"
	"strings"
)

func main() {
	// Read input from local file
	input, err := os.ReadFile("test.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse input
	pattern_list := parse_input(input)

	// Part 1
	part_1_result := part_1(pattern_list)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	part_2_result := part_2(input)
	fmt.Println("Part 2", part_2_result)
}

type pattern struct {
	grid [][]string
}

func parse_input(input []byte) []pattern {
	// Parse pattern
	// e.g. #.##..##.
	//		...
	//	    #.##.....
	input_string_list := strings.Split(string(input), "\n\n")
	pattern_list := make([]pattern, len(input_string_list))

	// Extract patterns from input strings
	for index, input_string := range input_string_list {
		pattern_string_list := strings.Split(input_string, "\n")
		pattern_line_list := make([][]string, len(pattern_string_list))
		for jndex, pattern_line := range pattern_string_list {
			pattern_line_list[jndex] = strings.Split(string(pattern_line), "")
		}
		pattern_list[index] = pattern{grid: pattern_line_list}
	}

	return pattern_list
}

func is_reflection_vertical(grid [][]string, horizontal_index int) bool {
	// If the horizontal index is 0, there is nothing to relfect
	if horizontal_index == 0 {
		return true
	}

	// Get the vertical lines from horizontal index -1 and horizontal index
	line_a := make([]string, len(grid))
	line_b := make([]string, len(grid))
	for y := 0; y < len(line_a); y++ {
		line_a[y] = grid[y][horizontal_index-1]
	}
	for y := 0; y < len(line_b); y++ {
		line_b[y] = grid[y][horizontal_index]
	}

	// If the lines are not equal, exit
	if reflect.DeepEqual(line_a, line_b) {
		return false
	}

	// Recurse by removing the vertical lines and decrementing horizontal index
	for y := 0; y < len(grid); y++ {
		grid[y] = append(grid[y][:horizontal_index-1], grid[y][horizontal_index:]...)
	}
	return is_reflection_vertical(grid, horizontal_index-1)
}

func part_1(pattern_list []pattern) int {
	fmt.Println(pattern_list)
	return 0
}

func part_2(input []byte) int {
	return 0
}
