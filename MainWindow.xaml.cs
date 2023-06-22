using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
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
using System.Media;
using System.Diagnostics.Eventing.Reader;

namespace Pacman_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Direction { Left, Right, Up, Down };
        public const int squareSize = 20;
        private int currentScore = 0;
        private int remainingLives = 3;
        private int totalFoodCount = 0;
        private int currFoodCount = 0;
        private bool isOutOfBounds = false;
        private Random random = new Random();
        private UIElement fruit = null;
        private Canvas pacmanCanvas;
        private Path pacmanPath;
        private Path Ghost1;
        private Path Ghost2;
        private Path Ghost3;
        private Path Ghost4;
        private bool isGhost1 = false;
        private bool isGhost2 = false;
        private bool isGhost3 = false;
        private bool isGhost4 = false;
        private List<UIElement> bigFoods = new List<UIElement>();
        private DrawPacmanImage PacmanImage = new DrawPacmanImage();
        private DispatcherTimer gameTimer = new System.Windows.Threading.DispatcherTimer();
        private DispatcherTimer ghostTimer = new System.Windows.Threading.DispatcherTimer();
        private bool ghostTimerOn = false;
        private Direction pacmanDirection = Direction.Right;
        private Direction ghostDirection = Direction.Left;

        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Tick += gameTimer_Tick;
            pacmanCanvas = new Canvas();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
            SpawnGhosts();
            RestartGame();
        }

        // Allow window to be dragged
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void RestartGame()
        {
            GameArea.Children.Remove(PacmanImage.UiElement);

            pacmanDirection = Direction.Right;
            gameTimer.Interval = TimeSpan.FromMilliseconds(300);

            // Set snake's starting position
            double startX = squareSize * 3;
            double startY = squareSize * 3;
            PacmanImage = new DrawPacmanImage() { Position = new Point(startX, startY) };

            // Draw pacmanPath and add it to game
            pacmanPath = DrawPacmanImage.DrawPacmanImageRight();

            Canvas pacmanCanvas = new Canvas();
            pacmanCanvas.Children.Add(pacmanPath);

            PacmanImage.UiElement = pacmanCanvas;

            GameArea.Children.Add(PacmanImage.UiElement);
            Canvas.SetTop(PacmanImage.UiElement, startY);
            Canvas.SetLeft(PacmanImage.UiElement, startX);

            UpdateGameStatus();
            gameTimer.IsEnabled = true;
        }

        private void SpawnGhosts()
        {
            GameArea.Children.Remove(Ghost1);
            GameArea.Children.Remove(Ghost2);
            GameArea.Children.Remove(Ghost3);
            GameArea.Children.Remove(Ghost4);

            Ghost1 = DrawGhost.CreateGhost(Brushes.Red);
            GameArea.Children.Add(Ghost1);
            Canvas.SetTop(Ghost1, squareSize * 13);
            Canvas.SetLeft(Ghost1, squareSize * 8);
            isGhost1 = true;

            Ghost2 = DrawGhost.CreateGhost(Brushes.Cyan);
            GameArea.Children.Add(Ghost2);
            Canvas.SetTop(Ghost2, squareSize * 8);
            Canvas.SetLeft(Ghost2, squareSize * 20);
            isGhost2 = true;

            Ghost3 = DrawGhost.CreateGhost(Brushes.Pink);
            GameArea.Children.Add(Ghost3);
            Canvas.SetTop(Ghost3, squareSize * 13);
            Canvas.SetLeft(Ghost3, squareSize * 10);
            isGhost3 = true;

            Ghost4 = DrawGhost.CreateGhost(Brushes.Yellow);
            GameArea.Children.Add(Ghost4);
            Canvas.SetTop(Ghost4, squareSize * 11);
            Canvas.SetLeft(Ghost4, squareSize * 17);
            isGhost4 = true;
        }
        private void DrawGameArea()
        {
            // Clear existing elements from the game area
            GameArea.Children.Clear();
            bigFoods.Clear();

            int maxX = (int)(GameArea.ActualWidth / squareSize);
            int maxY = (int)(GameArea.ActualHeight / squareSize);
            totalFoodCount = 0;
            currFoodCount = 0;
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    // Draw black squares for the game area
                    Rectangle rect = new Rectangle
                    {
                        Width = squareSize,
                        Height = squareSize,
                        Fill = Brushes.Black
                    };
                    GameArea.Children.Add(rect);
                    Canvas.SetTop(rect, y * squareSize);
                    Canvas.SetLeft(rect, x * squareSize);

                    // Check if the current square is one of the specified positions for separate food
                    int squareNumber = y * maxX + x;
                    if (squareNumber == 26 || squareNumber == 46 || squareNumber == 223 || squareNumber == 350 || squareNumber == 374)
                    {
                        // Add a separate food item to the current square
                        UIElement bigFood = new Ellipse()
                        {
                            Width = squareSize * 0.75,
                            Height = squareSize * 0.75,
                            Fill = Brushes.Tan
                        };
                        GameArea.Children.Add(bigFood);
                        Canvas.SetTop(bigFood, y * squareSize + squareSize * 0.25);
                        Canvas.SetLeft(bigFood, x * squareSize + squareSize * 0.25);
                        bigFoods.Add(bigFood);
                    }
                    else
                    {
                        currFoodCount++;
                        // Add smaller food to each square that is not a separate food position
                        UIElement food = new Ellipse()
                        {
                            Width = squareSize * 0.25,
                            Height = squareSize * 0.25,
                            Fill = Brushes.Tan
                        };
                        GameArea.Children.Add(food);
                        Canvas.SetTop(food, y * squareSize + squareSize * 0.5);
                        Canvas.SetLeft(food, x * squareSize + squareSize * 0.5);
                    }
                }
            }
            totalFoodCount = currFoodCount;
        }

        private void MovePacmanImage()
        {
            double nextX = PacmanImage.Position.X;
            double nextY = PacmanImage.Position.Y;

            switch (pacmanDirection)
            {
                case Direction.Left:
                    nextX -= squareSize;
                    break;
                case Direction.Right:
                    nextX += squareSize;
                    break;
                case Direction.Up:
                    nextY -= squareSize;
                    break;
                case Direction.Down:
                    nextY += squareSize;
                    break;
            }

            PacmanImage.Position = new Point(nextX, nextY);
            Canvas.SetTop(PacmanImage.UiElement, nextY);
            Canvas.SetLeft(PacmanImage.UiElement, nextX);

            DoCollisionCheck();
        }
        private void MoveGhost(Path ghost)
        {
            double currentX = Canvas.GetLeft(ghost);
            double currentY = Canvas.GetTop(ghost);
            double nextX = currentX;
            double nextY = currentY;

            bool good = false;
            Random random = new Random();
            Direction randomDirection = Direction.Right;
            while (!good)
            {
                randomDirection = (Direction)random.Next(0, 4);

                switch (randomDirection)
                {
                    case Direction.Left:
                        nextX = currentX - squareSize;
                        break;
                    case Direction.Right:
                        nextX = currentX + squareSize;
                        break;
                    case Direction.Up:
                        nextY = currentY - squareSize;
                        break;
                    case Direction.Down:
                        nextY = currentY + squareSize;
                        break;
                }

                if (nextX >= 0 && nextX < GameArea.ActualWidth && nextY >= 0 && nextY < GameArea.ActualHeight)
                {
                    good = true;
                }
            }

            ghostDirection = randomDirection;
            Canvas.SetLeft(ghost, nextX);
            Canvas.SetTop(ghost, nextY);

            DoCollisionCheck();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            MovePacmanImage();
            MoveGhost(Ghost1);
            MoveGhost(Ghost2);
            MoveGhost(Ghost3);
            MoveGhost(Ghost4);
        }
        
        private Point GetNextFruitPosition()
        {
            int maxX = (int)(GameArea.ActualWidth / squareSize);
            int maxY = (int)(GameArea.ActualHeight / squareSize);
            int foodX = random.Next(0, maxX) * squareSize;
            int foodY = random.Next(0, maxY) * squareSize;

            if ((PacmanImage.Position.X == foodX) && (PacmanImage.Position.Y == foodY))
                return GetNextFruitPosition();
            

            return new Point(foodX, foodY);
        }
        private void DrawFruit()
        {
            Point foodPosition = GetNextFruitPosition();
            fruit = new Ellipse()
            {
                Width = squareSize,
                Height = squareSize,
                Fill = Brushes.Red
            };
            GameArea.Children.Add(fruit);
            Canvas.SetTop(fruit, foodPosition.Y);
            Canvas.SetLeft(fruit, foodPosition.X);
        }

        // Event for controlling snake
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    pacmanDirection = Direction.Up;
                    pacmanPath = DrawPacmanImage.DrawPacmanImageUp();
                    ResetPacman(pacmanPath);
                    MovePacmanImage();
                    break;
                case Key.Down:
                    pacmanDirection = Direction.Down;
                    pacmanPath = DrawPacmanImage.DrawPacmanImageDown();
                    ResetPacman(pacmanPath);
                    MovePacmanImage();
                    break;
                case Key.Left:
                    pacmanDirection = Direction.Left;
                    pacmanPath = DrawPacmanImage.DrawPacmanImageLeft();
                    ResetPacman(pacmanPath);
                    MovePacmanImage();
                    break;
                case Key.Right:
                    pacmanDirection = Direction.Right;
                    pacmanPath = DrawPacmanImage.DrawPacmanImageRight();
                    ResetPacman(pacmanPath);
                    MovePacmanImage();
                    break;
            }
        }

        private void DoCollisionCheck()
        {
            CheckFruitCollision();
            CheckFoodCollision();
            CheckBigFoodCollision();
            CheckGhostCollision();
            CheckOutOfBounds();
        }

        private void CheckFruitCollision()
        {
            if (fruit == null)
            {
                if (currFoodCount == totalFoodCount / 2)
                {
                    DrawFruit();
                }
            }
            else if ((PacmanImage.Position.X == Canvas.GetLeft(fruit)) && (PacmanImage.Position.Y == Canvas.GetTop(fruit)))
            {
                EatFruit();
            }
        }

        private void CheckFoodCollision()
        {
            Ellipse collidedFood = null;
            foreach (UIElement food in GameArea.Children)
            {
                if (food is Ellipse && food != fruit)
                {
                    double left = Canvas.GetLeft(food) - squareSize * 0.5;
                    double top = Canvas.GetTop(food) - squareSize * 0.5;

                    if ((PacmanImage.Position.X == left) && (PacmanImage.Position.Y == top))
                    {
                        collidedFood = (Ellipse)food;
                        break;
                    }
                }
            }
            if (collidedFood != null)
            {
                EatFood(collidedFood);
            }
        }

        private void CheckBigFoodCollision()
        {
            UIElement collidedBigFood = null;
            foreach (UIElement bigFood in bigFoods)
            {
                double left = Canvas.GetLeft(bigFood) - squareSize * 0.25;
                double top = Canvas.GetTop(bigFood) - squareSize * 0.25;

                if ((PacmanImage.Position.X == left) && (PacmanImage.Position.Y == top))
                {
                    collidedBigFood = bigFood;
                    break;
                }
            }
            if (collidedBigFood != null)
            {
                EatBigFood(collidedBigFood);
            }
            if (currFoodCount == 0)
            {
                FinishGame();
            }

        }

        private void CheckGhostCollision()
        {
            double ghostCollision = IsGhostCollision();

            switch (ghostCollision)
            {
                case 0:
                    break;
                case 1:
                    if (HandleGhostCollision(Ghost1)) isGhost1 = false;
                    break;
                case 2:
                    if (HandleGhostCollision(Ghost2)) isGhost2 = false;
                    break;
                case 3:
                    if (HandleGhostCollision(Ghost3)) isGhost3 = false;
                    break;
                case 4:
                    if (HandleGhostCollision(Ghost4)) isGhost4 = false;
                    break;
            }
        }

        private void CheckOutOfBounds()
        {
            if ((PacmanImage.Position.Y < 0) || (PacmanImage.Position.Y >= GameArea.ActualHeight) ||
                (PacmanImage.Position.X < 0) || (PacmanImage.Position.X >= GameArea.ActualWidth))
            {
                if (!isOutOfBounds)
                {
                    isOutOfBounds = true;
                    LoseLife();
                }
            }
            else
            {
                isOutOfBounds = false;
            }
        }

        private Point GetElementPosition(UIElement element)
        {
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);
            return new Point(left, top);
        }


        private bool HandleGhostCollision(Path ghost)
        {
            SolidColorBrush blueBrush = new SolidColorBrush(Colors.Blue);
            SolidColorBrush ghostFillBrush = ghost.Fill as SolidColorBrush;
            if (ghostFillBrush != null && ghostFillBrush.Color == blueBrush.Color)
            {
                currentScore += 200;
                GameArea.Children.Remove(ghost);
                return true;
            }
            else
            {
                LoseLife();
                return false;
            }
        }
        private double IsGhostCollision()
        {
            double pacmanX = PacmanImage.Position.X;
            double pacmanY = PacmanImage.Position.Y;

            double ghost1X = Canvas.GetLeft(Ghost1);
            double ghost1Y = Canvas.GetTop(Ghost1);

            double ghost2X = Canvas.GetLeft(Ghost2);
            double ghost2Y = Canvas.GetTop(Ghost2);

            double ghost3X = Canvas.GetLeft(Ghost3);
            double ghost3Y = Canvas.GetTop(Ghost3);

            double ghost4X = Canvas.GetLeft(Ghost4);
            double ghost4Y = Canvas.GetTop(Ghost4);

            if (pacmanX == ghost1X && pacmanY == ghost1Y && isGhost1)
            {
                return 1;
            }
            else if (pacmanX == ghost2X && pacmanY == ghost2Y && isGhost2)
            {
                return 2;
            }
            else if (pacmanX == ghost3X && pacmanY == ghost3Y && isGhost3)
            {
                return 3;
            }
            else if (pacmanX == ghost4X && pacmanY == ghost4Y && isGhost4)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }


        private void LoseLife()
        {
            remainingLives--;
            if (remainingLives > 0)
            {
                ResetPacman(DrawPacmanImage.DrawPacmanImageRight());
                gameTimer.IsEnabled = false;
                MessageBox.Show($"You died! But you still have {remainingLives} lives remaining.", "Pacman");
                RestartGame();
            }
            else
            {
                EndGame();
            }
        }

        private void ResetPacman(Path pacmanPath)
        {
            GameArea.Children.Remove(PacmanImage.UiElement);
            // Create a new pacmanPath and add it to the GameArea
            pacmanCanvas.Children.Clear();
            pacmanCanvas.Children.Add(pacmanPath);
            PacmanImage.UiElement = pacmanCanvas;
            GameArea.Children.Add(PacmanImage.UiElement);
        }
        private void EatFruit()
        {
            currentScore += 100;
            GameArea.Children.Remove(fruit);
            UpdateGameStatus();
            
        }
        private void ToggleGhostColor()
        {
            SolidColorBrush originalGhost1Brush = new SolidColorBrush(Colors.Red);
            SolidColorBrush originalGhost2Brush = new SolidColorBrush(Colors.Cyan);
            SolidColorBrush originalGhost3Brush = new SolidColorBrush(Colors.Yellow);
            SolidColorBrush originalGhost4Brush = new SolidColorBrush(Colors.Pink);
            HandleGhostColor(Ghost1, originalGhost1Brush);
            HandleGhostColor(Ghost2, originalGhost2Brush);
            HandleGhostColor(Ghost3, originalGhost3Brush);
            HandleGhostColor(Ghost4, originalGhost4Brush);
        }

        private void HandleGhostColor(Path ghost, SolidColorBrush originalGhostBrush)
        {
            SolidColorBrush blueBrush = new SolidColorBrush(Colors.Blue);
            SolidColorBrush ghostFillBrush = ghost.Fill as SolidColorBrush;
            ghost.Fill = (ghostFillBrush.Color == blueBrush.Color) ? originalGhostBrush : blueBrush;
        }

        private void EatFood(Ellipse ellipse)
        {
            --currFoodCount;
            currentScore += 10;
            GameArea.Children.Remove(ellipse);
            UpdateGameStatus();
        }
        private void EatBigFood(UIElement bigFood)
        {
            currentScore += 50;
            GameArea.Children.Remove(bigFood);
            bigFoods.Remove(bigFood);
            UpdateGameStatus();            

            // Create a timer to call ToggleGhostColor() after eight seconds
            if(!ghostTimer.IsEnabled) {
                ToggleGhostColor();
                ghostTimer.Interval = TimeSpan.FromSeconds(8);
                ghostTimer.Tick += (sender, e) =>
                {
                    if (ghostTimer.IsEnabled) ToggleGhostColor();
                    ghostTimer.IsEnabled = false;
                    ghostTimer.Stop(); // Stop the timer after calling ToggleGhostColor()

                };
                ghostTimer.Start(); // Start the timer
                ghostTimer.IsEnabled = true;
            }
        }

        private void UpdateGameStatus()
        {
            this.tbStatusScore.Text = currentScore.ToString();
            this.tbStatusLives.Text = remainingLives.ToString();
        }
        private void EndGame()
        {
            gameTimer.IsEnabled = false;
            ghostTimer.IsEnabled = false;
            MessageBox.Show($"You lost all your lives! Your score was {currentScore}", "Pacman");
            ResetValues();
        }
        private void FinishGame()
        {
            gameTimer.IsEnabled = false;
            ghostTimer.IsEnabled = false;
            MessageBox.Show($"You completed the game! Your score was {currentScore}", "Pacman");
            ResetValues();
        }
        private void ResetValues()
        {
            GameArea.Children.Remove(fruit);
            currentScore = 0;
            remainingLives = 3;
            DrawGameArea();
            SpawnGhosts();
            RestartGame();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
