package main

import (
	"fmt"
	"os"
)

func main() {
	// Read input from local file
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Part 1
	part_1_result := part_1(input)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	part_2_result := part_2(input)
	fmt.Println("Part 2", part_2_result)
}

func part_1(input []byte) int {
	return 0
}

func part_2(input []byte) int {
	return 0
}
