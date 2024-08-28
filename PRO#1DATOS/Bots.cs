using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace PRO_1DATOS

{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Bot : Jugador
    {
        private static Random random = new Random();
        private Direction currentDirection;
        private CollisionManager collisionManager;

        public Bot(int x, int y, Image motoImagen, Color color, int offsetX, int offsetY, int gridWidth, int gridHeight, int cellSize, CollisionManager collisionManager)
            : base(x, y, motoImagen, offsetX, offsetY, gridWidth, gridHeight, cellSize)
        {
            this.collisionManager = collisionManager;
            currentDirection = GetRandomDirection();
        }

        public void MoverAleatorio()
        {
            // Decidir si cambiar de dirección
            int decision = random.Next(100);

            if (decision < 20) // 20% de probabilidad de cambiar de dirección
            {
                currentDirection = GetNewDirection();
            }

            // Moverse en la dirección actual
            switch (currentDirection)
            {
                case Direction.Up:
                    MoverArriba();
                    break;
                case Direction.Right:
                    MoverDerecha();
                    break;
                case Direction.Down:
                    MoverAbajo();
                    break;
                case Direction.Left:
                    MoverIzquierda();
                    break;
            }

            // Verificar colisión con los bordes de la cuadrícula
            if (X <= offsetX || X >= offsetX + gridWidth - cellSize || Y <= offsetY || Y >= offsetY + gridHeight - cellSize)
            {
                Explode(); // El bot explota si choca con el borde
            }
        }

        private Direction GetRandomDirection()
        {
            return (Direction)random.Next(4);
        }

        private Direction GetNewDirection()
        {
            // Obtener una nueva dirección que no sea la opuesta a la actual
            Direction newDirection;
            do
            {
                newDirection = GetRandomDirection();
            } while (IsOppositeDirection(newDirection));

            return newDirection;
        }

        private bool IsOppositeDirection(Direction newDirection)
        {
            return (currentDirection == Direction.Up && newDirection == Direction.Down) ||
                   (currentDirection == Direction.Down && newDirection == Direction.Up) ||
                   (currentDirection == Direction.Left && newDirection == Direction.Right) ||
                   (currentDirection == Direction.Right && newDirection == Direction.Left);
        }

        private void Explode()
        {
            // Crear una nueva explosión en la ubicación del bot
            collisionManager.AddExplosion(new Explosion(X, Y));

            // Eliminar el bot de la lista de bots
            collisionManager.RemoveBot(this);
        }
    }


}
