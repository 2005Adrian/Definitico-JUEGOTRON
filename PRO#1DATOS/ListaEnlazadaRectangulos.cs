using System;
using System.Collections.Generic;

namespace PRO_1DATOS
{
    public class ListaEnlazadaRectangulos
    {
        public List<RectanguloNodo> Matriz { get; set; }

        public ListaEnlazadaRectangulos(int filas, int columnas, int offsetX, int offsetY)
        {
            Matriz = new List<RectanguloNodo>();

            for (int fila = 0; fila < filas; fila++)
            {
                for (int columna = 0; columna < columnas; columna++)
                {
                    int x = offsetX + columna * 20;
                    int y = offsetY + fila * 20;
                    RectanguloNodo nodo = new RectanguloNodo(x, y, 20, 20);
                    Matriz.Add(nodo);
                }
            }
        }

        // This method will select a random node from the Matriz
        public RectanguloNodo ObtenerNodoAleatorio()
        {
            Random random = new Random();
            int indexAleatorio = random.Next(Matriz.Count); // Get a random index
            return Matriz[indexAleatorio];
        }
    }
}
