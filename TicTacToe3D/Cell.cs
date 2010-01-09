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
    public class Cell
    {
        public Cell(int plane, int column, int row)
        {
            Plane = plane;
            Column = column;
            Row = row;
        }

        public int Plane { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public static bool operator ==(Cell cell1, Cell cell2)
        {
            return cell1.Equals(cell2);
        }

        public static bool operator !=(Cell cell1, Cell cell2)
        {
            return !cell1.Equals(cell2);
        }

        public override bool Equals(object obj)
        {
            Cell cell = (Cell)obj;
            return this.Plane == cell.Plane && this.Column == cell.Column && this.Row == cell.Row;
        }

        public override int GetHashCode()
        {
            return Plane * 100 + Column * 10 + Row;
        }
    }
}
