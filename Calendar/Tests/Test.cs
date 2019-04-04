using Calendar.Controller;
using Calendar.Models.DataModels;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Calendar {
    [TestFixture]
    class Test {
        [TestCase]
        public void GenerateGreetingHello() {
            string result = Services.GenerateGreeting("13");
            Assert.AreEqual("Hello, ", result);
        }

        [TestCase]
        public void GenerateGreetingMorning() {
            string result = Services.GenerateGreeting("7");
            Assert.AreEqual("Good morning, ", result);
        }

        [TestCase]
        public void GenerateGreetingEvening() {
            string result = Services.GenerateGreeting("21");
            Assert.AreEqual("Good evening, ", result);
        }

        [TestCase]
        public void ReformatDate() {
            string result = Services.ReformatDate("01/04/2001");
            Assert.AreEqual("2001-04-01", result);
        }

        [TestCase]
        public void CheckDateCorrect() {
            Login login = new Login();

            bool result = Services.CheckDate("2001-03-01", login.MyLabels);

            Assert.AreEqual(true, result);
        }

        [TestCase]
        public void CheckDateIncorrect() {
            Login login = new Login();

            bool result = Services.CheckDate("200asdf1-03-01", login.MyLabels);

            Assert.AreEqual(false, result);
        }

        [TestCase]
        public void CheckNullCorrect() {
            Login login = new Login();
            List<string> inputs = new List<string>() { "Value", "Value1" };

            bool result = Services.CheckNull(inputs, login.MyLabels);

            Assert.AreEqual(true, result);
        }

        [TestCase]
        public void CheckNullIncorrect() {
            Login login = new Login();
            List<string> inputs = new List<string>() { "", "Value1" };

            bool result = Services.CheckNull(inputs, login.MyLabels);

            Assert.AreEqual(false, result);
        }

        [TestCase]
        public void AllLoginContentHidden() {
            Login login = new Login();

            login.HideContent();

            bool ok = true;

            foreach (var textBox in login.MyTextBoxes) {
                if (textBox.Visible) {
                    ok = false;
                }
            }

            foreach (var label in login.MyLabels) {
                if (label.Visible) {
                    ok = false;
                }
            }

            foreach (var button in login.MyButtons) {
                if (button.Visible) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void AllLoginContentShown() {
            Login login = new Login();

            login.ShowContent();

            bool ok = true;

            foreach (var textBox in login.MyTextBoxes) {
                if (!textBox.Visible) {
                    ok = false;
                }
            }

            foreach (var label in login.MyLabels) {
                if (!label.Visible) {
                    ok = false;
                }
            }

            foreach (var button in login.MyButtons) {
                if (!button.Visible) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void IntitializeLoginArrays() {
            Login login = new Login();
            bool ok = true;

            if (login.MyLabels.Count != 7) {
                ok = false;
            }

            if (login.MyButtons.Count != 1) {
                ok = false;
            }

            if (login.MyTextBoxes.Count != 5) {
                ok = false;
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void LoginControlsSize() {
            Login login = new Login();

            bool ok = true;

            foreach (var label in login.MyLabels) {
                if (label.Size != new Size(60, 20)) {
                    ok = false;
                }
            }

            foreach (var textBox in login.MyTextBoxes) {
                if (textBox.Size != new Size(60, 20)) {
                    ok = false;
                }
            }

            foreach (var button in login.MyButtons) {
                if (button.Size != new Size(60, 25)) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void LoginControlsText() {
            Login login = new Login();
            bool ok = true;

            List<string> text = new List<string>() { "Name", "Surname", "Birthdate", "Gender", "City", "dd/mm/yyyy", "" };

            for (int i = 0; i < login.MyLabels.Count; i++) {
                if (login.MyLabels[i].Text != text[i]) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void ControlsTransparency() {
            Login login = new Login();

            bool ok = true;

            foreach (var label in login.MyLabels) {
                if (label.BackColor != Color.Transparent) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);

        }

        [TestCase]
        public void GetLoginControls() {
            Login login = new Login();

            bool ok = true;

            List<Control> controls = login.getControls();

            if (controls.Count != 13) {
                ok = false;
            }

            Assert.AreEqual(true, ok);

        }

        [TestCase]
        public void AllToDoContentHidden() {
            ToDo todo = new ToDo(new Form1());

            todo.HideContent();

            bool ok = true;

            foreach (var textBox in todo.MyTextBoxes) {
                if (textBox.Visible) {
                    ok = false;
                }
            }

            foreach (var label in todo.MyLabels) {
                if (label.Visible) {
                    ok = false;
                }
            }

            foreach (var button in todo.MyButtons) {
                if (button.Visible) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void AllTodoContentShown() {
            ToDo todo = new ToDo(new Form1());

            todo.ShowContent();

            bool ok = true;

            foreach (var textBox in todo.MyTextBoxes) {
                if (!textBox.Visible) {
                    ok = false;
                }
            }

            foreach (var label in todo.MyLabels) {
                if (!label.Visible) {
                    ok = false;
                }
            }

            foreach (var button in todo.MyButtons) {
                if (!button.Visible) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void UpdateLabelsAndButtons() {
            ToDo todo = new ToDo(new Form1());
            bool ok = true;

            todo.UpdateLabelsAndButtons();

            if (todo.MyLabels.Count != 0 || todo.MyDelete.Count != 0 || todo.LabelNames.Count != 0) {
                ok = false;
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void IntitializeToDoArrays() {
            ToDo todo = new ToDo(new Form1());
            bool ok = true;

            if (todo.MyButtons.Count != 2) {
                ok = false;
            }

            if (todo.MyTextBoxes.Count != 1) {
                ok = false;
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void ToDoControlsSize() {
            ToDo todo = new ToDo(new Form1());

            bool ok = true;

            foreach (var textBox in todo.MyTextBoxes) {
                if (textBox.Size != new Size(285, 20)) {
                    ok = false;
                    break;
                }
            }

            foreach (var button in todo.MyButtons) {
                if (button.Size != new Size(60, 20)) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void ToDoControlsText() {
            ToDo todo = new ToDo(new Form1());
            bool ok = true;

            List<string> text = new List<string>() { "Add", "Delete all" };

            for (int i = 0; i < todo.MyButtons.Count; i++) {
                if (todo.MyButtons[i].Text != text[i]) {
                    ok = false;
                }
            }

            Assert.AreEqual(true, ok);
        }

        [TestCase]
        public void GetToDoControls() {
            ToDo todo = new ToDo(new Form1());

            bool ok = true;

            List<Control> controls = todo.getControls();

            if (controls.Count != 4) {
                ok = false;
            }

            Assert.AreEqual(true, ok);

        }

        [TestCase]
        public void CheckValidLogin() {
            var context = new UserEntity();
            var user = new User() {
                Name = "---",
                Surname = "Peshov",
                Birthdate = "04/07/2001",
                Gender = "Male",
                City = "Sofia"
            };

            context.Users.Add(user);
            context.SaveChanges();

            Assert.AreEqual(true, Services.CheckLogin());

            context.Users.Remove(context.Users.Single(t => t.Name == "---"));
            context.SaveChanges();
        }

        [TestCase]
        public void CreateUser() {
            List<string> info = new List<string>() { "---", "---", "04/07/2001", "Male", "Sofia" };
            Services.CreateUser(info);

            var context = new UserEntity();
            var user = context.Users.Where(u => u.Name == "---").First();

            Assert.AreEqual(info, new List<string>() { user.Name, user.Surname, "04/07/2001", user.Gender, user.City });

            context.Users.Remove(context.Users.Single(t => t.Name == "---"));
            context.SaveChanges();
        }

        [TestCase]
        public void GetUser() {
            var context = new UserEntity();
            var user = context.Users.FirstOrDefault<User>();

            Assert.AreEqual(user.Name, Services.GetUser().Name);
        }

        [TestCase]
        public void CreateTask() {
            string content = "task";
            Services.AddTask(content);

            var context = new TaskEntity();
            var task = context.Tasks.Where(t => t.Content == content).First();

            Assert.AreEqual(content, task.Content);

            context.Tasks.Remove(context.Tasks.Single(t => t.Content == content));
            context.SaveChanges();
        }

        [TestCase]
        public void GetAllTasks() {
            var context = new TaskEntity();
            List<Task> tasks = context.Tasks.ToList();

            Assert.AreEqual(tasks, Services.GetAllTasks().ToList());
        }

        [TestCase]
        public void DeleteAllTasks() {
            var context = new TaskEntity();
            var existing = context.Tasks.ToList();
            Services.DeleteAllTasks();

            Assert.AreEqual(false, context.Tasks.Any());

            context.Tasks.AddRange(existing);
            context.SaveChanges();
        }

        [TestCase]
        public void DeleteTask() {
            var context = new TaskEntity();
            int count = context.Tasks.Count();

            string content = "test";
            Task task = new Task() { Content = content };
            context.Tasks.Add(task);
            context.SaveChanges();

            Services.DeleteTask(content);

            Assert.AreEqual(count, context.Tasks.Count());
        }
    }
}
