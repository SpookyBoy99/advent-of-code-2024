using System;
using System.Collections.Generic;
using System.Linq;
using Internal;

public class Program
{
    public static void Main()
    {
        (var fileBlocks, var emptyBlocks) = ReadLine();

        Console.WriteLine(CalculatePart1(fileBlocks, emptyBlocks));
        Console.WriteLine(CalculatePart2(fileBlocks, emptyBlocks));
    }

    public static (int[], int[]) ReadLine()
    {
        // *** Create one list to keep track of the file blocks and one list to keep track of the empty spaces
        var fileBlocks = new List<int>();
        var emptyBlocks = new List<int>();

        // *** Read a single line
        string line = Console.ReadLine();

        // *** Loop over the chars in the line
        for (int i = 0; i < line.Length; i++)
        {
            if (i % 2 == 0)
            {
                fileBlocks.Add(line[i] - '0');
            }
            else
            {
                emptyBlocks.Add(line[i] - '0');
            }
        }

        // ***  Return the antenna locations and the map size
        return (fileBlocks.ToArray(), emptyBlocks.ToArray());
    }

    public static ulong CalculatePart1(int[] fileBlocks, int[] emptyBlocks)
    {
        // *** Value for accumulating the checksum
        ulong a = 0;

        // *** Counter for the current file/empty block index
        int fileIndex = 0;

        // *** Counter for the last file index
        int lastFileIndex = fileBlocks.Length - 1;

        // *** Counter for the current block
        int currentBlock = 0;

        // *** Keep track whether we are looking at a file or an empty block
        bool isFile = true;

        // *** Keep looping
        while (fileIndex <= lastFileIndex)
        {
            // *** Check if we are dealing with a file block or an empty block
            if (isFile)
            {
                // *** If there are no blocks left for the current index, switch to empty block and continue
                if (fileBlocks[fileIndex] == 0) {
                    isFile = false;
                    continue;
                }

                // *** Update the checksum accumulator
                a += (ulong)(fileIndex * currentBlock);

                // *** Decrement the number of remaining blocks for the current file
                fileBlocks[fileIndex] -= 1;
            }
            else
            {
                // *** If there are no blocks left in the current empty stretch, increment the file index and look at the next file
                if (emptyBlocks[fileIndex] == 0) {
                    isFile = true;
                    fileIndex += 1;
                    continue;
                }

                // *** If there are no block for the current last file, decrement the index and continue
                if (fileBlocks[lastFileIndex] == 0) {
                    lastFileIndex -= 1;
                    continue;
                }

                // *** Update the checksum accumulator
                a += (ulong)(lastFileIndex * currentBlock);

                // *** Decrement the number of remaining emtpy blocks
                emptyBlocks[fileIndex] -= 1;

                // *** Decrement the number of remaining blocks for the file
                fileBlocks[lastFileIndex] -= 1;
            }

            currentBlock += 1;
        }

        return a;
    }

    public static int CalculatePart2(int[] fileBlocks, int[] emptyBlocks)
    {
        return 0;
    }
}