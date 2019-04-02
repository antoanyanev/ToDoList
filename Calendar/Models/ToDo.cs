/*
    This is the second and main page of the app
    Use it to store, add and delete your taks
    The top label contains a greeting message and basic weather info
*/

using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Threading;
using Calendar.Controller;
using Calendar.Forms;

namespace Calendar {
    public class ToDo : Page {
        private Form1 form; // Form1 object to access the form
        private int labelStartX = 20; // Global constant for the beginning X point of the tasks labels
        private int labelStartY = 90; // Global constant for the beginning Y point of the tasks labels

        // Buttons //

        private Button buttonAdd;
        private Button buttonDeleteAll;

        // TextBoxes //

        private TextBox textBoxInput;

        // Labels //

        private Label labelInfo;

        public ToDo(Form1 form) : base() {
            // Intialize global variables //

            this.form = form;

            GenerateControls(); // Create all controls
            IntitializeArrays(); // Put them into their corresponding collection
            SetControlsSize(); // Set their size
            SetControlsText(); // Add text if needed
            SetControlsPosition(52, 70); // Place them on the screen

            Console.WriteLine(MyTextBoxes[0].Size);

            FetchTasks(); // Retrieve all tasks from DB
            GenerateLabelsAndButtons(); // Generate the necessary labels;
            HideContent(); // Hide all controls until needed
            UpdateInfo(); // Update the top page label
            RepeatUpdate(); // Updates the clock and weather every minute 

        }

        public void UpdateInfo() {
            // Generates the message for the info label (top page label)

            // Make sure there's a valid login in the Users table
            // Otherwise, use the default values

            var user = Services.GetUser();

            StringBuilder sb = new StringBuilder();
            string name ;
            string surname;
            string city;

            try
            {
                name = user.Name;
                surname = user.Surname;
                city = user.City;
            }
            catch
            {
                name = "";
                surname = "";
                city = "sofia";
            }          

            // Put toghether separate parts of the message

            sb.Append(Services.GenerateGreeting(Services.GetTime()));
            sb.Append(name + " ");
            sb.Append(surname + " ");
            sb.Append(Services.GetTime() + new string(' ', 20));
            sb.Append(city + " ");
            sb.Append(Services.GetWeather(city) + "°C");

            // Update the message

            labelInfo.Text = sb.ToString();
        }

        public void HideContent() {
            // Iterates through all available controls and Hides them

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

            foreach (Button button in MyDelete) {
                button.Hide();
            }
        }

        public void ShowContent() {
            // Iterates through all available controls and displays them

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

            foreach (Button button in MyDelete) {
                button.Show();
            }
        }

        public void AddTask(string content) {
            // Adds a new task to the dashboard

            Services.AddTask(content);

            // Update the local collection of tasks and display them again

            FetchTasks();
            GenerateLabelsAndButtons();
        }

        public void FetchTasks() {
            // Retrieves all tasks from the DB table
            var tasks = Services.GetAllTasks();

            for (int i = 0; i < tasks.Count; i++)
            {
                labelNames.Add(tasks[i].Content);
            }
            
        }

        private void DeleteAllTasks() {
            // Deletes all available tasks
            // Opens a warning message box

            // Setup parameters for message box

            string message = "Are you sure?";
            string caption = "Delete all?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Open message box with the parameters 

            result = MessageBox.Show(message, caption, buttons);

            // Only execute deletion if yes has been selected

            if (result == DialogResult.Yes) {

                Services.DeleteAllTasks();

                UpdateLabelsAndButtons();
            }
        }

        private void DeleteTask(string content) {
            // Deletes a task from the DB based on it's content

            Services.DeleteTask(content);
            
            // Refresh Tasks

            UpdateLabelsAndButtons();
            FetchTasks();
            GenerateLabelsAndButtons();

        }

        public void GenerateLabelsAndButtons() {
            // Generates label objects based on the values returned from the Fetchtasks() values stored in labelNames
            // labelX -> Defines the starting point of the labels on the X axis
            // labelY -> Defines the starting point of the labels on the Y axis

            foreach (var text in labelNames) {
                Label label = new Label();
                label.Text = text;
                label.Size = new Size(60, 20);
                label.Location = new Point(labelStartX, labelStartY);
                label.BackColor = System.Drawing.Color.Transparent;
                label.AutoSize = true;
                label.Font = new Font("Segoe UI", 20);
                label.MaximumSize = new Size(260, 0);
                label.Click += DeleteClicked;

                Button delete = new Button();
                delete.Size = new Size(32, 32);
                delete.Location = new Point(labelStartX + 260, labelStartY + 5);
                delete.Name = text;
                delete.Image = Image.FromFile("trash.png");
                delete.Click += DeleteClicked;

                labelStartY += 50;

                // Add the generated item to the actual label collection

                MyLabels.Add(label);
                MyDelete.Add(delete);

                form.Controls.Add(label);
                form.Controls.Add(delete);
            }

            labelStartY = 90;
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
                textBox.Size = new Size(285, 20);
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

            // Text Box //

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

        public List<Control> getControls() {
            // Returns a list of all controls
            // The return values is passed to the Form Controls array

            List<Control> controls = new List<Control>();

            controls.Add(labelInfo);
            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);
 
            return controls;
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