using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using Internal;

public class Program
{
    enum Direction : int
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    static (int x, int y)[] directions = { (0, -1), (1, 0), (0, 1), (-1, 0) };

    public static void Main()
    {
        (var map, var mapSize, var startPos) = ReadLines();
        Console.WriteLine(CalculatePart1(map, mapSize, startPos));
        Console.WriteLine(CalculatePart2(map, mapSize, startPos));
    }

    public static (HashSet<(int x, int y)>, (int width, int height), (int x, int y)) ReadLines()
    {
        // *** Create a set of blocked coordinats
        var map = new HashSet<(int x, int y)>();
        var mapSize = (width: 0, height: 0);
        var startPos = (x: 0, y: 0);

        // *** Allocate variable for the line
        string line;

        // *** Loop over the lines until an empty line is received
        while (!String.IsNullOrEmpty(line = Console.ReadLine()))
        {
            // *** Update the map size
            mapSize = (width: Math.Max(mapSize.width, line.Length), height: mapSize.height + 1);

            // *** Loop over all the chars of the row string
            for (int i = 0; i < line.Length; i++)
            {
                // *** Calculate the current pos
                var currentPos = (x: i, y: mapSize.height - 1);

                // *** Mark the current position as blocked if the item is an hashtag
                if (line[i] == '#')
                {
                    map.Add(currentPos);
                }

                // *** Check if the current position is the start position as well
                if (line[i] == '^')
                {
                    startPos = currentPos;
                }
            }
        }


        // *** Return a tuple with the map of blocked positions and the start position
        return (map, mapSize, startPos);
    }

    public static int CalculatePart1(HashSet<(int x, int y)> map, (int width, int height) mapSize, (int x, int y) startPos)
    {
        // *** Get the number of visited spaces
        return CalculateVisited(map, mapSize, startPos).visited.Count;
    }

    public static int CalculatePart2(HashSet<(int x, int y)> map, (int width, int height) mapSize, (int x, int y) startPos)
    {
        // *** Accumulate the number of infinite paths
        int a = 0;

        // *** Loop over all visited locations and try to add an object there
        foreach (var visited in CalculateVisited(map, mapSize, startPos).visited) {
            // *** Cannot place an item at the start position
            if (visited.Equals(startPos)) {
                continue;
            }

            // *** Create a copy of the map hashset
            var newMap = new HashSet<(int x, int y)>(map);

            // *** Add the newly obstucted point
            newMap.Add(visited);

            // *** Check if this map results in an infinite loop
            if (CalculateVisited(newMap, mapSize, startPos).isInfinite) {
                a += 1;
            }
        }

        return a;
    }

    public static (HashSet<(int x, int y)> visited, bool isInfinite) CalculateVisited(HashSet<(int x, int y)> map, (int width, int height) mapSize, (int x, int y) startPos)
    {
        // *** Create a variable to keep track of all states (position + direction) to detect loops
        var previousStates = new HashSet<(int x, int y, int dir)>();

        // *** Create a variable to accumulate all the visited positions
        var visited = new HashSet<(int x, int y)>();

        // *** Set the current post to the start pos
        var currentPos = startPos;

        // *** Set current direction to up
        var currentDir = (int)Direction.Up;

        // *** Keep looping until broken
        while (true)
        {
            // *** Get the current position state
            var currentState = (currentPos.x, currentPos.y, currentDir);

            // *** Check if the state has occurred before
            if (previousStates.Contains(currentState)) {
                // *** Stuck in a loop
                return (visited, true);
            }
            
            // *** Add the current position to the list of visited positions
            previousStates.Add(currentState);

            // *** Add the current position to the list of visited positions
            visited.Add(currentPos);

            // *** Calculate the step goal
            var targetPos = (x: currentPos.x + directions[currentDir].x, y: currentPos.y + directions[currentDir].y);

            // *** Check if the target position is within the map, otherwise exit
            if (targetPos.x < 0 || targetPos.x >= mapSize.width || targetPos.y < 0 || targetPos.y >= mapSize.height)
            {
                break;
            }

            // *** Check if the target position is blocked
            if (map.Contains(targetPos))
            {
                // *** Rotate and try again
                currentDir = (currentDir + 1) % directions.Length;
            }
            else
            {
                // *** Update the current position
                currentPos = targetPos;
            }
        }

        return (visited, false);
    }
}