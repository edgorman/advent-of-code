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
	// Read input from local file
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read input")
	}

	// Parse the races from input
	race_list := parse_races(strings.Split(string(input), "\n"))

	// Part 1
	part_1_result := part_1(race_list)
	fmt.Println("Part 1", part_1_result)

	// Update race list so it's one race
	time_string := ""
	distance_string := ""
	for _, race := range race_list {
		time_string += fmt.Sprint(race.time)
		distance_string += fmt.Sprint(race.distance)
	}
	time, _ := strconv.Atoi(time_string)
	distance, _ := strconv.Atoi(distance_string)

	// Part 2
	part_2_result := part_1([]race{{time: time, distance: distance}})
	fmt.Println("Part 2", part_2_result)
}

type race struct {
	time     int
	distance int
}

func parse_races(lines []string) []race {
	// Parse race info
	// e.g. Time:      7  15   30
	//		Distance:  9  40  200

	// Extract times and distances
	numbers_regexp := regexp.MustCompile(`(\d+)`)
	time_list := numbers_regexp.FindAllString(lines[0], -1)
	distance_list := numbers_regexp.FindAllString(lines[1], -1)

	// Return a list of time objects
	race_list := make([]race, 0)

	for index := 0; index < len(time_list); index++ {
		// Convert to integers
		time_value, _ := strconv.Atoi(time_list[index])
		distance_value, _ := strconv.Atoi(distance_list[index])

		race_list = append(
			race_list,
			race{
				time:     time_value,
				distance: distance_value,
			},
		)
	}

	return race_list
}

func part_1(race_list []race) int {
	// Treat each race like a quadratic equation, where:
	// y= time*x - x^2 - distance
	// Solve for y = 0 and round to whole number

	// Store number of ways each race can be beaten
	race_wins_sum := 1

	// For each race
	for _, race := range race_list {
		// Calculate discriminant
		discriminant := race.time*race.time - 4*-1*-race.distance

		// If discriminant is greater than 0, two solutions exist
		// Else if discriminant equal 0, one solutions exist
		// Else discriminant is less than 0, complex solutions exist, ignore
		if discriminant > 0 {
			// Calculate lower and upper bounds
			upper_root := (-float64(race.time) - math.Sqrt(float64(discriminant))) / (2 * -1)
			lower_root := (-float64(race.time) + math.Sqrt(float64(discriminant))) / (2 * -1)

			// If roots are whole numbers, increment decrement respectively
			if upper_root == math.Trunc(upper_root) {
				upper_root -= 1
			}
			if lower_root == math.Trunc(lower_root) {
				lower_root += 1
			}

			// Floor and Ceil the bounds respectively
			upper_root = math.Floor(upper_root)
			lower_root = math.Ceil(lower_root)

			// Multiply difference by race_wins_sum
			race_wins_sum *= int(upper_root) - int(lower_root) + 1
		} else if discriminant == 0 {
			fmt.Println("one solution exists")
		} else {
			fmt.Println("complex solutions exist")
		}
	}

	return race_wins_sum
}
