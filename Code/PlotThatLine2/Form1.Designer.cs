namespace PlotThatLine2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cityBox = new TextBox();
            Graph1 = new ScottPlot.WinForms.FormsPlot();
            SuspendLayout();
            // 
            // cityBox
            // 
            cityBox.Location = new Point(80, 12);
            cityBox.Name = "cityBox";
            cityBox.Size = new Size(100, 23);
            cityBox.TabIndex = 0;
            cityBox.TextChanged += cityBox_TextChanged;
            // 
            // Graph1
            // 
            Graph1.DisplayScale = 1F;
            Graph1.Location = new Point(40, 41);
            Graph1.Name = "Graph1";
            Graph1.Size = new Size(150, 150);
            Graph1.TabIndex = 1;
            Graph1.Load += Graph1_Load;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Graph1);
            Controls.Add(cityBox);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox cityBox;
        private ScottPlot.WinForms.FormsPlot Graph1;
    }
}
