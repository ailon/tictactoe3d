using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TicTacToe3D
{
	public class Board : Control
	{
		public Board()
		{
            this.DefaultStyleKey = typeof(Board);
		}

        public int Plane { get; set; }

        public Canvas BC;
        public override void OnApplyTemplate()
        {
            BC = (Canvas)GetTemplateChild("BC");
            if (Plane != 1)
            {
                UIElement logo = (UIElement)GetTemplateChild("LOGO");
                logo.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(BC);

            Game.Move(Plane, (int)Math.Floor(pos.X / 100), (int)Math.Floor(pos.Y / 100), 1);
        }

        public void Clear()
        {
            BC.Children.Clear();
        }
	}
}