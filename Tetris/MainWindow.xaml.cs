using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tetris.Domain;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        Domain.Shape shape;
        Domain.Shape next;
        DispatcherTimer timer;
        List<MapCell> map = new List<MapCell>();
        int rows = 0;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            map.Clear();
            for (int row = 0; row < 20; row++)
                for (int col = 0; col < 10; col++)
                    map.Add(new MapCell { Row = row, Col = col, Empty = true });

            if (timer != null)
                timer.Stop();
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Start();
            rows = score = 0;
            Rows.Text = "Rows: 0";
            Score.Text = "Score: 0";
            shape = null;
            next = null;
            GetNewShape();
            UpdateGrid();
        }

        private void GetNewShape()
        {
            shape = next;
            int rand = new Random().Next(0, 7);
            switch (rand)
            {
                case 0:
                    next = new Ishape();
                    break;
                case 1:
                    next = new Jshape();
                    break;
                case 2:
                    next = new Lshape();
                    break;
                case 3:
                    next = new Oshape();
                    break;
                case 4:
                    next = new Sshape();
                    break;
                case 5:
                    next = new Tshape();
                    break;
                case 6:
                    next = new Zshape();
                    break;
            }
            if (shape == null)
            {
                GetNewShape();
            }
        }

        private void UpdateGrid()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < 10; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 20; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }

            NextGrid.Children.Clear();
            NextGrid.RowDefinitions.Clear();
            NextGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < 4; i++)
            {
                NextGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 3; i++)
            {
                NextGrid.RowDefinitions.Add(new RowDefinition());
            }

            DrawMap();
            DrawShape();
            DrawNext();
        }

        private void DrawNext()
        {
            foreach (var cell in next.Cells)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = next.Color;
                NextGrid.Children.Add(rect);
                Grid.SetRow(rect, cell.Row);
                Grid.SetColumn(rect, cell.Col - 3);
            }
        }

        private void DrawMap()
        {
            foreach (var cell in map)
            {
                if (!cell.Empty)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = cell.Color;
                    MainGrid.Children.Add(rect);
                    Grid.SetRow(rect, cell.Row);
                    Grid.SetColumn(rect, cell.Col);
                }
            }
        }

        private void DrawShape()
        {
            foreach (var cell in shape.Cells)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = shape.Color;
                MainGrid.Children.Add(rect);
                Grid.SetRow(rect, cell.Row);
                Grid.SetColumn(rect, cell.Col);
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.A)
            {
                if (shape.Offset(-1, map))
                    UpdateGrid();
            }
            else if (e.Key == System.Windows.Input.Key.D)
            {
                if (shape.Offset(1, map))
                    UpdateGrid();
            }
            else if (e.Key == System.Windows.Input.Key.S)
            {
                if (shape.Move(map))
                    UpdateGrid();
            }
            else if (e.Key == System.Windows.Input.Key.W)
            {
                if (shape.Rotate(map))
                    UpdateGrid();
            }
            else if (e.Key == System.Windows.Input.Key.Space)
            {
                if (timer.IsEnabled)
                    timer.Stop();
                else
                    timer.Start();
            }
            if (!shape.CanMove)
            {
                AddShapeToMap();
                GetNewShape();
                UpdateGrid();
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            if (!shape.Move(map))
            {
                AddShapeToMap();
                GetNewShape();
            }
            UpdateGrid();
            if (map.Any(c => c.Row == 0 && !c.Empty))
            {
                MessageBox.Show("You lost");
                NewGame();
            }
        }

        private void AddShapeToMap()
        {
            foreach (var cell in shape.Cells)
            {
                var mapCell = map.FirstOrDefault(c => c.Row == cell.Row && c.Col == cell.Col);
                mapCell.Color = shape.Color;
                mapCell.Empty = false;
            }
            CheckFullRow();
        }

        private void CheckFullRow()
        {
            for (int i = 0; i < 20; i++)
            {
                int rowCount = map.Count(c => c.Empty == false && c.Row == i);
                if (rowCount == 10)
                {
                    List<Brush> colors = new List<Brush>();
                    foreach (var cell in map.Where(c => c.Row == i))
                    {
                        colors.Add(cell.Color);
                        cell.Empty = true;
                    }
                    rows++;
                    score += GetScore(colors);
                    Rows.Text = "Rows: " + rows;
                    Score.Text = "Score: " + score;
                }
            }
            for (int j = 0; j < 20; j++)
            {
                for (int i = 19; i > 0; i--)
                {
                    int rowDown = map.Count(c => c.Empty == false && c.Row == i);
                    int rowUp = map.Count(c => c.Empty == false && c.Row == i - 1);
                    if (rowUp > rowDown && rowDown == 0)
                        SwapRows(i, i - 1);
                }
            }
        }

        private int GetScore(List<Brush> colors)
        {
            int sum = 0;
            foreach (var col in colors.Distinct())
            {
                if (colors.Count(c => c == col) > 1)
                {
                    sum += colors.Count(c => c == col);
                }
            }
            return sum;
        }

        private void SwapRows(int r1, int r2)
        {
            for (int i = 0; i < 10; i++)
            {
                var cellDown = map.FirstOrDefault(c => c.Row == r1 && c.Col == i);
                var cellUp = map.FirstOrDefault(c => c.Row == r2 && c.Col == i);

                cellDown.Color = cellUp.Color;
                cellDown.Empty = cellUp.Empty;
                cellUp.Empty = true;
            }
        }
    }
}