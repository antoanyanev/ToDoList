using Calendar.Controller;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar.Views
{
    class Display
    {
        private Login login;
        private ToDo toDo;
        private SqlConnection dbCon;
        private Form1 form;
        private Control loginButton;
        private List<Control> controls;


        public Display(Form1 form)
        {
            this.form = form;
            login = new Login();
            toDo = new ToDo(form);
            controls = new List<Control>();
            dbCon = new SqlConnection(Services.GetConnectionString("Connection.json"));

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
            dbCon.Open();
            string result = "";
            using (dbCon)
            {
                SqlCommand command = new SqlCommand("SELECT * FROM USERS");
                command.Connection = dbCon;
                try
                {
                    result = command.ExecuteScalar().ToString();
                }
                catch
                {
                    login.ShowContent();
                    toDo.HideContent();

                    return;
                }
            }

            login.HideContent();
            toDo.ShowContent();
        }

        public void LoginButtonClicked(object sender, EventArgs e)
        {
            // Event handler bound to the login button

            login.CreateUser();
            login.HideContent();
            toDo.ShowContent();
            toDo.UpdateInfo();
        }
    }
}
