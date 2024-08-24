using System.Drawing;

namespace PRO_1DATOS
{
    public class RectanguloNodo
    {
        public Rectangle Rectangulo { get; set; }
        public RectanguloNodo? Arriba { get; set; } = null;
        public RectanguloNodo? Abajo { get; set; } = null;
        public RectanguloNodo? Izquierda { get; set; } = null;
        public RectanguloNodo? Derecha { get; set; } = null;

        public RectanguloNodo(int x, int y, int anchoCelda, int altoCelda)
        {
            Rectangulo = new Rectangle(x, y, anchoCelda, altoCelda);
        }
    }
}
