using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Net;

namespace Calendar {
    public class ToDo {
        private Form1 form;
        private const int labelStartx = 20;
        private const int labelStartY = 90;
        private string url1 = "http://api.openweathermap.org/data/2.5/weather?q=";
        private string url2 = "&appid=3d5632822352c9cd93370a8212356d3f";

        private SqlConnection dbCon;
        private Connection con;
        private string connectionString;

        private List<Button> MyButtons;
        private List<TextBox> MyTextBoxes;
        private List<Label> MyLabels;
        private List<string> labelNames;

        // Buttons //

        private Button buttonAdd;
        private Button buttonDelete;
        private Button buttonDeleteAll;

        // TextBoxes //

        private TextBox textBoxInput;

        // Labels //

        private Label labelInfo;

        public ToDo(Form1 form) {
            this.form = form;

            MyButtons = new List<Button>();
            MyTextBoxes = new List<TextBox>();
            MyLabels = new List<Label>();
            labelNames = new List<string>();

            connectionString = GetConnectionString("Connection.json");           

            GenerateControls();
            IntitializeArrays();
            SetControlsSize();
            SetControlsText();
            SetControlsPosition(22, 70);

            FetchTasks();
            GenerateLabels(labelStartx, labelStartY);
            HideContent();
            UpdateInfo();
        }

        public void UpdateInfo() {
            StringBuilder sb = new StringBuilder();
            string name;
            string surname;
            string city;

            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon) {
                SqlCommand command = new SqlCommand("SELECT * FROM USERS");
                command.Connection = dbCon;

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                try {
                    name = reader.GetString(1);
                    surname = reader.GetString(2);
                    city = reader.GetString(5).ToLower();
                } catch (Exception e) {
                    name = "";
                    surname = "";
                    city = "sofia";  // default city value
                }
            }

            dbCon.Close();

            sb.Append(name + " ");
            sb.Append(surname + new string(' ', 50));
            sb.Append(city + " ");
            sb.Append(GetWeather(city));

            labelInfo.Text = sb.ToString();
        }

        private void GenerateControls() {
            buttonAdd = new Button();
            buttonAdd.Click += AddClicked;
            buttonDelete = new Button();
            buttonDelete.Click += DeleteClicked;
            buttonDeleteAll = new Button();
            buttonDeleteAll.Click += DeleteAllClicked;

            textBoxInput = new TextBox();

            labelInfo = new Label();
            labelInfo.BackColor = Color.Transparent;
            labelInfo.AutoSize = true;
        }

        private void IntitializeArrays() {
            MyButtons.AddRange(new Button[] { buttonAdd, buttonDelete, buttonDeleteAll });

            MyTextBoxes.AddRange(new TextBox[] { textBoxInput });
        }

        private void SetControlsSize() {
            foreach (var textBox in MyTextBoxes) {
                textBox.Size = new Size(285, 60);
            }

            foreach (var button in MyButtons) {
                button.Size = new Size(60, 20);
            }

            labelInfo.Size = new Size(260, 20);
        }

        private void SetControlsText() {
            buttonAdd.Text = "Add";
            buttonAdd.Name = "AddButton";
            buttonDelete.Text = "Delete";
            buttonDelete.Name = "DeleteButton";
            buttonDeleteAll.Text = "Delete all";
            buttonDeleteAll.Name = "DeleteAllButton";

            textBoxInput.Name = "InputTextBox";
        }

        private void SetControlsPosition(int buttonStartX, int buttonStartY) {
            MyTextBoxes[0].Location = new Point(20, 40);

            foreach (var button in MyButtons) {
                button.Location = new Point(buttonStartX, buttonStartY);
                buttonStartX += 110;
            }

            labelInfo.Location = new Point(20, 20);
        }

        public void HideContent() {
            foreach (Label label in MyLabels) {
                label.Hide();
            }
            labelInfo.Hide();

            foreach (TextBox box in MyTextBoxes) {
                box.Hide();
            }

            foreach (Button button in MyButtons) {
                button.Hide();
            }
        }

        public void ShowContent() {
            foreach (TextBox box in MyTextBoxes) {
                box.Show();
            }
            labelInfo.Show();

            foreach (Button button in MyButtons) {
                button.Show();
            }

            foreach (Label label in MyLabels) {
                label.Show();
            }
        }

        public List<Control> getControls() {

            List<Control> controls = new List<Control>();

            controls.Add(labelInfo);
            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);

            return controls;
        }

        public void AddTask(string content) {
            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon) {
                string values = $"VALUES ('{content}')";

                SqlCommand command = new SqlCommand("INSERT INTO TASKS (Content)" + values, dbCon);
                command.Connection = dbCon;

                command.ExecuteScalar();
            }

            dbCon.Close();

            FetchTasks();

            GenerateLabels(labelStartx, labelStartY);
        }

        public string GetConnectionString(string file) {

            using (StreamReader r = new StreamReader(file)) {
                string json = r.ReadToEnd();
                con = JsonConvert.DeserializeObject<Connection>(json);
            }

            return con.ConnectionString;
        }

        public void FetchTasks() {
            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon) {
                SqlCommand command = new SqlCommand("SELECT * FROM TASKS");
                command.Connection = dbCon;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        labelNames.Add(reader.GetString(1));
                    }
                }
            }

            dbCon.Close();
        }

        private void DeleteAllTasks() {
            string message = "Are you sure?";
            string caption = "Delete all?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);


            if (result == DialogResult.Yes) {
                dbCon = new SqlConnection(connectionString);
                dbCon.Open();

                using (dbCon) {
                    SqlCommand command = new SqlCommand("DELETE FROM TASKS");
                    command.Connection = dbCon;

                    command.ExecuteScalar();
                }

                UpdateLabels();

                dbCon.Close();
            }
        }

        public void GenerateLabels(int labelX, int labelY) {
            foreach (var text in labelNames) {
                Label label = new Label();
                label.Text = text;
                label.Size = new Size(60, 20);
                label.Location = new Point(labelX, labelY);
                label.BackColor = System.Drawing.Color.Transparent;
                label.AutoSize = true;
                label.Font = new Font("Segoe UI", 20);
                label.MaximumSize = new Size(300, 0);

                labelY += 50;

                MyLabels.Add(label);

                if (!form.Controls.Contains(label)) {
                    form.Controls.Add(label);
                }
            }
        }

        public void UpdateLabels() {
            foreach (var label in MyLabels) {
                form.Controls.Remove(label);
            }
            labelNames.Clear();
            MyLabels.Clear();
        }

        private double GetWeather(string city) {
            double temp;
            string url = url1 + city + url2;

            var webRequest = WebRequest.Create(url) as HttpWebRequest;

            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "Nothing";

            using (var s = webRequest.GetResponse().GetResponseStream()) {
                using (var sr = new StreamReader(s)) {
                    string result = sr.ReadToEnd();
                    int index = result.IndexOf("temp");
                    temp = double.Parse(result.Substring(index + 6, 6));
                    temp -= 273.15; // kelvin to degrees centigrade conversion
                }
            }

            return temp;
        }

        public void AddClicked(object sender, EventArgs e) {
            UpdateLabels();
            AddTask(textBoxInput.Text.Trim());
            textBoxInput.Text = "";
        }

        public void DeleteAllClicked(object sender, EventArgs e) {
            DeleteAllTasks();
        }

        public void DeleteClicked(object sender, EventArgs e) {
            UpdateInfo();
        }
    }
}