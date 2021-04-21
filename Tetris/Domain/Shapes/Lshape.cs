using System.Windows.Media;

namespace Tetris.Domain
{
    public class Lshape : Shape
    {
        public Lshape() : base()
        {
            Color = Brushes.Orange;
            cells.Add(new Cell { Row = 0, Col = 5 });
            cells.Add(new Cell { Row = 1, Col = 5 });
            cells.Add(new Cell { Row = 2, Col = 5 });
            cells.Add(new Cell { Row = 2, Col = 6 });
        }
    }
}