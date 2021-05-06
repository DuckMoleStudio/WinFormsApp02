using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public static class FileManager
    {

        #region LOAD MAIN
        public static int LoadMain(List<ResultStringClass> displayTable, AutoCompleteStringCollection pilotList) // LOAD MAIN
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                int sortieNo = 100;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();


                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        pilotList.Clear();

                        string jsonString;
                        while ((jsonString = reader.ReadLine()) != null)
                        {
                            ResultStringClass r = JsonSerializer.Deserialize<ResultStringClass>(jsonString);
                            displayTable.Add(r); // load new string to single result table
                            sortieNo = r.sortieNo + 1;                                                                                

                            if (!pilotList.Contains(r.name))
                            {
                                pilotList.Add(r.name);
                            }

                        }

                    }


                }
                return sortieNo;
            }

        }
        #endregion

        #region SAVE MAIN
        public static void SaveMain(List<ResultStringClass> displayTable)    // SAVE MAIN
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
        #endregion

        #region LOAD SETTINGS
        public static void LoadSettings(SettingsClass settings)      // LOAD SETTINGS
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "settings files (*.set)|*.set|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string jsonString;
                        SettingsClass rr;
                        if ((jsonString = reader.ReadLine()) != null)
                        {
                            rr = JsonSerializer.Deserialize<SettingsClass>(jsonString);
                            settings.usePoints = rr.usePoints;
                            settings.winCondition = rr.winCondition;
                            settings.airKill = rr.airKill;
                            settings.killAssist = rr.killAssist;
                            settings.airHit = rr.airHit;
                            settings.groundHit = rr.groundHit;
                            settings.stayAlive = rr.stayAlive;
                            settings.friendlyAir = rr.friendlyAir;
                            settings.friendlyGround = rr.friendlyGround;
                            settings.condAir = rr.condAir;
                            settings.condGround = rr.condGround;
                            settings.winBonus = rr.winBonus;
                        }
                    }
                }
            }
        }
        #endregion

        #region SAVE SETTINGS
        public static void SaveSettings(SettingsClass settings)   // SAVE SETTINGS
        {
            StreamWriter myStream;

            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {

                saveFileDialog1.Filter = "settings files (*.set)|*.set|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.OverwritePrompt = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = new StreamWriter(saveFileDialog1.FileName)) != null)
                    {
                        // Saving 

                        myStream.WriteLine(JsonSerializer.Serialize<SettingsClass>(settings));
                        myStream.Close();
                    }
                }
            }
        }
        #endregion
    }
}
