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
        public int X { get; private set; }
        public int Y { get; private set; }
        public Image MotoImagen { get; set; }
        public Estela Estela { get; set; }
        private int offsetX;
        private int offsetY;
        private int gridWidth;  // Ancho total de la cuadrícula en píxeles
        private int gridHeight; // Alto total de la cuadrícula en píxeles
        private int cellSize;   // Tamaño de cada celda (asumimos que es cuadrada)

        public Jugador(int x, int y, Image motoImagen, int offsetX, int offsetY, int gridWidth, int gridHeight, int cellSize = 20, Color? colorEstela = null)
        {
            X = x;
            Y = y;
            MotoImagen = motoImagen;
            // Si no se especifica un color para la estela, se usa Cyan por defecto
            Estela = new Estela(x, y, colorEstela ?? Color.Cyan);
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            this.cellSize = cellSize;
        }
        public void MoverIzquierda()
        {
            if (X - cellSize >= offsetX)  // Verifica que no se salga por el lado izquierdo de la cuadrícula
            {
                X -= cellSize;
            }
            Estela.AgregarNodo(X, Y);
        }

        public void MoverDerecha()
        {
            if (X + cellSize < offsetX + gridWidth)  // Verifica que no se salga por el lado derecho de la cuadrícula
            {
                X += cellSize;
            }
            Estela.AgregarNodo(X, Y);
        }

        public void MoverArriba()
        {
             
            if (Y - cellSize >= offsetY)  // Verifica que no se salga por la parte superior de la cuadrícula
            {
                Y -= cellSize;
            }
            Estela.AgregarNodo(X, Y);
        }

        public void MoverAbajo()
        {
            if (Y + cellSize < offsetY + gridHeight)  // Verifica que no se salga por la parte inferior de la cuadrícula
            {
                Y += cellSize;
            }
            Estela.AgregarNodo(X, Y);
        }
    }
}
