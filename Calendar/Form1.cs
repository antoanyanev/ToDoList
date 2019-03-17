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
        public SqlConnection dbCon = new SqlConnection(
                "Data Source = (localdb)\\MSSQLLocalDB; " +
                "Initial Catalog = Data; " +
                "Integrated Security = True; " +
                "Connect Timeout = 30; " +
                "Encrypt = False; " +
                "TrustServerCertificate = True;" +
                " ApplicationIntent = ReadWrite; " +
                "MultiSubnetFailover = False");

        public Form1() {
            InitializeComponent();

        }

        private void LoginButton_Click(object sender, EventArgs e) {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void NameBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
