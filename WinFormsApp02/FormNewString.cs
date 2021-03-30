using System;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormNewString : Form
    {
        LogStringClass curLogString;
        AutoCompleteStringCollection pilotList;


        public FormNewString(LogStringClass logString, AutoCompleteStringCollection pilotList)
        {
            this.curLogString = logString;
            this.pilotList = pilotList;
            InitializeComponent();
            textBox1.AutoCompleteCustomSource = pilotList;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }

        

        public void button1_Click(object sender, EventArgs e) // FORM HITSFROM PAIR
        {
            
            FormNewPair testDialog = new FormNewPair();
                        
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                if (Int32.Parse(testDialog.maskedTextBox1.Text) != 0 && Int32.Parse(testDialog.maskedTextBox2.Text) != 0)
                {
                    HitsFromClass pair = new HitsFromClass(Int32.Parse(testDialog.maskedTextBox1.Text), Int32.Parse(testDialog.maskedTextBox2.Text));                                 
                    curLogString.hitsFrom.Add(pair);
                                      
                }
                else { MessageBox.Show("Void argument(s)"); }
                
            }
            else
            {
                MessageBox.Show("Discarded");
            }
            testDialog.Dispose();
        }

        private void button2_Click(object sender, EventArgs e) // SUBMIT LOG STRING
        {
            curLogString.health = Int32.Parse(maskedTextBox1.Text);
            curLogString.gunId = Int32.Parse(maskedTextBox2.Text);
            curLogString.roundsFired = Int32.Parse(maskedTextBox3.Text);
            curLogString.name = textBox1.Text;
            curLogString.team = Int32.Parse(maskedTextBox4.Text);
            curLogString.isGroundTarget = radioButton1.Checked;

            if (!pilotList.Contains(curLogString.name))
            {
                pilotList.Add(curLogString.name);
            }

            

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // NO SUBMIT UNTIL NAME ENTERED
        {   
            button2.Enabled = true;
        }
    }
}
