using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace Calendar {
    public class Login {
        // <----- Labels -----> //

        private Label labelName;
        private Label labelSurname;
        private Label labelBirthdate;
        private Label labelGender;
        private Label labelCity;
        private Label labelFormat;
        private Label labelError;

        // <----- Text Boxes -----> //

        private TextBox textBoxName;
        private TextBox textBoxSurname;
        private TextBox textBoxBirthdate;
        private TextBox textBoxGender;
        private TextBox textBoxCity;

        // <----- Button -----> //

        private Button buttonLogin;

        List<TextBox> MyTextBoxes;
        List<Label> MyLabels;
        List<Button> MyButtons;

        Connection con;
        string connectionString;

        public Login() {
            connectionString = GetConnectionString("Connection.json");

            this.MyTextBoxes = new List<TextBox>();
            this.MyLabels = new List<Label>();
            this.MyButtons = new List<Button>();

            GenerateControls();
            SetControlsText();
            IntitializeArrays();
            SetControlsPosition(330, 160, 420, 160);
            SetControlsSize(60, 20);
            ShowContent();
        }

        public void CreateUser() {
            SqlConnection dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon) {
                List<string> info = new List<string>(MyTextBoxes.Select(x => x.Text).Reverse().ToList());
                string date = ReformatDate(info[2]);

                if (CheckNull(info) && CheckDate(date)) {
                    string values = $"VALUES ('{info[0]}', '{info[1]}', '{date}', '{info[3]}', '{info[4]}')";

                    SqlCommand command = new SqlCommand("INSERT INTO USERS (Name, Surname, Birthdate, Gender, City)" + values, dbCon);
                    command.ExecuteScalar();

                    HideContent();
                }
            }

            dbCon.Close();
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

        private void GenerateControls() {
            // Labels //

            labelName = new Label();
            labelSurname = new Label();
            labelBirthdate = new Label();
            labelGender = new Label();
            labelCity = new Label();
            labelFormat = new Label();
            labelError = new Label();

            // Text Boxes //

            textBoxName = new TextBox();
            textBoxSurname = new TextBox();
            textBoxBirthdate = new TextBox();
            textBoxGender = new TextBox();
            textBoxCity = new TextBox();

            // Button //

            buttonLogin = new Button();
        }

        private void IntitializeArrays() {
            MyButtons.Add(buttonLogin);

            MyLabels.AddRange(new Label[] { labelName, labelSurname, labelBirthdate, labelGender, labelCity, labelFormat, labelError });

            MyTextBoxes.AddRange(new TextBox[] { textBoxName, textBoxSurname, textBoxBirthdate, textBoxGender, textBoxCity });
        }

        private void SetControlsPosition(int textStartX, int textStartY, int boxStartX, int boxStartY) {
            for (int i = 0; i < MyLabels.Count - 2; i++) {
                MyLabels[i].Location = new Point(textStartX, textStartY);
                textStartY += 30;
            }

            foreach (var textBox in MyTextBoxes) {
                textBox.Location = new Point(boxStartX, boxStartY);
                boxStartY += 30;
            }

            MyLabels[5].Location = new Point(530, 220);  // format
            MyLabels[6].Location = new Point(530, 340);  // error

            MyButtons[0].Location = new Point(420, 340);
        }

        private void SetControlsSize(int x, int y) {
            for (int i = 0; i < MyLabels.Count; i++) {
                MyLabels[i].Size = new Size(x, y);
            }

            foreach (var textBox in MyTextBoxes) {
                textBox.Size = new Size(x, y);
            }

            MyButtons[0].Size = new Size(x, y);
        }

        private void SetControlsText() {
            labelName.Text = "Name";
            labelSurname.Text = "Surname";
            labelBirthdate.Text = "Birthdate";
            labelGender.Text = "Gender";
            labelCity.Text = "City";
            labelFormat.Text = "dd/mm/yyyy";
            labelError.Text = "";

            buttonLogin.Text = "Login";
        }

        private bool CheckNull(List<string> input) {
            bool ok = true;

            foreach (string str in input) {
                if (str == String.Empty) {
                    ok = false;
                    MyLabels.Where(x => x.Name == "labelError").First().Text = "No fields should be left empty!";
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
                MyLabels.Where(x => x.Name == "labelError").First().Text = "Invalid Date!";
                ok = false;
            }

            return ok;
        }

        public string GetConnectionString(string file) {

            using (StreamReader r = new StreamReader(file)) {
                string json = r.ReadToEnd();
                con = JsonConvert.DeserializeObject<Connection>(json);
            }

            return con.ConnectionString;
        }

        public string ReformatDate(string input) {
            string date = String.Join("-", input.Split('/').Reverse().ToArray());

            return date;
        }

        public List<Control> getControls() {
            List<Control> controls = new List<Control>();

            controls.AddRange(MyLabels);           
            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);

            return controls;
        }
    }
}

