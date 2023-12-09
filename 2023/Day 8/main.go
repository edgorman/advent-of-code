package main

import (
	"fmt"
	"math"
	"os"
	"regexp"
	"strings"

	"golang.org/x/exp/maps"
)

func main() {
	// Read input from local file
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse input
	instruction_list, node_mappings := parse_input(input)

	// Part 1
	part_1_result := part_1(instruction_list, node_mappings)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	part_2_result := part_2(instruction_list, node_mappings)
	fmt.Println("Part 2", part_2_result)
}

func parse_input(input []byte) ([]string, map[string][]string) {
	// Parse instructions and mappings from input
	// e.g. RL
	//		AAA = (BBB, CCC)
	//		BBB = (DDD, EEE)
	input_lines := strings.Split(string(input), "\n")

	// Extract instructions
	instructions_list := strings.Split(input_lines[0], "")

	// Extract mappings
	node_mappings := make(map[string][]string)
	node_mappings_regexp := regexp.MustCompile(`\((.*)\)`)

	// For each mapping, extract the name and values into a map
	for index := 2; index < len(input_lines); index++ {
		node_mappings_name := strings.Split(input_lines[index], " ")[0]
		node_mappings_value := node_mappings_regexp.FindStringSubmatch(input_lines[index])[1]
		node_mappings[node_mappings_name] = strings.Split(node_mappings_value, ", ")
	}

	return instructions_list, node_mappings
}

func part_1(instructions_list []string, node_mappings map[string][]string) int {
	step_count := 0
	current_node := "AAA"
	target_node := "ZZZ"

	// Iterate until the current node becomes target node
	for current_node != target_node {
		next_instruction := instructions_list[step_count%len(instructions_list)]
		possible_nodes := node_mappings[current_node]

		// Update the current node mapping
		if next_instruction == "L" {
			current_node = possible_nodes[0]
		} else {
			current_node = possible_nodes[1]
		}

		step_count += 1
	}

	return step_count
}

// greatest common divisor (GCD) via Euclidean algorithm
func GCD(a, b int) int {
	for b != 0 {
		t := b
		b = a % b
		a = t
	}
	return a
}

func greatest_common_denominator(num_a float64, num_b float64) float64 {
	if num_a == 0 || num_b == 0 {
		return num_a + num_b
	}

	upper := math.Max(math.Abs(num_a), math.Abs(num_b))
	lower := math.Min(math.Abs(num_a), math.Abs(num_b))
	return greatest_common_denominator(math.Mod(upper, lower), lower)
}

func least_common_multiple(numbers []int) int {
	lcm := float64(numbers[0])

	for index := 1; index < len(numbers); index++ {
		num := float64(numbers[index])
		lcm = math.Abs(float64(lcm*num)) / greatest_common_denominator(lcm, num)
	}

	return int(lcm)
}

func part_2(instructions_list []string, node_mappings map[string][]string) int {
	// Get all mappings that end in a
	current_nodes := make([]string, 0)

	for key, _ := range node_mappings {
		if string(key[len(key)-1]) == "A" {
			current_nodes = append(current_nodes, key)
		}
	}

	// Store when each node reaches it's target
	current_nodes_target_step_mapping := make(map[string]int)
	step_count := 0

	// Iterate until all current nodes become target nodes
	for len(current_nodes) != len(current_nodes_target_step_mapping) {
		// Reset target nodes count and get next instruction
		next_instruction := instructions_list[step_count%len(instructions_list)]
		step_count += 1

		// Update each current node mapping
		for index := 0; index < len(current_nodes); index++ {
			possible_nodes := node_mappings[current_nodes[index]]

			if next_instruction == "L" {
				current_nodes[index] = possible_nodes[0]
			} else {
				current_nodes[index] = possible_nodes[1]
			}

			// If this node ends with Z, store this
			if string(current_nodes[index][len(current_nodes[index])-1]) == "Z" {
				if _, ok := current_nodes_target_step_mapping[current_nodes[index]]; !ok {
					current_nodes_target_step_mapping[current_nodes[index]] = step_count
				}
			}
		}

	}

	// Return the least common multiple of all mapping values
	return least_common_multiple(maps.Values(current_nodes_target_step_mapping))
}
