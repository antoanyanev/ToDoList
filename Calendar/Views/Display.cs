/*
    This is the Display controller class.
    Used to display content on the screen.
*/

using Calendar.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Calendar.Views
{
    class Display
    {
        private Login login; // Login object to access the Login class
        private ToDo toDo; // ToDo object to access the ToDo class
        private Form1 form; // Form1 object to access the form
        private Control loginButton; // Control object to access the login button
        private List<Control> controls; // The controls that will be added to the form

        public Display(Form1 form)
        {
            // Initialize all global variables //

            this.form = form;
            login = new Login();
            toDo = new ToDo(form);
            controls = new List<Control>();

            AddControlsToForm();
            CheckLogin();
        }

        private void CheckLogin()
        {
            // Checks if there is a registered user
            // And picks the correct start page

            if (Services.CheckLogin())
            {
                login.HideContent();
                toDo.ShowContent();
            }
            else
            {
                login.ShowContent();
                toDo.HideContent();
            }
        }

        private void AddControlsToForm()
        {
            // Adds all of the content from the pages to the form's controls

            controls = login.getControls();
            controls.AddRange(toDo.getControls());
            loginButton = controls.Where(x => x.Name == "LoginButton").First();
            loginButton.Click += LoginButtonClicked;

            form.AddControls(controls);
        }

        public void LoginButtonClicked(object sender, EventArgs e)
        {
            // Event handler bound to the login button

            if (login.CreateUser())
            {
                login.HideContent();
                toDo.ShowContent();
                toDo.UpdateInfo();
            }
        }
    }
}
