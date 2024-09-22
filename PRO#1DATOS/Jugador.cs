using System;
using System.Collections.Generic;
using System.Drawing;

namespace PRO_1DATOS
{
    public class Jugador
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Image MotoImagen { get; set; }
        public Estela Estela { get; set; }
        public int Velocidad { get; set; }
        public int Combustible { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public int CellSize { get; set; }
        public List<Poderes> Inventario { get; set; }
        public CollisionManager collisionManager { get; set; }

        public Color Motocolor { get; set; }

        public Jugador(int x, int y, Image motoImagen, int offsetX, int offsetY, int gridWidth, int gridHeight, int cellSize, Color color, CollisionManager collisionManager)
        {
            X = x;
            Y = y;
            MotoImagen = motoImagen;
            Estela = new Estela(x, y, 3, color); 
            Velocidad = new Random().Next(1, 11); 
            Combustible = 100; 
            OffsetX = offsetX;
            OffsetY = offsetY;
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            CellSize = cellSize;
            Combustible = 100;
            Motocolor = color;
            Inventario = new List<Poderes>();
            this.collisionManager = collisionManager;
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
                switch (poder.Tipo)
                {
                    case "combustible":
                        Combustible= 100; 
                        break;
                    case "crecimiento_estela":
                        Estela.Crecer(10);
                        break;
                    case "bomba":
                        Explode();  
                        break;
                    case "escudo":
                        ActivarEscudo(poder.Valor);  
                        break;
                    case "hiper_velocidad":
                        ActivarHiperVelocidad(poder.Valor);  
                        break;
                }
                Inventario.RemoveAt(index);  
            }
        }

        
        public void ActivarEscudo(int duracion)
        {

        }

        public void ActivarHiperVelocidad(int incremento)
        {
            Velocidad += incremento;
        }

        public void MoverIzquierda()
        {
            if (X - CellSize >= OffsetX)  
            {
                X -= CellSize;
                Estela.AgregarNodo(X, Y);  
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this))
            {
                collisionManager.Explode(this); 
            }
        }

        public void MoverDerecha()
        {
            if (X + CellSize < OffsetX + GridWidth) 
            {
                X += CellSize;
                Estela.AgregarNodo(X, Y);  
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this))
            {
                collisionManager.Explode(this);  
            }
        }

        public void MoverArriba()
        {
            if (Y - CellSize >= OffsetY) 
            {
                Y -= CellSize;
                Estela.AgregarNodo(X, Y);  
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this))
            {
                collisionManager.Explode(this);  
            }
        }

        public void MoverAbajo()
        {
            if (Y + CellSize < OffsetY + GridHeight)  
            {
                Y += CellSize;
                Estela.AgregarNodo(X, Y);  
            }
            ConsumirCombustible();
            if (collisionManager.CheckCollisions(this))
            {
                collisionManager.Explode(this);  
            }
        }

        private void Explode()
        {
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
            int celdasRecorridas = Velocidad * 2;
            ReducirCombustible(celdasRecorridas /10);
        }
        private void ActualizarEstela()
        {
            Estela.AgregarNodo(X, Y);
        }
        


        public void RevisarColisionesConItems(List<Poderes> items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (X == items[i].Posicion.X && Y == items[i].Posicion.Y)
                {
                    Console.WriteLine($"Recogido poder: {items[i].Tipo} en la posición {items[i].Posicion.X}, {items[i].Posicion.Y}");
                    if (items[i].Tipo == "combustible")
                    {
                        // Recargar la gasolina a 100
                        Combustible = 100;
                    }
                    
                    else
                    {
                        RecogerPoder(items[i]);
                        UsarPoder(Inventario.Count - 1);  // Activa el poder recién recogido
                    }
                    items.RemoveAt(i);  // elimino el ítem recogido para que desaparezca de la cuadricula
                }
            }
        }


    }


}
