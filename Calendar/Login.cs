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
        List<TextBox> MyTextBoxes;
        List<Label> MyLabels;
        List<Button> MyButtons;



        public Login(List<Button> buttons, List<Label> labels, List<TextBox> textBoxes) {
            this.MyTextBoxes = new List<TextBox>();
            this.MyLabels = new List<Label>();
            this.MyButtons = new List<Button>();

            IntitializeArrays(buttons, labels, textBoxes);
            CopyComponents(buttons, labels, textBoxes);

            ShowContent();
        }

        public void CreateUser() {
            SqlConnection dbCon = new SqlConnection(
                "Data Source = (localdb)\\MSSQLLocalDB; " +
                "Initial Catalog = Data; " +
                "Integrated Security = True; " +
                "Connect Timeout = 30; " +
                "Encrypt = False; " +
                "TrustServerCertificate = True;" +
                "ApplicationIntent = ReadWrite; " +
                "MultiSubnetFailover = False");

            dbCon.Open();

            using (dbCon) {
                List<string> info = new List<string>(MyTextBoxes.Select(x => x.Text).Reverse().ToList());

                if (CheckNull(info) && CheckDate(info[2])) {
                    string values = $"VALUES ('{info[0]}', '{info[1]}', '{info[2]}', '{info[3]}', '{info[4]}')";

                    SqlCommand command = new SqlCommand("INSERT INTO USERS (Name, Surname, Birthdate, Gender, City)" + values, dbCon);
                    command.ExecuteScalar();

                    HideContent();
                }
            }

            dbCon.Close();
        }

        private bool CheckNull(List<string> input) {
            bool ok = true;

            foreach (string str in input) {
                if (str == String.Empty) {
                    ok = false;
                    MyLabels.Where(x => x.Name == "ErrorLabel").First().Text = "No fields should be left empty!";
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
                MyLabels.Where(x => x.Name == "ErrorLabel").First().Text = "Invalid Date!";
                ok = false;
            }

            return ok;
        }

        private void CopyComponents(List<Button> buttons, List<Label> labels, List<TextBox> textBoxes) {
            for (int i = 0; i < buttons.Count; i++) {
                MyButtons[i] = buttons[i];
            }

            for (int i = 0; i < labels.Count; i++) {
                MyLabels[i] = labels[i];
            }

            for (int i = 0; i < textBoxes.Count; i++) {
                MyTextBoxes[i] = textBoxes[i];
            }
        }

        private void IntitializeArrays(List<Button> buttons, List<Label> labels, List<TextBox> textBoxes) {
            foreach (var button in buttons) {
                MyButtons.Add(new Button());
            }

            foreach (var label in labels) {
                MyLabels.Add(new Label());
            }

            foreach (var textBox in textBoxes) {
                MyTextBoxes.Add(new TextBox());
            }
        }

        public void HideContent() {
            foreach (TextBox box in MyTextBoxes) {
                box.Hide();
            }

            foreach (Label label in MyLabels) {
                label.Hide();
            }

            foreach (Button button in MyButtons) {
                button.Hide();
            }
        }

        public void ShowContent() {
            foreach (TextBox box in MyTextBoxes) {
                box.Show();
            }

            foreach (Label label in MyLabels) {
                label.Show();
            }

            foreach (Button button in MyButtons) {
                button.Show();
            }
        }
    }
}
