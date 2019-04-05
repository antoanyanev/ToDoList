using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Threading;
using ToDo.Controller;
using ToDo.Forms;

namespace ToDo {
    /// <summary>
    /// This is the second and main page of the app
    /// Use it to store, add and delete your taks
    /// The top label contains a greeting message and basic weather info
    /// </summary>
    public class ToDo : Page {
        #region Variables

        /// <summary>
        /// Form1 object to access the form
        /// </summary>
        private Form1 form;
        /// <summary>
        /// Global constant for the beginning X point of the tasks labels
        /// </summary>
        private int labelStartX = 20;
        /// <summary>
        /// Global constant for the beginning Y point of the tasks labels
        /// </summary>
        private int labelStartY = 90;

        /// <summary>
        /// AddButton object
        /// </summary>
        private Button buttonAdd;
        /// <summary>
        /// DeleteAllButton object
        /// </summary>
        private Button buttonDeleteAll;

        /// <summary>
        /// InputBox object
        /// </summary>
        private TextBox textBoxInput;

        /// <summary>
        /// InfoLabel object
        /// </summary>
        private Label labelInfo;

        #endregion
        #region Methods
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="form">Form1 object</param>
        public ToDo(Form1 form) : base() {
            this.form = form;
            GenerateControls(); // Create all controls
            IntitializeArrays(); // Put them into their corresponding collection
            SetControlsSize(); // Set their size
            SetControlsText(); // Add text if needed
            SetControlsPosition(52, 70); // Place them on the screen
            FetchTasks(); // Retrieve all tasks from DB
            GenerateLabelsAndButtons(); // Generate the necessary labels
            HideContent(); // Hide all controls until needed
            UpdateInfo(); // Update the top page label
            RepeatUpdate(); // Updates the clock and weather every minute 
        }

        /// <summary>
        /// Generates the message for the info label (top page label)
        /// </summary>
        public void UpdateInfo() {
            // Make sure there's a valid login in the Users table
            // Otherwise, use the default values

            var user = Services.GetUser();
            StringBuilder sb = new StringBuilder();
            string name;
            string surname;
            string city;

            try {
                name = user.Name;
                surname = user.Surname;
                city = user.City;
            }
            catch {
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

        /// <summary>
        /// Iterates through all available controls and Hides them
        /// </summary>
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

            foreach (Button button in MyDelete) {
                button.Hide();
            }
        }

        /// <summary>
        /// Iterates through all available controls and displays them
        /// </summary>
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

            foreach (Button button in MyDelete) {
                button.Show();
            }
        }

        /// <summary>
        /// Adds a new task to the dashboard
        /// </summary>
        /// <param name="content">Task content</param>
        public void AddTask(string content) {
            Services.AddTask(content);

            // Update the local collection of tasks and display them again

            FetchTasks();
            GenerateLabelsAndButtons();
        }

        /// <summary>
        /// Retrieves all tasks from the DB table
        /// </summary>
        public void FetchTasks() {
            var tasks = Services.GetAllTasks();

            for (int i = 0; i < tasks.Count; i++) {
                labelNames.Add(tasks[i].Content);
            }

        }

        /// <summary>
        /// // Deletes all available tasks
        /// </summary>
        private void DeleteAllTasks() {
            // Setup parameters for message box

            string message = "Are you sure?";
            string caption = "Delete all?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Open message box with the parameters 

            int taskCount = Services.GetAllTasks().Count;
            if (taskCount > 0) {
                result = MessageBox.Show(message, caption, buttons);
                if (result == DialogResult.Yes) {
                    Services.DeleteAllTasks();
                    UpdateLabelsAndButtons();
                }
            }
        }

        /// <summary>
        /// Deletes a task from the DB based on it's content
        /// </summary>
        /// <param name="content">Task content</param>
        private void DeleteTask(string content) {
            Services.DeleteTask(content);

            // Refresh Tasks

            UpdateLabelsAndButtons();
            FetchTasks();
            GenerateLabelsAndButtons();
        }

        /// <summary>
        /// Generates label objects based on the values returned from the Fetchtasks() values stored in labelNames
        /// </summary>
        public void GenerateLabelsAndButtons() {
            foreach (var text in labelNames) {
                Label label = new Label();
                label.Text = text;
                label.Size = new Size(60, 20);
                label.Location = new Point(labelStartX, labelStartY);
                label.BackColor = Color.Transparent;
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

        /// <summary>
        /// Cleares all stored info about the tasks so that it can be retrieved again if a change has been made
        /// </summary>
        public void UpdateLabelsAndButtons() {
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

        /// <summary>
        /// Separately initializing each control on this page
        /// </summary>
        private void GenerateControls() {
            buttonAdd = new Button();
            buttonAdd.Click += AddClicked;
            buttonDeleteAll = new Button();
            buttonDeleteAll.Click += DeleteAllClicked;

            textBoxInput = new TextBox();

            labelInfo = new Label();
            labelInfo.BackColor = Color.Transparent;
            labelInfo.AutoSize = true;
        }

        /// <summary>
        /// Add all controls to separate collecton for use in other methods and easier addition to the Form's Controls list
        /// </summary>
        private void IntitializeArrays() {
            MyButtons.AddRange(new Button[] { buttonAdd, buttonDeleteAll });
            MyTextBoxes.AddRange(new TextBox[] { textBoxInput });
        }

        /// <summary>
        /// Sets the size of all Controls
        /// </summary>
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

        /// <summary>
        /// Adds text to Controls
        /// </summary>
        private void SetControlsText() {
            // Buttons //

            buttonAdd.Text = "Add";
            buttonAdd.Name = "AddButton";
            buttonDeleteAll.Text = "Delete all";
            buttonDeleteAll.Name = "DeleteAllButton";

            // Text Box //

            textBoxInput.Name = "InputTextBox";
        }

        /// <summary>
        /// Sets the position of all Controls
        /// </summary>
        /// <param name="buttonStartX">Start X position of buttons</param>
        /// <param name="buttonStartY">Start Y position of buttons</param>
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

        /// <summary>
        /// Starts a second thread to update the clock and weather
        /// </summary>
        private void RepeatUpdate() {
            Thread updateThread = new Thread(Update);
            updateThread.Start();
        }

        /// <summary>
        /// Second thread's job
        /// </summary>
        private void Update() {
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

        /// <summary>
        /// Returns a list of all controls
        /// </summary>
        /// <returns>A list of Controls</returns>
        public List<Control> getControls() {
            // The return values is passed to the Form Controls array

            List<Control> controls = new List<Control>();

            controls.Add(labelInfo);
            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);

            return controls;
        }

        #endregion
        #region Events

        /// <summary>
        /// Event handler bound to the Add button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event parameters</param>
        public void AddClicked(object sender, EventArgs e) {
            UpdateLabelsAndButtons();
            AddTask(textBoxInput.Text.Trim());
            textBoxInput.Text = "";
        }

        /// <summary>
        /// Event handler bound to the DeleteAll button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event parameters</param>
        public void DeleteAllClicked(object sender, EventArgs e) {
            DeleteAllTasks();
        }

        /// <summary>
        /// Event handler bound to each delete button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event parameters</param>
        public void DeleteClicked(object sender, EventArgs e) {
            Button send = (Button)sender;
            string text = send.Name;
            DeleteTask(text);
        }

        #endregion
    }
}