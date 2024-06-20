package main

import (
	"fmt"
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
	game_strings_list := strings.Split(string(input), "\n")

	// Parse games
	games_list := make([]game, len(game_strings_list))
	for i := 0; i < len(game_strings_list); i++ {
		// Convert game_content to struct
		game_strings_content := game_strings_list[i]
		games_list[i] = parse_game(game_strings_content)
	}

	// Part 1
	max_red, max_green, max_blue := 12, 13, 14
	part_1_result := part_1(games_list, max_red, max_green, max_blue)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	part_2_result := part_2(games_list)
	fmt.Println("Part 2", part_2_result)
}

type set struct {
	red   int
	green int
	blue  int
}

type game struct {
	index int
	sets  []set
}

func parse_game(game_content string) game {
	// Parse game information
	// e.g. Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

	// Extract index
	index_regexp := regexp.MustCompile(`Game (\d+):`)
	parsed_index, _ := strconv.Atoi(index_regexp.FindStringSubmatch(game_content)[1])

	// Extract cubes content
	cubes_regexp := regexp.MustCompile(`Game \d+: (.*)`)
	cubes_content := strings.Split(cubes_regexp.FindStringSubmatch(game_content)[1], ";")

	parsed_cubes_list := make([]set, len(cubes_content))
	red_regexp := regexp.MustCompile(`(\d+) red`)
	green_regexp := regexp.MustCompile(`(\d+) green`)
	blue_regexp := regexp.MustCompile(`(\d+) blue`)

	// For each cubes content, extract red green and blue counts
	for i, cube_content := range cubes_content {
		parsed_red, parsed_green, parsed_blue := 0, 0, 0

		if red_regexp.MatchString(cube_content) {
			parsed_red, _ = strconv.Atoi(red_regexp.FindStringSubmatch(cube_content)[1])
		}
		if green_regexp.MatchString(cube_content) {
			parsed_green, _ = strconv.Atoi(green_regexp.FindStringSubmatch(cube_content)[1])
		}
		if blue_regexp.MatchString(cube_content) {
			parsed_blue, _ = strconv.Atoi(blue_regexp.FindStringSubmatch(cube_content)[1])
		}

		parsed_cubes_list[i] = set{
			red:   parsed_red,
			green: parsed_green,
			blue:  parsed_blue,
		}
	}

	// Return parsed game object
	return game{
		index: parsed_index,
		sets:  parsed_cubes_list,
	}
}

func part_1(games_list []game, max_red int, max_green int, max_blue int) int {
	total_index_sum := 0
	impossible_index_sum := 0

	for i := 0; i < len(games_list); i++ {
		game := games_list[i]

		// Check if any set of cubes seen is impossible
		for _, set := range game.sets {
			if set.red > max_red || set.green > max_green || set.blue > max_blue {
				// Increment by this game's index
				impossible_index_sum += game.index
				break
			}
		}

		total_index_sum += game.index
	}

	// Sum of possible game indexes is total - impossible sum
	possible_index_sum := total_index_sum - impossible_index_sum
	return possible_index_sum
}

func part_2(games_list []game) int {
	sum_game_power := 0

	for i := 0; i < len(games_list); i++ {
		game := games_list[i]
		min_red, min_green, min_blue := 0, 0, 0

		// Get the min number of cubes required per set
		for _, set := range game.sets {
			if min_red < set.red {
				min_red = set.red
			}
			if min_green < set.green {
				min_green = set.green
			}
			if min_blue < set.blue {
				min_blue = set.blue
			}
		}

		sum_game_power += min_red * min_green * min_blue
	}

	return sum_game_power
}
