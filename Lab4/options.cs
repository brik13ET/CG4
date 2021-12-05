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
	public partial class options : Form
	{
		
		public options(Form parent)
		{
			this.Tag = parent;
			InitializeComponent();
		}

		public int LC_LOW;
		public int LC_HIGH;
		public int BW_LEVEL;

				private void button1_Click(object sender, EventArgs e)
		{
			(this.Tag as Form).Show();
			this.Hide();
		}
	}
}
