using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace picture_zoom
{
    public partial class PictureViewForm : Form
    {
        public PictureViewForm()
        {
            InitializeComponent();
        }
        PictureBox ZoomPictureBox;

        Graphics GraphicCapture;
        Bitmap TemporalImage;
        bool isPictureBoxIn;
        int Zoom = 2; // 1px, 2px, 3px ....
        private void Form1_Load(object sender, EventArgs e)
        {
            ZoomPictureBox = new PictureBox
            {
                Size = new System.Drawing.Size(100, 100),
            };
            timer1.Start();
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            ZoomPictureBox.Location = new System.Drawing.Point
            {
                X = e.X - (ZoomPictureBox.Size.Width / 2),//mouse center position of X
                Y = e.Y - (ZoomPictureBox.Size.Height / 2),//mouse center position of X
            };
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            isPictureBoxIn = false;
            Cursor = Cursors.Default;
            //pictureBox1.Controls.Remove(ZoomPictureBox);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            isPictureBoxIn = true;
            Cursor = Cursors.Cross;
            //pictureBox1.Controls.Add(ZoomPictureBox);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //
            int mouseX = MousePosition.X;
            int mouseY = MousePosition.Y;
            //
            int imageW = pictureBox2.Width;
            int imageH = pictureBox2.Height;
            if (isPictureBoxIn)
            {
                TemporalImage = new Bitmap(imageW / Zoom, imageH / Zoom, System.Drawing.Imaging.PixelFormat.Format64bppPArgb);
                GraphicCapture = this.CreateGraphics();

                GraphicCapture = Graphics.FromImage(TemporalImage);
                GraphicCapture.CopyFromScreen(mouseX - imageW / (Zoom * 2), mouseY - imageH / (Zoom * 2), 0, 0, pictureBox1.Size);
                //
                Bitmap newImage = new Bitmap(imageW, imageH);
                GraphicCapture = Graphics.FromImage(newImage);
                GraphicCapture.SmoothingMode = SmoothingMode.HighQuality;
                GraphicCapture.DrawImage(TemporalImage, new Rectangle(0, 0, imageW, imageH));
                pictureBox2.Image = newImage;

                //
                Rectangle rect = new Rectangle(0, 0, imageW, imageH);
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(rect);
                pictureBox2.Region = new Region(path);
                //
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & Keys.Add) == Keys.Add)
            {
                Zoom++;
            }
            if ((e.KeyCode & Keys.Separator) == Keys.Separator)
            {
                if (Zoom > 2)
                {
                    Zoom--;
                }
            }
            if ((e.KeyCode & Keys.Escape) == Keys.Escape)
            {
                this.Close();
            }
        }
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta >= 0)
                Zoom++;
            else
            if (Zoom > 2)
            {
                Zoom--;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Supported Image File|*.jpg;*.jpeg;*.bmp;*.png;*.dib;*.gif";
            openFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Multiselect = false;
            openFileDialog.ReadOnlyChecked = false;
            openFileDialog.ShowHelp = true;
            openFileDialog.ShowReadOnly = false;
            openFileDialog.SupportMultiDottedExtensions = true;
            openFileDialog.Title = "Select an image...";
            openFileDialog.ValidateNames = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }
    }
}
