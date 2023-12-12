package main

import (
	"fmt"
	"math"
	"os"
	"strings"
)

func main() {
	// Read input from local file
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse input
	galaxy_positions, axes_with_galaxies_map := parse_input(input)

	// Part 1
	empty_axes_growth := 2
	part_1_result := part_1(galaxy_positions, axes_with_galaxies_map, empty_axes_growth)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	empty_axes_growth = 1000000
	part_2_result := part_1(galaxy_positions, axes_with_galaxies_map, empty_axes_growth)
	fmt.Println("Part 2", part_2_result)
}

type position struct {
	x int
	y int
}

func parse_input(input []byte) ([]position, map[string]bool) {
	// Parse the map of galaxies
	input_strings_list := strings.Split(string(input), "\n")

	// Find the galaxy positions and rows/columns with galaxies
	galaxy_positions := make([]position, 0)
	axes_with_galaxies_map := make(map[string]bool)

	// For each position in space
	for y := 0; y < len(input_strings_list); y++ {
		for x := 0; x < len(input_strings_list[y]); x++ {

			// If that position contains a galaxy, add to list of galaxy positions
			// And set the axes to contain a galaxy value
			if string(input_strings_list[y][x]) == "#" {
				galaxy_positions = append(galaxy_positions, position{x: x, y: y})
				axes_with_galaxies_map[fmt.Sprintf("y%v", y)] = true
				axes_with_galaxies_map[fmt.Sprintf("x%v", x)] = false
			}
		}
	}

	return galaxy_positions, axes_with_galaxies_map
}

func part_1(galaxy_positions []position, axes_with_galaxies_map map[string]bool, empty_axes_growth int) int {
	// Store sum of shortest paths between all galaxy pairs
	shortest_path_sum := 0

	// Iterate over every galaxy and it's pair galaxy once
	for index := 0; index <= len(galaxy_positions)-2; index++ {
		for jndex := index + 1; jndex <= len(galaxy_positions)-1; jndex++ {

			// Get the galaxies at these indexes
			galaxy_a, galaxy_b := galaxy_positions[index], galaxy_positions[jndex]
			min_x := int(math.Min(float64(galaxy_a.x), float64(galaxy_b.x)))
			min_y := int(math.Min(float64(galaxy_a.y), float64(galaxy_b.y)))
			max_x := int(math.Max(float64(galaxy_a.x), float64(galaxy_b.x)))
			max_y := int(math.Max(float64(galaxy_a.y), float64(galaxy_b.y)))

			// Calculate shorted distance by summing x and y differences
			// and multiplying the x and y axes that do not contain galaxies
			x_sum := 0
			y_sum := 0
			for x := min_x; x < max_x; x++ {
				if _, ok := axes_with_galaxies_map[fmt.Sprintf("x%v", x)]; !ok {
					x_sum += empty_axes_growth
				} else {
					x_sum += 1
				}
			}
			for y := min_y; y < max_y; y++ {
				if _, ok := axes_with_galaxies_map[fmt.Sprintf("y%v", y)]; !ok {
					y_sum += empty_axes_growth
				} else {
					y_sum += 1
				}
			}

			// Add sum of x and y distances to sum
			shortest_path_sum += x_sum + y_sum
		}
	}

	return shortest_path_sum
}

func part_2(input []byte) int {
	return 0
}
