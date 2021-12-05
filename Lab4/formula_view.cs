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
	public partial class formula_view : Form
	{
		public formula_view(string[] f)
		{
			InitializeComponent();
			if (f == null)
				this.Close();
			var ft = new Font("Arial", 24.5f, FontStyle.Regular, GraphicsUnit.Pixel);
			var s = new Size(this.Width, 28);
			for (int i = 0; i < f.Length; i++)
			{
				var l = new Label();
				l.Text = f[i];
				l.Font = ft;
				l.Size = s;
				l.AutoSize = true;
				l.Location = new Point(0, this.Size.Height * i);
				l.Dock = DockStyle.Top;
				this.Controls.Add(l);
			}
		}
	}
}
