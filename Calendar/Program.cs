using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDo {
    static class Program {
        /// <summary>C:\School\ToDoList\Calendar\Program.cs
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
