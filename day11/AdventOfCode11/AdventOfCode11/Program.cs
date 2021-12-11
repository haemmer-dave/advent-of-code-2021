using System;
using System.IO;
using System.Linq;

namespace AdventOfCode11 {
  public class Program {
    public static void Main(string[] args) {
      string filePath = args[0];

      if (!File.Exists(filePath)) {
        Console.WriteLine("File does not exist. Enter a correct file path!");
        return;
      }

      int[,] energyArray = new int[10, 10];
      int flashes = 0;

      // Read the input file containing the initial tako-energy-grid
      using FileStream fileStream = File.OpenRead(filePath);
      StreamReader reader = new(fileStream);
      for (int i = 0; i < 10; i++) {
        string? line = reader.ReadLine();
        for (int j = 0; j < 10; j++)
          if (line != null) {
            energyArray[i, j] = int.Parse(line[j].ToString());
          }
      }

      reader.Close();

      // Calculate the steps for the tako-energy-grid
      const int steps = 100;
      for (int i = 1; i <= steps; i++) {
        energyArray = CalculateStep(energyArray, out int newFlashes);
        flashes += newFlashes;
      }

      // Print the final tako-energy-grid
      for (int i = 0; i < 10; i++) {
        for (int j = 0; j < 10; j++) Console.Write(energyArray[i, j]);
        Console.WriteLine();
      }

      Console.WriteLine(flashes);
    }

    public static int[,] CalculateStep(int[,] energyArray, out int flashes) {
      bool[,] hasFlashedArray = new bool[10, 10];

      // Simply increase all energy levels by 1
      for (int i = 0; i < 10; i++) {
        for (int j = 0; j < 10; j++) energyArray[i, j] += 1;
      }

      // Iterate through array again. If energy > 9 then flash tako, remember it, and update next neighbors
      for (int i = 0; i < 10; i++) {
        for (int j = 0; j < 10; j++)
          if (energyArray[i, j] > 9) {
            if (hasFlashedArray[i, j]) {
              continue;
            }

            hasFlashedArray[i, j] = true;
            FlashNextNeighbor(i, j, energyArray, hasFlashedArray);
          }
      }

      // Energy levels of flashed takos is reset to 0
      for (int i = 0; i < 10; i++) {
        for (int j = 0; j < 10; j++)
          if (energyArray[i, j] > 9) {
            energyArray[i, j] = 0;
          }
      }

      flashes = hasFlashedArray.Cast<bool>().Count(x => x);
      return energyArray;
    }

    static void FlashNextNeighbor(int row, int column, int[,] energyArray, bool[,] hasFlashedArray) {
      // Calculate neighbor coordinates, check if within boundary, check if flashed. If not flashed, increase energy level by 1
      for (int r = row - 1; r <= row + 1; r++) {
        for (int c = column - 1; c <= column + 1; c++) {
          if (r is < 0 or > 9 || c is < 0 or > 9) {
            continue;
          }

          if (hasFlashedArray[r, c]) {
            continue;
          }

          energyArray[r, c] += 1;
          if (energyArray[r, c] > 9) {
            hasFlashedArray[r, c] = true;
            FlashNextNeighbor(r, c, energyArray, hasFlashedArray);
          }
        }
      }
    }
  }
}