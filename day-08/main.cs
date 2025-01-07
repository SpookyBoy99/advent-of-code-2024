using System;
using System.Collections.Generic;
using System.Linq;
using Internal;

public class Program
{
    public static void Main()
    {
        (var antennas, var mapSize) = ReadLines();

        Console.WriteLine(CalculatePart1(antennas, mapSize));
        Console.WriteLine(CalculatePart2(antennas, mapSize));
    }

    public static (Dictionary<char, List<Tuple<int, int>>>, (int width, int height)) ReadLines()
    {
        // *** Create a dictionary to store the antenna locations
        var antennas = new Dictionary<char, List<Tuple<int, int>>>();
        var mapSize = (width: 0, height: 0);

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
                var currentPos = Tuple.Create(i, mapSize.height - 1);

                // *** Check if the current char is not 'empty'
                if (line[i] != '.')
                {
                    // *** Check if the antenna does not exists in the dictionary
                    if (!antennas.ContainsKey(line[i]))
                    {
                        antennas.Add(line[i], new List<Tuple<int, int>>());
                    }

                    // *** Add the current antenna to the list
                    antennas[line[i]].Add(currentPos);
                }
            }
        }


        // ***  Return the antenna locations and the map size
        return (antennas, mapSize);
    }

    public static int CalculatePart1(Dictionary<char, List<Tuple<int, int>>> antennas, (int width, int height) mapSize)
    {
        var antinodes = CalculateAntinodes(antennas, mapSize, true);

        PlotMap(antennas, mapSize, antinodes);

        return antinodes.Count;
    }

    public static int CalculatePart2(Dictionary<char, List<Tuple<int, int>>> antennas, (int width, int height) mapSize)
    {
        var antinodes = CalculateAntinodes(antennas, mapSize, false);

        PlotMap(antennas, mapSize, antinodes);

        return antinodes.Count;
    }

    public static HashSet<(int x, int y)> CalculateAntinodes(Dictionary<char, List<Tuple<int, int>>> antennas, (int width, int height) mapSize, bool perfectDistanceOnly)
    {
        // *** Create a hashset that will contain all the antinode locations
        var antinodes = new HashSet<(int x, int y)>();

        // *** Loop over all coordinates and calculate the distance to each tower
        for (int x = 0; x < mapSize.width; x++)
        {
            for (int y = 0; y < mapSize.height; y++)
            {
                // *** Loop over all antenna frequencies
                foreach (var antennaGroup in antennas.Values)
                {
                    // *** Loop over all antennas in the group except the last one
                    for (int i = 0; i < antennaGroup.Count - 1; i++)
                    {
                        // *** If the current antenna is at the exact spot we are looking, skip it
                        if (antennaGroup[i].Equals(Tuple.Create(x, y)))
                        {
                            if (!perfectDistanceOnly)
                            {
                                antinodes.Add((x, y));
                            }

                            continue;
                        }

                        // *** Calculate the vector towards antennta 1
                        var antenna1Vec = (x: antennaGroup[i].Item1 - x, y: antennaGroup[i].Item2 - y);
                        var antenna1Dist = Math.Sqrt(Math.Pow(antenna1Vec.x, 2) + Math.Pow(antenna1Vec.y, 2));

                        // *** Loop over all antenna combinations that have not been tested yet
                        for (int j = i + 1; j < antennaGroup.Count; j++)
                        {
                            // *** If the current antenna is at the exact spot we are looking, skip it
                            if (antennaGroup[j].Equals(Tuple.Create(x, y)))
                            {
                                if (!perfectDistanceOnly)
                                {
                                    antinodes.Add((x, y));
                                }

                                continue;
                            }

                            // *** Calculate the vector towards antennta 2
                            var antenna2Vec = (x: antennaGroup[j].Item1 - x, y: antennaGroup[j].Item2 - y);
                            var antenna2Dist = Math.Sqrt(Math.Pow(antenna2Vec.x, 2) + Math.Pow(antenna2Vec.y, 2));

                            // *** Calculate the cos of the angle
                            var cosAntennaAngle = (antenna1Vec.x * antenna2Vec.x + antenna1Vec.y * antenna2Vec.y) / (antenna1Dist * antenna2Dist);

                            // *** If the cos of the angle is not 1 or -1, the vectors are not in line
                            if (Math.Abs(cosAntennaAngle - 1) > 1E-15)
                            {
                                continue;
                            }

                            // *** Calculate the ratio between the antenna distances
                            var frac = antenna1Dist / antenna2Dist;

                            // *** Check if the distance is optimal for a antinode
                            if (!perfectDistanceOnly || frac == 2 || frac == 0.5)
                            {
                                antinodes.Add((x, y));
                            }
                        }
                    }
                }
            }
        }

        return antinodes;
    }

    public static void PlotMap(Dictionary<char, List<Tuple<int, int>>> antennas, (int width, int height) mapSize, HashSet<(int x, int y)> antinodes)
    {
        var map = Enumerable.Range(0, mapSize.height).Select(i => new string('.', mapSize.width).ToCharArray()).ToArray();

        foreach (var node in antinodes)
        {
            map[node.y][node.x] = '#';
        }

        foreach (var antenna in antennas)
        {
            foreach (var pos in antenna.Value)
            {
                map[pos.Item2][pos.Item1] = antenna.Key;
            }
        }

        foreach (var row in map)
        {
            Console.WriteLine(row);
        }
    }
}