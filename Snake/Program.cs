using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    // Class for X,Y Coords
    public class ScreenPoint
    {
        // X is up, X starts from top to bottom
        public int X { get; set; }

        // Y is sideways, from Left to right
        public int Y { get; set; }
    }

    internal class Program
    {
        public static List<ScreenPoint> SnakeBody = new List<ScreenPoint>();
        public static string direction = "right";
        public static bool Alive = true;
        public static int Add = 0;
        public static ScreenPoint Food;


        private static void Main(string[] args)
        {
            // Starter Stuff
            Console.CursorVisible = false;
            Console.BufferHeight = 30;
            Console.Write("Press to start");
            Console.ReadKey();
            Console.SetCursorPosition(0, 0);
            Console.Write("                 ");
            Task.Factory.StartNew(() => { Game(); });

            while (true)
            {
                while (Alive == true)
                {
                    ConsoleKeyInfo Input = Console.ReadKey(true);

                    switch (Input.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (SnakeBody[1].Y != SnakeBody[0].Y - 1)
                            {
                                direction = "left";
                            }
                            break;

                        case ConsoleKey.RightArrow:
                            if (SnakeBody[1].Y != SnakeBody[0].Y + 1)
                            {
                                direction = "right";
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            if (SnakeBody[1].X != SnakeBody[0].X - 1)
                            {
                                direction = "up";
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            if (SnakeBody[1].X != SnakeBody[0].X + 1)
                            {
                                direction = "down";
                            }
                            break;

                        case ConsoleKey.A:
                            Add++;
                            break;
                    }
                    Console.Title = "Snake | Score: " + SnakeBody.Count;
                }
            }
        }

        public static void Game()
        {
            Console.Write("game Started");
            GameStartUp();

            Random Rnd = new Random();

            // Main Game Function
            while (Alive == true)
            {
                // Body Calculations for next frame before clearing
                int i = SnakeBody.Count - 1;

                if (Add > 0)
                {
                    SnakeBody.Add(new ScreenPoint
                    { X = 2000, Y = 2000 });
                    Add--;
                }

                while (i != 0)
                {
                    SnakeBody[i].X = SnakeBody[i - 1].X;
                    SnakeBody[i].Y = SnakeBody[i - 1].Y;
                    i--;
                }

                switch (direction)
                {
                    case "up":
                        SnakeBody[0].X -= 1;
                        break;

                    case "down":
                        SnakeBody[0].X += 1;
                        break;

                    case "left":
                        SnakeBody[0].Y -= 1;
                        break;

                    case "right":
                        SnakeBody[0].Y += 1;
                        break;
                }

                if (SnakeBody[0].X == Food.X && SnakeBody[0].Y == Food.Y)
                {
                    Console.SetCursorPosition(Food.Y, Food.X);
                    Console.Write(" ");
                    Add += 3;
                    Food.X = Rnd.Next(0, 30);
                    Food.Y = Rnd.Next(0, 100);
                }

                // Delay Timer and clear screen
                Thread.Sleep(100);
                Console.Clear();

                // Frame Render
                Console.SetCursorPosition(Food.Y, Food.X);
                Console.Write("#");

                try
                {
                    SnakeBody.ForEach(delegate (ScreenPoint Loc)
                    {
                        if (Loc.X != 2000)
                        {
                            Console.SetCursorPosition(Loc.Y, Loc.X);
                            Console.Write("O");
                        }
                    });
                }
                catch
                {
                    Alive = false;
                    DeathMenu();
                    break;
                }
            }
        }

        /// <summary>
        /// Death Menu Operations
        /// </summary>
        public static void DeathMenu()
        {
            Console.SetCursorPosition(52, 15);
            Console.WriteLine("YOU ARE DEAD");
            Console.SetCursorPosition(47, 16);
            Console.WriteLine("Press any Key to restart");
            Console.ReadKey(true);
            Console.WriteLine("                         ");
            Console.SetCursorPosition(47, 16);
            Console.WriteLine("                                  ");
            SnakeBody.Clear();
            Task.Factory.StartNew(() => { Game(); });
            Alive = true;
        }

        /// <summary>
        /// Game Initialisation stuff
        /// </summary>
        public static void GameStartUp()
        {
            Random Rnd = new Random();
            Food = new ScreenPoint { X = Rnd.Next(0, 35), Y = Rnd.Next(0, 100) };

            SnakeBody.Add(new ScreenPoint
            { X = 20, Y = 20 });

            SnakeBody.Add(new ScreenPoint
            { X = 19, Y = 20 });

            SnakeBody.Add(new ScreenPoint
            { X = 18, Y = 20 });

            SnakeBody.Add(new ScreenPoint
            { X = 17, Y = 20 });
        }
    }
}