using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar {
    public class Login {
        public Login() {

        }

        private string name;
        private string surname;
        private DateTime birthdate;
        private string gender;
        private string city;

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public string Surname {
            get { return surname; }
            set { surname = value; }
        }

        public DateTime Birthdate {
            get { return birthdate; }
            set { birthdate = value; }
        }

        public string Gender {
            get { return gender; }
            set { gender = value; }
        }

        public string City {
            get { return city; }
            set { city = value; }
        }

        public void CreateUser(SqlConnection dbCon) {
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
                    label1.Text = "All fields should not be null!";
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
                label1.Text = "Invalid Date!";
                ok = false;
            }

            return ok;
        }
    }
}
