using Snake.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        private static int ColBoundary = Console.WindowWidth;
        private static int RowBoundary = Console.WindowHeight;

        private static bool isGameRunning = true;

        private static NodeObject _food;
        private static Random _random = new Random();
        
        private static NodeObject _oldTail = new NodeObject();
        private static NodeObject _oldHead = new NodeObject();
        private static List<NodeObject> _obstacles;

        static void Main(string[] args)
        {
            RunGameLoop();
        }

        private static void RunGameLoop()
        {
            Console.CursorVisible = false;
            var snake = new Model.Snake();
            Direction snakeDirection = Direction.Right;

            DrawObstacles();

            while (isGameRunning)
            {
                ClearCurrentSnakePartsFromConsole(snake);

                DrawSnake(snake);
                DrawFood();

                snakeDirection = GetDirection(snakeDirection);
                snake.Move(snakeDirection);

                CheckSnakeCollision(snake);
                
                Thread.Sleep(snake.SnakeSpeedMilliSeconds);
            }
        }

        private static Direction GetDirection(Direction currentDirection)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo userInput = Console.ReadKey();
                switch (userInput.Key)
                {
                    case ConsoleKey.RightArrow:
                        return Direction.Right;
                    case ConsoleKey.LeftArrow:
                        return Direction.Left;
                    case ConsoleKey.UpArrow:
                        return Direction.Up;
                    case ConsoleKey.DownArrow:
                        return Direction.Down;
                    default: return Direction.Right;
                }
            }

            return currentDirection;
        }

        private static void CheckSnakeCollision(Model.Snake snake)
        {
            SnakeNode head = snake.Head;

            // check field boundaries
            bool isOutOfLeftTopBorders = head.Row < 0 || head.Col < 0;
            bool isOutOfRightBottomBorders = head.Row > RowBoundary || head.Col >= ColBoundary;
            if (isOutOfLeftTopBorders || isOutOfRightBottomBorders)
            {
                GameOver();
            }


            // check food collision
            if (_food != null && head.Row == _food.Row && head.Col == _food.Col)
            {
                _food = null;
                snake.Grow();
            }

            // check self collision
            var currentNode = head.Prev;
            while (currentNode != null)
            {
                if (head.Row == currentNode.Row && head.Col == currentNode.Col)
                {
                    GameOver();
                }

                currentNode = currentNode.Prev;
            }

            // check obstacles collision
            foreach (var obstacle in _obstacles)
            {
                if (head.Row == obstacle.Row && head.Col == obstacle.Col)
                {
                    GameOver();
                }
            }
        }

        private static void ClearCurrentSnakePartsFromConsole(Model.Snake snake)
        {
            PrintOnConsole(_oldTail.Row, _oldTail.Col, Constants.EMPTY_SPACE);
            PrintOnConsole(_oldHead.Row, _oldHead.Col, Constants.SNAKE_BODY);

            var snakeHead = snake.BodyParts.First();
            PrintOnConsole(snakeHead.Row, snakeHead.Col, snakeHead.Symbol);
        }

        private static void DrawSnake(Model.Snake snake)
        {
            _oldTail = snake.BodyParts.Last();
            _oldHead = snake.BodyParts.First();

            foreach (var snakeBodyPart in snake.BodyParts)
            {
                PrintOnConsole(snakeBodyPart.Row, snakeBodyPart.Col, snakeBodyPart.Symbol, ConsoleColor.Yellow);
            }
        }

        private static void DrawFood()
        {
            if (_food == null)
            {
                _food = new NodeObject
                {
                    Row = _random.Next(0, RowBoundary - 1),
                    Col = _random.Next(0, ColBoundary - 1)
                };
            }
            
            PrintOnConsole(_food.Row, _food.Col, Constants.FOOD, ConsoleColor.Green);
        }

        private static void GameOver()
        {
            isGameRunning = false;
            Console.Clear();
            Console.WriteLine("Game over");
        }

        private static void DrawObstacles()
        {
            _obstacles = new List<NodeObject>(Constants.OBSTACLES_COUNT);

            for (int i = 0; i < Constants.OBSTACLES_COUNT; i++)
            {
                int obstacleCol = _random.Next(0, ColBoundary);
                int obstacleRow = _random.Next(0, RowBoundary);

                var obstacle = new NodeObject
                {
                    Row = obstacleRow,
                    Col = obstacleCol
                };
                _obstacles.Add(obstacle);
                
                PrintOnConsole(obstacleRow, obstacleCol, Constants.OBSTACLE, ConsoleColor.Red);
            }
        }

        private static void PrintOnConsole(int row, int col, char symbol, ConsoleColor consoleColor = ConsoleColor.White)
        {
            // for some reason colors degrades performance and lowers the speed of the snake with time
            Console.SetCursorPosition(col, row);
            Console.ForegroundColor = consoleColor;
            Console.Write(symbol);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
