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
        List<TextBox> MyTextBoxes;
        List<Label> MyLabels;
        List<Button> MyButtons;
        Login login;

        public SqlConnection dbCon = new SqlConnection(
                "Data Source = (localdb)\\MSSQLLocalDB; " +
                "Initial Catalog = Data; " +
                "Integrated Security = True; " +
                "Connect Timeout = 30; " +
                "Encrypt = False; " +
                "TrustServerCertificate = True;" +
                "ApplicationIntent = ReadWrite; " +
                "MultiSubnetFailover = False");

        public Form1() {
            InitializeComponent();
            GetLoginComponents();
        }

        private void GetLoginComponents() {
            MyButtons = new List<Button>();
            MyLabels = new List<Label>();
            MyTextBoxes = new List<TextBox>();

            foreach (var button in Controls.OfType<Button>()) {
                MyButtons.Add(button);
            }

            foreach (var textBox in Controls.OfType<TextBox>()) {
                MyTextBoxes.Add(textBox);
            }

            foreach (var label in Controls.OfType<Label>()) {
                MyLabels.Add(label);
            }

            login = new Login(MyButtons, MyLabels, MyTextBoxes);
        }

        private void LoginButton_Click(object sender, EventArgs e) {
            login.CreateUser();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void NameBox_TextChanged(object sender, EventArgs e) {

        }
    }
}
