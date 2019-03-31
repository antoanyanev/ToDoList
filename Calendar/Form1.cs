using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
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
