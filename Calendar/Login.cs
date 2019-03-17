using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar {
    public class Login {
        Form1 form;
        List<TextBox> textBoxes;
        List<Label> labels;
        List<Button> buttons;
        
        public Login() {
            textBoxes = new List<TextBox>();
            labels = new List<Label>();
            buttons = new List<Button>();
            
            foreach (var box in form.Controls.OfType<TextBox>())
            {
                textBoxes.Add(box);
            }

            foreach (var label in form.Controls.OfType<Label>())
            {
                labels.Add(label);
            }

            foreach (var button in form.Controls.OfType<Button>())
            {
                buttons.Add(button);
            }

            HideContent();
        }

        public void CreateUser() {
            form.dbCon.Open();

            using (form.dbCon) {
                string[] info = new string[] { form.NameBox.Text, form.SurnameBox.Text, form.BirthdateBox.Text, form.GenderBox.Text, form.CityBox.Text };

                if (CheckNull(info) && CheckDate(info[2])) {
                    string values = $"VALUES ('{info[0]}', '{info[1]}', '{info[2]}', '{info[3]}', '{info[4]}')";

                    SqlCommand command = new SqlCommand("INSERT INTO USERS (Name, Surname, Birthdate, Gender, City)" + values, form.dbCon);
                    command.ExecuteScalar();
                }
            }

            form.dbCon.Close();
        }

        private bool CheckNull(string[] input) {
            bool ok = true;

            foreach (string str in input) {
                if (str == String.Empty) {
                    ok = false;
                    form.ErrorLabel.Text = "All fields should not be null!";
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
                form.ErrorLabel.Text = "Invalid Date!";
                ok = false;
            }

            return ok;
        }

        public void HideContent()
        {
            foreach (TextBox box in textBoxes)
            {
                box.Hide();
            }

            foreach (Label label in labels)
            {
                label.Hide();
            }

            foreach (Button button in buttons)
            {
                button.Hide();
            }
        }

        public void ShowContent()
        {
            foreach (TextBox box in textBoxes)
            {
                box.Show();
            }

            foreach (Label label in labels)
            {
                label.Show();
            }

            foreach (Button button in buttons)
            {
                button.Show();
            }
        }
    }
}
