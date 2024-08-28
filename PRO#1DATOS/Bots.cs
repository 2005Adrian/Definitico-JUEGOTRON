using System;
using System.Drawing;

namespace PRO_1DATOS
{
    public class Bot : Jugador
    {
        private static Random random = new Random();

        public Bot(int x, int y, Image motoImagen, Color colorEstela, int offsetX, int offsetY, int gridWidth, int gridHeight, int cellSize)
            : base(x, y, motoImagen, offsetX, offsetY, gridWidth, gridHeight, cellSize, colorEstela)
        {
        }

        public void MoverAleatorio()
        {
            int direccion = random.Next(4); // 0: Izquierda, 1: Derecha, 2: Arriba, 3: Abajo

            switch (direccion)
            {
                case 0:
                    MoverIzquierda();
                    break;
                case 1:
                    MoverDerecha();
                    break;
                case 2:
                    MoverArriba();
                    break;
                case 3:
                    MoverAbajo();
                    break;
            }
        }
    }
}
