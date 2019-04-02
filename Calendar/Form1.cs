/*
    This is the main class of the Application.

    Written by Antoan Yanev & Vladislav Milenkov
*/

using System.Collections.Generic;
using System.Windows.Forms;
using Calendar.Views;

namespace Calendar {
    public partial class Form1 : Form {
        private Display display; // Display object to access the Display class

        public Form1() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Makes the window not resizeable
            this.AutoScroll = true; // Adds a scroll when there are too many tasks
            display = new Display(this); // Initializes the display object

        }

        public void AddControls(List<Control> controls)
        {
            // Adds content to the form's controls

            Controls.AddRange(controls.ToArray());
        }
    }
}
