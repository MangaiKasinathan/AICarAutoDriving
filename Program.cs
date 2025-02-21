using System;
using System.Collections.Generic;

public class AutoDrivingCar
{
    public static void Main()
    {
        // Read field dimensions
        string[] fieldDimensions = Console.ReadLine().Split();
        int width = int.Parse(fieldDimensions[0]);
        int height = int.Parse(fieldDimensions[1]);

        // Initialize cars list
        var cars = new List<Car>();

        // Read cars and their commands
        while (true)
        {
            string carName = Console.ReadLine();
            if (string.IsNullOrEmpty(carName)) break;  // End of input

            string[] initialPosition = Console.ReadLine().Split();
            int x = int.Parse(initialPosition[0]);
            int y = int.Parse(initialPosition[1]);
            char direction = char.Parse(initialPosition[2]);
            string commands = Console.ReadLine();

            // Create a new car and add to list
            cars.Add(new Car(carName, x, y, direction, commands, width, height));
        }

        // Execute the movement and check for collisions
        var collision = CheckForCollisions(cars);
        if (collision != null)
        {
            Console.WriteLine($"{collision.Item1} {collision.Item2} {collision.Item3}");
        }
        else
        {
            Console.WriteLine("no collision");
        }
    }

    public static Tuple<string, string, int> CheckForCollisions(List<Car> cars)
    {
        // Dictionary to store positions and the car names that occupy them
        var positions = new Dictionary<string, string>();

        // Loop through each car and simulate its movement step by step
        for (int step = 0; ; step++)
        {
            foreach (var car in cars)
            {
                var position = car.Move(step);
                string positionKey = $"{position.Item1},{position.Item2}";

                // Check for collision
                if (positions.ContainsKey(positionKey))
                {
                    return new Tuple<string, string, int>(positions[positionKey], car.Name, step + 1);
                }

                positions[positionKey] = car.Name;
            }

            // Check if all cars have finished their commands
            bool allFinished = true;
            foreach (var car in cars)
            {
                if (!car.HasFinished)
                {
                    allFinished = false;
                    break;
                }
            }

            if (allFinished) break;
        }

        return null;
    }
}

public class Car
{
    public string Name { get; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public char Direction { get; private set; }
    private string Commands { get; }
    private int Width { get; }
    private int Height { get; }
    private int CurrentCommandIndex { get; set; }
    public bool HasFinished => CurrentCommandIndex >= Commands.Length;

    private static readonly char[] Directions = { 'N', 'E', 'S', 'W' };
    private static readonly int[,] Moves = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

    public Car(string name, int x, int y, char direction, string commands, int width, int height)
    {
        Name = name;
        X = x;
        Y = y;
        Direction = direction;
        Commands = commands;
        Width = width;
        Height = height;
        CurrentCommandIndex = 0;
    }

    public Tuple<int, int> Move(int step)
    {
        if (step < CurrentCommandIndex) return new Tuple<int, int>(X, Y); // Skip if we already processed this step

        if (CurrentCommandIndex >= Commands.Length) return new Tuple<int, int>(X, Y); // If finished all commands

        // Get the command at this step
        char command = Commands[CurrentCommandIndex];

        switch (command)
        {
            case 'L':
                TurnLeft();
                break;
            case 'R':
                TurnRight();
                break;
            case 'F':
                MoveForward();
                break;
        }

        CurrentCommandIndex++;
        return new Tuple<int, int>(X, Y);
    }

    private void TurnLeft()
    {
        int dirIndex = Array.IndexOf(Directions, Direction);
        Direction = Directions[(dirIndex + 3) % 4]; // Turn left (counter-clockwise)
    }

    private void TurnRight()
    {
        int dirIndex = Array.IndexOf(Directions, Direction);
        Direction = Directions[(dirIndex + 1) % 4]; // Turn right (clockwise)
    }

    private void MoveForward()
    {
        int dirIndex = Array.IndexOf(Directions, Direction);
        int newX = X + Moves[dirIndex, 0];
        int newY = Y + Moves[dirIndex, 1];

        // Check if within boundaries
        if (newX >= 0 && newX < Width && newY >= 0 && newY < Height)
        {
            X = newX;
            Y = newY;
        }
    }
}
