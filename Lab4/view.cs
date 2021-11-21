using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
	public partial class view : Form
	{
		Image im;
		int scale = 0;

		public view(Image i)
		{
			if (i == null)
				throw new ArgumentNullException("i", "Изображение не должно быть пустым");
			InitializeComponent();
			im = i;
			pictureBox1.Image = i;
			pictureBox1.MouseWheel += pictureBox1_MouseWheel;
			MessageBox.Show(((int)Math.Min(im.Width / 8, im.Height / 8)).ToString());
		}

		private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
		{
			(sender as PictureBox).Focus();
			scale += (int)(e.Delta / Math.Abs(e.Delta));
			if (scale <= 0)
				scale = 1;
			if (scale >= im.Width / 8 || scale >= im.Height / 8)
				scale = (int)Math.Min(im.Width / 8, im.Height / 8);
			this.Text = scale.ToString();
			var b = new Bitmap(im);
			b.SetResolution(1f / scale, 1f / scale);
			(sender as PictureBox).Image = b;
		}
	}
}
