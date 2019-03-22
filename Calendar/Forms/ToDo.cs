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
        private SqlConnection dbCon;
        private Connection con;
        private string connectionString;

        private List<Button> MyButtons;
        private List<TextBox> MyTextBoxes;
        private List<Label> MyLabels;

        // Buttons //

        private Button buttonAdd;
        private Button buttonDelete;
        private Button buttonDeleteAll;

        // TextBoxes //

        private TextBox textBoxInput;
        

        public ToDo() {
            MyButtons = new List<Button>();
            MyTextBoxes = new List<TextBox>();

            connectionString = GetConnectionString("Connection.json");
            dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            GenerateControls();
            IntitializeArrays();
            SetControlsSize();
            SetControlsText();
            SetControlsPosition(22, 40);
            HideContent();
        }

        private void GenerateControls()
        {
            buttonAdd = new Button();
            buttonAdd.Click += AddClicked;
            buttonDelete = new Button();
            buttonDeleteAll = new Button();

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
            using (dbCon)
            {
                string values = $"VALUES ('{content}')";

                SqlCommand command = new SqlCommand("INSERT INTO TASKS (Content)" + values, dbCon);
                command.ExecuteScalar();
            }
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

        public void AddClicked(object sender, EventArgs e)
        {
            AddTask(textBoxInput.Text.Trim());
        }
    }
}