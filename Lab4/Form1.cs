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
			t = new ToolTip();
			// Set up the delays for the ToolTip.
			t.AutoPopDelay = 3000;
			t.InitialDelay = 0;
			t.ReshowDelay = 1;
			// Force the ToolTip text to be displayed whether or not the form is active.
			t.ShowAlways = true;

			t.SetToolTip(pictureBox1, "Original");
			t.SetToolTip(pictureBox2, "Grayscale");
			t.SetToolTip(pictureBox3, "LinearContrast");
			t.SetToolTip(pictureBox4, "LinearSmoothMask");
			t.SetToolTip(pictureBox5, "LMS diff Gray");
			t.SetToolTip(pictureBox6, "Vector");

		}

		ToolTip t;

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.UseWaitCursor = true;
				Cursor.Current = Cursors.WaitCursor;
				this.Text = openFileDialog1.FileName;
				Bitmap b = Bitmap.FromFile(openFileDialog1.FileName) as Bitmap;
				pictureBox1.Image = b;
				int w = b.Width;
				int h = b.Height;
				var map = new Color[w, h];

				for (int i = 0; i < w; i++)
				{
					for (int j = 0; j < h; j++)
					{
						var c = b.GetPixel(i, j);
						map[i, j] = c;
					}
				}

				var graymap = grayscale(map);
				var contrast = Linear_contrast(map);
				var mask = linear_mask_filter(graymap);
				var dif = diff(graymap, mask);
				Cursor.Current = Cursors.Arrow;
				this.UseWaitCursor = false;

				Draw(graymap, pictureBox2);
				Draw(contrast, pictureBox3);
				Draw(mask, pictureBox4);
				Draw(dif, pictureBox5);


			}
		}

		static void Draw(byte[,] m, PictureBox p)
		{
			int w = m.GetLength(0);
			int h = m.GetLength(1);
			var b = new Bitmap(w, h);
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					Color c = Color.FromArgb(m[i, j], m[i, j], m[i, j]);
					b.SetPixel(i, j, c);
				}
			}
			p.Image = b;
		}
		static void Draw(Color[,] m, PictureBox p)
		{
			int w = m.GetLength(0);
			int h = m.GetLength(1);
			var b = new Bitmap(w, h);
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					Color c = m[i, j];
					b.SetPixel(i, j, c);
				}
			}
			p.Image = b;
		}
		static void Draw(byte[,] rmap, byte[,] gmap, byte[,] bmap, PictureBox p)
		{
			int w = rmap.GetLength(0);
			int h = rmap.GetLength(1);
			var b = new Bitmap(w, h);
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					Color c = Color.FromArgb(rmap[i, j], gmap[i, j], bmap[i, j]);
					b.SetPixel(i, j, c);
				}
			}
			p.Image = b;
		}

		static byte[,] grayscale(Color[,] m)
		{
			int w = m.GetLength(0);
			int h = m.GetLength(1);
			var ret = new byte[w, h];
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
				{
					ret[i, j] = (byte)(m[i, j].R * 0.3 + m[i, j].G * 0.59 + m[i, j].B * 0.11);

				}
			return ret;
		}

		static byte[,] diff(byte[,] a1, byte[,] a2)
		{
			int w = Math.Min(a1.GetLength(0), a2.GetLength(0));
			int h = Math.Min(a1.GetLength(1), a2.GetLength(1));
			var ret = new byte[w, h];
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					ret[i, j] = (byte)Math.Abs(a1[i, j] - a2[i, j]);
					if (ret[i, j] != 0)
					{
						ret[i, j] = 255;
					}
				}
			}
			return ret;
		}

		static void linear_constrast_init(Color[,] cmap, out Color hi, out Color lo)
		{
			hi = cmap[0, 0];
			lo = cmap[0, 0];

			for (int i = 0; i < cmap.GetLength(0); i++)
			{
				for (int j = 0; j < cmap.GetLength(1); j++)
				{
					if (cmap[i, j].R > hi.R)
						hi = Color.FromArgb(cmap[i, j].R, hi.G, hi.B);
					if (cmap[i, j].R < lo.R)
						lo = Color.FromArgb(cmap[i, j].R, lo.G, lo.B);
					if (cmap[i, j].G > hi.G)
						hi = Color.FromArgb(hi.R, cmap[i, j].G, hi.B);
					if (cmap[i, j].G < lo.G)
						lo = Color.FromArgb(lo.R, cmap[i, j].G, lo.B);
					if (cmap[i, j].B > hi.B)
						hi = Color.FromArgb(hi.R, hi.G, cmap[i, j].B);
					if (cmap[i, j].B < lo.B)
						lo = Color.FromArgb(lo.R, lo.G, cmap[i, j].B);
				}
			}

		}
		static Color[,] Linear_contrast(Color[,] m)
		{
			Color hi;
			Color lo;
			int w = m.GetLength(0);
			int h = m.GetLength(1);
			Color[,] ret = new Color[w, h];
			linear_constrast_init(m, out hi, out lo);
			byte qr = (byte)(hi.R - lo.R);
			byte qg = (byte)(hi.G - lo.G);
			byte qb = (byte)(hi.B - lo.B);
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					byte R = (byte)(int)((m[i, j].R - lo.R) * 255f / (float)qr);
					byte G = (byte)(int)((m[i, j].G - lo.G) * 255f / (float)qg);
					byte B = (byte)(int)((m[i, j].B - lo.B) * 255f / (float)qb);
					ret[i, j] = Color.FromArgb(255, R, G, B);
				}
			}
			return ret;
		}

		static byte[,] linear_mask_filter(byte[,] cmap)
		{
			int w = cmap.GetLength(0);
			int h = cmap.GetLength(1);
			var ret = new byte[w, h];
			var M = new float[,]
			{
				{ 1, 1, 1 },
				{ 1, 1, 1 },
				{ 1, 1, 1 }
			};
			byte bri = 0;
			for (int i = 1; i < w - 1; i++)
				for (int j = 1; j < h - 1; j++)
				{
					bri = 0;
					for (int I = -1; I <= 1; I++)
						for (int J = -1; J <= 1; J++)
							bri += (byte)(cmap[i + I, j + J] * M[1 + I, 1 + J] / 9);
					ret[i, j] = (byte)bri;
				}
			for (int i = 1; i < w-1; i++)
			{
				bri = 0;
				for (int I = -1; I <= 1; I++)
					for (int J = 0; J <= 1; J++)
						bri += (byte)(cmap[i + I, J] * M[1 + I, 1 + J] / 6);
				ret[i, 0] = (byte)bri;
				bri = 0;
				for (int I = -1; I <= 1; I++)
					for (int J = -1; J <= 0; J++)
						bri += (byte)(cmap[i + I, h - 1 + J] * M[1 + I, 1 + J] / 6);
				ret[i, h - 1] = (byte)bri;
			}
			for (int j = 1; j < h - 1; j++)
			{
				bri = 0;
				for (int I = 0; I <= 1; I++)
					for (int J = -1; J <= 1; J++)
						bri += (byte)(cmap[I, j + J] * M[1 + I, 1 + J] / 6);
				ret[0, j] = (byte)bri;
				bri = 0;
				for (int I = -1; I <= 0; I++)
					for (int J = -1; J <= 1; J++)
						bri += (byte)(cmap[w - 1 + I, j + J] * M[1 + I, 1 + J] / 6);
				ret[w - 1, j] = (byte)bri;
			}
			bri = 0;
			for (int I = 0; I <= 1; I++)
				for (int J = 0; J <= 1; J++)
					bri += (byte)(cmap[I, J] * M[1 + I, 1 + J] / 4);
			ret[0, 0] = bri;
			bri = 0;
			for (int I = -1; I <= 0; I++)
				for (int J = 0; J <= 1; J++)
					bri += (byte)(cmap[w - 1 + I, J] * M[1 + I, 1 + J] / 4);
			ret[w - 1, 0] = bri;
			bri = 0;
			for (int I = -1; I <= 0; I++)
				for (int J = -1; J <= 0; J++)
					bri += (byte)(cmap[w - 1 + I, h - 1 + J] * M[1 + I, 1 + J] / 4);
			ret[w - 1, h - 1] = bri;
			bri = 0;
			for (int I = 0; I <= 1; I++)
				for (int J = -1; J <= 0; J++)
					bri += (byte)(cmap[I, h - 1 + J] * M[1 + I, 1 + J] / 4);
			ret[0, h - 1] = bri;
			return ret;
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
			{
				Size s = flowLayoutPanel1.Controls[i].Size;
				s = new Size((int)(s.Width * 1.5f), (int)(s.Height * 1.47f));
				flowLayoutPanel1.Controls[i].Size = s;
			}
		}
		private void toolStripMenuItem3_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
			{
				Size s = flowLayoutPanel1.Controls[i].Size;
				s = new Size((int)(s.Width / 1.5f), (int)(s.Height / 1.47f));
				flowLayoutPanel1.Controls[i].Size = s;
			}
		}

		static private byte[,] hough_init(byte[,] M)
		{
			int w = M.GetLength(0);
			int h = M.GetLength(1);
			byte[,] ret = new byte[w, h];
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
					ret[i, j] = M[i, j] > 127 ? (byte)255 : (byte)0;
			return ret;
		}

		static private byte[,] hough(byte[,] M)
		{
			M = hough_init(M);
			int w = M.GetLength(0);
			int h = M.GetLength(1);
			int h_w = 360;
			int h_h = (int)Math.Sqrt(Math.Pow(w, 2) + Math.Pow(h, 2));

			var hough_acum = new int[h_w, h_h];
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
				{
					for (int a = 0; a < h_w; a++)
					{
						double r = Math.Abs(i * Math.Cos(a * Math.PI / 180) + j * Math.Sin(a * Math.PI / 180));
						hough_acum[a, (int)r] += 1;
					}
				}
			int max = hough_acum[0,0];
			int max_a = 0;
			int max_r = 0;
			for (int i = 0; i < h_w; i++)
				for (int j = 0; j < h_h; j++)
				{
					if (hough_acum[i,j] > max)
					{
						max = hough_acum[i, j];
						max_a = i;
						max_r = j;
					}
				}
			MessageBox.Show(string.Format("a: {0:0000}; r: {1:0000}; m: {2:0000}", max_a, max_r, max));
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
				{
					if (max_r == (int)(i*Math.Cos(max_a*Math.PI/180) + j * Math.Sin(max_a * Math.PI / 180)))
						M[i, j] = 0;
				}

			return M;
		}

		
		static T[,] Copy <T>(T[,] a)
		{
			int w = a.GetLength(0);
			int h = a.GetLength(1);
			T[,] M = new T[w, h];
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
					M[i, j] = a[i, j];
			return M;
		}

		private void pictureBox6_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.UseWaitCursor = true;
				Cursor.Current = Cursors.WaitCursor;
				Bitmap b = new Bitmap(this.openFileDialog1.FileName);
				byte[,] M = new byte[b.Width,b.Height];
				for (int i = 0; i < b.Width; i++)
				{
					for (int j = 0; j < b.Height; j++)
					{
						var c = b.GetPixel(i, j);
						M[i, j] = (byte)(c.R | c.G | c.B);
					}
				}
				var h = hough(Copy<byte>(M));
				Draw(h, pictureBox6);
				Cursor.Current = Cursors.Arrow;
				this.UseWaitCursor = false;
			}
		}

		private void pictureBox5_Click(object sender, EventArgs e)
		{

		}
	}
}
