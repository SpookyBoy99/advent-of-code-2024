using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Internal;

public class Program
{
    public static void Main()
    {
        (var rules, var updates) = ReadLines();
        Console.WriteLine(CalculatePart1(rules, updates));
        Console.WriteLine(CalculatePart2(rules, updates));
    }

    public static (Dictionary<int, HashSet<int>>, int[][]) ReadLines()
    {
        // *** Dictionary for the extracted rules
        var rules = new Dictionary<int, HashSet<int>>();

        // *** List for the updates
        var updates = new List<int[]>();

        // *** Allocate variable for the line
        string line;

        // *** Loop over the lines until an empty line is received as that is the end of the rules
        while (!String.IsNullOrEmpty(line = Console.ReadLine()))
        {
            // *** Split the line on the pipe and convert the numbers to integers
            var rule = line.Split("|").Select(num => Int32.Parse(num)).ToArray();

            // *** Check if the rule does not exists in the dictionary
            if (!rules.ContainsKey(rule[0]))
            {
                rules.Add(rule[0], new HashSet<int>());
            }

            // *** Add the value to the list in the dict
            rules[rule[0]].Add(rule[1]);
        }

        // *** Loop over the lines until an empty line is received
        while (!String.IsNullOrEmpty(line = Console.ReadLine()))
        {
            // *** Split the line on commas and convert the numbers to integers
            var numbers = line.Split(",").Select(num => Int32.Parse(num)).ToArray();

            // *** Add the numbers array to the updates list
            updates.Add(numbers);
        }

        // *** Return the rules ane updates
        return (rules, updates.ToArray());
    }

    public static int CalculatePart1(Dictionary<int, HashSet<int>> rules, int[][] updates)
    {
        // *** Accumulate the values
        int a = 0;

        // *** Loop over each update
        foreach (var update in updates)
        {
			if (ValidUpdate(rules, update)) {
				a += update[update.Length / 2];
			}
        }

        return a;
    }

    public static int CalculatePart2(Dictionary<int, HashSet<int>> rules, int[][] updates)
    {
        // *** Accumulate the values
        int a = 0;

		// *** Loop over all the updates
		foreach (var update in updates)
        {
			Console.WriteLine(String.Join(" ", update));
			Console.WriteLine(String.Join(" ", CreateValidUpdate(rules, update)));
			Console.WriteLine("----");
        }

        return a;
    }

    public static bool ValidUpdate(Dictionary<int, HashSet<int>> rules, int[] update)
    {
        // *** Loop over all the numbers in the update except the first
        for (int i = 1; i < update.Length; i++)
        {
            // *** If the page number is not in the rules list, continue to the next
            if (!rules.ContainsKey(update[i]))
            {
                continue;
            }

			// *** Get the ruleset
			var ruleSet = rules[update[i]];

            // *** Loop over all numbers in the update prior to the current number
            for (int j = 0; j < i; j++)
            {
				// *** If of the items before is in the rule set of pages that cannot appear before, the series is invalid
				if (ruleSet.Contains(update[j])) {
					return false;
				}
            }
        }

		return true;
    }

	public static int[] CreateValidUpdate(Dictionary<int, HashSet<int>> rules, int[] update)
    {
		// *** Create a new list that will contain the valid update
		var validUpdate = new List<int>();

        // *** Loop over all the numbers in the update except the first
        for (int i = 1; i < update.Length; i++)
        {
			// var insertAt = validUpdate

            // *** If the page number is not in the rules list, just add it to the valid update list
            if (!rules.ContainsKey(update[i]))
            {
                validUpdate.Add(update[i]);
            }

			// *** Get the ruleset
			var ruleSet = rules[update[i]];

            // *** Loop over all numbers in the update prior to the current number
            for (int j = 0; j < i; j++)
            {
				// *** If of the items before is in the rule set of pages that cannot appear before, insert the current item before that item
				if (ruleSet.Contains(update[j])) {
					validUpdate.Insert(j, update[i]);
					break;
				}
            }
        }

		return validUpdate.ToArray();
    }
}