using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class Form1 : Form
    {
        public Dictionary<int, ResultStringClass> resultTable;
        public List<ResultStringClass> displayTable = new List<ResultStringClass>();
        
        public SettingsClass settings = new SettingsClass(false,false,100,50,1,5,50,5,1,4,300,500); // default settings
        public int sortieCount = 1;
        public AutoCompleteStringCollection pilotList = new AutoCompleteStringCollection();

        public Form1()
        {
            InitializeComponent();
                      
        }

        private void button1_Click(object sender, EventArgs e) // SHOW RESULTS
        {

            using (FormShowResults showForm = new FormShowResults(displayTable,settings)) 
            {
                showForm.ShowDialog();
            }
                       

        }

        private void button2_Click(object sender, EventArgs e) // IMPORT LOGS
        {
            resultTable = new Dictionary<int, ResultStringClass>();
                        

            using (FormImportLog importForm = new FormImportLog(resultTable, sortieCount++, pilotList)) // going for new sortie log set            
            {               
                if (importForm.ShowDialog() == DialogResult.OK)
                {
                    foreach (ResultStringClass newString in resultTable.Values) // add from result table to display table
                    {
                        displayTable.Add(newString); 
                    }                        
                }

            }
        }

        private void button3_Click(object sender, EventArgs e) //LOAD
        {
            List<ResultStringClass> tmpTeamTable = new List<ResultStringClass>();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {                
                openFileDialog.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {            

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        displayTable = new List<ResultStringClass>();
                        pilotList = new AutoCompleteStringCollection();
                        string jsonString;
                        while ((jsonString = reader.ReadLine()) != null) 
                        {
                            ResultStringClass r = JsonSerializer.Deserialize<ResultStringClass>(jsonString);
                            displayTable.Add(r); // load new string to single result table
                            sortieCount = r.sortieNo + 1;

                    
                            Boolean newPilot = true;
                            foreach (string rr in pilotList) 
                            {
                                if (rr == r.name)
                                {
                                    newPilot = false;
                                }
                            }
                            if (newPilot) 
                            { 
                                pilotList.Add(r.name); // fill AutoComplete prompt table
                            } 
                        }
                        
                    }
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)  //SAVE
        {
            StreamWriter myStream;

            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {

                saveFileDialog1.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.OverwritePrompt = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = new StreamWriter(saveFileDialog1.FileName)) != null)
                    {
                        // Saving displayTable only, other info can be recovered from it

                        string jsonString;
                        foreach (ResultStringClass r in displayTable)
                        {
                            jsonString = JsonSerializer.Serialize<ResultStringClass>(r);
                            myStream.WriteLine(jsonString);
                        }
                        myStream.Close();
                    }
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
       
    }
}
