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
        public int CellSize { get; set; } 
        public SoundPlayer soundPlayer;
        public GameForm()
        {
            InitializeComponent();
            this.BackColor = Color.Magenta;
            CellSize = 20;  


            items = new List<Poderes>();
            if (Form.ActiveForm is GameForm gameForm)
            {
                var items = gameForm.items;

            }
            else
            {

            }

            reproducirSonido();
            InicializarJuego();
        }
        public void reproducirSonido()
        {
            SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.fondogameform);
            soundPlayer.PlayLooping();
        }
        

        public void MoverBots()
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


        public void GameForm_Load(object sender, EventArgs e)
        {

        }

        
        public void InicializarJuego()
        {
            random = new Random();
            int filas = this.ClientSize.Height / 20;
            int columnas = 25;
            int offsetX = (this.ClientSize.Width - columnas * CellSize) / 2; 
            int offsetY = 0;
            gridWidth = columnas * CellSize;
            gridHeight = filas * CellSize;


            listaEnlazada = new ListaEnlazadaRectangulos(filas, columnas, offsetX, offsetY);
            GenerarItemsAleatorios();

            int playerStartX = (offsetX + (columnas * CellSize / 2)) / CellSize * CellSize; 
            int playerStartY = (filas * CellSize - 60) / CellSize * CellSize; 

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

        public void GenerarItemsAleatorios()
        {
            string[] tiposDeItems = { "combustible", "crecimiento_estela", "bomba", "escudo", "hiper_velocidad" };

            for (int i = 0; i < 8; i++)
            {
                string tipo = tiposDeItems[random.Next(tiposDeItems.Length)];
                RectanguloNodo nodoAleatorio = listaEnlazada.ObtenerNodoAleatorio();
                Point posicionAleatoria = new Point(nodoAleatorio.Rectangulo.X, nodoAleatorio.Rectangulo.Y);

                int valorItem = tipo == "combustible" ? 100 : random.Next(10, 50);  

                Poderes gaso = new Poderes(tipo, posicionAleatoria,valorItem);
                

                items.Add(gaso);
               

                Console.WriteLine($"Generado poder: {tipo} en la posición {posicionAleatoria.X}, {posicionAleatoria.Y}");
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen neonPen = new Pen(Color.Aqua, 2);
            foreach (var nodo in listaEnlazada.Matriz)
            {
                e.Graphics.DrawRectangle(Pens.Aqua, nodo.Rectangulo);
            }

            foreach (var item in items)
            {
                item.Dibujar(e.Graphics);
            }

            DibujarBarraGasolina(e.Graphics);

            DibujarInventario(e.Graphics);

            jugador.Estela.DibujarEstela(e.Graphics);

            int jugadorX = (jugador.X / jugador.CellSize) * jugador.CellSize + (jugador.CellSize - jugador.MotoImagen.Width) / 2;
            int jugadorY = (jugador.Y / jugador.CellSize) * jugador.CellSize + (jugador.CellSize - jugador.MotoImagen.Height) / 2;

            e.Graphics.DrawImage(jugador.MotoImagen, jugadorX, jugadorY, jugador.CellSize, jugador.CellSize);

            foreach (var bot in bots)
            {
                bot.Estela.DibujarEstela(e.Graphics);
                e.Graphics.DrawImage(bot.MotoImagen, bot.X, bot.Y, 20, 20);
            }
        }


        public bool juegoTerminado = false;

        public void DibujarBarraGasolina(Graphics g)
        {
            int barraWidth = 200;
            int barraHeight = 20;
            float porcentajeGasolina = (float)jugador.Combustible / 100;

            g.DrawRectangle(Pens.Gray, 10, 10, barraWidth, barraHeight);
            g.FillRectangle(Brushes.Red, 10, 10, barraWidth * porcentajeGasolina, barraHeight);

            Image gasolinaImage = Recursos.ObtenerImagen("combustible");

            g.DrawImage(gasolinaImage, 10, 10, 30, 30); 

            if (jugador.Combustible == 0 && !juegoTerminado)
            {
                juegoTerminado = true;
                this.Close();
            }
        }



        public void DibujarInventario(Graphics g)
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

        public void OnKeyDown(object sender, KeyEventArgs e)
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
            jugador.RevisarColisionesConItems(items);  

            Invalidate();  
        }

        public Image CrearImagenMoto(int width, int height, Color color)
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

        public Color GetBotColor(int index)
        {
            Color[] colors = { Color.Blue, Color.Green, Color.Yellow, Color.Purple, Color.Orange };
            return colors[index % colors.Length];
        }
    }
}
