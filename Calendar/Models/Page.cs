using System.Collections.Generic;
using System.Windows.Forms;

namespace Calendar.Forms {
    public class Page {
        protected List<Button> MyButtons; // Collection of all buttons
        protected List<TextBox> MyTextBoxes; // Collection of all text boxes
        protected List<Label> MyLabels; // Collection of all labels
        protected List<Button> MyDelete;
        protected List<string> labelNames; // Auxiliary collection to store info from DB

        public Page() {
            MyButtons = new List<Button>();
            MyTextBoxes = new List<TextBox>();
            MyLabels = new List<Label>();
            labelNames = new List<string>();
            MyDelete = new List<Button>();
        }
    }
}
