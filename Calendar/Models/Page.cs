using System.Collections.Generic;
using System.Windows.Forms;

namespace Calendar.Forms {
    /// <summary>
    /// Page parent class
    /// </summary>
    public class Page {
        #region Variables

        /// <summary>
        /// Collection of all buttons
        /// </summary>
        protected List<Button> myButtons;
        /// <summary>
        /// Collection of all text boxes
        /// </summary>
        protected List<TextBox> myTextBoxes;
        /// <summary>
        /// Collection of all labels
        /// </summary>
        protected List<Label> myLabels;
        /// <summary>
        /// Collection of all delete buttons
        /// </summary>
        protected List<Button> myDelete;
        /// <summary>
        /// Auxiliary collection to store info from DB
        /// </summary>
        protected List<string> labelNames;

        #endregion
        #region Properties

        /// <summary>
        /// MyButtons property
        /// </summary>
        public List<Button> MyButtons { get { return this.myButtons; } }
        /// <summary>
        /// MyTextBoxes property
        /// </summary>
        public List<TextBox> MyTextBoxes { get { return this.myTextBoxes; } }
        /// <summary>
        /// MyLabels property
        /// </summary>
        public List<Label> MyLabels { get { return this.myLabels; } }
        /// <summary>
        /// MyDelete property
        /// </summary>
        public List<Button> MyDelete { get { return this.myDelete; } }
        /// <summary>
        /// LabelNames property
        /// </summary>
        public List<string> LabelNames { get { return this.labelNames; } }

        #endregion
        #region Methods

        /// <summary>
        /// Class constructor
        /// </summary>
        public Page() {
            myButtons = new List<Button>();
            myTextBoxes = new List<TextBox>();
            myLabels = new List<Label>();
            labelNames = new List<string>();
            myDelete = new List<Button>();
        }

        #endregion
    }
}
