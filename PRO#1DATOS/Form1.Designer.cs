
namespace PRO_1DATOS

{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <paraname="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            button1 = new Button();
            button3 = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = Properties.Resources._4df20992_e2ce_4dd5_988d_0bf2c205ed13;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1030, 883);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = Properties.Resources._225fc3bd_78a4_4b35_974f_6af2f7f9d9da;
            button1.Font = new Font("Stencil", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.LightSteelBlue;
            button1.Location = new Point(394, 237);
            button1.Name = "button1";
            button1.Size = new Size(212, 92);
            button1.TabIndex = 1;
            button1.Text = "PLAY";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImage = Properties.Resources._4df20992_e2ce_4dd5_988d_0bf2c205ed13;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Stencil", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.LightSkyBlue;
            button3.Location = new Point(348, 401);
            button3.Name = "button3";
            button3.Size = new Size(298, 92);
            button3.TabIndex = 3;
            button3.Text = "INSTRUCCIONES";
            button3.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Font = new Font("Showcard Gothic", 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(0, 0, 192);
            label1.Image = Properties.Resources._225fc3bd_78a4_4b35_974f_6af2f7f9d9da;
            label1.Location = new Point(211, 40);
            label1.Name = "label1";
            label1.Size = new Size(589, 121);
            label1.TabIndex = 4;
            label1.Text = "TRON GAME";
            label1.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1030, 881);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "TRONGAME ADRIAN ";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Label label1;
    }
}
