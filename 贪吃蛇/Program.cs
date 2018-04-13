using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 贪吃蛇
{

    struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Program
    {
        static bool Alive;
        static int Direct;
        static Random Rdn = new Random();
        static int[] Dx = new int[4] { -1, 0, 1, 0 };
        static int[] Dy = new int[4] { 0, -1, 0, 1 };
        static Queue<Point> Snake = new Queue<Point>();
        static int[,] Map = new int[18, 36];

        public static void Initialize()
        {
            Array.Clear(Map, 0, Map.GetLength(0) * Map.GetLength(1));
            Snake.Clear();
            Alive = true;
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                Map[i, 0] = 1;
                Map[i, Map.GetLength(1) - 1] = 1;
            }
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                Map[0, i] = 1;
                Map[Map.GetLength(0) - 1, i] = 1;
            }
            Point body = new Point(Map.GetLength(0) / 2, Map.GetLength(1) / 2);
            Map[body.x, body.y] = 3;
            Snake.Enqueue(body);
            Direct = Rdn.Next(4);
            Map[body.x + Dx[Direct], body.y + Dy[Direct]] = 2;
            Snake.Enqueue(new Point(body.x + Dx[Direct], body.y + Dy[Direct]));
            GenerateStar();
        }
        public static void ShowMap()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    switch (Map[i, j])
                    {
                        case 0:
                            Console.Write("  ");
                            break;
                        case 1:
                            Console.Write("■");
                            break;
                        case 2:
                            Console.Write("⊙");
                            break;
                        case 3:
                            Console.Write("●");
                            break;
                        case 4:
                            Console.Write("☆");
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("当前长度:{0}", Snake.Count);
        }

        public static void Move()
        {
            Console.WriteLine("W--向上    A--向左    S--向下    D--向右    Esc--退出");
            string key = Console.ReadKey().Key.ToString();
            Console.WriteLine("\b\t");
            switch (key)
            {
                case "W":
                    Direct = 0;
                    break;
                case "A":
                    Direct = 1;
                    break;
                case "S":
                    Direct = 2;
                    break;
                case "D":
                    Direct = 3;
                    break;
                case "Escape":
                    throw new Exception("谢谢你的游戏");
                default:
                    Console.WriteLine("输入错误");
                    System.Threading.Thread.Sleep(500);
                    return;
            }
            Point head = new Point(Snake.ElementAt(Snake.Count - 1).x + Dx[Direct], Snake.ElementAt(Snake.Count - 1).y + Dy[Direct]);
            if (Map[head.x, head.y] == 1 || Map[head.x, head.y] == 2 || Map[head.x, head.y] == 3)
            {
                Alive = false;
            }
            if (Map[head.x, head.y] != 4)
            {
                Map[head.x, head.y] = 2;
                Map[Snake.ElementAt(Snake.Count - 1).x, Snake.ElementAt(Snake.Count - 1).y] = 3;
                Snake.Enqueue(head);
                head = Snake.Dequeue();
                Map[head.x, head.y] = 0;
            }
            else
            {
                Map[head.x, head.y] = 2;
                Map[Snake.ElementAt(Snake.Count - 1).x, Snake.ElementAt(Snake.Count - 1).y] = 3;
                Snake.Enqueue(head);
                GenerateStar();
            }
        }

        public static void Resume()
        {
            Console.WriteLine("Game Over!!!");
            Console.WriteLine("R--重新开始    Esc--退出");
            string key = Console.ReadKey().Key.ToString();
            Console.WriteLine("\b\t");
            switch (key)
            {
                case "R":
                    Initialize();
                    break;
                case "Escape":
                    throw new Exception("谢谢你的游戏");
                default:
                    Console.WriteLine("输入错误");
                    System.Threading.Thread.Sleep(500);
                    return;
            }
        }

        public static void GenerateStar()
        {
            while (true)
            {
                int row = Rdn.Next(1, Map.GetLength(0) - 1);
                int column = Rdn.Next(1, Map.GetLength(1) - 1);
                if (Map[row, column] == 0)
                {
                    Map[row, column] = 4;
                    break;
                }
            }
        }
        static void Main(string[] args)
        {
            Initialize();
            while (true)
            {
                Console.Clear();
                ShowMap();
                if (Alive)
                {
                    try
                    {
                        Move();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
                else
                {
                    try
                    {
                        Resume();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
            }
            System.Threading.Thread.Sleep(500);
        }
    }
}
