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

			var o = new options(this);
			this.Tag = o;
			o.Hide();
			o.VisibleChanged += new EventHandler(updateOptions);


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
			t.SetToolTip(pictureBox5, "Vector");

		}

		public void updateOptions(object sender, EventArgs e)
		{
			var o = (sender as options);
			(o.Tag as Form1).LC_lo = o.trackBar1.Value;
			(o.Tag as Form1).LC_hi = o.trackBar2.Value;
			(o.Tag as Form1).BW_l = o.trackBar3.Value;
			if ((o.Tag as Form1).pictureBox1.Image != null)
				(o.Tag as Form1).redraw();
		}

		public int LC_lo;
		public int LC_hi;
		public int BW_l;

		ToolTip t;

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
		}

		static void Draw(bool[,] m, PictureBox p)
		{
			int w = m.GetLength(0);
			int h = m.GetLength(1);
			var b = new Bitmap(w, h);
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
					if (m[i, j])
						b.SetPixel(i, j, Color.Black);
					else
						b.SetPixel(i, j, Color.White);
			p.Image = b;
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
		bool[,] monochrome(byte[,] m)
		{
			int w = m.GetLength(0);
			int h = m.GetLength(1);
			var ret = new bool[w, h];
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
					ret[i, j] = m[i, j] <= BW_l;
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
		Color[,] Linear_contrast(Color[,] m)
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
					byte R = (byte)(((m[i, j].R - lo.R) * (LC_hi - LC_lo) / (float)qr) + LC_lo);
					byte G = (byte)(((m[i, j].G - lo.G) * (LC_hi - LC_lo) / (float)qg )+ LC_lo);
					byte B = (byte)(((m[i, j].B - lo.B) * (LC_hi - LC_lo) / (float)qb) + LC_lo);
					ret[i, j] = Color.FromArgb(R, G, B);
				}
			}
			return ret;
		}

		static byte[,] linear_mask_filter(byte[,] cmap)
		{
			int w = cmap.GetLength(0);
			int h = cmap.GetLength(1);
			var cmap2 = new byte[w + 2, h + 2];
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
					cmap2[i+1, j+1] = cmap[i, j];
			for (int i = 0; i < w; i++)
			{
				cmap2[i + 1, 0] = cmap[i, 0];
				cmap2[i + 1, h + 1] = cmap[i, h - 1];
			}
			for (int i = 0; i < h; i++)
			{
				cmap2[0, i + 1] = cmap[0, i];
				cmap2[w + 1, i + 1] = cmap[w- 1, i];
			}
			cmap2[0, 0] = cmap[0, 0];
			cmap2[0, h] = cmap[0, h-1];
			cmap2[w, 0] = cmap[w-1, 0];
			cmap2[w, h] = cmap[w-1, h-1];
			var ret = new byte[w, h];
			var M = new float[,]
			{
				{ 1, 1, 1 },
				{ 1, 1, 1 },
				{ 1, 1, 1 }
			};
			byte bri = 0;
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
				{
					bri = 0;
					for (int I = -1; I <= 1; I++)
						for (int J = -1; J <= 1; J++)
							bri += (byte)(cmap2[i + I + 1, j + J + 1] * M[1 + I, 1 + J] / 9);
					ret[i, j] = (byte)bri;
				}

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
			int h_h = (int)(Math.Max(w, h) * Math.Max(w, h));
			var m = new byte[w, h];
			var hough_acum = new int[h_w, h_h];
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
					if (M[i, j] == 0)
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
			string[] f = {
				string.Format("y = kx + b, k={0:0000.0000}, b={1:0000.0000}", -1 / Math.Tan(max_a), max_r / Math.Sin(max_a) ),
				string.Format("\u03C1=P/cos(\u03D5 - \u03B1), P = {0}, \u03B1 = {1:000.0000}", max_r, max_a )
			};
			line(m, max_a - 90, max_r);
			var view_f = new formula_view(f);
			view_f.Show();
			return m;
		}

		static void Dot(byte[,] m, int x, int y)
		{
			int w = m.GetLength(0), h = m.GetLength(1);
			if (x < w && x >= 0 && y < h && y >=0)
				m[x , y ] = 255;
		}

		static void line(byte[,] m, int angle, int rad)
		{
			int w = m.GetLength(0);
			int h = m.GetLength(1);
			int x0 = (int)(Math.Cos(Math.PI * (angle + 90) / 180) * rad);
			int y0 = (int)(Math.Sin(Math.PI * (angle + 90) / 180) * rad);
			angle %= 180;
			if (Math.Abs(angle) <= 45)
				for (int i = 0; i < w; i++)
					Dot(m, x0 + i, (int)(Math.Tan(Math.PI * angle / 180) * i+ y0));
			else
				for (int i = 0; i < h; i++)
					Dot(m, (int)(Math.Tan(Math.PI * angle / 180) * (x0 + i)), y0 + i);
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
		
		private void pictureBox1_DoubleClick(object sender, EventArgs e)
		{
			if ((sender as PictureBox).Image != null)
			{
				var f = new view((sender as PictureBox).Image);
				f.Show();
			}
		}

		private void pictureBox6_Click_1(object sender, MouseEventArgs e)
		{

		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(this.Tag as options).Show();
			this.Hide();

		}

		private void toolStripMenuItem1_MouseHover(object sender, EventArgs e)
		{
			(sender as ToolStripMenuItem).ShowDropDown();
		}

		private void toolStripMenuItem1_Click(object sender, MouseEventArgs e)
		{

		}

		public void redraw()
		{
			var b = (Bitmap)pictureBox1.Image;
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
			var mono = monochrome(graymap);
			Cursor.Current = Cursors.Arrow;
			this.UseWaitCursor = false;

			Draw(graymap, pictureBox2);
			Draw(contrast, pictureBox3);
			Draw(mask, pictureBox4);
			Draw(dif, pictureBox5);
			Draw(mono, pictureBox9);
		}

		private void pictureBox1_DoubleClick_1(object sender, EventArgs e)
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
				var mono = monochrome(graymap);
				Cursor.Current = Cursors.Arrow;
				this.UseWaitCursor = false;

				Draw(graymap, pictureBox2);
				Draw(contrast, pictureBox3);
				Draw(mask, pictureBox4);
				Draw(dif, pictureBox5);
				Draw(mono, pictureBox9);
			}
		}
	}
}
