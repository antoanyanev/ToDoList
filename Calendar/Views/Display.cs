using Calendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Calendar.Views
{
    class Display
    {
        private Login login;
        private ToDo toDo;
        private Form1 form;
        private Control loginButton;
        private List<Control> controls;


        public Display(Form1 form)
        {
            this.form = form;
            login = new Login();
            toDo = new ToDo(form);
            controls = new List<Control>();

            AddControlsToForm();
            CheckLogin();
        }

        private void AddControlsToForm()
        {
            controls = login.getControls();
            controls.AddRange(toDo.getControls());
            loginButton = controls.Where(x => x.Name == "LoginButton").First();
            loginButton.Click += LoginButtonClicked;

            form.AddControls(controls);
        }

        private void CheckLogin()
        {
            var context = new DataEntities();

            if (!context.Users.Any())
            {
                login.ShowContent();
                toDo.HideContent();
            }
            else
            {
                login.HideContent();
                toDo.ShowContent();
            }
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
