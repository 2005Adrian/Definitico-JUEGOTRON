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
        public int X { get; set; }
        public int Y { get; set; }
        public Image MotoImagen { get; set; }
        public Estela Estela { get; private set; }
        private int step = 10; // Cantidad de píxeles que se mueve la moto
        private int maxX;
        private int maxY;

        public Jugador(int x, int y, Image motoImagen, int maxX, int maxY)
        {
            X = x;
            Y = y;
            MotoImagen = motoImagen;
            Estela = new Estela(x, y);
            this.maxX = maxX;
            this.maxY = maxY;
        }

        public void MoverIzquierda()
        {
            if (X - step >= 0) // Verifica que no salga del borde izquierdo
            {
                X -= step;
                ActualizarEstela();
            }
        }

        public void MoverDerecha()
        {
            if (X + step + 40 <= maxX) // Verifica que no salga del borde derecho
            {
                X += step;
                ActualizarEstela();
            }
        }

        public void MoverArriba()
        {
            if (Y - step >= 0) // Verifica que no salga del borde superior
            {
                Y -= step;
                ActualizarEstela();
            }
        }

        public void MoverAbajo()
        {
            if (Y + step + 40 <= maxY) // Verifica que no salga del borde inferior
            {
                Y += step;
                ActualizarEstela();
            }
        }

        private void ActualizarEstela()
        {
            Estela.AgregarNodo(X, Y);
            Estela.EliminarUltimoNodo(); // Mantener la longitud constante
        }
    }
}