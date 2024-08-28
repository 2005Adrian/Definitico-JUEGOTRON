using System;
using System.Drawing;
using System.Windows.Forms;

namespace PRO_1DATOS
{
    public class Explosion
    {
        public int x;
        public int y;
        public int frame;
        public int totalFrames;
        public System.Windows.Forms.Timer explosionTimer;

        public bool IsComplete { get;  set; }

        public Explosion(int x, int y, int totalFrames = 10, int interval = 50)
        {
            this.x = x;
            this.y = y;
            this.frame = 0;
            this.totalFrames = totalFrames;
            this.IsComplete = false;

            explosionTimer = new System.Windows.Forms.Timer();
            explosionTimer.Interval = interval;
            explosionTimer.Tick += (s, e) => UpdateFrame();
            explosionTimer.Start();
        }

        private void UpdateFrame()
        {
            frame++;
            if (frame >= totalFrames)
            {
                IsComplete = true;
                explosionTimer.Stop();
            }
        }

        public void Draw(Graphics g)
        {
            if (!IsComplete)
            {
                // Simple explosion animation using a circle
                int size = 20 + (frame * 5); // Increase size each frame
                g.FillEllipse(Brushes.OrangeRed, x - size / 2, y - size / 2, size, size);
            }
        }
    }
}
