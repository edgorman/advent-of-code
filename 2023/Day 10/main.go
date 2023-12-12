package main

import (
	"errors"
	"fmt"
	"os"
	"strings"
)

func main() {
	// Read input from local file
	input, err := os.ReadFile("test.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse input
	pipe_grid, start_position := parse_input(input)

	// Part 1
	part_1_result := part_1(pipe_grid, start_position)
	fmt.Println("Part 1", part_1_result)

	// Part 2
	part_2_result := part_2(pipe_grid, start_position)
	fmt.Println("Part 2", part_2_result)
}

type position struct {
	x int
	y int
}

const (
	// Types of pipe
	START_PIPE      string = "S"
	VERTICAL_PIPE   string = "|"
	HORIZONTAL_PIPE string = "-"
	N_E_PIPE        string = "L"
	N_W_PIPE        string = "J"
	S_W_PIPE        string = "7"
	S_E_PIPE        string = "F"
)

func parse_input(input []byte) ([][]string, position) {
	// Convert bytes to list of strings
	input_strings_list := strings.Split(string(input), "\n")

	// Store pipes in 2d array
	pipe_grid := make([][]string, len(input_strings_list))
	var start_position position

	// For each line, store inner pipe values
	for index, input_string := range input_strings_list {
		pipe_grid[index] = strings.Split(input_string, "")

		// Check if line contains start position
		start_x := strings.Index(input_string, START_PIPE)
		if start_x > -1 {
			start_position = position{x: start_x, y: index}
		}
	}

	return pipe_grid, start_position
}

func find_next_position(pipe_grid [][]string, current_position position, last_position position) (position, error) {
	// Define min and max position bounds
	min_x, min_y := 0, 0
	max_x, max_y := len(pipe_grid[0]), len(pipe_grid)

	// Define which pipes can move north/east/south/west
	north_facing_pipe := map[string]bool{START_PIPE: true, VERTICAL_PIPE: true, HORIZONTAL_PIPE: false, N_E_PIPE: true, N_W_PIPE: true, S_W_PIPE: false, S_E_PIPE: false}
	east_facing_pipe := map[string]bool{START_PIPE: true, VERTICAL_PIPE: false, HORIZONTAL_PIPE: true, N_E_PIPE: true, N_W_PIPE: false, S_W_PIPE: false, S_E_PIPE: true}
	south_facing_pipe := map[string]bool{START_PIPE: true, VERTICAL_PIPE: true, HORIZONTAL_PIPE: false, N_E_PIPE: false, N_W_PIPE: false, S_W_PIPE: true, S_E_PIPE: true}
	west_facing_pipe := map[string]bool{START_PIPE: true, VERTICAL_PIPE: false, HORIZONTAL_PIPE: true, N_E_PIPE: false, N_W_PIPE: true, S_W_PIPE: true, S_E_PIPE: false}

	// Check if can move north
	if current_position.y-1 >= min_y && current_position.y-1 != last_position.y {
		if val, _ := north_facing_pipe[pipe_grid[current_position.y][current_position.x]]; val {
			if val, _ := south_facing_pipe[pipe_grid[current_position.y-1][current_position.x]]; val {
				return position{x: current_position.x, y: current_position.y - 1}, nil
			}
		}
	}
	// Check if can move east
	if current_position.x+1 <= max_x && current_position.x+1 != last_position.x {
		if val, _ := east_facing_pipe[pipe_grid[current_position.y][current_position.x]]; val {
			if val, _ := west_facing_pipe[pipe_grid[current_position.y][current_position.x+1]]; val {
				return position{x: current_position.x + 1, y: current_position.y}, nil
			}
		}
	}
	// Check if can move south
	if current_position.y+1 <= max_y && current_position.y+1 != last_position.y {
		if val, _ := south_facing_pipe[pipe_grid[current_position.y][current_position.x]]; val {
			if val, _ := north_facing_pipe[pipe_grid[current_position.y+1][current_position.x]]; val {
				return position{x: current_position.x, y: current_position.y + 1}, nil
			}
		}
	}
	// Check if can move west
	if current_position.x-1 >= min_x && current_position.x-1 != last_position.x {
		if val, _ := west_facing_pipe[pipe_grid[current_position.y][current_position.x]]; val {
			if val, _ := east_facing_pipe[pipe_grid[current_position.y][current_position.x-1]]; val {
				return position{x: current_position.x - 1, y: current_position.y}, nil
			}
		}
	}

	// We shouldn't be here, raise an error
	return position{x: 0, y: 0}, errors.New(fmt.Sprintf("Could not find a valid next position for position %v", current_position))
}

func part_1(pipe_grid [][]string, start_position position) int {
	// Helper variables
	step_count := 1
	last_position := start_position

	// Calculate the first next position, handling an error that is raised
	current_position, err := find_next_position(pipe_grid, start_position, last_position)
	if err != nil {
		fmt.Println(err)
		os.Exit(1)
	}

	// Step until you reach the starting pipe again
	for pipe_grid[current_position.y][current_position.x] != START_PIPE {

		tmp_position := current_position
		current_position, err = find_next_position(pipe_grid, current_position, last_position)
		if err != nil {
			fmt.Println(err)
			os.Exit(1)
		}
		last_position = tmp_position
		step_count += 1

	}

	// Return half the step count (should be even number)
	return step_count / 2
}

func part_2(pipe_grid [][]string, start_position position) int {
	// Helper variables
	position_list := []position{start_position}
	last_position := start_position

	// Calculate the first next position, handling an error that is raised
	current_position, err := find_next_position(pipe_grid, start_position, last_position)
	position_list = append(position_list, current_position)
	if err != nil {
		fmt.Println(err)
		os.Exit(1)
	}

	// Step until you reach the starting pipe again
	for pipe_grid[current_position.y][current_position.x] != START_PIPE {

		tmp_position := current_position
		current_position, err = find_next_position(pipe_grid, current_position, last_position)
		if err != nil {
			fmt.Println(err)
			os.Exit(1)
		}
		last_position = tmp_position
		position_list = append(position_list, current_position)

	}

	// Use shoelace formula to determine polygon area
	// https://en.wikipedia.org/wiki/Shoelace_formula
	double_area := 0
	last_d := -100
	for index := 0; index < len(position_list)-1; index++ {
		a := position_list[index]
		b := position_list[index+1]
		d := (a.x * b.y) - (a.y * b.x)

		if d != last_d {
			double_area += d
			last_d = d
		}
	}

	return double_area / 2
}
