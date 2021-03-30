using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace RCCombatCalc
{
    public partial class FormShowResults : Form
    {
        List<ResultStringClass> displayTable;
        SettingsClass settings;

        public FormShowResults(List<ResultStringClass> displayTable, SettingsClass settings)
        {
            this.displayTable = displayTable;
            this.settings = settings;

            InitializeComponent();

                        
        }

        private void button3_Click(object sender, EventArgs e) // LOAD SETTINGS
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
            // now adjust form values
            radioButton1.Checked = settings.usePoints;
            radioButton2.Checked = settings.winCondition;
            maskedTextBox1.Text = settings.airKill.ToString();
            maskedTextBox2.Text = settings.killAssist.ToString();
            maskedTextBox3.Text = settings.airHit.ToString();
            maskedTextBox4.Text = settings.groundHit.ToString();
            maskedTextBox8.Text = settings.stayAlive.ToString();            
            maskedTextBox6.Text = settings.friendlyAir.ToString();
            maskedTextBox7.Text = settings.friendlyGround.ToString();
            maskedTextBox9.Text = settings.condAir.ToString();
            maskedTextBox10.Text = settings.condGround.ToString();
            maskedTextBox11.Text = settings.winBonus.ToString();
        }

        private void button4_Click(object sender, EventArgs e) // SAVE SETTINGS
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

        private void button5_Click(object sender, EventArgs e) // APPLY CHANGES TO SETTINGS
        {

            settings.usePoints = radioButton1.Checked;
            settings.winCondition = radioButton2.Checked;
            settings.airKill = Int32.Parse(maskedTextBox1.Text);
            settings.killAssist = Int32.Parse(maskedTextBox2.Text);
            settings.airHit = Int32.Parse(maskedTextBox3.Text);
            settings.groundHit = Int32.Parse(maskedTextBox4.Text);
            settings.stayAlive = Int32.Parse(maskedTextBox8.Text);            
            settings.friendlyAir = Int32.Parse(maskedTextBox6.Text);
            settings.friendlyGround = Int32.Parse(maskedTextBox7.Text);
            settings.condAir = Int32.Parse(maskedTextBox9.Text);
            settings.condGround = Int32.Parse(maskedTextBox10.Text);
            settings.winBonus = Int32.Parse(maskedTextBox11.Text);
                    
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e) // enable Apply if changed
        {
            button5.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e) // DISPLAY SOLO RESULTS
        {
            int ccSortieNo = 0;
            List<ResultStringClass> displayTableTotal = new List<ResultStringClass>();

            StringBuilder displayResult = new StringBuilder("** SOLO RESULTS **"); // Output text for "Show Results"



            foreach (ResultStringClass r in displayTable) // single sorties
            {
                if (r.sortieNo != ccSortieNo)
                {
                    displayResult.Append($"\n\n{"== SORTIE"} {r.sortieNo} {" =="}");
                    displayResult.Append($"\n{"Name",-30}  {"fired",-6}  {"H+",-4}  {"H-",-4}  {"Gr",-4}  {"sK",-2}  {"gK",-2}  {"D",-2}  {"Killed by",-30} {"Points", -8}\n");
                    ccSortieNo = r.sortieNo;
                }
                displayResult.Append($"\n{r.name,-30}  {r.roundsFired,-6}  {r.hitsAchieved,-4}  {r.hitsTaken,-4}  {r.hitsGround,-4}  {r.soloKills,-2}  {r.groupKills,-2}  {r.killed,-2}  {r.killedBy,-30} {Points.CalcSolo(r, settings), -8}");
                
                Boolean newPilot = true;
                foreach (ResultStringClass rr in displayTableTotal) // combined results
                {
                    if (rr.name == r.name)
                    {
                        rr.roundsFired += r.roundsFired;
                        rr.hitsAchieved += r.hitsAchieved;
                        rr.hitsTaken += r.hitsTaken;
                        rr.hitsGround += r.hitsGround;
                        rr.soloKills += r.soloKills;
                        rr.killed += r.killed;
                        rr.groupKills += r.groupKills;
                        newPilot = false;
                    }
                }
                if (newPilot)
                {
                    displayTableTotal.Add(r.ShallowCopy()); // clone to avoid pointers from 2 tables to 1 instance, shallow will do (single level)
                }

            }

            displayResult.Append($"\n\n\n{"== TOTAL =="}\n");                      

            foreach (ResultStringClass r in displayTableTotal) // combined results
            {
                displayResult.Append($"\n{r.name,-30}  {r.roundsFired,-6}  {r.hitsAchieved,-4}  {r.hitsTaken,-4}  {r.hitsGround,-4}  {r.soloKills,-2}  {r.groupKills,-2}  {r.killed,-32}   {Points.CalcSolo(r, settings),-8}");
            }

            displayResult.Append($"\n\n\n");

            FormDisplayResults displayForm = new FormDisplayResults(displayResult);
            displayForm.Show();  // dialog unneccessary, may spawn many o'em

        }

        private void FormShowResults_Shown(object sender, EventArgs e)
        {
            
                radioButton1.Checked = settings.usePoints;
                radioButton2.Checked = settings.winCondition;
                maskedTextBox1.Text = settings.airKill.ToString();
                maskedTextBox2.Text = settings.killAssist.ToString();
                maskedTextBox3.Text = settings.airHit.ToString();
                maskedTextBox4.Text = settings.groundHit.ToString();
                maskedTextBox8.Text = settings.stayAlive.ToString();                
                maskedTextBox6.Text = settings.friendlyAir.ToString();
                maskedTextBox7.Text = settings.friendlyGround.ToString();
                maskedTextBox9.Text = settings.condAir.ToString();
                maskedTextBox10.Text = settings.condGround.ToString();
                maskedTextBox11.Text = settings.winBonus.ToString();

            

        }

        private void button2_Click(object sender, EventArgs e)      // DISPLAY TEAM RESULTS
        {
            List<ResultStringClass> displayTeamTable = new List<ResultStringClass>();
            List<ResultStringClass> displayTeamTableTotal = new List<ResultStringClass>();

            // Fill these bloody tables first

            foreach (ResultStringClass r in displayTable)
            {
                Boolean newTeamSortie = true;
                foreach (ResultStringClass rr in displayTeamTable) // sortie results, 1st bloody table
                {
                    if ((rr.team == r.team) && (r.sortieNo == rr.sortieNo)) // check by BOTH team & sortie
                    {
                        rr.roundsFired += r.roundsFired;
                        rr.hitsAchieved += r.hitsAchieved;
                        rr.hitsTaken += r.hitsTaken;
                        rr.hitsGround += r.hitsGround;
                        rr.ffAir += r.ffAir;                 // friendly fire added
                        rr.ffGround += r.ffGround;
                        rr.soloKills += r.soloKills;
                        rr.killed += r.killed;
                        rr.groupKills += r.groupKills;

                        newTeamSortie = false;
                    }
                }
                if (newTeamSortie)
                    displayTeamTable.Add(r.ShallowCopy());


                Boolean newTeamSortieTotal = true;
                foreach (ResultStringClass rr in displayTeamTableTotal) // total results, 2nd bloody table
                {
                    if (rr.team == r.team) // check by team only
                    {
                        rr.roundsFired += r.roundsFired;
                        rr.hitsAchieved += r.hitsAchieved;
                        rr.hitsTaken += r.hitsTaken;
                        rr.hitsGround += r.hitsGround;
                        rr.ffAir += r.ffAir;
                        rr.ffGround += r.ffGround;
                        rr.soloKills += r.soloKills;
                        rr.killed += r.killed;
                        rr.groupKills += r.groupKills;
                        newTeamSortieTotal = false;
                    }
                }
                if (newTeamSortieTotal)
                {
                    displayTeamTableTotal.Add(r.ShallowCopy());
                }
            }
                // now we can draw...
                
                int ccSortieNo = 0;                
                StringBuilder displayResult = new StringBuilder("** TEAM RESULTS **"); // Output text for "Show TEAM Results"
                
                foreach (ResultStringClass rrr in displayTeamTable) // single team sorties
                {
                    if (rrr.sortieNo != ccSortieNo)
                    {
                        displayResult.Append($"\n\n{"== SORTIE"} {rrr.sortieNo} {" =="}");
                        displayResult.Append($"\n{"Team",-6}  {"fired",-6}  {"H+",-4}  {"H-",-4}  {"Gr",-4}  {"sK",-2}  {"gK",-2}  {"D", -2}     {"fA",-2}  {"fG",-5}  {"Points",-8}\n");
                        ccSortieNo = rrr.sortieNo;
                    }
                    displayResult.Append($"\n" +
                        $"{rrr.team,-6}  " +
                        $"{rrr.roundsFired,-6}  " +
                        $"{rrr.hitsAchieved,-4}  " +
                        $"{rrr.hitsTaken,-4}  " +
                        $"{rrr.hitsGround,-4}  " +
                        $"{rrr.soloKills,-2}  " +
                        $"{rrr.groupKills,-2}  " +
                        $"{rrr.killed,-5}  " +
                        $"{rrr.ffAir, -2}  " +
                        $"{rrr.ffGround, -5}  " +
                        $"{Points.CalcTeam(rrr, settings),-8}");

                  

                }

                displayResult.Append($"\n\n\n{"== TOTAL =="}\n");

                foreach (ResultStringClass rrr in displayTeamTableTotal) // combined results
                {
                    displayResult.Append($"\n" +
                        $"{rrr.team,-6}  " +
                        $"{rrr.roundsFired,-6}  " +
                        $"{rrr.hitsAchieved,-4}  " +
                        $"{rrr.hitsTaken,-4}  " +
                        $"{rrr.hitsGround,-4}  " +
                        $"{rrr.soloKills,-2}  " +
                        $"{rrr.groupKills,-2}  " +
                        $"{rrr.killed,-5}  " +
                        $"{rrr.ffAir,-2}  " +
                        $"{rrr.ffGround,-5}  " +
                        $"{Points.CalcTeam(rrr, settings),-8}");
                    
                }

                displayResult.Append($"\n\n\n");

                FormDisplayResults displayForm = new FormDisplayResults(displayResult);
                displayForm.Show();  // dialog unneccessary, may spawn many o'em                                 

    }
    }
}
