using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PRO_1DATOS
{
    public partial class GameForm : Form
    {
        public ListaEnlazadaRectangulos listaEnlazada;
        public Jugador jugador;
        public List<Poderes> items;
        public List<Bot> bots;
        public CollisionManager collisionManager;
        public bool gameOver = false;
        private Random random = new Random();
        public int gridWidth;
        public int gridHeight;
        private System.Windows.Forms.Timer botsTimer;


        public GameForm()
        {
            InitializeComponent();
            this.BackColor = Color.White;

            // Inicializar la lista de items
            items = new List<Poderes>();

            InicializarJuego();
        }

       

        private void InicializarJuego()
        {
            int filas = this.ClientSize.Height / 20;
            int columnas = 25;
            int offsetX = (this.ClientSize.Width - columnas * 20) / 2;
            int offsetY = 0;

            gridWidth = columnas * 20;
            gridHeight = filas * 20;

            listaEnlazada = new ListaEnlazadaRectangulos(filas, columnas, offsetX, offsetY);
            GenerarItemsAleatorios();

            int playerStartX = offsetX + (columnas * 20 / 2) - 20;
            int playerStartY = filas * 20 - 60;

            collisionManager = new CollisionManager(new List<Jugador>(), new List<Bot>());

            jugador = new Jugador(playerStartX, playerStartY, Recursos.ObtenerImagen("moto"), offsetX, offsetY, gridWidth, gridHeight, 20, Color.Red, collisionManager);
            collisionManager.jugadores.Add(jugador);  // Añadir jugador al collisionManager

            // Crear bots y añadir al collision manager
            bots = new List<Bot>();
            for (int i = 0; i < 4; i++)
            {
                Color botColor = GetBotColor(i);
                int botStartX = offsetX + random.Next(columnas) * 20;
                int botStartY = offsetY + random.Next(filas) * 20;
                Bot bot = new Bot(botStartX, botStartY, CrearImagenMoto(20, 20, botColor), botColor, offsetX, offsetY, gridWidth, gridHeight, 20, collisionManager);
                bots.Add(bot);
                collisionManager.bots.Add(bot);

               

            }

            this.DoubleBuffered = true;
            this.KeyDown += OnKeyDown;
        }
        private void GameForm_Load(object sender, EventArgs e)
        {
            // Temporizador para mover los bots
            botsTimer = new System.Windows.Forms.Timer();
            botsTimer.Interval = 200; // Mueve los bots cada 200ms
            botsTimer.Tick += (s, e) => MoverBots(); // Asocia el método MoverBots
            botsTimer.Start(); // Iniciar el temporizador
        }
        private void MoverBots()
        {
            for (int i = bots.Count - 1; i >= 0; i--)
            {
                var bot = bots[i];
                bot.MoverAleatorio(this); // Pasamos el GameForm actual como parámetro

                if (collisionManager.CheckCollisions(bot))
                {
                    bots.RemoveAt(i);  // Eliminar el bot de la lista si colisiona
                }
            }

            // Redibuja la pantalla para reflejar los movimientos
            Invalidate();
        }


        private void GenerarItemsAleatorios()
        {
            string[] tiposDeItems = { "combustible", "crecimiento_estela", "bomba", "escudo", "hiper_velocidad" };

            for (int i = 0; i < 5; i++)
            {
                string tipo = tiposDeItems[random.Next(tiposDeItems.Length)];
                RectanguloNodo nodoAleatorio = listaEnlazada.ObtenerNodoAleatorio();
                Point posicionAleatoria = new Point(nodoAleatorio.Rectangulo.X, nodoAleatorio.Rectangulo.Y);

                Poderes nuevoItem = new Poderes(tipo, posicionAleatoria);
                items.Add(nuevoItem);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Dibujar la cuadrícula
            foreach (var nodo in listaEnlazada.Matriz)
            {
                e.Graphics.DrawRectangle(Pens.Cyan, nodo.Rectangulo);
            }

            // Dibujar los ítems
            foreach (var item in items)
            {
                item.Dibujar(e.Graphics);
            }

            // Dibujar barra de gasolina
            DibujarBarraGasolina(e.Graphics);

            // Dibujar inventario
            DibujarInventario(e.Graphics);

            // Dibujar estela del jugador y bots
            jugador.Estela.DibujarEstela(e.Graphics);

            // Ajustar la imagen del jugador para que esté centrada en la celda
            e.Graphics.DrawImage(jugador.MotoImagen, jugador.X + jugador.CellSize / 2 - 10, jugador.Y + jugador.CellSize / 2 - 10, 20, 20);

            // Dibujar los bots
            foreach (var bot in bots)
            {
                bot.Estela.DibujarEstela(e.Graphics);
                e.Graphics.DrawImage(bot.MotoImagen, bot.X + bot.CellSize / 2 - 10, bot.Y + bot.CellSize / 2 - 10, 20, 20);
            }
        }


        private void DibujarBarraGasolina(Graphics g)
        {
            int barraWidth = 200;
            int barraHeight = 20;
            float porcentajeGasolina = (float)jugador.Combustible / 100;

            g.DrawRectangle(Pens.White, 10, 10, barraWidth, barraHeight);
            g.FillRectangle(Brushes.Red, 10, 10, barraWidth * porcentajeGasolina, barraHeight);
        }

        private void DibujarInventario(Graphics g)
        {
            int startX = 10;
            int startY = 40;
            int size = 40;

            g.DrawString("Inventario:", new Font("Arial", 12), Brushes.White, startX, startY);

            for (int i = 0; i < jugador.Inventario.Count; i++)
            {
                Poderes poder = jugador.Inventario[i];
                g.DrawImage(poder.Imagen, startX, startY + (i * (size + 10)), size, size); // Dibuja el poder en el inventario
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (jugador.Combustible > 0)  // Asegurarse de que solo se pueda mover si hay combustible
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        jugador.MoverIzquierda(this);
                        break;
                    case Keys.Right:
                        jugador.MoverDerecha(this);
                        break;
                    case Keys.Up:
                        jugador.MoverArriba(this);
                        break;
                    case Keys.Down:
                        jugador.MoverAbajo(this);
                        break;
                    case Keys.C:
                        jugador.UsarPoder(jugador.Inventario.FindIndex(p => p.Tipo == "crecimiento_estela"));
                        break;
                    case Keys.B:
                        jugador.UsarPoder(jugador.Inventario.FindIndex(p => p.Tipo == "bomba"));
                        break;
                    case Keys.E:
                        jugador.UsarPoder(jugador.Inventario.FindIndex(p => p.Tipo == "escudo"));
                        break;
                    case Keys.H:
                        jugador.UsarPoder(jugador.Inventario.FindIndex(p => p.Tipo == "hiper_velocidad"));
                        break;
                }

                Invalidate();  // Redibujar la pantalla
            }
        }


            // Método auxiliar para crear la imagen de la moto
            private Image CrearImagenMoto(int width, int height, Color color)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                Brush brush = new SolidBrush(color);
                g.FillRectangle(brush, 0, 0, width, height);
            }
            return bmp;
        }

        // Método auxiliar para obtener un color para los bots
        private Color GetBotColor(int index)
        {
            Color[] colors = { Color.Blue, Color.Green, Color.Yellow, Color.Purple, Color.Orange };
            return colors[index % colors.Length];
        }
        public void TerminarJuego(string mensaje)
        {
            // Mostrar un mensaje al jugador
            MessageBox.Show(mensaje, "Juego terminado");

            // Detener el temporizador para que los bots dejen de moverse
            if (botsTimer != null)
            {
                botsTimer.Stop();
            }

            // Desactivar la entrada de teclado del jugador
            this.KeyDown -= OnKeyDown;

            // Opcional: Cerrar el formulario de juego después de mostrar el mensaje
            this.Close();

            // Opcional: Volver al lobby o reiniciar el juego
            // Form1 lobby = new Form1();
            // lobby.Show(); 
        }


    }
}
