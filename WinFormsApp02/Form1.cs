using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class Form1 : Form
    {
        #region COMMON VARS
        public Dictionary<int, ResultStringClass> resultTable;
        public List<ResultStringClass> displayTable = new List<ResultStringClass>();
        public SettingsClass settings = new SettingsClass(false,false,100,50,1,5,50,5,1,4,300,500); // default settings
        public int sortieCount = 1;
        public AutoCompleteStringCollection pilotList = new AutoCompleteStringCollection();
        #endregion

        #region MAIN FORM
        public Form1()
        {
            InitializeComponent();                  
        }
        #endregion

        #region SHOW SOLO
        private void button1_Click(object sender, EventArgs e) // SHOW SOLO RESULTS
        {
            ViewResults.ShowSolo(displayTable, settings);
        }
        #endregion

        #region SHOW TEAM
        private void button5_Click(object sender, EventArgs e) // SHOW TEAM RESULTS
        {
            ViewResults.ShowTeam(displayTable, settings);
        }
        #endregion

        #region IMPORT LOGS
        private void button2_Click(object sender, EventArgs e) // IMPORT LOGS
        {            
            using (FormImportLog importForm = new FormImportLog(displayTable, sortieCount++, pilotList)) // going for new sortie log set        
            {
                importForm.ShowDialog();              
                label14.Text = "";
                foreach (string rr in pilotList)
                {
                    label14.Text += (rr + '\n'); // and main window text
                }                                

            }
        }
        #endregion

        #region LOAD MAIN
                private void button3_Click(object sender, EventArgs e) //LOAD MAIN FILE
        {
            displayTable = new List<ResultStringClass>();
            sortieCount = FileManager.LoadMain(displayTable, pilotList);            
            
            label14.Text = "";
            foreach (string rr in pilotList)
            {
                label14.Text += (rr + '\n'); // and main window text
            }
        }
        #endregion

        #region SAVE MAIN
        private void button4_Click(object sender, EventArgs e)  //SAVE MAIN FILE
        {
            FileManager.SaveMain(displayTable);
        }
        #endregion

        #region APPLY SETTINGS
        private void button6_Click(object sender, EventArgs e) // APPLY SETTINGS
        {
            settings.airKill = Int32.Parse(maskedTextBox1.Text);
            settings.killAssist = Int32.Parse(maskedTextBox2.Text);
            settings.airHit = Int32.Parse(maskedTextBox3.Text);
            settings.groundHit = Int32.Parse(maskedTextBox4.Text);
            settings.stayAlive = Int32.Parse(maskedTextBox5.Text);
            settings.friendlyAir = Int32.Parse(maskedTextBox6.Text);
            settings.friendlyGround = Int32.Parse(maskedTextBox7.Text);
            
        }
        #endregion

        #region LOAD SETTINGS
        private void button7_Click(object sender, EventArgs e) // LOAD SETTINGS
        {
            FileManager.LoadSettings(settings);

            // now adjust form values
            
            maskedTextBox1.Text = settings.airKill.ToString();
            maskedTextBox2.Text = settings.killAssist.ToString();
            maskedTextBox3.Text = settings.airHit.ToString();
            maskedTextBox4.Text = settings.groundHit.ToString();
            maskedTextBox5.Text = settings.stayAlive.ToString();
            maskedTextBox6.Text = settings.friendlyAir.ToString();
            maskedTextBox7.Text = settings.friendlyGround.ToString();            
        }
        #endregion

        #region SAVE SETTINGS
        private void button8_Click(object sender, EventArgs e) // SAVE SETTINGS
        {
            FileManager.SaveSettings(settings);           
        }
        #endregion
    }
}
