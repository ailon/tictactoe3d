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
    public class WinningLine
    {
        public WinningLine(Cell cell1, Cell cell2, Cell cell3)
        {
            Cell1 = cell1;
            Cell2 = cell2;
            Cell3 = cell3;
        }

        public Cell Cell1 { get; set; }
        public Cell Cell2 { get; set; }
        public Cell Cell3 { get; set; }

        public override bool Equals(object obj)
        {
            WinningLine line = (WinningLine)obj;
            return Cell1.Equals(line.Cell1) && Cell2.Equals(line.Cell2) && Cell3.Equals(line.Cell3);
        }

        public override int GetHashCode()
        {
            return Cell1.GetHashCode() * 1000000 + Cell2.GetHashCode() * 1000 + Cell3.GetHashCode();
        }
    }
}
