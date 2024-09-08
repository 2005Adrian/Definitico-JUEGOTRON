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

            // Revisar colisiones con estelas y jugadores/bots
            if (collisionManager.CheckCollisions(this))
            {
                Explode(); // Eliminar bot si colisiona con estela o jugador
            }

            // Consumir combustible
            ConsumirCombustible();
            // Explota si se queda sin combustible
            if (Combustible <= 0)
            {
                Explode();
            }
        }

        public void RecogerPoder(Poderes poder)
        {
            base.RecogerPoder(poder); // Llama al método de la clase base (Jugador)
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

        public void Explode()
        {
            collisionManager.AddExplosion(new Explosion(X, Y));
            collisionManager.RemoveBot(this);
        }
        private void ConsumirCombustible()
        {
            int celdasRecorridas = Velocidad * 5;
            Combustible -= celdasRecorridas / 5;
            if (Combustible <= 0)
            {
                Explode();  // Si se queda sin combustible, explota
            }
        }

        public void RevisarColisionesConItems(List<Poderes> items)
        {
            // Recorrer todos los ítems en la lista para verificar si el bot colisiona con ellos
            for (int i = items.Count - 1; i >= 0; i--)
            {
                // Si la posición del bot coincide con la posición del ítem
                if (X == items[i].Posicion.X && Y == items[i].Posicion.Y)
                {
                    Console.WriteLine($"Bot ha recogido el poder: {items[i].Tipo} en la posición {items[i].Posicion.X}, {items[i].Posicion.Y}");

                    // Recoger el poder y activarlo inmediatamente
                    RecogerPoder(items[i]);
                    UsarPoder(Inventario.Count - 1);  // Activa el poder recién recogido

                    // Elimina el ítem de la lista de ítems
                    items.RemoveAt(i);  // Eliminar el ítem recogido para que desaparezca visualmente
                }
            }
        }

    }
}