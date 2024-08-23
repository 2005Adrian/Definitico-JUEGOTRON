using System;
using System.Drawing;
using System.Windows.Forms;

namespace PRO_1DATOS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm();

            // Muestra el formulario del juego
            gameForm.Show();

            // Ocultar el formulario principal (lobby)
            this.Hide();

            // Manejar el cierre del formulario de juego para cerrar la aplicación
            gameForm.FormClosed += (s, args) => this.Show();
        }

        public void transparentButton1_Click(object sender, EventArgs e)
        {
            // Aquí puedes agregar cualquier funcionalidad específica que quieras para este botón.
        }

        public void label1_Click(object sender, EventArgs e)
        {
            // Aquí puedes agregar la funcionalidad para el clic en label1 si es necesario.
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Lógica a ejecutar cuando el texto cambie en textBox1
        }
    }

    public class TransparentButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 255, 255, 255)), this.ClientRectangle); // Esto hace que el fondo sea completamente transparente
        }
    }
}