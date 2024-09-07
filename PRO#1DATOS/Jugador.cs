using System;
using System.Collections.Generic;
using System.Drawing;

namespace PRO_1DATOS
{
    public class Jugador
    {
        public int X { get;  set; }
        public int Y { get;  set; }
        public Image MotoImagen { get;  set; }
        public Estela Estela { get;  set; }
        public int Velocidad { get;  set; }
        public int Combustible { get;  set; }
        public int OffsetX { get;  set; }
        public int OffsetY { get;  set; }
        public int GridWidth { get;  set; }
        public int GridHeight { get;  set; }
        public int CellSize { get;  set; }
        public List<Poderes> Inventario { get; set; }   
        public CollisionManager collisionManager { get; set; }

        public  Color Motocolor { get; set; }

        public Jugador(int x, int y, Image motoImagen, int offsetX, int offsetY, int gridWidth, int gridHeight, int cellSize, Color color, CollisionManager collisionManager)
        {
            X = x;
            Y = y;
            MotoImagen = motoImagen;
            Estela = new Estela(x, y, 3, color); // Ajusta esto si es necesario
            Velocidad = new Random().Next(1, 11); // Velocidad aleatoria entre 1 y 10
            Combustible = 100; // Combustible inicial al máximo
            OffsetX = offsetX;
            OffsetY = offsetY;
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            CellSize = cellSize;
            Combustible = 100;
            Motocolor = color;
            Inventario= new List<Poderes>();
            this.collisionManager = collisionManager ;
        }

        public void RecogerPoder(Poderes poder)
        {
            Inventario.Add(poder);
        }


        public void UsarPoder(int index)
        {
            if (index < 0 && index < Inventario.Count) 
            {
                Poderes poder = Inventario[index];
                switch(poder.Tipo)
                {
                    case "Celda de Combustible":
                        Combustible += poder.Valor;
                        if (Combustible > 100) Combustible = 100; // Limitar a 100
                        break;
                    case "Crecimiento de Estela":
                        Estela.Crecer(10);
                        break;
                    case "Bomba":
                        Explode();  // Explota si usa una bomba
                        break;
                    case "Escudo":
                        ActivarEscudo(poder.Valor);  // Activa escudo por un tiempo
                        break;
                    case "Hiper Velocidad":
                        ActivarHiperVelocidad(poder.Valor);  // Incrementar velocidad temporalmente
                        break;
                }
                Inventario.RemoveAt(index);  // Eliminar el ítem usado del inventario
            }
        }
       

        public void ActivarEscudo(int duracion)
        {

        }

        public void ActivarHiperVelocidad(int incremento)
        {
            Velocidad += incremento;
        }
        // Métodos para mover el jugador y consumir combustible
        public void MoverIzquierda()
        {
            if (X - CellSize >= OffsetX)  // Verifica que no se salga por el lado izquierdo de la cuadrícula
            {
                X -= CellSize;
                Estela.AgregarNodo(X, Y);  // Agregar un nuevo nodo a la estela
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this)) Explode(); // Verifica colisiones después de moverse
        }

        public void MoverDerecha()
        {
            if (X + CellSize < OffsetX + GridWidth)  // Verifica que no se salga por el lado derecho de la cuadrícula
            {
                X += CellSize;
                Estela.AgregarNodo(X, Y);  // Agregar un nuevo nodo a la estela
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this)) Explode(); // Verifica colisiones después de moverse
        }

        public void MoverArriba()
        {
            if (Y - CellSize >= OffsetY)  // Verifica que no se salga por el lado superior de la cuadrícula
            {
                Y -= CellSize;
                Estela.AgregarNodo(X, Y);  // Agregar un nuevo nodo a la estela
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this)) Explode(); // Verifica colisiones después de moverse
        }

        public void MoverAbajo()
        {
            if (Y + CellSize < OffsetY + GridHeight)  // Verifica que no se salga por el lado inferior de la cuadrícula
            {
                Y += CellSize;
                Estela.AgregarNodo(X, Y);  // Agregar un nuevo nodo a la estela
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this)) Explode(); // Verifica colisiones después de moverse
        }

        private void Explode()
        {
            // Implementar la lógica de explosión
            collisionManager.AddExplosion(new Explosion(X, Y));
            collisionManager.RemoveJugador(this);
        }
        public void ReducirCombustible(int cantidad)
        {
            Combustible -= cantidad;
            if (Combustible < 0) Combustible = 0;
        }

        private void ConsumirCombustible()
        {
            int celdasRecorridas = Velocidad * 5;
            ReducirCombustible(celdasRecorridas / 5);
        }
        private void ActualizarEstela()
        {
            Estela.AgregarNodo(X, Y);
        }
    }


}
