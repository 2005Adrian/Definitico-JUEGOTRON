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
        //public int maxLongitud;
        public Color color { get; private set; }
        public Estela(int xInicial, int yInicial, Color color)
        {
            this.color = color;
            Cabeza = new NodoEstela(xInicial, yInicial);
            Longitud = 1;

            // Asegúrate de que la estela del jugador no colisione consigo misma al iniciar
            NodoEstela segundoNodo = new NodoEstela(xInicial, yInicial + 20); // Estela se extiende lejos del jugador
            Cabeza.Siguiente = segundoNodo;
            Longitud++;
        }


        public void AgregarNodo(int x, int y)
        {
            NodoEstela nuevoNodo = new NodoEstela(x, y);
            nuevoNodo.Siguiente = Cabeza;
            Cabeza = nuevoNodo;
            Longitud++;
        }

        ///public void EliminarUltimoNodo()
        ///{
           // if (Cabeza == null) return;

           // NodoEstela actual = Cabeza;
            //while (actual.Siguiente?.Siguiente != null)
            //{
               // actual = actual.Siguiente;
           // }

            //actual.Siguiente = null;
           // Longitud--;
        //}
        

        public void DibujarEstela(Graphics g)
        {
            NodoEstela nodoActual = Cabeza;
            while (nodoActual != null)
            {
                using (Brush brush = new SolidBrush(this.color)) // Usamos el color personalizado
                {
                    g.FillRectangle(brush, nodoActual.X, nodoActual.Y, 10, 10);
                }
                nodoActual = nodoActual.Siguiente;
            }
        }
    }
}
