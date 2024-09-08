using System.Collections.Generic;
using System.Drawing;
namespace PRO_1DATOS
{
    public class CollisionManager
    {
        public List<Jugador> jugadores;
        public List<Bot> bots;
        public List<Explosion> explosiones;
        public CollisionManager(List<Jugador> jugadores, List<Bot> bots)
        {
            this.jugadores = jugadores ?? new List<Jugador>(); // Inicializar si es nulo
            this.bots = bots ?? new List<Bot>(); // Inicializar si es nulo
            this.explosiones = new List<Explosion>();
        }
        public bool CheckCollisions(Jugador jugador)
        {
            foreach (var bot in bots)
            {
                if (VerificarColisionConEstela(jugador, bot.Estela))
                {
                    explosiones.Add(new Explosion(jugador.X, jugador.Y));
                    RemoveJugador(jugador);  // Remove the player if a collision happens
                    return true;
                }
            }

            foreach (var otroJugador in jugadores)
            {
                if (otroJugador != jugador && VerificarColisionConEstela(jugador, otroJugador.Estela))
                {
                    explosiones.Add(new Explosion(jugador.X, jugador.Y));
                    RemoveJugador(jugador);  // Remove the player if a collision happens
                    return true;
                }
            }

            return false;
        }

        private bool VerificarColisionConEstela(Jugador jugador, Estela estela)
        {
            var nodoActual = estela.Cabeza;
            int posicionesIgnoradas = 0;
            while (nodoActual != null)
            {
                // Ignorar las primeras tres posiciones de la estela
                if (posicionesIgnoradas < 3)
                {
                    posicionesIgnoradas++;
                }
                else
                {
                    // Verificar si la posición del jugador coincide con alguna posición de la estela (excepto las primeras tres)
                    if (jugador.X == nodoActual.X && jugador.Y == nodoActual.Y)
                    {
                        return true;
                    }
                }
                nodoActual = nodoActual.Siguiente;
            }
            return false;
        }


        // Método corregido para agregar explosiones
        public void AddExplosion(Explosion explosion)
        {
            explosiones.Add(explosion);
        }
        public void RemoveBot(Bot bot)
        {
            bots.Remove(bot);
        }
        public void RemoveJugador(Jugador jugador)
        {
            jugadores.Remove(jugador);
        }
        public void DrawExplosions(Graphics g)
        {
            foreach (var explosion in explosiones)
            {
                explosion.Draw(g);
            }
        }
    }
}