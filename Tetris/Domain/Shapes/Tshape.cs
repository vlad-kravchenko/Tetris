using System.Windows.Media;

namespace Tetris.Domain
{
    public class Tshape : Shape
    {
        public Tshape() : base()
        {
            Color = Brushes.Purple;
            cells.Add(new Cell { Row = 0, Col = 5 });
            cells.Add(new Cell { Row = 1, Col = 4 });
            cells.Add(new Cell { Row = 1, Col = 5 });
            cells.Add(new Cell { Row = 1, Col = 6 });
        }
    }
}