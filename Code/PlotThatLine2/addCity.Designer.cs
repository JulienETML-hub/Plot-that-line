namespace PlotThatLine2
{
    partial class addCity
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
            nameOfCity = new TextBox();
            latitudeOfCity = new TextBox();
            longitudeOfCity = new TextBox();
            validate = new Button();
            CountryOfCity = new TextBox();
            SuspendLayout();
            // 
            // nameOfCity
            // 
            nameOfCity.Location = new Point(316, 109);
            nameOfCity.Name = "nameOfCity";
            nameOfCity.Size = new Size(160, 23);
            nameOfCity.TabIndex = 0;
            nameOfCity.Text = "Insérez le nom de la ville";
            nameOfCity.TextAlign = HorizontalAlignment.Center;
            nameOfCity.TextChanged += nameOfCity_TextChanged;
            // 
            // latitudeOfCity
            // 
            latitudeOfCity.Location = new Point(316, 203);
            latitudeOfCity.Name = "latitudeOfCity";
            latitudeOfCity.Size = new Size(150, 23);
            latitudeOfCity.TabIndex = 1;
            latitudeOfCity.Text = "Insérez la latitude";
            latitudeOfCity.TextChanged += latitudeOfCity_TextChanged;
            // 
            // longitudeOfCity
            // 
            longitudeOfCity.Location = new Point(316, 241);
            longitudeOfCity.Name = "longitudeOfCity";
            longitudeOfCity.Size = new Size(150, 23);
            longitudeOfCity.TabIndex = 2;
            longitudeOfCity.Text = "Insérez la longitude";
            longitudeOfCity.TextChanged += longitudeOfCity_TextChanged;
            // 
            // validate
            // 
            validate.Location = new Point(363, 299);
            validate.Name = "validate";
            validate.Size = new Size(75, 23);
            validate.TabIndex = 3;
            validate.Text = "Valider";
            validate.UseVisualStyleBackColor = true;
            validate.Click += validate_Click;
            // 
            // CountryOfCity
            // 
            CountryOfCity.Location = new Point(338, 150);
            CountryOfCity.Name = "CountryOfCity";
            CountryOfCity.Size = new Size(100, 23);
            CountryOfCity.TabIndex = 4;
            CountryOfCity.Text = "Insérez le pays";
            CountryOfCity.TextChanged += CountryOfCity_TextChanged;
            // 
            // addCity
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(CountryOfCity);
            Controls.Add(validate);
            Controls.Add(longitudeOfCity);
            Controls.Add(latitudeOfCity);
            Controls.Add(nameOfCity);
            Name = "addCity";
            Text = "addCity";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox nameOfCity;
        private TextBox latitudeOfCity;
        private TextBox longitudeOfCity;
        private Button validate;
        private TextBox CountryOfCity;
    }
}