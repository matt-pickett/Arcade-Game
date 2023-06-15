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
    public static class DrawGhost
    {
        public static Path CreateGhost(Brush fillBrush)
        {
            double size = MainWindow.squareSize; // Adjust the size as needed
            double radius = size / 2.0;

            GeometryGroup geometryGroup = new GeometryGroup();

            // Draw the body of the ghost
            geometryGroup.Children.Add(new EllipseGeometry(new Point(radius, radius), radius, radius));

            // Draw the eyes
            double eyeSize = size / 7.0; // Increase the eye size slightly
            double eyeOffsetX = size / 4.0;
            double eyeOffsetY = size / 4.0;
            double eyePosY = size * 2.0 / 3.0; // Adjust the value to move the eyes lower down

            Path leftEye = new Path()
            {
                Data = new EllipseGeometry(new Point(radius - eyeOffsetX, eyePosY - eyeOffsetY), eyeSize, eyeSize),
                Fill = Brushes.White // Set the eye color to white
            };

            Path rightEye = new Path()
            {
                Data = new EllipseGeometry(new Point(radius + eyeOffsetX, eyePosY - eyeOffsetY), eyeSize, eyeSize),
                Fill = Brushes.White // Set the eye color to white
            };

            geometryGroup.Children.Add(new GeometryGroup() { Children = { leftEye.Data } });
            geometryGroup.Children.Add(new GeometryGroup() { Children = { rightEye.Data } });

            Path ghost = new Path()
            {
                Data = geometryGroup,
                Width = size,
                Height = size,
                Fill = fillBrush // Adjust the fill color as needed
            };

            return ghost;
        }









    }
}
