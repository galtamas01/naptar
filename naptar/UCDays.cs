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
    public partial class UCDays : UserControl
    {
        public UCDays()
        {
            InitializeComponent();
        }

        private void UCDays_Load(object sender, EventArgs e)
        {

        }

        public void dayEnumerator(int ord)
        {
            label1.Text = ord.ToString();
        }

        public string EventLabel
        {
            get { return labelEvents.Text; }
            set { labelEvents.Text = value; }
        }
    }
}
