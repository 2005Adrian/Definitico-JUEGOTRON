using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1DATOS
{
    public class Bot : Jugador
    {
        private static Random random = new Random();

        public Bot(int x, int y, Image motoImagen, Color colorEstela, int offsetX, int offsetY, int gridWidth, int gridHeight, int cellSize)
            : base(x, y, motoImagen, offsetX, offsetY, gridWidth, gridHeight, cellSize, colorEstela)
        {
           
        }

        public void MoverHaciaJugador(Jugador jugador)
        {
            // Decide si el bot se mueve hacia el jugador o de forma aleatoria
            int decision = random.Next(100);

            if (decision < 50) // 70% de probabilidad de seguir al jugador
            {
                // Movimiento hacia el jugador
                if (Math.Abs(this.X - jugador.X) > Math.Abs(this.Y - jugador.Y))
                {
                    if (this.X > jugador.X)
                    {
                        MoverIzquierda();
                    }
                    else if (this.X < jugador.X)
                    {
                        MoverDerecha();
                    }
                }
                else
                {
                    if (this.Y > jugador.Y)
                    {
                        MoverArriba();
                    }
                    else if (this.Y < jugador.Y)
                    {
                        MoverAbajo();
                    }
                }
            }
            else
            {
                // Movimiento aleatorio
                MoverAleatorio();
            }
        }

        private void MoverAleatorio()
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
