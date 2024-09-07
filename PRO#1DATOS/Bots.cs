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
        : base(x, y, motoImagen, offsetX, offsetY, gridWidth, gridHeight, cellSize, color, collisionManager)
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
            if (X <= OffsetX || X >= OffsetX + GridWidth - CellSize || Y <= OffsetY || Y >= OffsetY + GridHeight - CellSize)
            {
                Explode(); // El bot explota si choca con el borde
            }

            // Consumir combustible
            ConsumirCombustible();

            // Explota si se queda sin combustible
            if (Combustible <= 0)
            {
                Explode();
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
            collisionManager.AddExplosion(new Explosion(X, Y));
            collisionManager.RemoveBot(this);
        }

        private void ConsumirCombustible()
        {
            int celdasRecorridas = Velocidad * 5;
            Combustible -= celdasRecorridas / 5;
            if (Combustible < 0) Combustible = 0;
        }
    }
}

