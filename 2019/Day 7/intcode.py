class IntCode:

    def __init__(self, intcode):
        self.index = 0
        self.output_ = 0
        self.intcode = intcode

    def add(self, a, b):
        return a + b
    
    def mult(self, a, b):
        return a * b
    
    def store(self, v, p):
        self.intcode[p] = v
    
    def output(self, p):
        return self.intcode[p]
    
    def lessthan(self, a, b):
        return a < b

    def equals(self, a, b):
        return a == b
    
    def parse_mode(self, position, mode):
        if mode == "1": return self.intcode[position]
        else: return self.intcode[self.intcode[position]]
    
    def run(self, input_):
        # Iterate until reach the stop code
        while self.intcode[self.index] != 99:
            opcode = int(str(self.intcode[self.index])[-2:])
            modes = str(self.intcode[self.index])[:-2][::-1] + '000'

            # Addition
            if opcode == 1:
                self.intcode[self.intcode[self.index + 3]] = self.add(
                    self.parse_mode(self.index + 1, modes[0]), 
                    self.parse_mode(self.index + 2, modes[1])
                )
                self.index += 4
            # Multiplication
            elif opcode == 2:
                self.intcode[self.intcode[self.index + 3]] = self.mult(
                    self.parse_mode(self.index + 1, modes[0]), 
                    self.parse_mode(self.index + 2, modes[1])
                )
                self.index += 4
            # Storage
            elif opcode == 3:
                if len(input_) > 0:
                    self.store(input_.pop(0), self.intcode[self.index + 1])
                    self.index += 2
                else:
                    raise Exception("No input for storage")
            # Output
            elif opcode == 4:
                self.output_ = self.output(self.intcode[self.index + 1])
                self.index += 2
            # Jump if true
            elif opcode == 5:
                if self.parse_mode(self.index + 1, modes[0]) != 0:
                    self.index = self.parse_mode(self.index + 2, modes[1])
                else:
                    self.index += 3
            # Jump if false
            elif opcode == 6:
                if self.parse_mode(self.index + 1, modes[0]) == 0:
                    self.index = self.parse_mode(self.index + 2, modes[1])
                else:
                    self.index += 3
            # Less than
            elif opcode == 7:
                value = 0
                position = self.intcode[self.index + 3]
                if self.lessthan(
                    self.parse_mode(self.index + 1, modes[0]), 
                    self.parse_mode(self.index + 2, modes[1])
                ):
                    value = 1
                self.store(value, position)
                self.index += 4
            # Equals
            elif opcode == 8:
                value = 0
                position = self.intcode[self.index + 3]
                if self.equals(
                    self.parse_mode(self.index + 1, modes[0]), 
                    self.parse_mode(self.index + 2, modes[1])
                ):
                    value = 1
                self.store(value, position)
                self.index += 4
            # Unknown
            else:
                raise Exception("Unknown op code")

        return self.output_
