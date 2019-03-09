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
                string name = NameBox.Text;
                string surname = SurnameBox.Text;
                string birthdate = BirthdateBox.Text;
                string gender = GenderBox.Text;
                string city = CityBox.Text;

                string values = $"VALUES ('{name}', '{surname}', '{birthdate}', '{gender}', '{city}')";

                SqlCommand command = new SqlCommand("INSERT INTO USERS (Name, Surname, Birthdate, Gender, City)" + values, dbCon);
                command.ExecuteScalar();
            }

            dbCon.Close();
        }
    }
}
