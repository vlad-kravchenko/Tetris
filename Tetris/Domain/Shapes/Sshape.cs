using System.Windows.Media;

namespace Tetris.Domain
{
    public class Sshape : Shape
    {
        public Sshape() : base()
        {
            Color = Brushes.Olive;
            cells.Add(new Cell { Row = 0, Col = 5 });
            cells.Add(new Cell { Row = 0, Col = 6 });
            cells.Add(new Cell { Row = 1, Col = 4 });
            cells.Add(new Cell { Row = 1, Col = 5 });
        }
    }
}