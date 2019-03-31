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
        ToDo toDo;

        public Form1() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoScroll = true;
            login = new Login();
            toDo = new ToDo(this);
            toDo.HideContent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            List<Control> controls = login.getControls(toDo);
            controls.AddRange(toDo.getControls());
            Controls.AddRange(controls.ToArray());

            login.HideContent();
            toDo.ShowContent();
        }
    }
}
