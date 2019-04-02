using System.Collections.Generic;
using System.Windows.Forms;

namespace Calendar.Forms {
    public class Page {
        protected List<Button> myButtons; // Collection of all buttons
        protected List<TextBox> myTextBoxes; // Collection of all text boxes
        protected List<Label> myLabels; // Collection of all labels
        protected List<Button> myDelete; // Collection of all delete buttons
        protected List<string> labelNames; // Auxiliary collection to store info from DB

        public Page() {
            myButtons = new List<Button>();
            myTextBoxes = new List<TextBox>();
            myLabels = new List<Label>();
            labelNames = new List<string>();
            myDelete = new List<Button>();
        }
        public List<Button> MyButtons { get { return this.myButtons; } }
        public List<TextBox> MyTextBoxes { get { return this.myTextBoxes; } }
        public List<Label> MyLabels { get { return this.myLabels; } }
        public List<Button> MyDelete { get { return this.myDelete; } }
        public List<string> LabelNames { get { return this.labelNames; } }
    }
}
