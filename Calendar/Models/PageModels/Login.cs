using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Calendar.Controller;
using Calendar.Forms;

namespace Calendar {
    /// <summary>
    /// This is the first app page
    /// Use it to create your user and then login
    /// </summary>
    public class Login : Page {
        #region Variables

        /// <summary>
        /// Defines the X size of the controls
        /// </summary>
        private const int sizeX = 60;
        /// <summary>
        /// Defines the Y size of the controls
        /// </summary>
        private const int sizeY = 20;
        /// <summary>
        /// Defines the starting X point of the labels
        /// </summary>
        private const int firstLabelX = 80;
        /// <summary>
        /// Defines the starting Y point of the labels
        /// </summary>
        private const int firstLabelAndBoxY = 160;
        /// <summary>
        /// Defines the starting X point of the text boxes
        /// </summary>
        private const int firstTextBoxX = 170;

        /// <summary>
        /// NameLabel object
        /// </summary>
        private Label labelName;
        /// <summary>
        /// SurnameLabel object
        /// </summary>
        private Label labelSurname;
        /// <summary>
        /// BirthdateLabel object
        /// </summary>
        private Label labelBirthdate;
        /// <summary>
        /// GenderLabel object
        /// </summary>
        private Label labelGender;
        /// <summary>
        /// CityLabel object
        /// </summary>
        private Label labelCity;
        /// <summary>
        /// FormatLabel object
        /// </summary>
        private Label labelFormat;
        /// <summary>
        /// ErrorLabel object
        /// </summary>
        private Label labelError;
        
        /// <summary>
        /// NameTextBox object
        /// </summary>
        private TextBox textBoxName;
        /// <summary>
        /// SurnametextBox object
        /// </summary>
        private TextBox textBoxSurname;
        /// <summary>
        /// BirthdateTextBox object
        /// </summary>
        private TextBox textBoxBirthdate;
        /// <summary>
        /// GendertextBox object
        /// </summary>
        private TextBox textBoxGender;
        /// <summary>
        /// CitytextBox object
        /// </summary>
        private TextBox textBoxCity;

        /// <summary>
        /// LoginButton object
        /// </summary>
        private Button buttonLogin;

        #endregion
        #region

        /// <summary>
        /// Constructor method
        /// </summary>
        public Login() : base() {
            GenerateControls(); // Initialize all controls
            SetControlsText(); // Set the text to the controls
            IntitializeArrays(); // Add the controls to their corresponging collection
            SetControlsPosition(firstLabelX, firstLabelAndBoxY, firstTextBoxX, firstLabelAndBoxY); // Place them on the screen
            SetControlsSize(sizeX, sizeY); // Give them a size
            SetLabelsTransparency(); // Make the Labels' BG colour tranapsrent
        }

        /// <summary>
        /// Registers a valid user
        /// </summary>
        /// <returns>True or False</returns>
        public bool CreateUser() {
            List<string> info = new List<string>(MyTextBoxes.Select(x => x.Text).ToList());
            string date = Services.ReformatDate(info[2]);

            if (Services.CheckNull(info, MyLabels) && Services.CheckDate(date, MyLabels)) {
                Services.CreateUser(info);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Iterates over all controls and hides them
        /// </summary>
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

        /// <summary>
        /// Iterates over all controls and displays them
        /// </summary>
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

        /// <summary>
        ///             // Initializes all controls
        /// </summary>
        private void GenerateControls() {
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
        }

        /// <summary>
        /// Adds all controls to their corresponding collection
        /// </summary>
        private void IntitializeArrays() {
            MyButtons.Add(buttonLogin);
            MyLabels.AddRange(new Label[] { labelName, labelSurname, labelBirthdate, labelGender, labelCity, labelFormat, labelError });
            MyTextBoxes.AddRange(new TextBox[] { textBoxName, textBoxSurname, textBoxBirthdate, textBoxGender, textBoxCity });
        }

        /// <summary>
        /// Sets the position for every control element
        /// </summary>
        /// <param name="textStartX">Labels start X</param>
        /// <param name="textStartY">Labels start Y</param>
        /// <param name="boxStartX">textBoxes start X</param>
        /// <param name="boxStartY">TextBoxes start Y</param>
        private void SetControlsPosition(int textStartX, int textStartY, int boxStartX, int boxStartY) {
            for (int i = 0; i < MyLabels.Count - 2; i++) {
                MyLabels[i].Location = new Point(textStartX, textStartY);
                textStartY += 30;
            }

            foreach (var textBox in MyTextBoxes) {
                textBox.Location = new Point(boxStartX, boxStartY);
                boxStartY += 30;
            }

            MyLabels[5].Location = new Point(250, 220);  // format label
            MyLabels[6].Location = new Point(190, 345);  // error label

            MyButtons[0].Location = new Point(130, 340); // login button
        }

        /// <summary>
        /// Sets the size for all controls
        /// </summary>
        /// <param name="x">Width</param>
        /// <param name="y">Height</param>
        private void SetControlsSize(int x, int y) {
            for (int i = 0; i < MyLabels.Count; i++) {
                MyLabels[i].Size = new Size(x, y);
            }

            foreach (var textBox in MyTextBoxes) {
                textBox.Size = new Size(x, y);
            }

            MyButtons[0].Size = new Size(x, y + 5);
        }

        /// <summary>
        /// Adds text to the controls if necessary
        /// </summary>
        private void SetControlsText() {
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

        /// <summary>
        /// Makes the BG of all labels transparent
        /// </summary>
        private void SetLabelsTransparency() {
            for (int i = 0; i < MyLabels.Count; i++) {
                MyLabels[i].BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// Returns a collection of all controls to be added to the Form Controls collection
        /// </summary>
        /// <returns>List of Controls</returns>
        public List<Control> getControls() {
            List<Control> controls = new List<Control>();

            controls.AddRange(MyLabels);
            controls.AddRange(MyTextBoxes);
            controls.AddRange(MyButtons);

            return controls;
        }

        #endregion
    }
}
