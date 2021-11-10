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

		}

		ToolTip t;

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.UseWaitCursor = true;
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
				var dif = diff(graymap, mask, w, h);
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
					Color c = Color.FromArgb(m[i, j], m[i, j],m[i, j]);
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
					ret[i, j] = (byte)(m[i,j].R * 0.3 + m[i, j].G * 0.59 + m[i, j].B * 0.11);
					
				}
			return ret;
		}
		
		static byte[,] diff(byte[,] a1, byte[,] a2, int w, int h)
		{
			var ret = new byte[w,h];
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					ret[i, j] = (byte)Math.Abs(a1[i,j] - a2[i,j]);
					if (ret[i, j] != 0)
					{
						ret[i, j] = 255; 
					}
				}
			}
			return ret;
		}

		static void linear_constrast_init (Color[,] cmap, out Color hi, out Color lo)
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
					ret[i,j] = Color.FromArgb(255, R, G, B);
				}
			}
			return ret;
		}

		static byte[,] linear_mask_filter (byte[,] cmap)
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
			for (int i = 1; i < w - 1; i++)
				for (int j = 1; j < h - 1; j++)
				{
					double bri = 0;
					for (int I = -1; I <= 1; I++)
						for (int J = -1; J <= 1; J++)
						{
							bri += (cmap[i + I, j + J] * M[1 + I, 1 + J] / 9);
						}
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
	}
}
