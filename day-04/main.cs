using System;
using System.Collections.Generic;
					
public class Program
{
	public const string SEARCH_STRING = "XMAS";
	public const string CROSS_PATTERN = "MAS";

	public static void Main()
	{
		var lines = ReadLines();
		Console.WriteLine(CalculatePart1(lines));
		Console.WriteLine(CalculatePart2(lines));
	}

	public static string[] ReadLines() {
		// *** Array for the lines of the input
		var lines = new List<string>();

		// *** Allocate variable for the line
		string line;

		// *** Loop over the lines until an empty line is received
		while (!String.IsNullOrEmpty(line = Console.ReadLine())) {
            lines.Add(line);
		}

		// *** lines the list as an array
		return lines.ToArray();
	}

	public static int CalculatePart1(string[] lines) {
		// *** Accumulate the values
		int a = 0;

		// *** Loop over all the characters of all the lines of the input
		for (int y = 0; y < lines.Length; y++) {
			for (int x = 0; x < lines[y].Length; x++) {
				// *** Get the current char
				char currentChar = lines[y][x];

				// *** We only care if the current char is an X
				if (currentChar != SEARCH_STRING[0]) {
					continue;
				}

				// *** Look in all 8 directions 
				for (int yDir = -1; yDir <= 1; yDir++) {
					for (int xDir = -1; xDir <= 1; xDir++) {
						// *** If both the xDir and yDir are zero, skip as this will look at nothing
						if (yDir == 0 && xDir == 0) {
							continue;
						}

						// *** Check if the current direction matches
						bool match = true;

						// *** Loop over each char of the string after the first to see if it matches in the current direction
						for (int i = 1; i < SEARCH_STRING.Length; i++) {
							// *** Calculate the x and y coordinates of the char that we are looking at in the input
							(int x_, int y_) = (x + xDir * i, y + yDir * i);

							// *** Check if both coordinates are in bounds of the input
							if (y_ < 0 || y_ >= lines.Length || x_ < 0 || x_ >= lines[y_].Length) {
								match = false;
								break;
							}

							// *** Check if the char at those coordinates matches what we expect
							if (lines[y_][x_] != SEARCH_STRING[i]) {
								match = false;
								break;
							}
						}

						// *** If it made it through the loop without setting match to false, increase the number of matches
						if (match) {
							a++;
						}
					}
				}
			}
		}

		return a;
	}

	public static int CalculatePart2(string[] lines) {
		// *** Accumulate the values
		int a = 0;

		// *** Loop over all the characters of all the lines of the input, except the outermost characters
		for (int y = 1; y < lines.Length - 1; y++) {
			for (int x = 1; x < lines[y].Length - 1; x++) {
				// *** Get the current char
				char currentChar = lines[y][x];

				// *** We only care if the current char is an A
				if (currentChar != CROSS_PATTERN[1]) {
					continue;
				}

				// *** Keep track if the pattern has been found
				bool match = true;

				// *** Checkif the pattern is found in both directions
				foreach (int o in new int[] {-1, 1}) {
					// *** Look at the first set of corners
					var corners = new char[] {lines[y - 1][x - o], lines[y + 1][x + o]};

					// *** Check if the pattern is found
					if (corners[0] == CROSS_PATTERN[0] && corners[1] == CROSS_PATTERN[2]) {
						continue;
					}

					// *** Check if the pattern is found in reverse
					if (corners[0] == CROSS_PATTERN[2] && corners[1] == CROSS_PATTERN[0]) {
						continue;
					}

					match = false;
					break;
				}

				// *** If it made it through the loop without setting match to false, increase the number of matches
				if (match) {
					a++;
				}
			}
		}

		return a;
	}	
}