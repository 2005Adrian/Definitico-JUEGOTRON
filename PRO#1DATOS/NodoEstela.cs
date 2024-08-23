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
        public int Longitud { get; private set; }

        public Estela(int xInicial, int yInicial)
        {
            // Crear la estela inicial con 3 posiciones
            Cabeza = new NodoEstela(xInicial, yInicial);
            NodoEstela segundoNodo = new NodoEstela(xInicial, yInicial + 10);
            NodoEstela tercerNodo = new NodoEstela(xInicial, yInicial + 20);

            Cabeza.Siguiente = segundoNodo;
            segundoNodo.Siguiente = tercerNodo;

            Longitud = 3;
        }

        public void AgregarNodo(int x, int y)
        {
            NodoEstela nuevoNodo = new NodoEstela(x, y);
            nuevoNodo.Siguiente = Cabeza;
            Cabeza = nuevoNodo;
            Longitud++;
        }

        public void EliminarUltimoNodo()
        {
            if (Cabeza == null) return;

            NodoEstela actual = Cabeza;
            while (actual.Siguiente?.Siguiente != null)
            {
                actual = actual.Siguiente;
            }

            actual.Siguiente = null;
            Longitud--;
        }

        public void DibujarEstela(Graphics g)
        {
            NodoEstela nodoActual = Cabeza;
            while (nodoActual != null)
            {
                g.FillRectangle(Brushes.Cyan, nodoActual.X, nodoActual.Y, 10, 10);
                nodoActual = nodoActual.Siguiente;
            }
        }
    }
}
