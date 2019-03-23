/*
    This is the second and main page of the app
    Use it to store, add and delete your taks
    The top label contains a greeting message and basic weather info

    Written by Antoan Yanev & Vladislav Milenkov
*/

using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Net;
using System.Threading;

namespace Calendar {
    public class ToDo {
        private Form1 form; // Form1 object to access the 
        private const int labelStartx = 20; // Global constant for the beginning X point of the tasks labels
        private const int labelStartY = 90; // Global constant for the beginning Y point of the tasks labels
        private string url1 = "http://api.openweathermap.org/data/2.5/weather?q="; // First part of the Weather API URL
        private string url2 = "&appid=3d5632822352c9cd93370a8212356d3f"; // Second part of the Weather API URL, split to add city parameter

        private SqlConnection dbCon; // DB connection variable
        private Connection con; // DB connection type to get the DB connection string
        private string connectionString; // DB connection string after parsing

        private List<Button> MyButtons; // Collection of all buttons
        private List<TextBox> MyTextBoxes; // Collection of all text boxes
        private List<Label> MyLabels; // Collection of all labels
        private List<Button> MyDelete;
        private List<string> labelNames; // Auxiliary collection to store info from DB

        // Buttons //

        private Button buttonAdd;
        private Button buttonDeleteAll;

        // TextBoxes //

        private TextBox textBoxInput;

        // Labels //

        private Label labelInfo;

        public ToDo(Form1 form) {
            // Intialize global variables

            this.form = form;

            MyButtons = new List<Button>();
            MyTextBoxes = new List<TextBox>();
            MyLabels = new List<Label>();
            labelNames = new List<string>();
            MyDelete = new List<Button>();

            connectionString = GetConnectionString("Connection.json");

            GenerateControls(); // Create all controls
            IntitializeArrays(); // Put them into their corresponding collection
            SetControlsSize(); // Set their size
            SetControlsText(); // Add text if needed
            SetControlsPosition(52, 70); // Place them on the screen

            FetchTasks(); // Retrieve all tasks from DB
            GenerateLabelsAndButtons(labelStartx, labelStartY); // Generate the necessary labels;
            HideContent(); // Hide all controls until needed
            UpdateInfo(); // Update the top page label
            RepeatUpdate();
        }

        public void UpdateInfo() {
            // Generates the message for the info label (top page label)

            StringBuilder sb = new StringBuilder();
            string name;
            string surname;
            string city;

            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            // Get all necessary info from the DB

            using (dbCon) {
                SqlCommand command = new SqlCommand("SELECT * FROM USERS");
                command.Connection = dbCon;

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                // Make sure there's at least one valid login in the Users table
                // Otherwise, use the default values

                try {
                    name = reader.GetString(1);
                    surname = reader.GetString(2);
                    city = reader.GetString(5).ToLower();
                }
                catch (Exception e) {
                    name = "";
                    surname = "";
                    city = "sofia";  // default city value
                }
            }

            // Close the DB connection after use 

            dbCon.Close();

            // Put toghether separate parts of the message
            sb.Append(GenerateGreeting());
            sb.Append(name + " ");
            sb.Append(surname + " ");
            sb.Append(GetTime() + new string(' ', 20));
            sb.Append(city + " ");
            sb.Append(GetWeather(city) + "°C");

            // Update the message

            labelInfo.Text = sb.ToString();
        }

