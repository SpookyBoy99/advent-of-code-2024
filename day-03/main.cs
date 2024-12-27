using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
					
public class Program
{
	public static void Main()
	{
		var lines = ReadLines();
		Console.WriteLine(CalculatePart1(lines));
		Console.WriteLine(CalculatePart2(lines));
	}

	public static string ReadLines() {
		// *** Add each line to a single string
		StringBuilder lines = new StringBuilder();

		// *** Allocate variable for the line
		string line;

		// *** Loop over the lines until an empty line is received
		while (!String.IsNullOrEmpty(line = Console.ReadLine())) {
            lines.AppendLine(line);
		}

		// *** lines the list as an array
		return lines.ToString();
	}

	public static int CalculatePart1(string lines) {
		// *** Accumulate the values
		int a = 0;

		// *** Pattern for matching the mul operator
		Regex r = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

		// *** Loop over all the matches in the string
		for (Match m = r.Match(lines); m.Success; m = m.NextMatch()) {
			a += Int32.Parse(m.Groups[1].Value) * Int32.Parse(m.Groups[2].Value);
		}

		return a;
	}

	public static int CalculatePart2(string lines) {
		// *** Accummulate all the matches
		int a = 0;

		// *** Pattern to match all string sections that are enabled
		Regex r = new Regex(@"(?:^|do\(\))(.*?)(?:don't\(\)|$)", RegexOptions.Singleline);

		// *** Accumulate all the active values using the code from part 1
		for (Match m = r.Match(lines); m.Success; m = m.NextMatch()) {
			a += CalculatePart1(m.Groups[1].Value);
		}

		// *** Return the accumulated values
		return a;
	}
}