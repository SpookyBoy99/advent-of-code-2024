using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Internal;

public class Program
{
    public static void Main()
    {
        var entries = ReadLines();
        Console.WriteLine(CalculatePart1(entries));
        Console.WriteLine(CalculatePart2(entries));
    }

    public static List<(ulong result, ulong[] values)> ReadLines()
    {
        // *** Create a list to store the equations
        var equations = new List<(ulong result, ulong[] values)>();

        // *** Allocate variable for the line
        string line;

        // *** Loop over the lines until an empty line is received
        while (!String.IsNullOrEmpty(line = Console.ReadLine()))
        {
            // *** Split the line on the colon
            var tokens = line.Split(": ");

            // *** First token is the result
            var result = UInt64.Parse(tokens[0]);

            // *** Split the second token by space
            var values = tokens[1].Split(' ').Select(v => UInt64.Parse(v)).ToArray();

            // *** Add the equation to the list
            equations.Add((result, values));
        }


        // *** Return the list of equations
        return equations;
    }

    public static ulong CalculatePart1(List<(ulong result, ulong[] values)> entries)
    {
        return CalculateSumOfValid(entries);
    }

    public static ulong CalculatePart2(List<(ulong result, ulong[] values)> entries)
    {
        return CalculateSumOfValid(entries, true);
    }

    public static ulong CalculateSumOfValid(List<(ulong result, ulong[] values)> entries, bool allowConcat = false)
    {
        // *** Accumulate the sum of the correct entries
        ulong a = 0;

        // *** Loop over each entry
        foreach (var entry in entries)
        {
            ulong result = entry.result;
            ulong[] values = entry.values;

            // *** Create a stack for all the options that are going to be tried
            var stack = new Stack<(ulong acc, int i)>();
            stack.Push((values[0], 0));

            // *** Keep looping as long as there are values in the queue
            while (stack.Count > 0)
            {
                // *** Get the next value
                (ulong acc, int i) = stack.Pop();

                // *** Grab the next value
                ulong v = values[i + 1];

                // *** Calculate the values
                ulong add = acc + v;
                ulong mul = acc * v;
                ulong concat = UInt64.Parse(acc.ToString() + v.ToString());

                // *** Check if the next value is the last one
                if (i + 1 == values.Length - 1)
                {
                    // *** Check if adding or multiplying the accumulation yields a correct result
                    if (add == result || mul == result || (allowConcat && concat == result))
                    {
                        // *** Increase the number of valid entries and break
                        a += result;
                        break;
                    }
                }
                else
                {
                    // *** Try adding the next value to the current accumulation
                    if (add <= result)
                    {
                        stack.Push((add, i + 1));
                    }

                    // *** Try multiplying the next value with the current accumulation
                    if (mul <= result)
                    {
                        stack.Push((mul, i + 1));
                    }

                    // *** Try concating the next value with the current accumulation
                    if (allowConcat && concat <= result)
                    {
                        stack.Push((concat, i + 1));
                    }
                }
            }
        }

        return a;
    }
}