using System;
using System.Collections.Generic;
using System.Linq;
					
public class Program
{
	public static void Main()
	{
		var values = ReadLines();
		Console.WriteLine(CalculatePart1(values));
		Console.WriteLine(CalculatePart2(values));
	}

	public static int[][] ReadLines() {
		// *** Create a list for the values
		var values = new List<int[]>();

		// *** Allocate variable for the line
		string line;

		// *** Loop over the lines until an empty line is received
		while (!String.IsNullOrEmpty(line = Console.ReadLine())) {
			// *** Split the string
			string[] numbers = line.Split(" ");

			// *** Add the array of numbers to the list
			values.Add(numbers.Select(s => Int32.Parse(s)).ToArray());
		}

		// *** Return the list as an array
		return values.ToArray();
	}

	public static int CalculatePart1(int[][] values) {
		return CalculateSafeSeries(values, false);
	}

	public static int CalculatePart2(int[][] values) {
		return CalculateSafeSeries(values, true);
	}

	public static int CalculateSafeSeries(int[][] values, bool enableDampener) {
		// *** Count the number of safe series here
		int c = 0;

		// *** Loop over all the series
		foreach (var v in values) {
			// *** Check whether a series is safe
			if (enableDampener ? SeriesIsSafeWithDampening(v) : SeriesIsSafe(v)) {
				c += 1;
			}
		}

		return c;
	}

	public static bool SeriesIsSafe(int[] series) {
		// *** If there are less then 2 items it is always safe
		if (series.Length < 2) {
			return true;
		}

		// *** Determine if series is ascending or descending
		bool asc = series[0] > series[1];

		// *** Loop over all values - 1
		for (int i = 0; i < series.Length - 1; i += 1) {
			// *** Check if the jump to the next variable is within limits
			if (!ValuesSafe(series[i], series[i + 1], asc)) {
				return false;
			}
		}

		return true;
	}

	public static bool SeriesIsSafeWithDampening(int[] series) {
		// *** If there are less then 2 items it is always safe
		if (series.Length < 2) {
			return true;
		}

		// *** Ignore one of every series
		for (int j = 0; j < series.Length; j += 1) {
			// *** Variable to determine if series should be descending or ascending
			bool asc = j == 0 ? series[1] > series[2] : j == 1 ? series[0] > series[2] : series[0] > series[1];

			// *** Keep track whether this iteration is safe
			bool seriesIsSafe = true;

			// *** Loop over all values - 1
			for (int i = 0; i < series.Length - 1; i += 1) {
				// *** If the current value is the skip value, continue the loop
				if (i == j) {
					continue;
				}

				// *** Check if the next value should be skipped
				if (i + 1 == j) {
					// *** If the next value is the skip value, compare with the value after that
					if (i + 2 == series.Length || ValuesSafe(series[i], series[i + 2], asc)) {
						continue;
					}
				} else {
					// *** If this pair is safe, continue
					if (ValuesSafe(series[i], series[i + 1], asc)) {
						continue;
					}
				}

				// *** If none of the options work, break the loop as this series is valid
				seriesIsSafe = false;
				break;
			}

			if (seriesIsSafe) {
				return true;
			}
		}

		return false;
	}

	public static bool ValuesSafe(int low, int high, bool asc) {
		// *** Calculate the difference between the current and next value
		var diff = low - high;

		// *** Flip if ascending series
		if (!asc) {
			diff = -diff;
		}

		// *** Break this loop if it is in range
		return diff >= 1 && diff <= 3;
	}
}