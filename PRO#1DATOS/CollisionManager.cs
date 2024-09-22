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
            this.explosiones = new List<Explosion>();
        }
        public bool CheckCollisions(Jugador jugador)
        {
            foreach (var otroJugador in jugadores)
            {
                if (otroJugador != jugador)
                {
                    if (VerificarColisionConEstela(jugador, otroJugador.Estela))
                    {
                        Explode(jugador);  
                        return true;
                    }
                }
            }

            foreach (var bot in bots)
            {
                if (VerificarColisionConEstela(jugador, bot.Estela))
                {
                    Explode(jugador);  
                    return true;
                }
            }

            return false;
        }

        public bool CheckCollisions(Bot bot)
        {
            foreach (var otroBot in bots)
            {
                if (otroBot != bot)
                {
                    if (VerificarColisionConEstela(bot, otroBot.Estela))
                    {
                        Explode(bot); 
                        return true;
                    }
                }
            }

            foreach (var jugador in jugadores)
            {
                if (VerificarColisionConEstela(bot, jugador.Estela))
                {
                    Explode(bot);  
                    return true;
                }
            }

            return false;
        }


        public bool VerificarColisionConEstela(Jugador jugador, Estela estela)
        {
            var nodoActual = estela.Cabeza;

            while (nodoActual != null)
            {
                if (jugador.X == nodoActual.X && jugador.Y == nodoActual.Y)
                {
                    return true;  
                }
                nodoActual = nodoActual.Siguiente;
            }

            return false;  
        }
        public void Explode(Jugador jugador)
        {
            AddExplosion(new Explosion(jugador.X, jugador.Y));
            RemoveJugador(jugador);
        }

        public void Explode(Bot bot)
        {
            AddExplosion(new Explosion(bot.X, bot.Y));
            RemoveBot(bot);
        }


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