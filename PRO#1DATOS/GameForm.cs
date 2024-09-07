using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PRO_1DATOS
{
    public partial class GameForm : Form
    {
        public ListaEnlazadaRectangulos listaEnlazada;
        public Jugador jugador;
        public Image motoImagen;
        public List<Bot> bots;
        public int gridWidth;
        public int gridHeight;
        public System.Windows.Forms.Timer timer;
        public CollisionManager collisionManager;
        public Random random;
        public bool gameOver = false; // Esta es la variable que indica si el juego ha terminado
        public List<Poderes> items; // Lista de ítems

        public GameForm()
        {
            InitializeComponent();
            this.BackColor = Color.Black;

            // Inicializamos 'items' antes de cualquier otro uso
            items = new List<Poderes>();

            InicializarJuego(); // Llamamos a InicializarJuego después de la inicialización
        }

        private void MoverBots()
        {
            // Añadir un retardo inicial para evitar la colisión inmediata
            if (timer.Interval == 100) // Intervalo inicial
            {
                timer.Interval = 500; // Aumenta el intervalo después del primer tick
            }
            if (gameOver) return;

            foreach (var bot in bots.ToList())
            {
                bot.MoverAleatorio();

                if (collisionManager.CheckCollisions(bot))
                {
                    bots.Remove(bot);
                }
            }

            if (bots.Count == 0 && !gameOver)
            {
                gameOver = true;
                MessageBox.Show("¡Has ganado!");
                this.Close();
                return;
            }

            if (collisionManager.CheckCollisions(jugador))
            {
                // Asegúrate de que el juego se detiene después de la primera colisión
                if (!gameOver)
                {
                    gameOver = true; // Marca el juego como terminado
                    MessageBox.Show("Has perdido!");
                    this.Close();
                }
                return;
            }

            Invalidate();
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            // Aquí puedes agregar la lógica que quieras ejecutar cuando se cargue GameForm.
        }

        private void InicializarJuego()
        {
            random = new Random();
            int filas = this.ClientSize.Height / 20;
            int columnas = 25;

            // Llamamos a GenerarItemsAleatorios después de que 'items' ha sido inicializada
            GenerarItemsAleatorios();

            gridWidth = columnas * 20;
            gridHeight = filas * 20;
            int offsetX = (this.ClientSize.Width - gridWidth) / 2;
            int offsetY = 0;
            listaEnlazada = new ListaEnlazadaRectangulos(filas, columnas, offsetX, offsetY);

            int playerStartX = offsetX + (gridWidth / 2) - 20;
            int playerStartY = gridHeight - 60;
            bots = new List<Bot>();
            collisionManager = new CollisionManager(new List<Jugador>(), new List<Bot>());

            // Crear el jugador y agregarlo al CollisionManager
            jugador = new Jugador(playerStartX, playerStartY, Recursos.ObtenerImagen("moto"), offsetX, offsetY, gridWidth, gridHeight, 20, Color.Red, collisionManager);
            collisionManager.jugadores.Add(jugador);

            // Crear bots y añadirlos al CollisionManager
            for (int i = 0; i < 4; i++)
            {
                Color botColor = GetBotColor(i);
                int botStartX = offsetX + random.Next(columnas) * 20;
                int botStartY = offsetY + random.Next(filas) * 20;
                Image botImagen = CrearImagenMoto(20, 20, botColor);
                Bot bot = new Bot(botStartX, botStartY, botImagen, botColor, offsetX, offsetY, gridWidth, gridHeight, 20, collisionManager);
                bots.Add(bot);
                collisionManager.bots.Add(bot);
            }

            this.DoubleBuffered = true;
            this.KeyDown += new KeyEventHandler(OnKeyDown);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200;
            timer.Tick += (s, e) => MoverBots();
            timer.Start();
        }

        public void GenerarItemsAleatorios()
        {
            string[] tiposDeitems = { "Celda de Combustible", "Crecimiento de Estela", "Bomba", "Escudo", "Hiper Velocidad" };

            for (int i = 0; i < 5; i++)
            {
                string tipo = tiposDeitems[random.Next(tiposDeitems.Length)];
                int x = random.Next(gridWidth / 20) * 20;
                int y = random.Next(gridHeight / 20) * 20;
                Poderes nuevoItem = new Poderes(tipo, new Point(x, y));

                items.Add(nuevoItem); // Ahora 'items' está inicializado y se pueden agregar elementos
            }
        }

        public void RevisarColisionesConItems()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (jugador.X == items[i].Posicion.X && jugador.Y == items[i].Posicion.Y)
                {
                    jugador.RecogerPoder(items[i]);
                    items.RemoveAt(i);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen neonPen = new Pen(Color.Cyan, 2);
            // Dibujar la cuadrícula con el desplazamiento
            foreach (var nodo in listaEnlazada.Matriz)
            {
                e.Graphics.DrawRectangle(neonPen, nodo.Rectangulo);
            }

            foreach (var item in items)
            {
                item.Dibujar(e.Graphics);
            }
            DibujarInventario(e.Graphics);

            jugador.Estela.DibujarEstela(e.Graphics);

            // Dibujar la moto
            e.Graphics.DrawImage(jugador.MotoImagen, jugador.X, jugador.Y, 20, 20);
            foreach (var bot in bots)
            {
                bot.Estela.DibujarEstela(e.Graphics);

                e.Graphics.DrawImage(bot.MotoImagen, bot.X, bot.Y, 20, 20);
            }
            collisionManager.DrawExplosions(e.Graphics);
        }

        private void DibujarInventario(Graphics g)
        {
            int startX = this.ClientSize.Width - 150;
            int startY = 10;
            int size = 40;

            for (int i = 0; i < jugador.Inventario.Count; i++)
            {
                g.DrawImage(jugador.Inventario[i].Imagen, startX, startY + (i * (size + 10)), size, size);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    jugador.MoverIzquierda();
                    break;
                case Keys.Right:
                    jugador.MoverDerecha();
                    break;
                case Keys.Up:
                    jugador.MoverArriba();
                    break;
                case Keys.Down:
                    jugador.MoverAbajo();
                    break;
                case Keys.D1:  // Usar primer ítem del inventario
                    jugador.UsarPoder(0);
                    break;
                case Keys.D2:  // Usar segundo ítem del inventario
                    jugador.UsarPoder(1);
                    break;
            }

            Invalidate();  // Redibujar la pantalla
        }

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

        private Color GetBotColor(int index)
        {
            Color[] colors = { Color.Blue, Color.Green, Color.Yellow, Color.Purple, Color.Orange };
            return colors[index % colors.Length];
        }
    }
}
