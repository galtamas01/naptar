using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace naptar
{
    public partial class RemoveEventForm : Form
    {
        private Form1 parentForm;
        public RemoveEventForm(Form1 parent)
        {
            InitializeComponent();
            this.parentForm = parent;
        }

        private void dtpDelete_ValueChanged(object sender, EventArgs e)
        {
            lbDelete.Items.Clear();
            foreach (Event ev in Form1.events)
            {
                if (ev.EventDate == dtpDelete.Value.ToString("yyyy-MM-dd")) { lbDelete.Items.Add(ev.EventName); }
            }
           
            //lbDelete.Items.Clear();
            //lbDelete.Items.Add(SQLiteDataAccess.LoadEventByDay(Form1.events, dtpDelete.Value.ToString("yyyy-MM-dd")));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = lbDelete.Items.Count - 1; i >=0; i--)
            {
                
                for (int j = Form1.events.Count - 1; j >= 0; j--)
                {
                    Event ev = Form1.events[j];
                    if (ev.EventName == lbDelete.Items[i].ToString() && lbDelete.GetItemChecked(i))
                    {
                        Form1.events.Remove(ev);
                        SQLiteDataAccess.DeleteManually(ev.EventName, ev.EventDate);
                    }
                    
                }
                if (lbDelete.GetItemChecked(i)) { lbDelete.Items.RemoveAt(i); }
                
            }
            if (parentForm != null)
            {
                //MessageBox.Show("Form1 megtalalva");
                parentForm.RefreshButtonClick();
            }

        }

        private void RemoveEventForm_Load(object sender, EventArgs e)
        {
            Form1.events.Clear();
            Form1.InitializeEvents();
        }
    }
}
