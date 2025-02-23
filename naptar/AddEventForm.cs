using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace naptar
{
    public partial class AddEventForm : Form
    {
        private Form1 parentForm;
        public AddEventForm(Form1 parent)
        {
            InitializeComponent();
            this.parentForm = parent;
        }
        
        private void AddEventForm_Load(object sender, EventArgs e)
        {
            dtpEventDate.MinDate = DateTime.Now;
            //tbEventName.MaxLength = 8;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbEventName.Text != string.Empty)
            {
                Event esemeny = new Event();
                esemeny.EventDate = dtpEventDate.Value.ToString("yyyy-MM-dd");
                esemeny.EventTime = Convert.ToInt32(numTime.Value);
                esemeny.EventName = tbEventName.Text;

                /*Form1.events.Add(esemeny);
                SQLiteDataAccess.SaveEvent(esemeny);

                DialogResult = DialogResult.OK;*/

                try
                {
                    SQLiteDataAccess.SaveEvent(esemeny);
                    MessageBox.Show("Esemény sikeresen mentve.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt: {ex.Message}");
                }
            }
            else { MessageBox.Show("Add meg az esemény nevét!"); }

            if (parentForm != null)
            {
                //MessageBox.Show("Form1 megtalalva");
                parentForm.RefreshButtonClick();
            }
            

        }

        private void AddEventForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
