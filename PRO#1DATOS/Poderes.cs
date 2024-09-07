using System.Drawing;

namespace PRO_1DATOS
{
    public class Poderes
    {
        public string Tipo { get; set; }
        public int Valor { get; set; }
        public Point Posicion { get; set; }
        public Image Imagen { get; set; }

        public Poderes(string tipo, Point posicion)
        {
            Tipo = tipo;
            Posicion = posicion;
            Valor = (tipo == "combustible") ? 50 : 1;  // Valor predeterminado para combustible u otros poderes
            Imagen = ObtenerImagenPoder(tipo);
        }

        private Image ObtenerImagenPoder(string tipo)
        {
            // Devuelve una imagen diferente según el tipo de poder
            switch (tipo)
            {
                case "combustible":
                    return Recursos.ObtenerImagen("combustible");
                case "crecimiento_estela":
                    return Recursos.ObtenerImagen("crecimiento_estela");
                case "bomba":
                    return Recursos.ObtenerImagen("bomba");
                case "escudo":
                    return Recursos.ObtenerImagen("escudo");
                case "hiper_velocidad":
                    return Recursos.ObtenerImagen("hiper_velocidad");
                default:
                    return null;
            }
        }

        public void Dibujar(Graphics g)
        {
            g.DrawImage(Imagen, Posicion.X, Posicion.Y, 20, 20);  // Tamaño de 20x20
        }
    }
}
