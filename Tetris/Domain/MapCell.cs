using System.Windows.Media;

namespace Tetris.Domain
{
    public class MapCell : Cell
    {
        public bool Empty { get; set; }
        public Brush Color { get; set; }
    }
}