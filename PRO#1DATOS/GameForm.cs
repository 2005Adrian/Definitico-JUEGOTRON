using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        public GameForm()
        {
            InitializeComponent();
            this.BackColor = Color.White;

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


            InicializarJuego();
        }

        // MoverBots method: Handles moving the bots.
        private void MoverBots()
        {
            // Ensure the game is not over before processing bot movements
            if (gameOver) return;

            // Move each bot and check for collisions
            foreach (var bot in bots.ToList())
            {
                bot.MoverAleatorio();  // Move the bot randomly within the game bounds

                // Check if the bot collides with anything
                if (collisionManager.CheckCollisions(bot))
                {
                    bots.Remove(bot);  // Remove the bot if a collision is detected
                }
            }

            // If all bots are removed, the player wins
            if (bots.Count == 0 && !gameOver)
            {
                gameOver = true;
                MessageBox.Show("¡Has ganado!");
                this.Close();  // Close the game form after winning
                return;
            }

            // Check if the player collided with any bot or object
            if (collisionManager.CheckCollisions(jugador))
            {
                // Trigger the game-over state if the player loses
                if (!gameOver)
                {
                    gameOver = true;  // Mark the game as over
                    MessageBox.Show("Has perdido!");  // Show the game-over message
                    this.Close();  // Close the game form after losing
                }
                return;
            }

            Invalidate();  // Redraw the screen after each tick to update the game state
        }


        private void GameForm_Load(object sender, EventArgs e)
        {
            // Logic to initialize when GameForm loads, if needed.
        }

        // Method to initialize the game.
        private void InicializarJuego()
        {
            random = new Random();
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

            // Initialize the player and add to collision manager.
            jugador = new Jugador(playerStartX, playerStartY, Recursos.ObtenerImagen("moto"), offsetX, offsetY, gridWidth, gridHeight, 20, Color.Red, collisionManager);
            collisionManager.jugadores.Add(jugador);

            // Initialize bots and add to collision manager.
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

        // Method to generate random items in the game.
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

                // Imprimir en la consola la generación de un nuevo ítem
                Console.WriteLine($"Generado poder: {tipo} en la posición {posicionAleatoria.X}, {posicionAleatoria.Y}");
            }
        }


        // Paint method to handle all drawing on the form.
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen neonPen = new Pen(Color.Cyan, 2);
            // Drawing the grid
            foreach (var nodo in listaEnlazada.Matriz)
            {
                e.Graphics.DrawRectangle(Pens.Cyan, nodo.Rectangulo);
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
            e.Graphics.DrawImage(jugador.MotoImagen, jugador.X, jugador.Y, 20, 20);

            foreach (var bot in bots)
            {
                bot.Estela.DibujarEstela(e.Graphics);
                e.Graphics.DrawImage(bot.MotoImagen, bot.X, bot.Y, 20, 20);
            }
        }

        // Method to draw the fuel bar.
        private void DibujarBarraGasolina(Graphics g)
        {
            int barraWidth = 200;
            int barraHeight = 20;
            float porcentajeGasolina = (float)jugador.Combustible / 100;

            g.DrawRectangle(Pens.White, 10, 10, barraWidth, barraHeight);
            g.FillRectangle(Brushes.Red, 10, 10, barraWidth * porcentajeGasolina, barraHeight);
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

        // Helper method to create the player's moto image.
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

        // Helper method to get bot colors.
        private Color GetBotColor(int index)
        {
            Color[] colors = { Color.Blue, Color.Green, Color.Yellow, Color.Purple, Color.Orange };
            return colors[index % colors.Length];
        }
    }
}
