using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PRO_1DATOS
{
    public class NodoEstela
    {
        public int X { get; set; }
        public int Y { get; set; }
        public NodoEstela Siguiente { get; set; }

        public NodoEstela(int x, int y)
        {
            X = x;
            Y = y;
            Siguiente = null;
        }
    }

    public class Estela
    {
        public NodoEstela Cabeza { get; private set; }
        private int longitudMaxima;
        private Color color;

        public Estela(int xInicial, int yInicial, int longitudMaxima, Color color)
        {
            this.color = color;
            this.longitudMaxima = longitudMaxima;
            Cabeza = new NodoEstela(xInicial, yInicial);
        }

        public void AgregarNodo(int x, int y)
        {
            NodoEstela nuevoNodo = new NodoEstela(x, y);
            nuevoNodo.Siguiente = Cabeza;
            Cabeza = nuevoNodo;
        }

        public void DibujarEstela(Graphics g)
        {
            NodoEstela nodoActual = Cabeza;
            while (nodoActual != null)
            {
                g.FillRectangle(new SolidBrush(color), nodoActual.X, nodoActual.Y, 20, 20);
                nodoActual = nodoActual.Siguiente;
            }
        }
        public void  Crecer(int incremento)
        {
            NodoEstela nodoActual = Cabeza;
            for (int i = 0; i < incremento; i++)
            {
                if (nodoActual != null)
                {
                    AgregarNodo(nodoActual.X, nodoActual.Y);

                }
            }
        }

    }
}
