using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Calendar.Views;

namespace Calendar {
    public partial class Form1 : Form {
        private Display display;

        public Form1() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoScroll = true;
            display = new Display(this);

        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        public void AddControls(List<Control> controls)
        {
            Controls.AddRange(controls.ToArray());
        }
    }
}
