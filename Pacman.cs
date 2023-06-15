using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pacman_Game
{
    public class DrawPacmanImage
    {
        public UIElement? UiElement { get; set; }

        public Point Position { get; set; }

        public bool IsHead { get; set; }

        public static Path DrawPacmanImageRight()
        {
            return CreatePacmanImage(-0);
        }

        public static Path DrawPacmanImageLeft()
        {
            return CreatePacmanImage(-180);
        }

        public static Path DrawPacmanImageDown()
        {
            return CreatePacmanImage(-270);
        }

        public static Path DrawPacmanImageUp()
        {
            return CreatePacmanImage(-90);
        }

        private static Path CreatePacmanImage(double rotateByAngle)
        {
            double radius = MainWindow.squareSize / 2.0;

            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(new EllipseGeometry(new Point(radius, radius), radius, radius));

            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(radius, radius);
            figure.Segments.Add(new LineSegment(new Point(radius * 2, 0), isStroked: true));

            double sectorStartAngle = 45;
            double sectorEndAngle = 315;
            double sectorRadius = radius;
            double sectorStartX = radius + Math.Cos(sectorStartAngle * Math.PI / 180) * sectorRadius;
            double sectorStartY = radius - Math.Sin(sectorStartAngle * Math.PI / 180) * sectorRadius;
            double sectorEndX = radius + Math.Cos(sectorEndAngle * Math.PI / 180) * sectorRadius;
            double sectorEndY = radius - Math.Sin(sectorEndAngle * Math.PI / 180) * sectorRadius;
            figure.Segments.Add(new LineSegment(new Point(sectorStartX, sectorStartY), isStroked: true));
            figure.Segments.Add(new ArcSegment(new Point(sectorEndX, sectorEndY), new Size(sectorRadius, sectorRadius), 0, false, SweepDirection.Clockwise, isStroked: true));

            figure.Segments.Add(new LineSegment(new Point(radius * 2, radius * 2), isStroked: true));
            figure.IsClosed = true;
            geometryGroup.Children.Add(new PathGeometry(new[] { figure }));

            Path pacman = new Path()
            {
                Data = geometryGroup,
                Width = MainWindow.squareSize,
                Height = MainWindow.squareSize,
                Fill = Brushes.Yellow
            };

            // Rotate the pacmanPath character
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform(rotateByAngle, radius, radius)); // Adjust the rotation angle as needed
            pacman.RenderTransform = transformGroup;

            return pacman;
        }
    }
}
