using System;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormConsistency : Form
    {

        int gunId;
        public FormConsistency(int gunId)
        {
            this.gunId = gunId;
            InitializeComponent();
            label1.Text = "Invalid gunId = " + gunId + " encountered!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
