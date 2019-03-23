/*
    This is the first app page
    Use it to create your user and then login

    Written by Antoan Yanev & Vladislav Milenkov
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;

namespace Calendar {
    public class Login {
        ToDo toDo; // Creates a ToDo instanse to acces the Hide and Show methods

        private const int sizeX = 60; // Defines the X size of the controls
        private const int sizeY = 20; // Defines the Y size of the controls
        private const int firstLabelX = 80; // Defines the starting X point of the labels
        private const int firstLabelAndBoxY = 160; // Defines the starting Y point of the labels
        private const int firstTextBoxX = 170; // Defines the starting X point of the text boxes
        
        // Labels //

        private Label labelName;
        private Label labelSurname;
        private Label labelBirthdate;
        private Label labelGender;
        private Label labelCity;
        private Label labelFormat;
        private Label labelError;

        // Text Boxes //

        private TextBox textBoxName;
        private TextBox textBoxSurname;
        private TextBox textBoxBirthdate;
        private TextBox textBoxGender;
        private TextBox textBoxCity;

        // Buttons //

        private Button buttonLogin;

        // All Coontrols Collections //

        List<TextBox> MyTextBoxes;
        List<Label> MyLabels;
        List<Button> MyButtons;

        private Connection con; // Connection type object used for parsing
        private string connectionString; // Actual DB connection string

        public Login() {
            // Initialize all global variables //

            connectionString = GetConnectionString("Connection.json");

            this.MyTextBoxes = new List<TextBox>();
            this.MyLabels = new List<Label>();
            this.MyButtons = new List<Button>();

            GenerateControls(); // Initialize all controls
            SetControlsText(); // Set the text to the controls
            IntitializeArrays(); // Add the controls to their corresponging collection
            SetControlsPosition(firstLabelX, firstLabelAndBoxY, firstTextBoxX, firstLabelAndBoxY); // Place them on the screen
            SetControlsSize(sizeX, sizeY); // Give them a size
            SetLabelsTransparency(); // Make the Labels' BG colour tranapsrent
            ShowContent(); // make all controls visible
        }

        public void CreateUser() {
            // Opens a connection to the DB and after validations adds the user to the users Table

            SqlConnection dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon) {
                List<string> info = new List<string>(MyTextBoxes.Select(x => x.Text).ToList());
                string date = ReformatDate(info[2]);

                if (CheckNull(info) && CheckDate(date)) {
                    string values = $"VALUES ('{info[0]}', '{info[1]}', '{date}', '{info[3]}', '{info[4]}')";

                    SqlCommand command = new SqlCommand("INSERT INTO USERS (Name, Surname, Birthdate, Gender, City)" + values, dbCon);
                    command.ExecuteScalar();

                    HideContent();
                    toDo.ShowContent();
                }
            }

            dbCon.Close();
        }

        public void HideContent() {
            // Iterates over all controls and hides them

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
            // Iterates over all controls and displayes them

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
            // Initializes all controls

            // Labels //

            labelName = new Label();
            labelSurname = new Label();
            labelBirthdate = new Label();
            labelGender = new Label();
            labelCity = new Label();
            labelFormat = new Label();
            labelError = new Label();
            labelError.AutoSize = true;

            // Text Boxes //

            textBoxName = new TextBox();
            textBoxSurname = new TextBox();
            textBoxBirthdate = new TextBox();
            textBoxGender = new TextBox();
            textBoxCity = new TextBox();

            // Button //

            buttonLogin = new Button();
            buttonLogin.Click += Clicked;
        }

        private void IntitializeArrays() {
            // Adds all controls to their corresponding collection

            MyButtons.Add(buttonLogin);

            MyLabels.AddRange(new Label[] { labelName, labelSurname, labelBirthdate, labelGender, labelCity, labelFormat, labelError });

            MyTextBoxes.AddRange(new TextBox[] { textBoxName, textBoxSurname, textBoxBirthdate, textBoxGender, textBoxCity });
        }

        private void SetControlsPosition(int textStartX, int textStartY, int boxStartX, int boxStartY) {
            // Sets the position for every control element

            for (int i = 0; i < MyLabels.Count - 2; i++) {
                MyLabels[i].Location = new Point(textStartX, textStartY);
                textStartY += 30;
            }

            foreach (var textBox in MyTextBoxes) {
                textBox.Location = new Point(boxStartX, boxStartY);
                boxStartY += 30;
            }

            MyLabels[5].Location = new Point(250, 220);  // format
            MyLabels[6].Location = new Point(190, 345);  // error

            MyButtons[0].Location = new Point(130, 340);
        }

        private void SetControlsSize(int x, int y) {
            // Sets the size for all controls

            for (int i = 0; i < MyLabels.Count; i++) {
                MyLabels[i].Size = new Size(x, y);
            }

            foreach (var textBox in MyTextBoxes) {
                textBox.Size = new Size(x, y);
            }

            MyButtons[0].Size = new Size(x, y + 5);
        }

        private void SetControlsText() {
            // Adds text to the controls if necessary

            labelName.Text = "Name";
            labelSurname.Text = "Surname";
            labelBirthdate.Text = "Birthdate";
            labelGender.Text = "Gender";
            labelCity.Text = "City";
            labelFormat.Text = "dd/mm/yyyy";
            labelError.Text = "";
            labelError.Name = "labelError";

            buttonLogin.Text = "Login";
            buttonLogin.Name = "LoginButton";
        }

        private void SetLabelsTransparency() {
            // Makes the BG of all labels transparent

            for (int i = 0; i < MyLabels.Count; i++) {
                MyLabels[i].BackColor = System.Drawing.Color.Transparent;
            }
        }

        private bool CheckNull(List<string> input) {
            // Checks if any fields are left empty

            bool ok = true;

            foreach (string str in input) {
                if (str == String.Empty) {
                    ok = false;
                    MyLabels.Where(x => x.Name == "labelError").First().Text = "No empty fields!";
                    break;
                }
            }

            return ok;
        }

        private bool CheckDate(string input) {
            // Checks if the birthdate is in the correct format

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
            // Retrieves the DB connection string from the Connection.json file

            using (StreamReader r = new StreamReader(file)) {
                string json = r.ReadToEnd();
                con = JsonConvert.DeserializeObject<Connection>(json);
            }

            return con.ConnectionString;
        }

        public string ReformatDate(string input) {
            // Reformats the default DateTime format from yyyy-mm-dd to dd/mm/yyyy

            return String.Join("-", input.Split('/').Reverse().ToArray());
        }

        public List<Control> getControls(ToDo todo) {
            // Returns a collection of all controls to be added to the Form Controls collection

            this.toDo = todo;

            List<Control> controls = new List<Control>();

            controls.AddRange(MyLabels);           
            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);

            return controls;
        }

        public void Clicked(object sender, EventArgs e) {
            // Event handler bound to the login button

            CreateUser();
            toDo.UpdateInfo();
        }
    }
}
