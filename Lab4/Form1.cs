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
				pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);


			}
		}

		private void pictureBox1_Resize(object sender, EventArgs e)
		{

		}

		static Color grayscale(Color c)
		{
			byte gray = (byte)(c.R * 0.2 + c.G * 0.4 + c.B * 0.1);
			return Color.FromArgb(255, gray, gray, gray);
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{

		}
	}
}
