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
using System.Windows.Media.Animation;

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

            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    _highlighters[c, r] = (Rectangle)GetTemplateChild("HL_" + c.ToString() + r.ToString());
                }
            }
        }

        private Rectangle[,] _highlighters = new Rectangle[3, 3];

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(BC);

            if (Game.Move(Plane, (int)Math.Floor(pos.X / 100), (int)Math.Floor(pos.Y / 100), 1))
            {
                Game.AIMove();
            }
        }

        public void Clear()
        {
            BC.Children.Clear();
        }

        public void HighlightCell(int column, int row, Color color)
        {
            _highlighters[column, row].Fill = new SolidColorBrush(color);
            Storyboard sb = new Storyboard();
            DoubleAnimation ca = new DoubleAnimation();
            ca.From = 0;
            ca.To = 1;
            ca.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            ca.AutoReverse = true;
            ca.RepeatBehavior = new RepeatBehavior(3);
            sb.Children.Add(ca);
            Storyboard.SetTarget(sb, _highlighters[column, row].Fill);
            Storyboard.SetTargetProperty(sb, new PropertyPath(SolidColorBrush.OpacityProperty));
            sb.Begin();
        }
	}
}