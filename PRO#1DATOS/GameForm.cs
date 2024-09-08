using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

using System.Media;

namespace PRO_1DATOS
{
    public partial class GameForm : Form
    {
        // Declaring the necessary variables.
        public ListaEnlazadaRectangulos listaEnlazada;
        public Jugador jugador;
        public Image motoImagen;
        public List<Poderes> items;
        public List<Bot> bots;
        public CollisionManager collisionManager;
        public bool gameOver = false;
        private Random random = new Random();
        public int gridWidth;
        public int gridHeight;
        public System.Windows.Forms.Timer timer;
        public int CellSize { get; set; }  // Agregar CellSize si no está definido

        public SoundPlayer soundPlayer;
        public GameForm()
        {
            InitializeComponent();
            this.BackColor = Color.Magenta;
            CellSize = 20;  // Puedes ajustar este valor según el tamaño de las celdas

            // Initialize 'items' before use
            items = new List<Poderes>();
            if (Form.ActiveForm is GameForm gameForm)
            {
                var items = gameForm.items;
                // Continúa procesando los items...
            }
            else
            {
                // Maneja el caso donde el form no es el activo o no es de tipo GameForm
            }

            reproducirSonido();
            InicializarJuego();
        }
        public void reproducirSonido()
        {
            SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.fondogameform);
            soundPlayer.PlayLooping();
        }

        private void MoverBots()
        {
            if (gameOver) return;

            foreach (var bot in bots.ToList())
            {
                bot.MoverAleatorio(); 
                bot.RevisarColisionesConItems(items);

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
                if (!gameOver)
                {
                    gameOver = true;  
                    MessageBox.Show("Has perdido!");  
                    this.Close();  
                }
                return;
            }

            Invalidate();  
        }


        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        
        private void InicializarJuego()
        {
            random = new Random();
            int filas = this.ClientSize.Height / 20;
            int columnas = 25;
            int offsetX = (this.ClientSize.Width - columnas * CellSize) / 2;  // Centrar la cuadrícula
            int offsetY = 0;
            gridWidth = columnas * CellSize;
            gridHeight = filas * CellSize;


            listaEnlazada = new ListaEnlazadaRectangulos(filas, columnas, offsetX, offsetY);
            GenerarItemsAleatorios();

            int playerStartX = (offsetX + (columnas * CellSize / 2)) / CellSize * CellSize;  // Alinear al tamaño de la celda
            int playerStartY = (filas * CellSize - 60) / CellSize * CellSize;  // Alinear al tamaño de la celda

            collisionManager = new CollisionManager(new List<Jugador>(), new List<Bot>());

            jugador = new Jugador(playerStartX, playerStartY, Recursos.ObtenerImagen("moto"), offsetX, offsetY, gridWidth, gridHeight, 20, Color.Red, collisionManager);
            collisionManager.jugadores.Add(jugador);

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

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200;
            timer.Tick += (s, e) => MoverBots();
            timer.Start();

            this.KeyDown += OnKeyDown;
        }

