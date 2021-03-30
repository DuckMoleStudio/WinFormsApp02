using System;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormNewPair : Form
    {
        public FormNewPair()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e) // values added in calling form, FormNewString
        {  this.Close(); }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e) // to activate "Go" button
        {
            button1.Enabled = true;
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
