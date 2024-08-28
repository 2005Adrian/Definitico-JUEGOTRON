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


        public GameForm()
        {
            InitializeComponent();
            InicializarJuego();
            this.BackColor = Color.Black;

            random = new Random();
            // Crear la imagen de la moto programáticamente
            Image motoImagen = CrearImagenMoto(40, 40, Color.Red); // Tamaño de 40x40 píxeles

            // Definir el tamaño de la cuadrícula en celdas
            int filas = this.ClientSize.Height / 20;
            int columnas = 25; // Ajustar a un tamaño más pequeño, menos ancho

            // Calcular el ancho y alto de la cuadrícula en píxeles
            int gridWidth = columnas * 20; // 20 píxeles por celda
            int gridHeight = filas * 20;

            // Crear la cuadrícula centrada en el formulario
            int offsetX = (this.ClientSize.Width - gridWidth) / 2; // Centrar horizontalmente
            int offsetY = 0; // No cambiar la posición vertical

            // Calcular la posición para que la moto aparezca en el centro inferior del mapa
            int startX = offsetX + (gridWidth / 2) - 20;  // Centro horizontal, ajustado al tamaño de la moto
            int startY = gridHeight - 60; // Cerca del borde inferior de la cuadrícula

            // Crear instancia del jugador en el centro inferior de la cuadrícula
            jugador = new Jugador(startX, startY, motoImagen, offsetX, offsetY, gridWidth, gridHeight, 20);


            bots = new List<Bot>();
            collisionManager = new CollisionManager(new List<Jugador> { jugador }, bots);

            for (int i = 0; i < 5; i++)
            {
                Color botColor = GetBotColor(i);
                int botStartX = offsetX + random.Next(columnas) * 20;
                int botStartY = offsetY + random.Next(filas) * 20;
                Image botImagen = CrearImagenMoto(20, 20, botColor);
                Bot bot = new Bot(botStartX, botStartY, botImagen, botColor, offsetX, offsetY, gridWidth, gridHeight, 20, collisionManager);
                bots.Add(bot);
            }




            this.DoubleBuffered = true; // Para reducir el parpadeo durante el movimiento

            // Manejar eventos de teclado
            this.KeyDown += new KeyEventHandler(OnKeyDown);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200; // 1 segundo
            timer.Tick += (s, e) => MoverBots();
            timer.Start();

           
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

            // Definir el tamaño de la cuadrícula en celdas
            int filas = this.ClientSize.Height / 20;
            int columnas = 25;

            // Calcular el ancho y alto de la cuadrícula en píxeles
            int gridWidth = columnas * 20;
            int gridHeight = filas * 20;

            int offsetX = (this.ClientSize.Width - gridWidth) / 2;
            int offsetY = 0;
            listaEnlazada = new ListaEnlazadaRectangulos(filas, columnas, offsetX, offsetY);


            // Crear instancia del jugador en la posición inicial segura
            // Crear instancia del jugador en una posición segura
            int playerStartX = offsetX + (gridWidth / 2) - 20;
            int playerStartY = gridHeight - 60;
            jugador = new Jugador(playerStartX, playerStartY, CrearImagenMoto(40, 40, Color.Red), offsetX, offsetY, gridWidth, gridHeight, 20);

            // Crear bots en posiciones aleatorias seguras
            bots = new List<Bot>();
            collisionManager = new CollisionManager(new List<Jugador> { jugador }, bots);

            for (int i = 0; i < 5; i++)
            {
                Color botColor = GetBotColor(i);
                int botStartX, botStartY;
                do
                {
                    botStartX = offsetX + random.Next(columnas) * 20;
                    botStartY = offsetY + random.Next(filas) * 20;
                } while ((botStartX == playerStartX && botStartY == playerStartY) ||
                         bots.Any(b => b.X == botStartX && b.Y == botStartY));

                Image botImagen = CrearImagenMoto(20, 20, botColor);
                Bot bot = new Bot(botStartX, botStartY, botImagen, botColor, offsetX, offsetY, gridWidth, gridHeight, 20, collisionManager);
                bots.Add(bot);
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen neonPen = new Pen(Color.Cyan, 2);
            jugador.Estela.DibujarEstela(e.Graphics);

            
            // Dibujar la cuadrícula con el desplazamiento
            foreach (var nodo in listaEnlazada.Matriz)
            {
                e.Graphics.DrawRectangle(neonPen, nodo.Rectangulo);
            }
            // Dibujar la moto
            e.Graphics.DrawImage(jugador.MotoImagen, jugador.X, jugador.Y, 20, 20);
            foreach (var bot in bots)
            {
                bot.Estela.DibujarEstela(e.Graphics);

                e.Graphics.DrawImage(bot.MotoImagen, bot.X, bot.Y, 20, 20);
            }
            collisionManager.DrawExplosions(e.Graphics);

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Mover el jugador según la tecla presionada
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
            }

            // Forzar el redibujado del formulario para que la moto se mueva
            Invalidate();
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

