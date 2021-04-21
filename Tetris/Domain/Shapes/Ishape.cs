using System.Windows.Media;

namespace Tetris.Domain
{
    public class Ishape : Shape
    {
        public Ishape() : base()
        {
            Color = Brushes.Green;
            cells.Add(new Cell { Row = 0, Col = 3 });
            cells.Add(new Cell { Row = 0, Col = 4 });
            cells.Add(new Cell { Row = 0, Col = 5 });
            cells.Add(new Cell { Row = 0, Col = 6 });
        }
    }
}