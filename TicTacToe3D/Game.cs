using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TicTacToe3D
{
    public static class Game
    {
        static Game()
        {
            Cells = new int[3, 3, 3];
            Cells[1, 1, 1] = 3;
        }

        public static void Reset()
        {
            Cells = new int[3, 3, 3];
            Cells[1, 1, 1] = 3;
            FinalAnim.Stop();
            foreach(Board b in Boards)
                b.Clear();
            ScoreX.Text = "0";
            ScoreY.Text = "0";
            GameOver.Visibility = Visibility.Collapsed;
        }

        public static int[, ,] Cells;
        public static Board[] Boards = new Board[3];
        public static TextBlock ScoreX;
        public static TextBlock ScoreY;
        public static TextBlock GameOver;
        public static Storyboard FinalAnim;
        
        public static bool Move(int plane, int column, int row, int fig)
        {
            if (Cells[plane, column, row] == 0)
            {
                Cells[plane, column, row] = fig;

                int x = column * 100 + 20;
                int y = row * 100 + 20;

                if (fig == 1)
                {
                    Line l1 = new Line() { X1 = x, Y1 = y, X2 = x + 60, Y2 = y + 60, Stroke = new SolidColorBrush(Colors.Purple), StrokeThickness = 5 };
                    Line l2 = new Line() { X1 = x + 60, Y1 = y, X2 = x, Y2 = y + 60, Stroke = new SolidColorBrush(Colors.Purple), StrokeThickness = 5 };
                    Boards[plane].BC.Children.Add(l1);
                    Boards[plane].BC.Children.Add(l2);
                    AIMove();
                }
                else
                {
                    Ellipse el = new Ellipse() { Width = 60, Height = 60, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 5 };
                    el.SetValue(Canvas.LeftProperty, (double)x);
                    el.SetValue(Canvas.TopProperty, (double)y);
                    Boards[plane].BC.Children.Add(el);
                }
                
                CountLines();

                if (IsFinished)
                {
                    FinalAnim.Begin();
                    GameOver.Visibility = Visibility.Visible;
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool TryAIMove(int p, int c, int r, int n0, int n1, int n2)
        {
            if ((n2 == 2 || n1 == 2) && n0 == 1)
            {
                Move(p, c, r, 2);
                return true;
            }
            else
                return false;
        }

        static void AIMove()
        {
            Random rnd = new Random();

            int no2 = 0;
            int no1 = 0;
            int no0 = 0;
            int c0 = -1;
            int r0 = -1;
            int p0 = -1;

            // 3d lines
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {

                    // 3d diagonals
                    if (c % 2 == 0 && r % 2 == 0)
                    {
                        // diagonal #1
                        no2 = 0;
                        no0 = 0;
                        no1 = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            int rd = i == 0 ? r : (i == 1 ? 1 : 2 - r);
                            no2 += Cells[i, c, rd] == 2 ? 1 : 0;
                            no1 += Cells[i, c, rd] == 1 ? 1 : 0;
                            if (Cells[i, c, rd] == 0)
                            {
                                no0++;
                                p0 = i;
                                r0 = rd;
                            }
                        }
                        if ((no2 == 2 || no1 == 2) && no0 == 1)
                        {
                            Move(p0, c, r0, 2);
                            return;
                        }


                        // diagonal #2
                        no2 = 0;
                        no0 = 0;
                        no1 = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            int cd = i == 0 ? c : (i == 1 ? 1 : 2 - c);
                            no2 += Cells[i, cd, r] == 2 ? 1 : 0;
                            no1 += Cells[i, cd, r] == 1 ? 1 : 0;
                            if (Cells[i, cd, r] == 0)
                            {
                                no0++;
                                p0 = i;
                                c0 = cd;
                            }
                        }
                        if (TryAIMove(p0, c0, r, no0, no1, no2))
                            return;
                    }

                    no2 = 0;
                    no0 = 0;
                    no1 = 0;
                    // vertical lines
                    for (int p = 0; p < 3; p++)
                    {
                        no2 += Cells[p, c, r] == 2 ? 1 : 0;
                        no1 += Cells[p, c, r] == 1 ? 1 : 0;
                        if (Cells[p, c, r] == 0)
                        {
                            no0++;
                            p0 = p;
                        }
                    }
                    if (TryAIMove(p0, c, r, no0, no1, no2))
                        return;
                }
            }

            // 2d lines
            for (int p = 0; p < 3; p++)
            {
                // diagonal #1
                no2 = 0;
                no0 = 0;
                no1 = 0;
                for (int i = 0; i < 3; i++)
                {
                    no2 += Cells[p, i, i] == 2 ? 1 : 0;
                    no1 += Cells[p, i, i] == 1 ? 1 : 0;
                    if (Cells[p, i, i] == 0)
                    {
                        no0++;
                        r0 = i;
                        c0 = i;
                    }
                }
                if (TryAIMove(p, c0, r0, no0, no1, no2))
                    return;

                // diagonal #2
                no2 = 0;
                no0 = 0;
                no1 = 0;
                for (int i = 0; i < 3; i++)
                {
                    no2 += Cells[p, 2 - i, i] == 2 ? 1 : 0;
                    no1 += Cells[p, 2 - i, i] == 1 ? 1 : 0;
                    if (Cells[p, 2 - i, i] == 0)
                    {
                        no0++;
                        c0 = 2 - i;
                        r0 = i;
                    }
                }
                if (TryAIMove(p, c0, r0, no0, no1, no2))
                    return;

                for (int c = 0; c < 3; c++)
                {
                    // vertical lines
                    no2 = 0;
                    no0 = 0;
                    no1 = 0;
                    for (int r = 0; r < 3; r++)
                    {
                        no2 += Cells[p, c, r] == 2 ? 1 : 0;
                        no1 += Cells[p, c, r] == 1 ? 1 : 0;
                        if (Cells[p, c, r] == 0)
                        {
                            no0++;
                            r0 = r;
                        }
                    }
                    if (TryAIMove(p, c, r0, no0, no1, no2))
                        return;
                }
                for (int r = 0; r < 3; r++)
                {
                    // horizontal lines
                    no2 = 0;
                    no0 = 0;
                    no1 = 0;
                    for (int c = 0; c < 3; c++)
                    {
                        no2 += Cells[p, c, r] == 2 ? 1 : 0;
                        no1 += Cells[p, c, r] == 1 ? 1 : 0;
                        if (Cells[p, c, r] == 0)
                        {
                            no0++;
                            c0 = c;
                        }
                    }
                    if (TryAIMove(p, c0, r, no0, no1, no2))
                        return;
                }
            }


            // random if no smart move
            while (!Move(rnd.Next(3), rnd.Next(3), rnd.Next(3), 2)) { }
        }

        static void CountLines()
        {
            int[] lineCounts = new int[3];

            // 3d lines
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    // vertical lines
                    if (Cells[0, c, r] == Cells[1, c, r] && Cells[0, c, r] == Cells[2, c, r])
                        lineCounts[Cells[0, c, r]]++;

                    // 3d diagonals
                    if (c % 2 == 0 && r % 2 == 0)
                    {
                        if (Cells[0, c, r] == Cells[1, c, 1] && Cells[0, c, r] == Cells[2, c, 2 - r])
                            lineCounts[Cells[0, c, r]]++;
                        if (Cells[0, c, r] == Cells[1, 1, r] && Cells[0, c, r] == Cells[2, 2 - c, r])
                            lineCounts[Cells[0, c, r]]++;
                    }
                }
            }

            // 2d lines
            for (int p = 0; p < 3; p++)
            {
                // diagonals
                if (Cells[p, 0, 0] == Cells[p, 1, 1] && Cells[p, 0, 0] == Cells[p, 2, 2])
                    lineCounts[Cells[p, 0, 0]]++;
                if (Cells[p, 2, 0] == Cells[p, 1, 1] && Cells[p, 2, 0] == Cells[p, 0, 2])
                    lineCounts[Cells[p, 2, 0]]++;


                for (int c = 0; c < 3; c++)
                {
                    // vertical lines
                    if (Cells[p, c, 0] == Cells[p, c, 1] && Cells[p, c, 0] == Cells[p, c, 2])
                        lineCounts[Cells[p, c, 0]]++;
                }
                for (int r = 0; r < 3; r++)
                {
                    // horizontal lines
                    if (Cells[p, 0, r] == Cells[p, 1, r] && Cells[p, 0, r] == Cells[p, 2, r])
                        lineCounts[Cells[p, 0, r]]++;
                }
            }


            ScoreX.Text = lineCounts[1].ToString();
            ScoreY.Text = lineCounts[2].ToString();
        }

        public static bool IsFinished
        {
            get
            {
                for (int p = 0; p < 3; p++)
                    for (int c = 0; c < 3; c++)
                        for (int r = 0; r < 3; r++)
                            if (Cells[p, c, r] == 0)
                                return false;

                return true;
            }
        }
    }
}
