using System;
using System.Text;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormDisplayResults : Form
    {
        StringBuilder displayResult;
        public FormDisplayResults(StringBuilder displayResult)
        {
            this.displayResult = displayResult;
            InitializeComponent();
            label1.Text = displayResult.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
