﻿namespace Lab4
{
	partial class options
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.trackBar2 = new System.Windows.Forms.TrackBar();
			this.trackBar3 = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.button1.Location = new System.Drawing.Point(0, 242);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(284, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Применить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "ЛК. Минимум";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "ЛК. Максимум";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(0, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "ЧБ. Порог";
			// 
			// trackBar1
			// 
			this.trackBar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.trackBar1.LargeChange = 8;
			this.trackBar1.Location = new System.Drawing.Point(0, 13);
			this.trackBar1.Maximum = 255;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(284, 37);
			this.trackBar1.TabIndex = 6;
			// 
			// trackBar2
			// 
			this.trackBar2.Dock = System.Windows.Forms.DockStyle.Top;
			this.trackBar2.LargeChange = 8;
			this.trackBar2.Location = new System.Drawing.Point(0, 63);
			this.trackBar2.Maximum = 255;
			this.trackBar2.Name = "trackBar2";
			this.trackBar2.Size = new System.Drawing.Size(284, 37);
			this.trackBar2.TabIndex = 7;
			// 
			// trackBar3
			// 
			this.trackBar3.Dock = System.Windows.Forms.DockStyle.Top;
			this.trackBar3.LargeChange = 8;
			this.trackBar3.Location = new System.Drawing.Point(0, 113);
			this.trackBar3.Maximum = 255;
			this.trackBar3.Name = "trackBar3";
			this.trackBar3.Size = new System.Drawing.Size(284, 37);
			this.trackBar3.TabIndex = 8;
			// 
			// options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 265);
			this.Controls.Add(this.trackBar3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.trackBar2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Name = "options";
			this.Text = "options";
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.TrackBar trackBar2;
		public System.Windows.Forms.TrackBar trackBar3;
		public System.Windows.Forms.TrackBar trackBar1;
	}
}