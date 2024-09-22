using System;
using System.Drawing;
using System.Drawing;

namespace PRO_1DATOS
{
    public class Poderes
    {
        public string Tipo { get; private set; }
        public int Valor { get; private set; }
        public Point Posicion { get; private set; }
        public Image Imagen { get; private set; }

        private Random random;

        public Poderes(string tipo, Point posicion, int valorItem)
        {
            random = new Random();
            Tipo = tipo;
            Posicion = posicion;
            Valor = (tipo == "combustible") ? 50 : 1; 
            Imagen = ObtenerImagenPoder(tipo);
        }

        private Image ObtenerImagenPoder(string tipo)
        {
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
            g.DrawImage(Imagen, Posicion.X, Posicion.Y, 20, 20);  
        }
    }
}