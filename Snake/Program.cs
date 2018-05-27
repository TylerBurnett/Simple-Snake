using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    #region Game class

    internal class Program
    {
        public static int BodyQueue = 0;
        public static ScreenPoint Food;
        public static bool PlayerAlive = true;
        public static Object PlayerDirection = new Left();
        public static List<ScreenPoint> SnakeBody = new List<ScreenPoint>();

        /// <summary>
        /// Key Input thread, and program insertion thread
        /// </summary>
        private static void Main()
        {
            // Starter Stuff
            Console.CursorVisible = false;
            Console.BufferHeight = 50;
            Console.Write("Press to start");

            //waiting for player
            Console.ReadKey();
            Task.Factory.StartNew(() => { Game(); });

            // Key input loop on main thread
            while (true)
            {
                while (PlayerAlive)
                {
                    ConsoleKeyInfo Input = Console.ReadKey(true);

                    switch (Input.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (SnakeBody[1].Y != SnakeBody[0].Y - 1)
                            {
                                PlayerDirection = new Left();
                            }
                            break;

                        case ConsoleKey.RightArrow:
                            if (SnakeBody[1].Y != SnakeBody[0].Y + 1)
                            {
                                PlayerDirection = new Right();
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            if (SnakeBody[1].X != SnakeBody[0].X - 1)
                            {
                                PlayerDirection = new Up();
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            if (SnakeBody[1].X != SnakeBody[0].X + 1)
                            {
                                PlayerDirection = new Down();
                            }
                            break;

                        case ConsoleKey.A:
                            BodyQueue++;
                            break;
                    }

                    Console.Title = "Snake | Score: " + SnakeBody.Count;
                }
            }
        }

        /// <summary>
        /// Calculates the snakes body position for frame render
        /// </summary>
        /// <param name="SnakeBody">The snake body</param>
        /// <returns>The calculated frame</returns>
        public static List<ScreenPoint> CalculateFrame(List<ScreenPoint> SnakeBody, ScreenPoint Food)
        {
            Random Rnd = new Random();

            if (BodyQueue > 0)
            {
                SnakeBody.Add(new ScreenPoint
                { X = 2000, Y = 2000 });
                BodyQueue--;
            }

            for (int i = SnakeBody.Count - 1; i != 0; i--)
            {
                SnakeBody[i].X = SnakeBody[i - 1].X;
                SnakeBody[i].Y = SnakeBody[i - 1].Y;
            }

            SnakeBody[0] = ((DirectionBase)PlayerDirection).Calculate(SnakeBody[0]);

            if (SnakeBody[0].X == Food.X && SnakeBody[0].Y == Food.Y)
            {
                Console.SetCursorPosition(Food.Y, Food.X);
                Console.Write("");
                BodyQueue += 3;
                Food.X = Rnd.Next(0, Console.BufferHeight);
                Food.Y = Rnd.Next(0, Console.BufferWidth);
            }

            return SnakeBody;
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
            PlayerAlive = true;
            Task.Factory.StartNew(() => { Game(); });
        }

        /// <summary>
        /// Game thread
        /// </summary>
        public static void Game()
        {
            InitilizeSnake();

            // Main Game Function
            while (PlayerAlive)
            {
                // Body Calculations for next frame before clearing
                SnakeBody = CalculateFrame(SnakeBody, Food);

                // Frame Render
                RenderFrame(SnakeBody, Food);

                // Delay Timer and clear screen
                Thread.Sleep(100);
                Console.Clear();
            }

            if (PlayerAlive == false)
            {
                DeathMenu();
            }
        }

        /// <summary>
        /// Game Initialisation stuff
        /// </summary>
        public static void InitilizeSnake()
        {
            Console.Clear();

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

        /// <summary>
        /// Renders the frame on the console screen
        /// </summary>
        /// <param name="SnakeBody">The snake body for render</param>
        public static void RenderFrame(List<ScreenPoint> SnakeBody, ScreenPoint Food)
        {
            Console.SetCursorPosition(Food.Y, Food.X);
            Console.Write("#");

            try
            {
                foreach (ScreenPoint Loc in SnakeBody)
                {
                    if (Loc.X != 2000)
                    {
                        Console.SetCursorPosition(Loc.Y, Loc.X);
                        Console.Write("O");
                    }
                }
            }
            catch (Exception E)
            {
                Debug.WriteLine(E.Message + "\n" + E.Source + "\n" + E.StackTrace);
                PlayerAlive = false;
            }
        }
    }

    #endregion Game class

    #region Data Classes

    public abstract class DirectionBase
    {
        public abstract ScreenPoint Calculate(ScreenPoint Head);
    }

    public class Down : DirectionBase
    {
        public override ScreenPoint Calculate(ScreenPoint Head)
        {
            Head.X += 1;
            return Head;
        }
    }

    public class Left : DirectionBase
    {
        public override ScreenPoint Calculate(ScreenPoint Head)
        {
            Head.Y -= 1;
            return Head;
        }
    }

    public class Right : DirectionBase
    {
        public override ScreenPoint Calculate(ScreenPoint Head)
        {
            Head.Y += 1;
            return Head;
        }
    }

    // Class for X,Y Coords
    public class ScreenPoint
    {
        // X is up, X starts from top to bottom
        public int X { get; set; }

        // Y is sideways, from Left to right
        public int Y { get; set; }
    }

    public class Up : DirectionBase
    {
        public override ScreenPoint Calculate(ScreenPoint Head)
        {
            Head.X -= 1;
            return Head;
        }
    }

    #endregion Data Classes
}