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

namespace Calendar {
    public partial class Form1 : Form {
        Login login;

        public Form1() {
            InitializeComponent();
            login = new Login();
        }

        private void Form1_Load(object sender, EventArgs e) {
            List<Control> controls = login.getControls();
            Controls.AddRange(controls.ToArray());
        }
    }
}
