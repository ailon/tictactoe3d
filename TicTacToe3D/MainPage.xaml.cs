using System;
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
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			// Required to initialize variables
			InitializeComponent();
            Game.Boards[0] = B0;
            Game.Boards[1] = B1;
            Game.Boards[2] = B2;
            Game.ScoreX = ScoreX;
            Game.ScoreY = ScoreY;
            Game.FinalAnim = (Storyboard)Resources["RotateBoards"];
            Game.GameOver = GameOver;
            Game.LineSound = LineSound;
            Game.MoveSound = MoveSound;
        }

		Point prevPoint;
		bool bDown = false;
		private void BoardRoot_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            bDown = true;
			prevPoint = e.GetPosition(BoardRoot);
		}

		private void BoardRoot_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (bDown)
			{
                BoardRoot.CaptureMouse();
				Point newPoint = e.GetPosition(BoardRoot);
				
				double xDiff = newPoint.X - prevPoint.X;
				double yDiff = newPoint.Y - prevPoint.Y;
				
				((PlaneProjection)P0.Projection).RotationX += yDiff;
				((PlaneProjection)P1.Projection).RotationX += yDiff;
				((PlaneProjection)P2.Projection).RotationX += yDiff;
				
				((PlaneProjection)P0.Projection).RotationY += xDiff;
				((PlaneProjection)P1.Projection).RotationY += xDiff;
				((PlaneProjection)P2.Projection).RotationY += xDiff;
				
				prevPoint = newPoint;
			}
		}

		private void BoardRoot_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			BoardRoot.ReleaseMouseCapture();
			bDown = false;
		}

        private void NewGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            Game.Reset();
		}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Ellipse el = new Ellipse();
                el.Fill = new SolidColorBrush(Colors.White);
                el.Width = rnd.NextDouble() * 2;
                el.Height = el.Width;
                el.IsHitTestVisible = false;
                el.SetValue(Canvas.LeftProperty, rnd.NextDouble() * 800);
                el.SetValue(Canvas.TopProperty, rnd.NextDouble() * 500);
                LayoutRoot.Children.Add(el);
            }
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }
    }
}