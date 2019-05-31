using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Snake Game!");
            Console.CursorVisible = false;
            var snake = new Snake();
            //snake.PrintAcsciiCodes();
            snake.GameSetup();
        }
    }

    class Snake
    {
        enum SnakeDirection { Left, Right, Up, Down };
        SnakeDirection direction = SnakeDirection.Left;
        Boolean isPlaying = false;
        List<Coordinate> snake = new List<Coordinate>();
        Coordinate food = new Coordinate(0, 0);
        int score = 0;
        int counter = 0;
        Timer timer;

        public void GameSetup()
        {
            Console.Clear();
            PrintFrame();
            SetScore(0);
            score = 0;
            counter = 0;
            SetTime("00:00");
            StartPlaying();

            var ch = Console.ReadKey(true).Key;
            if (ch == ConsoleKey.Enter)
            {
                GameSetup();
            }
        }
        public void StartPlaying()
        {
            isPlaying = true;
            snake = new List<Coordinate>();
            PrepareSnake();
            FeedSnake();
            StartTimer();
            direction = SnakeDirection.Left;
            while (isPlaying)
            {
                MoveSnake();
                ControlKeys();
                Console.ForegroundColor = ConsoleColor.Black;
                Thread.Sleep(100);
            }
        }
        public void StartTimer()
        {
            timer = new Timer(TimerCallback, null, 1000, Timeout.Infinite);
        }
        private void TimerCallback(Object state)
        {
            counter += 1;
            SetTime(TimeSpan.FromSeconds(counter).ToString());
            timer.Change(1000, Timeout.Infinite);
        }

        public void StopPlaying()
        {
            isPlaying = false;
            snake.RemoveAll(delegate(Coordinate c)
            {
                return true;
            });
            timer.Dispose();
        }
        public void PrepareSnake()
        {
            snake.Add(new Coordinate(40, 10));
            snake.Add(new Coordinate(41, 10));
            snake.Add(new Coordinate(42, 10));
        }
        public void ControlKeys()
        {
            if (Console.KeyAvailable)
            {
                var ch = Console.ReadKey(true).Key;
                switch (ch)
                {
                    case ConsoleKey.UpArrow:
                        if (direction != SnakeDirection.Down)
                        {
                            direction = SnakeDirection.Up;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != SnakeDirection.Up)
                        {
                            direction = SnakeDirection.Down;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != SnakeDirection.Right)
                        {
                            direction = SnakeDirection.Left;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != SnakeDirection.Left)
                        {
                            direction = SnakeDirection.Right;
                        }
                        break;
                }
            }
        }

        public void MoveSnake()
        {
            Coordinate head = snake[0];
            switch (direction)
            {
                case SnakeDirection.Left:
                    var newHeadL = new Coordinate(head.x - 1, head.y);
                    if (head.x == 1 || SelfHit(newHeadL))
                    {
                        GameOver();
                    }
                    else
                    {
                        snake.Insert(0, newHeadL);
                        WriteSnake(CheckFood(newHeadL));
                    }
                    break;
                case SnakeDirection.Right:
                    var newHeadR = new Coordinate(head.x + 1, head.y);
                    if (head.x == 78 || SelfHit(newHeadR))
                    {
                        GameOver();
                    }
                    else
                    {
                        snake.Insert(0, newHeadR);
                        WriteSnake(CheckFood(newHeadR));
                    }
                    break;
                case SnakeDirection.Up:
                    var newHeadU = new Coordinate(head.x, head.y - 1);
                    if (head.y == 1 || SelfHit(newHeadU))
                    {
                        GameOver();
                    }
                    else
                    {
                        snake.Insert(0, newHeadU);
                        WriteSnake(CheckFood(newHeadU));
                    }
                    break;
                case SnakeDirection.Down:
                    var newHeadD = new Coordinate(head.x, head.y + 1);
                    if (head.y == 21 || SelfHit(newHeadD))
                    {
                        GameOver();
                    }
                    else
                    {
                        snake.Insert(0, newHeadD);
                        WriteSnake(CheckFood(newHeadD));
                    }
                    break;
            }

        }
        public void WriteSnake(Boolean ate)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < snake.Count - 2; i++)
            {
                Console.SetCursorPosition(snake[i].x, snake[i].y);
                Console.Write("█");
            }
            if (ate)
            {
                FeedSnake();
            }
            else
            {
                var tail = snake[snake.Count - 1];
                Console.SetCursorPosition(tail.x, tail.y);
                Console.Write(" ");
                snake.RemoveAt(snake.Count - 1);
            }

        }
        public Boolean SelfHit(Coordinate newHead)
        {
            Boolean hit = false;
            foreach (Coordinate item in snake)
            {
                if (item.x == newHead.x && item.y == newHead.y)
                {
                    hit = true;
                    break;
                }
            }
            return hit;
        }
        public void PrintFrame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            for (int x = 0; x < 80; x++)
            {
                for (int y = 0; y < 23; y++)
                {
                    if (y == 0 || y == 22 || x == 0 || x == 79)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("█");
                    }
                }
            }
        }
        public void SetScore(int score)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 23);
            Console.Write("Score : {0}", score);
        }
        public void SetTime(string timeString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(60, 23);
            Console.Write("Time : {0}", timeString);
        }
        public void PrintAcsciiCodes()
        {
            Encoding e = Encoding.GetEncoding("iso-8859-1");
            Console.OutputEncoding = e;
            for (int i = 1; i < 256; i++)
            {
                Console.Write(i + " = " + (char)i + "\t");
            }
        }
        public void GameOver()
        {
            StopPlaying();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(35, 10);
            Console.Write("GAME OVER");
        }
        public void FeedSnake()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            food = GetPosition();
            Console.SetCursorPosition(food.x, food.y);
            Console.Write("*");
        }
        public Coordinate GetPosition()
        {
            Random rnd = new Random();
            var c = new Coordinate(rnd.Next(1, 78), rnd.Next(1, 21));
            foreach (Coordinate item in snake)
            {
                if (item.x == c.x && item.y == c.y)
                {
                    c = GetPosition();
                    break;
                }
            }
            return c;
        }
        public Boolean CheckFood(Coordinate head)
        {
            Boolean ate = false;
            if (food.x == head.x && food.y == head.y)
            {
                ate = true;
                score++;
                SetScore(score);
            }
            return ate;
        }
    }

    class Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

}