        public void HideContent() {
            // Iterates through all available controls and Hides them
            // Used when switching pages

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
            // Iterates through all available controls and displays them
            // Used when switcing pages

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

        public void AddTask(string content) {
            // Adds a new task to the dashboard

            // Open a connection to DB to insert the new item

            if (content != String.Empty) {
                dbCon = new SqlConnection(connectionString);
                dbCon.Open();

                using (dbCon) {
                    string values = $"VALUES ('{content}')";

                    SqlCommand command = new SqlCommand("INSERT INTO TASKS (Content)" + values, dbCon);
                    command.Connection = dbCon;

                    command.ExecuteScalar();
                }

                // Close the connection

                dbCon.Close();
            }
            // Update the local collection of tasks and display them again

            FetchTasks();
            GenerateLabelsAndButtons(labelStartx, labelStartY);

        }

        public void FetchTasks() {
            // Retrieves all tasks from the DB table

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

        public void GenerateLabelsAndButtons(int labelX, int labelY) {
            // Generates label objects based on the values returned from the Fetchtasks() values stored in labelNames
            // labelX -> Defines the starting point of the labels on the X axis
            // labelY -> Defines the starting point of the labels on the Y axis

            foreach (var text in labelNames) {
                Label label = new Label();
                label.Text = text;
                label.Size = new Size(60, 20);
                label.Location = new Point(labelX, labelY);
                label.BackColor = System.Drawing.Color.Transparent;
                label.AutoSize = true;
                label.Font = new Font("Segoe UI", 20);
                label.MaximumSize = new Size(260, 0);
                label.Click += DeleteClicked;

                Button delete = new Button();
                delete.Size = new Size(32, 32);
                delete.Location = new Point(labelX + 260, labelY + 5);
                delete.Name = text;
                delete.Image = Image.FromFile("trash.png");
                delete.Click += DeleteClicked;

                labelY += 50;

                // Add the generated item to the actual label collection

                MyLabels.Add(label);
                MyDelete.Add(delete);

                if (!form.Controls.Contains(label)) {
                    form.Controls.Add(label);
                    form.Controls.Add(delete);
                }
            }
        }

        public void UpdateLabelsAndButtons() {
            // Cleares all stored info about the tasks so that it can be retrieved again if a change has been made

            foreach (var label in MyLabels) {
                form.Controls.Remove(label);
            }

            foreach (var button in MyDelete) {
                form.Controls.Remove(button);
            }

            labelNames.Clear();
            MyLabels.Clear();
            MyDelete.Clear();
        }

        private void GenerateControls() {
            // Separately initializing each control on this page

            buttonAdd = new Button();
            buttonAdd.Click += AddClicked;
            buttonDeleteAll = new Button();
            buttonDeleteAll.Click += DeleteAllClicked;

            textBoxInput = new TextBox();

            labelInfo = new Label();
            labelInfo.BackColor = Color.Transparent;
            labelInfo.AutoSize = true;
        }

        private void IntitializeArrays() {
            // Add all controls to separate collecton for use in other methods and easier addition to the Form's Controls list

            MyButtons.AddRange(new Button[] { buttonAdd, buttonDeleteAll });

            MyTextBoxes.AddRange(new TextBox[] { textBoxInput });
        }

        private void SetControlsSize() {
            // Text Boxes //

            foreach (var textBox in MyTextBoxes) {
                textBox.Size = new Size(285, 60);
            }

            // Buttons //

            foreach (var button in MyButtons) {
                button.Size = new Size(60, 20);
            }

            // Info Label //

            labelInfo.Size = new Size(260, 20);
        }

        private void SetControlsText() {
            // Buttons //

            buttonAdd.Text = "Add";
            buttonAdd.Name = "AddButton";
            buttonDeleteAll.Text = "Delete all";
            buttonDeleteAll.Name = "DeleteAllButton";

            // text Box //

            textBoxInput.Name = "InputTextBox";
        }

        private void SetControlsPosition(int buttonStartX, int buttonStartY) {
            // Text Boxes //

            MyTextBoxes[0].Location = new Point(20, 40);

            // Buttons //

            foreach (var button in MyButtons) {
                button.Location = new Point(buttonStartX, buttonStartY);
                buttonStartX += 160;
            }

            // Top Label //

            labelInfo.Location = new Point(20, 20);
        }

        private void DeleteAllTasks() {
            // Deletes all available tasks
            // Opens a warning message box

            // setup parameters for message box

            string message = "Are you sure?";
            string caption = "Delete all?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Open message box with the parameters 

            result = MessageBox.Show(message, caption, buttons);

            // Only execute deletion if yes has been selected

            if (result == DialogResult.Yes) {
                dbCon = new SqlConnection(connectionString);
                dbCon.Open();

                using (dbCon) {
                    SqlCommand command = new SqlCommand("DELETE FROM TASKS");
                    command.Connection = dbCon;

                    command.ExecuteScalar();
                }

                UpdateLabelsAndButtons();

                dbCon.Close();
            }
        }

        private void DeleteTask(string content) {
            // Deletes a task from the DB based on it's content

            // Open a DB connection
            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon) {
                SqlCommand command = new SqlCommand($"DELETE FROM TASKS WHERE CONTENT='{content}'");
                command.Connection = dbCon;

                command.ExecuteNonQuery();
            }

            // Refresh Tasks

            UpdateLabelsAndButtons();
            FetchTasks();
            GenerateLabelsAndButtons(labelStartx, labelStartY);

            // Close DB connection

            dbCon.Close();
        }

        private void RepeatUpdate() {
            // Opens a second thread to update the clock and weather

            Thread updateThread = new Thread(Update);
            updateThread.Start();
        }

        private void Update() {
            // Second thread's job

            // Get current time and synchronize to the system clock
            // Repeat every sixty seconds after the initial cycle

            int second = int.Parse(DateTime.Now.ToString("ss"));
            int startWait = (60 - second) * 1000;
            int wait;
            int i = 0;

            while (true) {
                if (i > 0) {
                    wait = 60 * 1000;
                }
                else {
                    wait = startWait;
                    i++;
                }

                UpdateInfo();
                Thread.Sleep(wait);
            }
        }

        private int GetWeather(string city) {
            // Gets the weather based on the city parameter
            // Executes a HTTP GEt request to an API (www.openweathermap.org)
            // returns the temperature value as an integer

            // Add the city parameter to the request URL

            int temp;
            string url = url1 + city + url2;

            // Initialize the request with the URL

            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "Nothing";

            // Check if the request wit hthe city is valid
            // Otherwise, use the default value of 0

            try {
                using (var s = webRequest.GetResponse().GetResponseStream()) {
                    using (var sr = new StreamReader(s)) {
                        string result = sr.ReadToEnd();
                        int index = result.IndexOf("temp");
                        temp = int.Parse(result.Substring(index + 6, 3));
                        temp -= 273; // kelvin to degrees centigrade conversion
                    }
                }
            }
            catch (Exception e) {
                temp = 0;
            }

            return temp;
        }

        private string GetTime() {
            // Returns the time now {HH:mm}

            return DateTime.Now.ToString("HH:mm");
        }

        private string GenerateGreeting() {
            // Generates a greeting message based on the time of day

            int hour = int.Parse(GetTime().Split(':')[0]);
            string message = "";

            if (hour >= 0 && hour <= 12) {
                message = "Good morning, ";
            }
            else if (hour > 12 && hour <= 19) {
                message = "Helo, ";
            }
            else if (hour > 19 && hour < 24) {
                message = "Good evening, ";
            }

            return message;
        }

        public List<Control> getControls() {
            // Returns a list of all controls
            // The return values is passed to the Form Controls array

            List<Control> controls = new List<Control>();

            controls.Add(labelInfo);
            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);

            return controls;
        }

        public string GetConnectionString(string file) {
            // Returns the DB connection string stored in the Connecton.json file

            using (StreamReader r = new StreamReader(file)) {
                string json = r.ReadToEnd();
                con = JsonConvert.DeserializeObject<Connection>(json);
            }

            return con.ConnectionString;
        }     

        public void AddClicked(object sender, EventArgs e) {
            // Event handler bound to the Add button

            UpdateLabelsAndButtons();
            AddTask(textBoxInput.Text.Trim());
            textBoxInput.Text = "";
        }

        public void DeleteAllClicked(object sender, EventArgs e) {
            // Event handler bound to the DeleteAll button
            DeleteAllTasks();
        }

        public void DeleteClicked(object sender, EventArgs e) {
            // Event handler bound to each delete button

            Button send = (Button)sender;
            string text = send.Name;

            DeleteTask(text);
        }
    }
}