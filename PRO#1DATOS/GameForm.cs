using System;
using System.Drawing;
using System.Windows.Forms;

namespace PRO_1DATOS
{
    public partial class GameForm : Form
    {
        private ListaEnlazadaRectangulos listaEnlazada;
        private Jugador jugador;
        private Image motoImagen;
        private List<Bot> bots;
        private int gridWidth;
        private int gridHeight;
        private System.Windows.Forms.Timer timer;


        public GameForm()
        {
            InitializeComponent();
            InicializarJuego();
            this.BackColor = Color.Black;

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
            for (int i = 0; i < 5; i++)
            {
                Image botImagen = CrearImagenMoto(20, 20, GetBotColor(i));
                Bot bot = new Bot(startX, startY, botImagen, GetBotColor(i), offsetX, offsetY, gridWidth, gridHeight, 20);
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
            foreach (var bot in bots)
            {
                bot.MoverHaciaJugador(jugador);
            }

            Invalidate(); // Forzar el redibujado del formulario para reflejar el movimiento de los bots
        }

        private Color GetBotColor(int index)
        {
            Color[] colors = { Color.Blue, Color.Green, Color.Yellow, Color.Purple, Color.Orange };
            return colors[index % colors.Length];
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            // Aquí puedes agregar la lógica que quieras ejecutar cuando se cargue GameForm.
        }
        private void InicializarJuego()
        {
            // Definir el tamaño de la cuadrícula en celdas
            int filas = 20; // Ajusta el número de filas según tus necesidades
            int columnas = 25; // Ajusta el número de columnas según tus necesidades

            // Calcular el ancho y alto de la cuadrícula en píxeles
            gridWidth = columnas * 20; // 20 píxeles por celda
            gridHeight = filas * 20;

            // Calcular el desplazamiento para centrar la cuadrícula en el formulario
            int offsetX = (this.ClientSize.Width - gridWidth) / 2;
            int offsetY = (this.ClientSize.Height - gridHeight) / 2;

            // Crear la cuadrícula centrada en el formulario
            listaEnlazada = new ListaEnlazadaRectangulos(filas, columnas, offsetX, offsetY);

            // Crear instancia del jugador en el centro inferior de la cuadrícula
            int startX = offsetX + (columnas / 2) * 20;  // Centro horizontal de la cuadrícula
            int startY = offsetY + (filas - 1) * 20;     // Posición en la última fila de la cuadrícula
            jugador = new Jugador(startX, startY, motoImagen, offsetX, offsetY, gridWidth, gridHeight);
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

    }
}

