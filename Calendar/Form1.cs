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
        public Form1() {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e) {
            Console.WriteLine("Logna se pi4");

            SqlConnection dbCon = new SqlConnection(
                "Data Source = (localdb)\\MSSQLLocalDB; " +
                "Initial Catalog = Data; " +
                "Integrated Security = True; " +
                "Connect Timeout = 30; " +
                "Encrypt = False; " +
                "TrustServerCertificate = True;" +
                " ApplicationIntent = ReadWrite; " +
                "MultiSubnetFailover = False");

            dbCon.Open();

            using (dbCon) {
                string[] info = new string[] { NameBox.Text, SurnameBox.Text, BirthdateBox.Text, GenderBox.Text, CityBox.Text };

                if (CheckNull(info) && CheckDate(info[2])) {
                    string values = $"VALUES ('{info[0]}', '{info[1]}', '{info[2]}', '{info[3]}', '{info[4]}')";

                    SqlCommand command = new SqlCommand("INSERT INTO USERS (Name, Surname, Birthdate, Gender, City)" + values, dbCon);
                    command.ExecuteScalar();
                } 
            }

            dbCon.Close();
        }

        private bool CheckNull(string[] input) {
            bool ok = true;

            foreach (string str in input) {
                if (str == String.Empty) {
                    ok = false;
                    break;
                }
            }

            return ok;
        }

        private bool CheckDate(string input) {
            bool ok = true;

            Regex rx = new Regex(@"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))");

            MatchCollection matches = rx.Matches(input);

            if (matches.Count != 1) {
                ok = false;
            }

            return ok;
        }
    }
}
