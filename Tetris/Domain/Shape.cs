using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Domain
{
    public class Shape
    {
        protected bool stopped = false;
        protected List<Cell> cells = new List<Cell>();

        public bool CanMove { get { return !stopped; } }
        public List<Cell> Cells { get { return cells; } }
        public Brush Color { get; protected set; }

        public Shape() { }

        public bool Offset(int step, List<MapCell> map)
        {
            if (step == 0 || stopped) return false;
            else if (step > 0)
            {
                if (cells.Max(c => c.Col) > 8) return false;
                foreach (var cell in cells)
                {
                    if (map.FirstOrDefault(c => c.Col == cell.Col + step && c.Row == cell.Row && !c.Empty) != null) return false;
                    if (map.FirstOrDefault(c => c.Row == cell.Row && Math.Abs(c.Col - cell.Col) == 1 && !c.Empty) != null) return false;
                }
                foreach (var cell in cells)
                {
                    cell.Col += step;
                }
                return true;
            }
            else
            {
                if (cells.Min(c => c.Col) < 1) return false;
                foreach (var cell in cells)
                {
                    if (map.FirstOrDefault(c => c.Col == cell.Col - step && c.Row == cell.Row && !c.Empty) != null) return false;
                    if (map.FirstOrDefault(c => c.Row == cell.Row && Math.Abs(c.Col - cell.Col) == 1 && !c.Empty) != null) return false;
                }
                foreach (var cell in cells)
                {
                    cell.Col += step;
                }
                return true;
            }
        }

        public bool Move(List<MapCell> map)
        {
            int maxRow = cells.Max(c => c.Row);
            if (maxRow > 18 || stopped)
            {
                stopped = true;
                return false;
            }
            foreach (var cell in cells)
            {
                if (map.FirstOrDefault(c => c.Row == cell.Row + 1 && c.Col == cell.Col && !c.Empty) != null) return false;
            }
            cells.ForEach(c => c.Row++);
            return true;
        }

        public bool Rotate(List<MapCell> map)
        {
            if (stopped) return false;
            int baseIndex = 2;
            List<Cell> test = new List<Cell>();
            for (int i = 0; i < cells.Count; i++)
            {
                Point rotated = RotatePoint(new Point(cells[i].Col, cells[i].Row), new Point(cells[baseIndex].Col, cells[baseIndex].Row), 90 * Math.PI / 180);
                Cell testCell = new Cell();
                testCell.Row = (int)rotated.Y;
                testCell.Col = (int)rotated.X;
                test.Add(testCell);
            }
            foreach(var cell in test)
            {
                if (!InRange(cell)) return false;
                if (map.Any(c => c.Row == cell.Row && c.Col == cell.Col && !c.Empty)) return false;
            }
            cells = test;
            return true;
        }

        private bool InRange(Cell cell)
        {
            return cell.Row > -1 && cell.Row < 20 && cell.Col > -1 && cell.Col < 10;
        }

        private Point RotatePoint(Point point, Point pivot, double radians)
        {
            var cosTheta = Math.Cos(radians);
            var sinTheta = Math.Sin(radians);

            var x = (cosTheta * (point.X - pivot.X) - sinTheta * (point.Y - pivot.Y) + pivot.X);
            var y = (sinTheta * (point.X - pivot.X) + cosTheta * (point.Y - pivot.Y) + pivot.Y);

            return new Point((float)x, (float)y);
        }
    }
}