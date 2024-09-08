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
            this.jugadores = jugadores ?? new List<Jugador>();
            this.bots = bots ?? new List<Bot>();
            this.explosiones = new List<Explosion>(); // Asegúrate de inicializar esta lista
        }

        public bool CheckCollisions(Jugador jugador)
        {
            // Verifica las colisiones del jugador con los bots o con las estelas
            foreach (var otroJugador in jugadores)
            {
                if (jugador != otroJugador && VerificarColisionConEstela(jugador, otroJugador.Estela))
                {
                    RemoveJugador(jugador);
                    return true;
                }
            }

            foreach (var bot in bots)
            {
                if (VerificarColisionConEstela(jugador, bot.Estela))
                {
                    RemoveJugador(jugador);
                    return true;
                }
            }

            // Verificar colisión con su propia estela
            if (VerificarColisionConEstela(jugador, jugador.Estela))
            {
                RemoveJugador(jugador);
                return true;
            }

            return false;
        }

        public bool CheckCollisions(Bot bot)
        {
            // Verificar colisión con estelas de todos los bots y jugadores
            foreach (var otroBot in bots)
            {
                if (otroBot != bot && VerificarColisionConEstela(bot, otroBot.Estela))
                {
                    RemoveBot(bot);
                    return true;
                }
            }

            foreach (var jugador in jugadores)
            {
                if (VerificarColisionConEstela(bot, jugador.Estela))
                {
                    RemoveBot(bot);
                    return true;
                }
            }

            // Verificar colisión con su propia estela
            if (VerificarColisionConEstela(bot, bot.Estela))
            {
                RemoveBot(bot);
                return true;
            }

            return false;
        }

        public void RemoveJugador(Jugador jugador)
        {
            jugadores.Remove(jugador);
        }

        public void RemoveBot(Bot bot)
        {
            bots.Remove(bot);
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

        public void AddExplosion(Explosion explosion)
        {
            explosiones.Add(explosion);
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
