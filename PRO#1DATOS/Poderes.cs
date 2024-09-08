using System;
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

        public Poderes(string tipo, Point posicion)
        {
            random = new Random();
            Tipo = tipo;
            Posicion = posicion;

            // Asignar un valor aleatorio y una imagen basada en el tipo
            switch (tipo)
            {
                case "Celda de Combustible":
                    Valor = random.Next(10, 50); // Combustible aleatorio entre 10 y 50
                    Imagen = Recursos.ObtenerImagen("combustible");
                    break;
                case "Crecimiento de Estela":
                    Valor = random.Next(1, 10);  // Crecimiento aleatorio entre 1 y 10
                    Imagen = Recursos.ObtenerImagen("crecimiento_estela");
                    break;
                case "Bomba":
                    Valor = 0; // Valor 0 porque las bombas explotan
                    Imagen = Recursos.ObtenerImagen("bomba");
                    break;
                case "Escudo":
                    Valor = random.Next(5, 15);  // Tiempo de escudo aleatorio entre 5 y 15 segundos
                    Imagen = Recursos.ObtenerImagen("escudo");
                    break;
                case "Hiper Velocidad":
                    Valor = random.Next(10, 20); // Velocidad extra aleatoria
                    Imagen = Recursos.ObtenerImagen("hiper_velocidad");
                    break;
            }
        }

        public void Dibujar(Graphics g)
        {
            // Dibujar la imagen en la posición del ítem/poder
            g.DrawImage(Imagen, Posicion.X, Posicion.Y, 20, 20);
        }
    }
}
