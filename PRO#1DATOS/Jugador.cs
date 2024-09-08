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
        private bool escudoActivo { get; set; }
        private System.Windows.Forms.Timer shieldTimer;
        private System.Windows.Forms.Timer hiperVelocidadTimer;

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
            Inventario = new List<Poderes>();
            this.collisionManager = collisionManager;

            escudoActivo = false;
            shieldTimer = new System.Windows.Forms.Timer();
            shieldTimer.Interval = 20000; // 20 segundos
            shieldTimer.Tick += (s, e) => DesactivarEscudo();

            hiperVelocidadTimer = new System.Windows.Forms.Timer();
            hiperVelocidadTimer.Interval = 10000; // Hiper velocidad durante 10 segundos
            hiperVelocidadTimer.Tick += (s, e) => DesactivarHiperVelocidad();
        }

        public void RecogerPoder(Poderes poder)
        {
            if (poder.Tipo == "combustible")
            {
                Combustible += poder.Valor;
                if (Combustible > 100) Combustible = 100;
            }
            else
            {
                Inventario.Add(poder);
            }
        }

        public void UsarPoder(int index)
        {
            if (index >= 0 && index < Inventario.Count)
            {
                Poderes poder = Inventario[index];
                switch (poder.Tipo)
                {
                    case "combustible":
                        Combustible += poder.Valor;
                        if (Combustible > 100) Combustible = 100;
                        break;
                    case "crecimiento_estela":
                        ActivarCrecimientoEstela(10);
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
            escudoActivo = true;
            MotoImagen = Recursos.ObtenerImagen("escudo");
            shieldTimer.Start();
        }

        public void DesactivarEscudo()
        {
            escudoActivo = false;
            MotoImagen = Recursos.ObtenerImagen("moto"); // Volver a la imagen original
            shieldTimer.Stop();
        }

        public void ActivarCrecimientoEstela(int incremento)
        {
            Estela.Crecer(incremento);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 30000;
            timer.Tick += (s, e) => ReducirEstela(incremento);
            timer.Start();
        }

        private void ReducirEstela(int decremento)
        {
            Estela.Reducir(decremento);
        }

        public void ActivarHiperVelocidad(int incremento)
        {
            Velocidad += incremento;
            hiperVelocidadTimer.Interval = 15000; // La hiper velocidad dura 15 segundos
            hiperVelocidadTimer.Start(); // Iniciar el temporizador para desactivar la hiper velocidad
        }

        private void DesactivarHiperVelocidad()
        {
            Velocidad = new Random().Next(1, 11); // Restablecer a una velocidad aleatoria
            hiperVelocidadTimer.Stop(); // Detener el temporizador
        }

        public void MoverIzquierda(GameForm gameForm)
        {
            if (X - CellSize >= OffsetX && Combustible > 0)  // Asegurarnos de que solo se pueda mover si hay combustible
            {
                X -= CellSize;
                Estela.AgregarNodo(X + CellSize / 2, Y + CellSize / 2);  // Ajusta la posición de la estela al centro de la celda
                ConsumirCombustible(); // Consumir combustible solo al moverse
                RevisarColisionesConItems(gameForm);
                if (collisionManager.CheckCollisions(this)) Explode();
            }
        }

        public void MoverDerecha(GameForm gameForm)
        {
            if (X + CellSize < OffsetX + GridWidth && Combustible > 0)  // Asegurarnos de que solo se pueda mover si hay combustible
            {
                X += CellSize;
                Estela.AgregarNodo(X + CellSize / 2, Y + CellSize / 2);  // Ajusta la posición de la estela al centro de la celda
                ConsumirCombustible(); // Consumir combustible solo al moverse
                RevisarColisionesConItems(gameForm);
                if (collisionManager.CheckCollisions(this)) Explode();
            }
        }

       

        public void MoverArriba(GameForm gameForm)
        {
            if (Y - CellSize >= OffsetY && Combustible > 0)  // Asegurarnos de que solo se pueda mover si hay combustible
            {
                Y -= CellSize;
                Estela.AgregarNodo(X + CellSize / 2, Y + CellSize / 2);  // Ajusta la posición de la estela al centro de la celda
                ConsumirCombustible(); // Consumir combustible solo al moverse
                RevisarColisionesConItems(gameForm);
                if (collisionManager.CheckCollisions(this)) Explode();
            }
        }


        public void MoverAbajo(GameForm gameForm)
        {
            if (Y + CellSize < OffsetY + GridHeight && Combustible > 0)  // Asegurarnos de que solo se pueda mover si hay combustible
            {
                Y += CellSize;
                Estela.AgregarNodo(X + CellSize / 2, Y + CellSize / 2);  // Ajusta la posición de la estela al centro de la celda
                ConsumirCombustible(); // Consumir combustible solo al moverse
                RevisarColisionesConItems(gameForm);
                if (collisionManager.CheckCollisions(this)) Explode();
            }
        }


        public void ConsumirCombustible()
        {
            if (Combustible > 0)
            {
                Combustible -= 1;
                if (Combustible <= 0)
                {
                    Explode();
                }
            }
        }



        public void Explode()
        {
            // Implementación de la explosión
        }

        public void RevisarColisionesConItems(GameForm gameform)
        {
            var items = gameform.items; // Ahora se accede directamente a los ítems del GameForm

            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (X == items[i].Posicion.X && Y == items[i].Posicion.Y)
                {
                    RecogerPoder(items[i]);
                    items.RemoveAt(i);
                }
            }
        }

    }
}
