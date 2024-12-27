using System;
using System.Collections.Generic;
using System.Linq;
					
public class Program
{
	public static void Main()
	{
		var (left, right) = ReadLines();
		Console.WriteLine(CalculatePart1(left, right));
		Console.WriteLine(CalculatePart2(left, right));
	}

	public static (int[], int[]) ReadLines() {
		// *** Create a list for left and right values
		var left = new List<int>();
		var right = new List<int>();

		// *** Allocate variable for the line
		string line;

		// *** Loop over the lines until an empty line is received
		while (!String.IsNullOrEmpty(line = Console.ReadLine())) {
			// *** Split the string
			string[] numbers = line.Split("   ");

			// *** Skip line if it does not contain two values
			if (numbers.Length != 2) {
				continue;
			}

			// *** Add the values to the lists
			left.Add(Int32.Parse(numbers[0]));
			right.Add(Int32.Parse(numbers[1]));
		}

		// *** Return a tuple with both lists
		return (left.ToArray(), right.ToArray());
	}

	public static int CalculatePart1(int[] left, int[] right) {
		// *** Sort both arrays independent
		Array.Sort(left);
		Array.Sort(right);

		// *** Accumulate the values here
		int a = 0;

		// *** Loop over all values together
		foreach (var v in left.Zip(right, Tuple.Create)) {
			a += Math.Abs(v.Item2 - v.Item1);
		}

		return a;
	}

	public static int CalculatePart2(int[] left, int[] right) {
		// *** 	Create a dictionary to keep track of the occurence of each value
		var frequencies = new Dictionary<int, int>();

		// *** Loop over all right items to calculate the frequencies
		foreach (var v in right) {
			// *** Try to get the current occurence count, if it does not exist it will be zero
			frequencies.TryGetValue(v, out var currentCount); 

			// *** Increment the frequency for v with 1
			frequencies[v] = currentCount + 1;
		}

		// *** Accumulate the values here
		int a = 0;

		// *** Loop over all values in left
		foreach (var v in left) {
			// *** Try to get the frequency count for v, if it does not exist it will be zero
			frequencies.TryGetValue(v, out var frequencyCount);

			// *** Add it to the accumulation
			a += v * frequencyCount;
		}

		return a;
	}
}