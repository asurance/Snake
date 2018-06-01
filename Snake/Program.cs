using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    struct Point
    {
        public int x;
        public int y;
        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    /// <summary>
    /// 方向类型
    /// </summary>
    public enum Direction { 上, 下, 左, 右 }

    public class Program
    {
        #region 变量
        /// <summary>
        /// 是否存活
        /// </summary>
        static bool alive;
        /// <summary>
        /// 地图行数
        /// </summary>
        static int length0;
        /// <summary>
        /// 地图列数
        /// </summary>
        static int length1;
        /// <summary>
        /// 当前方向
        /// </summary>
        static Direction direction;
        /// <summary>
        /// 头部所在位置
        /// </summary>
        static Point head;
        /// <summary>
        /// 蛇位置
        /// </summary>
        static Queue<Point> Snake = new Queue<Point>();
        /// <summary>
        /// x方向便宜
        /// </summary>
        static int[] dx = new int[4] { 0, 0, -1, 1 };
        /// <summary>
        /// y方向便宜
        /// </summary>
        static int[] dy = new int[4] { -1, 1, 0, 0 };
        /// <summary>
        /// 随机数
        /// </summary>
        static Random rdn = new Random();
        /// <summary>
        /// 当前输入
        /// </summary>
        static string input = "";
        /// <summary>
        /// 地图信息
        /// </summary>
        static int[,] Map = new int[18, 36];                //0-"  ",1-"■",2-"⊙",3-"●",4-"☆"
        #endregion

        /// <summary>
        /// 主函数入口
        /// </summary>
        static void Main()
        {
            InitialzeMap();
            Timer timer = new Timer(150);
            timer.Start();
            timer.Elapsed += TimerElapsed;
            while (alive)
            {
                input = RealTimeInput();
                if (input != "")
                {
                    switch (input)
                    {
                        case "W":
                            direction = Direction.上;
                            break;
                        case "S":
                            direction = Direction.下;
                            break;
                        case "A":
                            direction = Direction.左;
                            break;
                        case "D":
                            direction = Direction.右;
                            break;
                        default:
                            RefreshInfo(input);
                            break;
                    }
                }
            }
            RefreshInfo("当前长度" + Snake.Count);
            Console.WriteLine("\n游戏结束");
        }

        /// <summary>
        /// 定期更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Point target = new Point(head.x + dx[(int)direction], head.y + dy[(int)direction]);
            #region 走到空地
            if (Map[target.y, target.x] == 0)
            {
                Snake.Enqueue(target);
                Set(head, 3);
                Set(target, 2);
                head = target;
                target = Snake.Dequeue();
                Set(target, 0);
                RefreshInfo("当前长度" + Snake.Count);
            }
            #endregion
            #region 走到星
            else if (Map[target.y, target.x] == 4)
            {
                Snake.Enqueue(target);
                Set(head, 3);
                Set(target, 2);
                head = target;
                RefreshInfo("当前长度" + Snake.Count);
                GenerateStar();
            }
            #endregion
            #region 走到不能走的位置
            else
            {
                alive = false;
            }
            #endregion
        }

        /// <summary>
        /// 初始化地图
        /// </summary>
        static void InitialzeMap()
        {
            #region 初始化地图信息
            Array.Clear(Map, 0, Map.Length);
            length0 = Map.GetLength(0);
            length1 = Map.GetLength(1);
            for (int i = 0; i < length0; i++)
            {
                Map[i, 0] = 1;
                Map[i, length1 - 1] = 1;
            }
            for (int i = 0; i < length1; i++)
            {
                Map[0, i] = 1;
                Map[length0 - 1, i] = 1;
            }
            #endregion
            #region 初始化蛇信息
            alive = true;
            direction = (Direction)rdn.Next(4);
            Snake.Clear();
            head = new Point(length1 / 2, length0 / 2);
            Snake.Enqueue(head);
            Map[head.y, head.x] = 2;
            Point target;
            switch (direction)
            {
                case Direction.上:
                    target = new Point(length1 / 2, length0 / 2 + 1);
                    break;
                case Direction.下:
                    target = new Point(length1 / 2, length0 / 2 - 1);
                    break;
                case Direction.左:
                    target = new Point(length1 / 2 + 1, length0 / 2);
                    break;
                case Direction.右:
                    target = new Point(length1 / 2 - 1, length0 / 2);
                    break;
                default:
                    target = new Point();
                    break;
            }
            Map[target.y, target.x] = 3;
            Snake.Enqueue(target);
            #endregion
            #region 初始化地图
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
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
                        default:
                            Console.WriteLine("生成地图时发生位置错误");
                            break;
                    }
                }
                Console.WriteLine();
            }
            #endregion
            GenerateStar();
        }

        /// <summary>
        /// 生成星
        /// </summary>
        static void GenerateStar()
        {
            int row = rdn.Next(length0);
            int col = rdn.Next(length1);
            while (Map[row, col] != 0)
            {
                row = rdn.Next(length0);
                col = rdn.Next(length1);
            }
            Set(new Point(col, row), 4);
        }

        /// <summary>
        /// 刷新信息
        /// </summary>
        /// <param name="info">信息</param>
        static void RefreshInfo(string info)
        {
            lock (rdn)
            {
                Console.SetCursorPosition(0, length0);
                for (int i = 0; i < length1; i++)
                {
                    Console.Write("  ");
                }
                Console.SetCursorPosition(0, length0);
                Console.Write(info);
            }
        }

        /// <summary>
        /// 实时输入
        /// </summary>
        /// <returns>输入</returns>
        static string RealTimeInput()
        {
            if (Console.KeyAvailable)
            {
                string res;
                lock (rdn)
                {
                    Console.SetCursorPosition(length1 - 2, length0);
                    res = Console.ReadKey().Key.ToString();
                    Console.SetCursorPosition(length1 - 2, length0);
                    Console.Write("  ");
                }
                return res;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="type">位置类型</param>
        static void Set(Point pos, int type)
        {
            Map[pos.y, pos.x] = type;
            lock (rdn)
            {
                Console.SetCursorPosition(pos.x * 2, pos.y);
                switch (type)
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
                        Console.WriteLine("Set函数中type错误");
                        break;
                }
            }
        }
    }
}
