using System.Collections;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            char[] input = lines[0].ToCharArray();

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(char[] input)
        {
            // Convert hexadecimal to binary values
            char[] binary = ConvertHexToBin(input);

            // Parse binary into a packet
            Packet packet = ParsePackets(binary).Item1;

            // Return sum of versions
            return packet.SumVersions().ToString();
        }

        static char[] ConvertHexToBin(char[] hexadecimal)
        {
            char[] bin = new char[hexadecimal.Length * 4];

            // Convert each hexademical character to binary value
            for(int i = 0; i < hexadecimal.Length; i++)
            {
                char h = hexadecimal[i];
                string b = Convert.ToString(Convert.ToInt32(h.ToString(), 16), 2).PadLeft(4, '0');

                bin[(i * 4) + 0] = b[0];
                bin[(i * 4) + 1] = b[1];
                bin[(i * 4) + 2] = b[2];
                bin[(i * 4) + 3] = b[3];
            }

            return bin;
        }

        static Tuple<Packet, char[]> ParsePackets(char[] binary)
        {
            // Extract version and type from start
            int version = Convert.ToInt32(new string(binary[0..3]), 2);
            int type = Convert.ToInt32(new string(binary[3..6]), 2);

            // If type is a literal
            if (type == 4)
            {
                // Get literal value from binary
                int index = 6;
                string literalBinary = "";

                // Read binary until last segment starts with 0
                do
                {
                    literalBinary += new string(binary[(index + 1)..(index + 5)]);
                    index += 5;
                }
                while (binary[index - 5] == '1');

                // Convert literal binary to value
                long literalValue = Convert.ToInt64(literalBinary, 2);

                // Return literal packet and remaining binary
                return Tuple.Create(new Packet(version, literalValue), binary[index..binary.Length]);
            }
            // Else type is an operator
            else
            {
                List<Packet> subpackets = new List<Packet>();

                // Read subpackets depending on length type
                char lengthType = binary[6];

                if (lengthType == '0')
                {
                    // Next 15 bits are length of subpackets
                    int lengthOfPackets = Convert.ToInt32(new string(binary[7..22]), 2);
                    char[] remainingBinary = binary[22..(22 + lengthOfPackets)];

                    // While there is binary remaining, parse and add to subpackets list
                    while (remainingBinary.Length > 0)
                    {
                        var result = ParsePackets(remainingBinary);
                        remainingBinary = result.Item2;
                        subpackets.Add(result.Item1);
                    }

                    // Return operator packet and remaining binary
                    return Tuple.Create(
                        new Packet(version, type, subpackets.ToArray()),
                        binary[(22 + lengthOfPackets)..binary.Length]
                    );
                }
                else
                {
                    // Next 11 bits are number of subpackets
                    int numOfPackets = Convert.ToInt32(new string(binary[7..18]), 2);
                    char[] remainingBinary = binary[18..binary.Length];

                    // For each packet, parse and add to subpackets list
                    for (int i = 0; i < numOfPackets; i++)
                    {
                        var result = ParsePackets(remainingBinary);
                        remainingBinary = result.Item2;
                        subpackets.Add(result.Item1);
                    }

                    // Return operator packet and remaining binary
                    return Tuple.Create(
                        new Packet(version, type, subpackets.ToArray()), 
                        remainingBinary
                    );
                }
            }

        }

        static string PartTwo(char[] input)
        {
            // Convert hexadecimal to binary values
            char[] binary = ConvertHexToBin(input);

            // Parse binary into a packet
            Packet packet = ParsePackets(binary).Item1;

            // Return sum of versions
            return packet.Evaluate().ToString();
        }
    }

    class Packet
    {
        private int _version;
        private int _type;

        private long _literalValue;
        private Packet[] _operatorValues;

        public Packet(int version, long literalValue)
        {
            _version = version;
            _type = 4;
            _literalValue = literalValue;
            _operatorValues = new Packet[0];
        }

        public Packet(int version, int type, Packet[] operatorValues)
        {
            _version = version;
            _type = type;
            _literalValue = 0;
            _operatorValues = operatorValues;
        }

        public int SumVersions()
        {
            int sum = _version;

            // If packet contains subpackets
            if (!(_type == 4))
            {
                // Add versions of each subpacket
                foreach(Packet p in _operatorValues)
                {
                    sum += p.SumVersions();
                }
            }

            return sum;
        }

        public long Evaluate()
        {
            // If packet contains literal value
            if (_type == 4)
            {
                return _literalValue;
            }
            // Else packet contains operator and subpackets
            else
            {
                // Evaluate operators and store in array
                long[] values = new long[_operatorValues.Length];
                
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = _operatorValues[i].Evaluate();
                }

                // Execute operation depending on type
                switch(_type)
                {
                    case 0: return values.Sum(); 
                    case 1: return values.Aggregate((a, x) => a * x);
                    case 2: return values.Min();
                    case 3: return values.Max();
                    case 5: return values[0] > values[1] ? 1 : 0;
                    case 6: return values[0] < values[1] ? 1 : 0;
                    case 7: return values[0] == values[1] ? 1 : 0;
                    default: return -1;
                }
            }
        }
    }
}