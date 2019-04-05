using System.Collections.Generic;
using System.Windows.Forms;
using ToDo.Views;

namespace ToDo {
    /// <summary>
    /// This is the main class of the Application
    /// Written by Antoan Yanev & Vladislav Milenkov
    /// </summary>
    public partial class Form1 : Form {
        #region Variables

        /// <summary>
        /// Display object to access the Display class
        /// </summary>
        private Display display;

        #endregion
        #region Methods

        /// <summary>
        /// Constructor method
        /// </summary>
        public Form1() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Makes the window not resizeable
            this.AutoScroll = true; // Adds a scroll when there are too many tasks
            display = new Display(this); // Initializes the display object
        }

        /// <summary>
        /// Adds content to the form's controls
        /// </summary>
        /// <param name="controls">A List of Controls</param>
        public void AddControls(List<Control> controls) {
            Controls.AddRange(controls.ToArray());
        }

        #endregion

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }
    }
}
