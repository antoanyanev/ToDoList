using Calendar.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Calendar.Views {
    /// <summary>
    /// This is the Display controller class.
    /// Used to display content on the screen.
    /// </summary>
    class Display {
        #region Variables

        /// <summary>
        /// Login object to access the Login class
        /// </summary>
        private Login login;
        /// <summary>
        /// ToDo object to access the ToDo class
        /// </summary>
        private ToDo toDo;
        /// <summary>
        /// Form1 object to access the form
        /// </summary>
        private Form1 form;
        /// <summary>
        /// Control object to access the login button
        /// </summary>
        private Control loginButton;
        /// <summary>
        /// The controls that will be added to the form
        /// </summary>
        private List<Control> controls;

        #endregion
        #region Methods

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="form">Formq object</param>
        public Display(Form1 form) {
            this.form = form;
            login = new Login();
            toDo = new ToDo(form);
            controls = new List<Control>();

            AddControlsToForm();
            CheckLogin();
        }

        /// <summary>
        /// Checks if there is a registered user
        /// And picks the correct start page
        /// </summary>
        private void CheckLogin() {
            if (Services.CheckLogin()) {
                login.HideContent();
                toDo.ShowContent();
            }
            else {
                login.ShowContent();
                toDo.HideContent();
            }
        }

        /// <summary>
        /// Adds all of the content from the pages to the form's controls
        /// </summary>
        private void AddControlsToForm() {
            controls = login.getControls();
            controls.AddRange(toDo.getControls());
            loginButton = controls.Where(x => x.Name == "LoginButton").First();
            loginButton.Click += LoginButtonClicked;

            form.AddControls(controls);
        }

        /// <summary>
        /// Event handler bound to the login button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event parameters</param>
        public void LoginButtonClicked(object sender, EventArgs e) {
            if (login.CreateUser()) {
                login.HideContent();
                toDo.ShowContent();
                toDo.UpdateInfo();
            }
        }

        #endregion
    }
}
