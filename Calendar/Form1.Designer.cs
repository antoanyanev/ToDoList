namespace Calendar {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.NameLabel = new System.Windows.Forms.Label();
            this.SurnameLabel = new System.Windows.Forms.Label();
            this.BirthdateLabel = new System.Windows.Forms.Label();
            this.GenderLabel = new System.Windows.Forms.Label();
            this.CityLabel = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.SurnameBox = new System.Windows.Forms.TextBox();
            this.BirthdateBox = new System.Windows.Forms.TextBox();
            this.GenderBox = new System.Windows.Forms.TextBox();
            this.CityBox = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(100, 48);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name";
            // 
            // SurnameLabel
            // 
            this.SurnameLabel.AutoSize = true;
            this.SurnameLabel.Location = new System.Drawing.Point(100, 77);
            this.SurnameLabel.Name = "SurnameLabel";
            this.SurnameLabel.Size = new System.Drawing.Size(49, 13);
            this.SurnameLabel.TabIndex = 1;
            this.SurnameLabel.Text = "Surname";
            // 
            // BirthdateLabel
            // 
            this.BirthdateLabel.AutoSize = true;
            this.BirthdateLabel.Location = new System.Drawing.Point(100, 104);
            this.BirthdateLabel.Name = "BirthdateLabel";
            this.BirthdateLabel.Size = new System.Drawing.Size(49, 13);
            this.BirthdateLabel.TabIndex = 2;
            this.BirthdateLabel.Text = "Birthdate";
            // 
            // GenderLabel
            // 
            this.GenderLabel.AutoSize = true;
            this.GenderLabel.Location = new System.Drawing.Point(100, 130);
            this.GenderLabel.Name = "GenderLabel";
            this.GenderLabel.Size = new System.Drawing.Size(42, 13);
            this.GenderLabel.TabIndex = 3;
            this.GenderLabel.Text = "Gender";
            // 
            // CityLabel
            // 
            this.CityLabel.AutoSize = true;
            this.CityLabel.Location = new System.Drawing.Point(100, 160);
            this.CityLabel.Name = "CityLabel";
            this.CityLabel.Size = new System.Drawing.Size(24, 13);
            this.CityLabel.TabIndex = 4;
            this.CityLabel.Text = "City";
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(153, 48);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(100, 20);
            this.NameBox.TabIndex = 5;
            // 
            // SurnameBox
            // 
            this.SurnameBox.Location = new System.Drawing.Point(153, 76);
            this.SurnameBox.Name = "SurnameBox";
            this.SurnameBox.Size = new System.Drawing.Size(100, 20);
            this.SurnameBox.TabIndex = 6;
            // 
            // BirthdateBox
            // 
            this.BirthdateBox.Location = new System.Drawing.Point(153, 102);
            this.BirthdateBox.Name = "BirthdateBox";
            this.BirthdateBox.Size = new System.Drawing.Size(100, 20);
            this.BirthdateBox.TabIndex = 7;
            // 
            // GenderBox
            // 
            this.GenderBox.Location = new System.Drawing.Point(153, 130);
            this.GenderBox.Name = "GenderBox";
            this.GenderBox.Size = new System.Drawing.Size(100, 20);
            this.GenderBox.TabIndex = 8;
            // 
            // CityBox
            // 
            this.CityBox.Location = new System.Drawing.Point(153, 157);
            this.CityBox.Name = "CityBox";
            this.CityBox.Size = new System.Drawing.Size(100, 20);
            this.CityBox.TabIndex = 9;
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(153, 205);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 10;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(625, 508);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.CityBox);
            this.Controls.Add(this.GenderBox);
            this.Controls.Add(this.BirthdateBox);
            this.Controls.Add(this.SurnameBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.CityLabel);
            this.Controls.Add(this.GenderLabel);
            this.Controls.Add(this.BirthdateLabel);
            this.Controls.Add(this.SurnameLabel);
            this.Controls.Add(this.NameLabel);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblSurname;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.TextBox texttxtDate;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.TextBox txtGender;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label SurnameLabel;
        private System.Windows.Forms.Label BirthdateLabel;
        private System.Windows.Forms.Label GenderLabel;
        private System.Windows.Forms.Label CityLabel;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.TextBox SurnameBox;
        private System.Windows.Forms.TextBox BirthdateBox;
        private System.Windows.Forms.TextBox GenderBox;
        private System.Windows.Forms.TextBox CityBox;
        private System.Windows.Forms.Button LoginButton;
    }
}

