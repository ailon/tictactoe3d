using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Ailon.TicTacToe3D
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
            CompletedLines.Clear();
            _over = false;
        }

        public static int[, ,] Cells;
        public static Board[] Boards = new Board[3];
        public static TextBlock ScoreX;
        public static TextBlock ScoreY;
        public static TextBlock ScoreXTotal;
        public static TextBlock ScoreYTotal;
        public static TextBlock ScoreXGames;
        public static TextBlock ScoreYGames;
        private static int _xCurrent = 0;
        private static int _yCurrent = 0;
        private static int _xTotal = 0;
        private static int _yTotal = 0;
        private static int _xGames = 0;
        private static int _yGames = 0;
        public static TextBlock GameOver;
        public static Storyboard FinalAnim;
        public static List<WinningLine> CompletedLines = new List<WinningLine>();
        public static MediaElement LineSound;
        public static MediaElement MoveSound;

        private static bool _over = false;
        
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
                    MoveSound.Stop();
                    MoveSound.Play();
                }
                else
                {
                    Ellipse el = new Ellipse() { Width = 60, Height = 60, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 5 };
                    el.SetValue(Canvas.LeftProperty, (double)x);
                    el.SetValue(Canvas.TopProperty, (double)y);
                    Boards[plane].BC.Children.Add(el);
                }
                
                var lines = CountLines();
                foreach (WinningLine wl in lines)
                {
                    Color color = Cells[wl.Cell1.Plane, wl.Cell1.Column, wl.Cell1.Row] == 1 ? Colors.Yellow : Colors.Green;
                    color.A = 50;
                    Boards[wl.Cell1.Plane].HighlightCell(wl.Cell1.Column, wl.Cell1.Row, color);
                    Boards[wl.Cell2.Plane].HighlightCell(wl.Cell2.Column, wl.Cell2.Row, color);
                    Boards[wl.Cell3.Plane].HighlightCell(wl.Cell3.Column, wl.Cell3.Row, color);
                }

                if (IsFinished && !_over)
                {
                    _over = false;
                    _xTotal += _xCurrent;
                    _yTotal += _yCurrent;
                    if (_xCurrent > _yCurrent)
                    {
                        _xGames++;
                    }
                    else if (_xCurrent < _yCurrent)
                    {
                        _yGames++;
                    }
                    ScoreXTotal.Text = _xTotal.ToString();
                    ScoreYTotal.Text = _yTotal.ToString();
                    ScoreXGames.Text = _xGames.ToString();
                    ScoreYGames.Text = _yGames.ToString();
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

        public static void AIMove()
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

        static List<WinningLine> newCompletedLines = new List<WinningLine>();
        static void AddNewCompletedLine(int cellContent, WinningLine line)
        {
            if (!CompletedLines.Contains(line) && cellContent > 0)
            {
                newCompletedLines.Add(line);
                LineSound.Stop();
                LineSound.Play();
            }
        }

        static int CheckCells(Cell cell1, Cell cell2, Cell cell3)
        {
            if (Cells[cell1.Plane, cell1.Column, cell1.Row] == Cells[cell2.Plane, cell2.Column, cell2.Row]
                && Cells[cell1.Plane, cell1.Column, cell1.Row] == Cells[cell3.Plane, cell3.Column, cell3.Row])
            {
                AddNewCompletedLine(Cells[cell1.Plane, cell1.Column, cell1.Row], new WinningLine(cell1, cell2, cell3));
                return 1;
            }
            else
            {
                return 0;
            }
        }

        static List<WinningLine> CountLines()
        {
            newCompletedLines.Clear();
            int[] lineCounts = new int[3];

            // 3d lines
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    // vertical lines
                    lineCounts[Cells[0, c, r]] += CheckCells(new Cell(0, c, r), new Cell(1, c, r), new Cell(2, c, r));

                    // 3d diagonals
                    if (c % 2 == 0 && r % 2 == 0)
                    {
                        lineCounts[Cells[0, c, r]] += CheckCells(new Cell(0, c, r), new Cell(1, c, 1), new Cell(2, c, 2 - r));
                        lineCounts[Cells[0, c, r]] += CheckCells(new Cell(0, c, r), new Cell(1, 1, r), new Cell(2, 2 - c, r));
                    }
                }
            }

            // 2d lines
            for (int p = 0; p < 3; p++)
            {
                // diagonals
                lineCounts[Cells[p, 0, 0]] += CheckCells(new Cell(p, 0, 0), new Cell(p, 1, 1), new Cell(p, 2, 2));
                lineCounts[Cells[p, 2, 0]] += CheckCells(new Cell(p, 2, 0), new Cell(p, 1, 1), new Cell(p, 0, 2));

                for (int c = 0; c < 3; c++)
                {
                    // vertical lines
                    lineCounts[Cells[p, c, 0]] += CheckCells(new Cell(p, c, 0), new Cell(p, c, 1), new Cell(p, c, 2));
                }
                for (int r = 0; r < 3; r++)
                {
                    // horizontal lines
                    lineCounts[Cells[p, 0, r]] += CheckCells(new Cell(p, 0, r), new Cell(p, 1, r), new Cell(p, 2, r));
                }
            }

            _xCurrent = lineCounts[1];
            _yCurrent = lineCounts[2];
            ScoreX.Text = _xCurrent.ToString();
            ScoreY.Text = _yCurrent.ToString();

            CompletedLines.AddRange(newCompletedLines);

            return new List<WinningLine>(newCompletedLines);
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
