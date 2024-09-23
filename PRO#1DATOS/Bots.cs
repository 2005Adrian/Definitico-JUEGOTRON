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
            if (X <= OffsetX || X >= OffsetX + GridWidth - CellSize || Y <= OffsetY || Y >= OffsetY + GridHeight - CellSize)
            {
                Explode(); 
            }

            if (collisionManager.CheckCollisions(this))
            {
                Explode(); 
            }

            ConsumirCombustible();
            if (Combustible <= 0)
            {
                Explode();
            }
        }

        public void RecogerPoder(Poderes poder)
        {
            base.RecogerPoder(poder); 
        }



        public Direction GetRandomDirection()
        {
            return (Direction)random.Next(4);
        }
        public Direction GetNewDirection()
        {
            Direction newDirection;
            do
            {
                newDirection = GetRandomDirection();
            } while (IsOppositeDirection(newDirection));
            return newDirection;
        }
        public bool IsOppositeDirection(Direction newDirection)
        {
            return (currentDirection == Direction.Up && newDirection == Direction.Down) ||
                   (currentDirection == Direction.Down && newDirection == Direction.Up) ||
                   (currentDirection == Direction.Left && newDirection == Direction.Right) ||
                   (currentDirection == Direction.Right && newDirection == Direction.Left);
        }

        public void Explode()
        {
            collisionManager.AddExplosion(new Explosion(X, Y));
            collisionManager.RemoveBot(this);
        }
        public void ConsumirCombustible()
        {
            int celdasRecorridas = Velocidad * 5;
            Combustible -= celdasRecorridas / 5;
            if (Combustible <= 0)
            {
                Explode();  
            }
        }

        public void RevisarColisionesConItems(List<Poderes> items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (X == items[i].Posicion.X && Y == items[i].Posicion.Y)
                {
                    Console.WriteLine($"Bot ha recogido el poder: {items[i].Tipo} en la posición {items[i].Posicion.X}, {items[i].Posicion.Y}");

                    RecogerPoder(items[i]);
                    UsarPoder(Inventario.Count - 1);  

                    items.RemoveAt(i);  
                }
            }
        }

    }
}