using System.Drawing;
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
            CrearMatriz(filas, columnas, offsetX, offsetY);  // Llamamos a CrearMatriz para llenar la lista
        }

        private void CrearMatriz(int filas, int columnas, int offsetX, int offsetY)
        {
            for (int fila = 0; fila < filas; fila++)
            {
                for (int columna = 0; columna < columnas; columna++)
                {
                    int x = offsetX + columna * 20;
                    int y = offsetY + fila * 20;
                    RectanguloNodo nodo = new RectanguloNodo(x, y, 20, 20);
                    Matriz.Add(nodo);  // Agregamos el nodo a la lista
                }
            }
        }

        // Método para seleccionar un nodo aleatorio de la matriz
        public RectanguloNodo ObtenerNodoAleatorio()
        {
            if (Matriz.Count == 0)
            {
                throw new InvalidOperationException("La lista Matriz está vacía. No se puede obtener un nodo aleatorio.");
            }

            Random random = new Random();
            int indexAleatorio = random.Next(Matriz.Count); // Obtenemos un índice aleatorio
            return Matriz[indexAleatorio];
        }
    }
}
