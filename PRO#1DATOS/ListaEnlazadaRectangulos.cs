using System.Drawing;

namespace PRO_1DATOS
{
    public class ListaEnlazadaRectangulos
    {
        public RectanguloNodo[,] Matriz { get; private set; }

        public ListaEnlazadaRectangulos(int filas, int columnas, int offsetX = 0, int offsetY = 0)
        {
            Matriz = new RectanguloNodo[filas, columnas];
            CrearMatriz(filas, columnas, offsetX, offsetY);
        }

        private void CrearMatriz(int filas, int columnas, int offsetX, int offsetY)
        {
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    Matriz[i, j] = new RectanguloNodo(offsetX + j * 20, offsetY + i * 20, 20, 20);

                    // Conectar los nodos adyacentes
                    if (i > 0)
                    {
                        Matriz[i, j].Arriba = Matriz[i - 1, j];
                        Matriz[i - 1, j].Abajo = Matriz[i, j];
                    }
                    if (j > 0)
                    {
                        Matriz[i, j].Izquierda = Matriz[i, j - 1];
                        Matriz[i, j - 1].Derecha = Matriz[i, j];
                    }
                }
            }
        }
    }
}
