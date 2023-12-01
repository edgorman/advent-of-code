package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	// Read document from local file
	document, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Could not read document")
	}
	calibration_list := strings.Split(string(document), "\n")

	// Part 1
	part1 := part1(calibration_list)
	fmt.Println("Part 1", part1)

	// Part 2
	part2 := part2(calibration_list)
	fmt.Println("Part 2", part2)
}

func part1(calibration_list []string) int {
	calibration_sum := 0
	for calibation_idx := 0; calibation_idx < len(calibration_list); calibation_idx++ {

		calibration := calibration_list[calibation_idx]
		calibration_value := ""

		for i := 0; i < len(calibration); i++ {

			character := string(calibration[i])
			if _, err := strconv.Atoi(character); err == nil {
				calibration_value += character
				break
			}

		}
		for i := len(calibration) - 1; i >= 0; i-- {

			character := string(calibration[i])
			if _, err := strconv.Atoi(character); err == nil {
				calibration_value += character
				break
			}

		}

		if v, err := strconv.Atoi(calibration_value); err == nil {
			calibration_sum += v
		}

	}

	return calibration_sum
}

func part2(calibration_list []string) int {
	calibration_sum := 0
	for calibation_idx := 0; calibation_idx < len(calibration_list); calibation_idx++ {

		calibration := calibration_list[calibation_idx]
		calibration_value := ""

	forwards_character_loop:
		for i := 0; i < len(calibration); i++ {

			character := string(calibration[i])
			if _, err := strconv.Atoi(character); err == nil {
				calibration_value += character
				break
			}

			characters := string(calibration[i:])
			switch {
			case strings.HasPrefix(characters, "one"):
				calibration_value += "1"
				break forwards_character_loop
			case strings.HasPrefix(characters, "two"):
				calibration_value += "2"
				break forwards_character_loop
			case strings.HasPrefix(characters, "three"):
				calibration_value += "3"
				break forwards_character_loop
			case strings.HasPrefix(characters, "four"):
				calibration_value += "4"
				break forwards_character_loop
			case strings.HasPrefix(characters, "five"):
				calibration_value += "5"
				break forwards_character_loop
			case strings.HasPrefix(characters, "six"):
				calibration_value += "6"
				break forwards_character_loop
			case strings.HasPrefix(characters, "seven"):
				calibration_value += "7"
				break forwards_character_loop
			case strings.HasPrefix(characters, "eight"):
				calibration_value += "8"
				break forwards_character_loop
			case strings.HasPrefix(characters, "nine"):
				calibration_value += "9"
				break forwards_character_loop
			default:
				continue forwards_character_loop
			}

		}

	backwards_character_loop:
		for i := len(calibration) - 1; i >= 0; i-- {

			character := string(calibration[i])
			if _, err := strconv.Atoi(character); err == nil {
				calibration_value += character
				break
			}

			characters := string(calibration[i:])
			switch {
			case strings.HasPrefix(characters, "one"):
				calibration_value += "1"
				break backwards_character_loop
			case strings.HasPrefix(characters, "two"):
				calibration_value += "2"
				break backwards_character_loop
			case strings.HasPrefix(characters, "three"):
				calibration_value += "3"
				break backwards_character_loop
			case strings.HasPrefix(characters, "four"):
				calibration_value += "4"
				break backwards_character_loop
			case strings.HasPrefix(characters, "five"):
				calibration_value += "5"
				break backwards_character_loop
			case strings.HasPrefix(characters, "six"):
				calibration_value += "6"
				break backwards_character_loop
			case strings.HasPrefix(characters, "seven"):
				calibration_value += "7"
				break backwards_character_loop
			case strings.HasPrefix(characters, "eight"):
				calibration_value += "8"
				break backwards_character_loop
			case strings.HasPrefix(characters, "nine"):
				calibration_value += "9"
				break backwards_character_loop
			default:
				continue backwards_character_loop
			}

		}

		if v, err := strconv.Atoi(calibration_value); err == nil {
			calibration_sum += v
		}

	}

	return calibration_sum
}