        private void GenerarItemsAleatorios()
        {
            string[] tiposDeItems = { "combustible", "crecimiento_estela", "bomba", "escudo", "hiper_velocidad" };

            for (int i = 0; i < 8; i++)
            {
                string tipo = tiposDeItems[random.Next(tiposDeItems.Length)];
                RectanguloNodo nodoAleatorio = listaEnlazada.ObtenerNodoAleatorio();
                Point posicionAleatoria = new Point(nodoAleatorio.Rectangulo.X, nodoAleatorio.Rectangulo.Y);

                int valorItem = tipo == "combustible" ? 100 : random.Next(10, 50);  // Establecer el valor del combustible a 100

                Poderes gaso = new Poderes(tipo, posicionAleatoria,valorItem);
                

                items.Add(gaso);
               // items.Add(hiper);
               // items.Add(crece);

                // Imprimir en la consola la generación de un nuevo ítem
                Console.WriteLine($"Generado poder: {tipo} en la posición {posicionAleatoria.X}, {posicionAleatoria.Y}");
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen neonPen = new Pen(Color.Aqua, 2);
            // Drawing the grid
            foreach (var nodo in listaEnlazada.Matriz)
            {
                e.Graphics.DrawRectangle(Pens.Aqua, nodo.Rectangulo);
            }

            // Drawing items
            foreach (var item in items)
            {
                item.Dibujar(e.Graphics);
            }

            // Drawing fuel bar
            DibujarBarraGasolina(e.Graphics);

            // Drawing inventory
            DibujarInventario(e.Graphics);

            // Drawing player and bots
            jugador.Estela.DibujarEstela(e.Graphics);

            // Centrar el jugador en la celda, ajustando su imagen al tamaño de la celda.
            int jugadorX = (jugador.X / jugador.CellSize) * jugador.CellSize + (jugador.CellSize - jugador.MotoImagen.Width) / 2;
            int jugadorY = (jugador.Y / jugador.CellSize) * jugador.CellSize + (jugador.CellSize - jugador.MotoImagen.Height) / 2;

            // Dibujar al jugador centrado
            e.Graphics.DrawImage(jugador.MotoImagen, jugadorX, jugadorY, jugador.CellSize, jugador.CellSize);

            // Dibujar los bots
            foreach (var bot in bots)
            {
                bot.Estela.DibujarEstela(e.Graphics);
                e.Graphics.DrawImage(bot.MotoImagen, bot.X, bot.Y, 20, 20);
            }
        }


        private bool juegoTerminado = false;

        private void DibujarBarraGasolina(Graphics g)
        {
            int barraWidth = 200;
            int barraHeight = 20;
            float porcentajeGasolina = (float)jugador.Combustible / 100;

            // Dibujar la barra de gasolina
            g.DrawRectangle(Pens.Gray, 10, 10, barraWidth, barraHeight);
            g.FillRectangle(Brushes.Red, 10, 10, barraWidth * porcentajeGasolina, barraHeight);

            // Obtener la imagen de combustible de los recursos
            Image gasolinaImage = Recursos.ObtenerImagen("combustible");

            // Dibujar la imagen de gasolina junto a la barra
            g.DrawImage(gasolinaImage, 10, 10, 30, 30); // Ajusta la posición y tamaño según sea necesario

            // Comprobar si el combustible es 0 y mostrar el mensaje solo una vez
            if (jugador.Combustible == 0 && !juegoTerminado)
            {
                juegoTerminado = true;
                this.Close();// Evitar que se vuelva a mostrar el mensaje
                                       // Lógica adicional para finalizar el juego o reiniciar al jugador
            }
        }



        // Method to draw the player's inventory.
        private void DibujarInventario(Graphics g)
        {
            int startX = 10;
            int startY = 40;
            int size = 40;

            g.DrawString("Inventario:", new Font("Arial", 12), Brushes.White, startX, startY);

            for (int i = 0; i < jugador.Inventario.Count; i++)
            {
                Poderes poder = jugador.Inventario[i];
                g.DrawImage(poder.Imagen, startX, startY + (i * (size + 10)), size, size);
            }
        }

        // Key event handler for player movement and using powers.
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    jugador.MoverIzquierda();  // Pass 'this' as the GameForm reference
                    break;
                case Keys.Right:
                    jugador.MoverDerecha();    // Pass 'this' as the GameForm reference
                    break;
                case Keys.Up:
                    jugador.MoverArriba();     // Pass 'this' as the GameForm reference
                    break;
                case Keys.Down:
                    jugador.MoverAbajo();      // Pass 'this' as the GameForm reference
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
            jugador.RevisarColisionesConItems(items);  // Pasar la lista de ítems para verificar colisiones


            Invalidate();  // Redraw the screen
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
