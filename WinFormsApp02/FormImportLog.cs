using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormImportLog : Form
    {

        
        int sortieNo;
        AutoCompleteStringCollection pilotList;
        public List<ResultStringClass> displayTable;
        public Parser parser;
        



        public FormImportLog(List<ResultStringClass> displayTable, int sortieNo, AutoCompleteStringCollection pilotList)
        {
            this.displayTable = displayTable;
            this.sortieNo = sortieNo;
            this.pilotList = pilotList;
            parser = new Parser(displayTable);
            
            

            InitializeComponent();
        }

        /*private void button1_Click(object sender, EventArgs e) // GET NEW LOG (MANUAL MOCKERY)
        {
            LogStringClass curLogString = new LogStringClass();
            
            using (FormNewString newForm = new FormNewString(curLogString, pilotList))
            {
                if (newForm.ShowDialog() == DialogResult.OK)       //to avoid empty string adding on abnormal closing              
                { 
                    parser.AddString(curLogString);
                    label2.Text += ("Added log for: " + curLogString.name + ", team: " + curLogString.team.ToString() + '\n');
                }
                
            }

            
           
        }*/

        private void button2_Click(object sender, EventArgs e) // PARSE LOGS
        {

            // check consistency, if ok -- exit, if not, run ignore|add script

            List<int> consistencyList = new List<int>();
            List<int> gunIdIgnoreList = new List<int>();

            if (parser.ConsistencyCheck(consistencyList)) // if ok, parse
            {
                parser.Parse(sortieNo, gunIdIgnoreList);
                this.Close();
            }
            else // if not, deal with inconsistency (spawn a winform, show bad ID's and update ignore list)
            {
                using (FormConsistency newForm = new FormConsistency(consistencyList, gunIdIgnoreList))
                {
                    if (newForm.ShowDialog() == DialogResult.Ignore)
                    {
                        parser.Parse(sortieNo, gunIdIgnoreList);
                        this.Close();
                    }
                    else 
                    {
                        this.DialogResult = DialogResult.None; // continue import, do not close
                    }
                }
            }


        }

        private void button3_Click(object sender, EventArgs e) // CONNECTION & IMPORT
        {
            LogStringClass curLogString = new LogStringClass();

            using (FormConnection newForm = new FormConnection(curLogString, pilotList))
            {
                if (newForm.ShowDialog() == DialogResult.OK)       //to avoid empty string adding on abnormal closing              
                { 
                    parser.AddString(curLogString);
                    label2.Text += ("Added log for: " + curLogString.name + ", team: " + curLogString.team.ToString() + '\n');
                }

            }

            
        }
    }
}
