using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PRO_1DATOS
{
    

    public class Estela
    {
        public NodoEstela Cabeza { get; private set; }
        private Color color;

        public Estela(int x, int y, int longitudInicial, Color color)
        {
            this.color = color;
            
            for (int i = 0; i < longitudInicial; i++)
            {
                AgregarNodo(x, y);
            }
        }

        public void AgregarNodo(int x, int y)
        {
            int ajustedX = (x / 20) * 20;
            int ajustedY = (y / 20) * 20;

            NodoEstela nuevoNodo = new NodoEstela(x, y);
            nuevoNodo.Siguiente = Cabeza;
            Cabeza = nuevoNodo;

            if (ObtenerLongitud() > 10)
            {
                Reducir(1);
            }
        }

        public void Reducir(int decremento)
        {
            NodoEstela actual = Cabeza;
            for (int i = 0; i < decremento && actual.Siguiente != null; i++)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = null; // Cortar la estela
        }

        public int ObtenerLongitud()
        {
            int longitud = 0;
            NodoEstela actual = Cabeza;
            while (actual != null)
            {
                longitud++;
                actual = actual.Siguiente;
            }
            return longitud;
        }

        public void DibujarEstela(Graphics g)
        {
            NodoEstela actual = Cabeza;
            while (actual != null)
            {
                g.FillRectangle(new SolidBrush(color), actual.X, actual.Y, 20, 20);  // Asegúrate de que las coordenadas estén alineadas con las celdas
                actual = actual.Siguiente;
            }
        }

        public void Crecer(int incremento)
        {
            NodoEstela nodoActual = Cabeza;
            for (int i = 0; i < incremento; i++)
            {
                if (nodoActual != null)
                {
                    AgregarNodo(nodoActual.X, nodoActual.Y);

                }
                AgregarNodo(Cabeza.X, Cabeza.Y);
            }
        }
    }

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
}