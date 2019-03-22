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
using System.Collections.Generic;
using System;

namespace Calendar {
    public class ToDo {
        private Form1 form;
        private const int labelStartx = 20;
        private const int labelStartY = 60;

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
            SetControlsPosition(22, 40);
            HideContent();

            FetchTasks();

            GenerateLabels(labelStartx, labelStartY);
        }

        private void GenerateControls()
        {
            buttonAdd = new Button();
            buttonAdd.Click += AddClicked;
            buttonDelete = new Button();
            buttonDeleteAll = new Button();
            buttonDeleteAll.Click += DeleteAllClicked;

            textBoxInput = new TextBox();
        }

        private void IntitializeArrays()
        {
            MyButtons.AddRange(new Button[] { buttonAdd, buttonDelete, buttonDeleteAll});

            MyTextBoxes.AddRange(new TextBox[] { textBoxInput });
        }

        private void SetControlsSize()
        {
            foreach (var textBox in MyTextBoxes)
            {
                textBox.Size = new Size(285, 60);
            }

            foreach(var button in MyButtons)
            {
                button.Size = new Size(60, 20);
            }
        }

        private void SetControlsText()
        {
            buttonAdd.Text = "Add";
            buttonAdd.Name = "AddButton";
            buttonDelete.Text = "Delete";
            buttonDelete.Name = "DeleteButton";
            buttonDeleteAll.Text = "Delete all";
            buttonDeleteAll.Name = "DeleteAllButton";

            textBoxInput.Name = "InputTextBox";
        }

        private void SetControlsPosition(int buttonStartX, int buttonStartY)
        {
            MyTextBoxes[0].Location = new Point(20, 10);

            foreach (var button in MyButtons)
            {
                button.Location = new Point(buttonStartX, buttonStartY);
                buttonStartX += 110;
            }
        }

        public void HideContent()
        {
            foreach (TextBox box in MyTextBoxes)
            {
                box.Hide();
            }

            foreach (Button button in MyButtons)
            {
                button.Hide();
            }
        }

        public void ShowContent()
        {
            foreach (TextBox box in MyTextBoxes)
            {
                box.Show();
            }

            foreach (Button button in MyButtons)
            {
                button.Show();
            }
        }

        public List<Control> getControls()
        {

            List<Control> controls = new List<Control>();

            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);

            return controls;
        }

        public void AddTask(string content)
        {
            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon)
            {
                string values = $"VALUES ('{content}')";

                SqlCommand command = new SqlCommand("INSERT INTO TASKS (Content)" + values, dbCon);
                command.Connection = dbCon;

                command.ExecuteScalar();
            }

            dbCon.Close();

            FetchTasks();

            GenerateLabels(labelStartx, labelStartY);
        }

        public string GetConnectionString(string file)
        {

            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                con = JsonConvert.DeserializeObject<Connection>(json);
            }

            return con.ConnectionString;
        }

        public void FetchTasks()
        {
            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            using (dbCon)
            {
                SqlCommand command = new SqlCommand("SELECT * FROM TASKS");
                command.Connection = dbCon;

                SqlDataReader reader = command.ExecuteReader();               

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        labelNames.Add(reader.GetString(1));
                    }
                }
            }

            dbCon.Close();
        }

        public void AddClicked(object sender, EventArgs e)
        {
            UpdateLabels();
            AddTask(textBoxInput.Text.Trim());
            textBoxInput.Text = "";
        }

        public void DeleteAllClicked(object sender, EventArgs e)
        {
            DeleteAllTasks();
        }

        private void DeleteAllTasks()
        {
            string message = "Are you sure?";
            string caption = "Delete all?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);
            

            if(result == DialogResult.Yes) {
                dbCon = new SqlConnection(connectionString);
                dbCon.Open();

                using (dbCon)
                {
                    SqlCommand command = new SqlCommand("DELETE FROM TASKS");
                    command.Connection = dbCon;

                    command.ExecuteScalar();
                }

                UpdateLabels();

                dbCon.Close();
            }
        }

        public void GenerateLabels(int labelX, int labelY)
        {
            foreach (var text in labelNames)
            {
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

            Console.WriteLine(string.Join(", ", MyLabels));
        }

        public void UpdateLabels()
        {
            foreach (var label in MyLabels)
            {
                form.Controls.Remove(label);
            }
            labelNames.Clear();
            MyLabels.Clear();
        }
    }
}