using System.Windows.Media;

namespace Tetris.Domain
{
    public class Jshape : Shape
    {
        public Jshape() : base()
        {
            Color = Brushes.Red;
            cells.Add(new Cell { Row = 0, Col = 6 });
            cells.Add(new Cell { Row = 1, Col = 6 });
            cells.Add(new Cell { Row = 2, Col = 6 });
            cells.Add(new Cell { Row = 2, Col = 5 });
        }
    }
}