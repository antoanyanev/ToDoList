using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Calendar.Controller;

namespace Calendar.Forms {
    public class Page {
        protected SqlConnection dbCon; // DB connection variable
        protected Connection con; // DB connection type to get the DB connection string
        protected string connectionString = Services.GetConnectionString("Connection.json"); // DB connection string after parsing

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
