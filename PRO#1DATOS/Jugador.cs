using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PRO_1DATOS
{
    public class Jugador
    {
        public int X { get;  set; }
        public int Y { get;  set; }
        public Image MotoImagen { get; set; }

        public Jugador(int x, int y, Image motoImagen)
        {
            X = x;
            Y = y;
            MotoImagen = motoImagen;
        }

        public void MoverIzquierda()
        {
            X -= 20; // Mover 20 píxeles a la izquierda
        }

        public void MoverDerecha()
        {
            X += 20; // Mover 20 píxeles a la derecha
        }

        public void MoverArriba()
        {
            Y -= 20; // Mover 20 píxeles hacia arriba
        }

        public void MoverAbajo()
        {
            Y += 20; // Mover 20 píxeles hacia abajo
        }
    }
}
