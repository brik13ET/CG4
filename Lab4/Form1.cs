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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				var b = Bitmap.FromFile(openFileDialog1.FileName);
				pictureBox1.Image = b;
				pictureBox2.Image = new Bitmap(b);
				for (int i = 0; i < b.Width; i++)
				{
					for (int j = 0; j < b.Height; j++)
					{
						((Bitmap)pictureBox2.Image).SetPixel(i, j, grayscale((b as Bitmap).GetPixel(i, j)));
					}
				}

			}
		}
		
		static Color grayscale(Color c)
		{
			byte gray = (byte)(c.R * 0.2 + c.G * 0.4 + c.B * 0.1);
			return Color.FromArgb(255, gray, gray, gray);
		}

		static void Linear_cotrast(Bitmap b, PictureBox p)
		{
			byte low_l = 255;
			byte hi_l = 0;
			p.Image = new Bitmap(b);
			for (int i = 0; i < b.Width; i++)
			{
				for (int j = 0; j < b.Height; j++)
				{ // r == g == b == bright
					if (b.GetPixel(i, j).B > hi_l)
					{
						hi_l = b.GetPixel(i, j).B;
					}
					if (b.GetPixel(i, j).B < low_l)
					{
						low_l = b.GetPixel(i, j).B;
					}
				}
			}
			byte r = (byte)(hi_l - low_l);
			double q = 255 * r;
			for (int i = 0; i < b.Width; i++)
			{
				for (int j = 0; j < b.Height; j++)
				{
					byte c = (byte)((b.GetPixel(i, j).B - low_l) * 255 / r);
					(p.Image as Bitmap).SetPixel(i, j, Color.FromArgb(c, c, c));
				}
			}

		}
	}
}
