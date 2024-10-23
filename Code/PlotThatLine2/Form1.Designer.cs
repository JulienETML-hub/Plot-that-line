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
            Graph1 = new ScottPlot.WinForms.FormsPlot();
            checkedListBox1 = new CheckedListBox();
            addCity = new Button();
            dateTimePickerDebut = new DateTimePicker();
            dateTimePickerFin = new DateTimePicker();
            Début = new Label();
            label2 = new Label();
            Search = new Button();
            SuspendLayout();
            // 
            // Graph1
            // 
            Graph1.DisplayScale = 1F;
            Graph1.Location = new Point(161, 2);
            Graph1.Name = "Graph1";
            Graph1.Size = new Size(177, 183);
            Graph1.TabIndex = 1;
            // 
            // checkedListBox1
            // 
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.ForeColor = SystemColors.MenuText;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(12, 33);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(120, 94);
            checkedListBox1.TabIndex = 2;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // addCity
            // 
            addCity.Location = new Point(12, 162);
            addCity.Name = "addCity";
            addCity.Size = new Size(98, 42);
            addCity.TabIndex = 3;
            addCity.Text = "Ajouter une ville";
            addCity.UseVisualStyleBackColor = true;
            addCity.Click += addCityB_Click;
            // 
            // dateTimePickerDebut
            // 
            dateTimePickerDebut.Checked = false;
            dateTimePickerDebut.Location = new Point(12, 415);
            dateTimePickerDebut.Name = "dateTimePickerDebut";
            dateTimePickerDebut.Size = new Size(194, 23);
            dateTimePickerDebut.TabIndex = 5;
            dateTimePickerDebut.Value = new DateTime(2024, 9, 17, 0, 0, 0, 0);
            dateTimePickerDebut.ValueChanged += dateTimePickerDebut_ValueChanged;
            // 
            // dateTimePickerFin
            // 
            dateTimePickerFin.Location = new Point(212, 415);
            dateTimePickerFin.Name = "dateTimePickerFin";
            dateTimePickerFin.Size = new Size(200, 23);
            dateTimePickerFin.TabIndex = 6;
            dateTimePickerFin.Value = new DateTime(2024, 9, 27, 0, 0, 0, 0);
            dateTimePickerFin.ValueChanged += dateTimePickerFin_ValueChanged;
            // 
            // Début
            // 
            Début.AutoSize = true;
            Début.Location = new Point(94, 384);
            Début.Name = "Début";
            Début.Size = new Size(39, 15);
            Début.TabIndex = 7;
            Début.Text = "Début";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(300, 384);
            label2.Name = "label2";
            label2.Size = new Size(23, 15);
            label2.TabIndex = 8;
            label2.Text = "Fin";
            // 
            // Search
            // 
            Search.Location = new Point(23, 210);
            Search.Name = "Search";
            Search.Size = new Size(75, 23);
            Search.TabIndex = 9;
            Search.Text = "Rechercher";
            Search.UseVisualStyleBackColor = true;
            Search.Click += Search_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Search);
            Controls.Add(label2);
            Controls.Add(Début);
            Controls.Add(dateTimePickerFin);
            Controls.Add(dateTimePickerDebut);
            Controls.Add(addCity);
            Controls.Add(checkedListBox1);
            Controls.Add(Graph1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ScottPlot.WinForms.FormsPlot Graph1;
        private CheckedListBox checkedListBox1;
        private Button addCity;
        //private Button refresh;
        private DateTimePicker dateTimePickerDebut;
        private DateTimePicker dateTimePickerFin;
        private Label Début;
        private Label label2;
        private Button Search;
    }
}
