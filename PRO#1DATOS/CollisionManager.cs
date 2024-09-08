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
            // Verificar colisión del jugador con las estelas de otros jugadores y bots
            foreach (var otroJugador in jugadores)
            {
                if (otroJugador != jugador)
                {
                    // Verificar si colisiona con la estela de otro jugador
                    if (VerificarColisionConEstela(jugador, otroJugador.Estela))
                    {
                        Explode(jugador);  // Eliminar el jugador si colisiona con la estela de otro jugador
                        return true;
                    }
                }
            }

            foreach (var bot in bots)
            {
                // Verificar si colisiona con la estela de un bot
                if (VerificarColisionConEstela(jugador, bot.Estela))
                {
                    Explode(jugador);  // Eliminar el jugador si colisiona con la estela de un bot
                    return true;
                }
            }

            return false;
        }

        public bool CheckCollisions(Bot bot)
        {
            // Verificar colisión del bot con las estelas de jugadores y otros bots
            foreach (var otroBot in bots)
            {
                if (otroBot != bot)
                {
                    // Verificar si el bot colisiona con la estela de otro bot
                    if (VerificarColisionConEstela(bot, otroBot.Estela))
                    {
                        Explode(bot);  // Eliminar el bot si colisiona con la estela de otro bot
                        return true;
                    }
                }
            }

            foreach (var jugador in jugadores)
            {
                // Verificar si el bot colisiona con la estela de un jugador
                if (VerificarColisionConEstela(bot, jugador.Estela))
                {
                    Explode(bot);  // Eliminar el bot si colisiona con la estela del jugador
                    return true;
                }
            }

            return false;
        }


        public bool VerificarColisionConEstela(Jugador jugador, Estela estela)
        {
            var nodoActual = estela.Cabeza;

            // Recorrer toda la estela y comprobar si hay colisión
            while (nodoActual != null)
            {
                // Verificar si la posición del jugador coincide con la posición de cualquier parte de la estela
                if (jugador.X == nodoActual.X && jugador.Y == nodoActual.Y)
                {
                    return true;  // Colisión detectada con la estela
                }
                nodoActual = nodoActual.Siguiente;
            }

            return false;  // No hay colisión
        }
        public void Explode(Jugador jugador)
        {
            // Agregar explosión en la posición del jugador
            AddExplosion(new Explosion(jugador.X, jugador.Y));
            // Eliminar el jugador
            RemoveJugador(jugador);
        }

        // Método para explotar un bot
        public void Explode(Bot bot)
        {
            // Agregar explosión en la posición del bot
            AddExplosion(new Explosion(bot.X, bot.Y));
            // Eliminar el bot
            RemoveBot(bot);
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