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
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse the input
	seed_list, seed_mapping_list := parse_seeds(string(input))

	// Part 1
	part_1_result := part_1(seed_list, seed_mapping_list)
	fmt.Println("Part 1", part_1_result)

	// Update seed ranges
	for index := 0; index < len(seed_list)-1; index += 2 {
		seed_list[index].range_ = seed_list[index+1].index
		seed_list[index+1].range_ = -1
	}

	// Part 2
	part_2_result := part_1(seed_list, seed_mapping_list)
	fmt.Println("Part 2", part_2_result)
}

type seed struct {
	index  int
	range_ int
}

type seed_mapping struct {
	name       string
	range_list []seed_range
}

type seed_range struct {
	start  int
	end    int
	range_ int
}

func parse_seeds(seed_input string) ([]seed, []seed_mapping) {
	// Extract seed numbers from seed_input
	// e.g. seeds: 79 14 55 13
	seed_strings_regexp := regexp.MustCompile(`seeds:\s+(.*)`)
	seed_strings_text := seed_strings_regexp.FindStringSubmatch(seed_input)[1]
	seed_strings_list := strings.Split(seed_strings_text, " ")

	seed_list := make([]seed, 0)
	for _, seed_string := range seed_strings_list {
		value, _ := strconv.Atoi(seed_string)
		seed_list = append(seed_list, seed{index: value, range_: 1})
	}

	// Extract seed mappings from seed_input
	// e.g. temperature-to-humidity map:
	//		50 98 2
	//      52 50 48
	seed_mapping_outer_regexp := regexp.MustCompile(`(.*:\n[\d|\s]+)`)
	seed_mapping_outer_list := seed_mapping_outer_regexp.FindAllStringSubmatch(seed_input, -1)
	seed_mapping_name_regexp := regexp.MustCompile(`^(.*) map:`)
	seed_mapping_text_regexp := regexp.MustCompile(`(?m)(\d.*)`)

	seed_mapping_list := make([]seed_mapping, 0)

	// For each seed mapping
	for _, seed_mapping_outer_text := range seed_mapping_outer_list {
		// Extract mapping name
		seed_mapping_name := seed_mapping_name_regexp.FindStringSubmatch(seed_mapping_outer_text[0])[1]

		// Extract mapping values
		seed_mapping_text_list := seed_mapping_text_regexp.FindAllStringSubmatch(seed_mapping_outer_text[0], -1)
		seed_range_list := make([]seed_range, 0)

		// For each mapping text
		for _, seed_mapping_text := range seed_mapping_text_list {

			// Convert values to mappings
			seed_mapping_value_list := strings.Split(seed_mapping_text[0], " ")
			end, _ := strconv.Atoi(seed_mapping_value_list[0])
			start, _ := strconv.Atoi(seed_mapping_value_list[1])
			range_, _ := strconv.Atoi(seed_mapping_value_list[2])

			seed_range_list = append(
				seed_range_list,
				seed_range{
					start:  start,
					end:    end,
					range_: range_,
				},
			)
		}

		seed_mapping_list = append(
			seed_mapping_list,
			seed_mapping{
				name:       seed_mapping_name,
				range_list: seed_range_list,
			},
		)
	}

	return seed_list, seed_mapping_list
}

func part_1(seed_list []seed, seed_mapping_list []seed_mapping) int {
	lowest_seed_position := math.MaxInt

	// For each seed
	for _, seed := range seed_list {
		// Skip if seed range is negative
		if seed.range_ < 0 {
			continue
		}

		for seed_position := seed.index; seed_position <= seed.index+seed.range_; seed_position++ {
			new_seed_position := seed_position

			// Apply mappings until all seed_mappings have been traversed
			for _, seed_mapping := range seed_mapping_list {
				for _, seed_range := range seed_mapping.range_list {
					// If this seed range has a valid mapping, apply it
					// otherwise continue
					if new_seed_position >= seed_range.start && new_seed_position < seed_range.start+seed_range.range_ {
						new_seed_position = seed_range.end + (new_seed_position - seed_range.start)
						break
					}
				}
			}

			if new_seed_position < lowest_seed_position {
				lowest_seed_position = new_seed_position
			}
		}
	}

	return lowest_seed_position
}
