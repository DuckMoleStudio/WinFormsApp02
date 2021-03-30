using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormImportLog : Form
    {

        List<LogStringClass> log = new List<LogStringClass>();
        Dictionary<int, ResultStringClass> resultTable;
        int sortieNo;
        AutoCompleteStringCollection pilotList;

        public FormImportLog(Dictionary<int, ResultStringClass> resultTable, int sortieNo, AutoCompleteStringCollection pilotList)
        {
            this.resultTable = resultTable;
            this.sortieNo = sortieNo;
            this.pilotList = pilotList;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // GET NEW LOG
        {
            LogStringClass curLogString = new LogStringClass();
            
            using (FormNewString newForm = new FormNewString(curLogString, pilotList))
            {
                if (newForm.ShowDialog() == DialogResult.OK)       //to avoid empty string adding on abnormal closing              
                { log.Add(curLogString); }
                
            }

            label2.Text += ("Added log for: " + curLogString.name + ", team: " + curLogString.team.ToString() + '\n');
           
        }

        private void button2_Click(object sender, EventArgs e) // PARSE LOGS
        {
            //MAIN PARSER FOLLOWS 


            // consistency check now!
            Boolean consistency;
            Boolean needNewString = false;
            List<int> gunIdList = new List<int>();
            List<int> gunIdIgnoreList = new List<int>(); // if we need to ignore invalid IDs

            do
            {
                consistency = true;
                foreach (LogStringClass ccString in log) 
                {
                    gunIdList.Add(ccString.gunId);
                }

                foreach (LogStringClass ccString in log)
                {
                    foreach (HitsFromClass ccHitsFrom in ccString.hitsFrom)
                    {
                        if (!gunIdList.Contains(ccHitsFrom.gunId) && !gunIdIgnoreList.Contains(ccHitsFrom.gunId)) // found unknown gunId, must process
                        {
                            FormConsistency ccForm = new FormConsistency(ccHitsFrom.gunId);
                            
                            switch (ccForm.ShowDialog(this))
                            { 
                                case DialogResult.Ignore:  
                                    gunIdIgnoreList.Add(ccHitsFrom.gunId);
                                    MessageBox.Show("Ignoring GunId "+ ccHitsFrom.gunId);
                                    break;

                                case DialogResult.Retry:
                                    needNewString = true;
                                    MessageBox.Show("Import data for GunId " + ccHitsFrom.gunId);
                                    break;

                                default:
                                    MessageBox.Show("Unknown case!!"); // for smth unpredictable
                                    break;

                            }
                                                       
                            
                            ccForm.Dispose();

                            consistency = false;
                        }

                    }
                }

                if (needNewString) // new import after inconsistency encountered
                {
                    LogStringClass curLogString = new LogStringClass();
                    using (FormNewString newForm = new FormNewString(curLogString, pilotList))
                    {
                        if (newForm.ShowDialog() == DialogResult.OK)       //to avoid empty string adding on abnormal closing              
                        {
                            log.Add(curLogString);
                            needNewString = false;
                        }
                    }
                }

            } while (!consistency);

            // init table, general
            foreach (LogStringClass ccString in log)
            {
                if(!ccString.isGroundTarget)
                resultTable.Add(ccString.gunId, new ResultStringClass(ccString.name, ccString.team, sortieNo, ccString.roundsFired, 0, 0, 0, 0, 0, 0, 0, 0, "none"));
            }

                                 

            // now process main part

            foreach (LogStringClass ccString in log) 
            {
                
                
                foreach (HitsFromClass ccHitsFrom in ccString.hitsFrom)
                {

                    if (!gunIdIgnoreList.Contains(ccHitsFrom.gunId)) // only valid gunId's
                    {
                        if (!ccString.isGroundTarget) resultTable[ccString.gunId].hitsTaken += ccHitsFrom.hits;     // calculate hits taken...

                        if ((resultTable[ccHitsFrom.gunId].team != ccString.team) || (ccString.team == 0))  // different teams or no teams
                        {
                            if (ccString.isGroundTarget)
                            {
                                resultTable[ccHitsFrom.gunId].hitsGround += ccHitsFrom.hits; // correct GROUND hits (by others to current GT)
                            }
                            else
                            {
                                resultTable[ccHitsFrom.gunId].hitsAchieved += ccHitsFrom.hits; // correct AIR hits (by others to current pilot)
                            }
                        }
                        else // same team, friendly fire!
                        {
                            if (ccString.isGroundTarget)
                            {
                                resultTable[ccHitsFrom.gunId].ffGround += ccHitsFrom.hits; // friendly GROUND hits (by others to current GT)
                            }
                            else
                            {
                                resultTable[ccHitsFrom.gunId].ffAir += ccHitsFrom.hits; // friendly AIR hits (by others to current pilot)
                            }
                        }
                                                
                    }
                    else 
                    { 
                        MessageBox.Show("Ignored gunId: "+ ccHitsFrom.gunId); // was in ignore list
                    }

                }

                if (ccString.health == 0)    // was killed? assign killer(s)...
                {
                    resultTable[ccString.gunId].killed = 1;
                    resultTable[ccString.gunId].killedBy = "group";

                    foreach (HitsFromClass ccHitsFrom in ccString.hitsFrom)                      
                    {
                        if (ccHitsFrom.hits >= (resultTable[ccString.gunId].hitsTaken * 0.7)) // solo kill, hardwired criteria
                        { 
                            resultTable[ccHitsFrom.gunId].soloKills += 1;
                            resultTable[ccString.gunId].killedBy = resultTable[ccHitsFrom.gunId].name;
                        }
                        else 
                        {
                            if (ccHitsFrom.hits > (resultTable[ccString.gunId].hitsTaken * 0.3)) // group kill, hardwired criteria
                            { resultTable[ccHitsFrom.gunId].groupKills += 1; }
                        }
                    }

                }
               
                
            } 

            this.Close();
           
        }
    }
}
