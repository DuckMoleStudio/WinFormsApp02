using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCombatCalc
{
    public static class ViewResults  // generate & show result tables
    {
               

        public static void ShowSolo(List<ResultStringClass> displayTable, SettingsClass settings) // SOLO RESULTS
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

        public static void ShowTeam(List<ResultStringClass> displayTable, SettingsClass settings) // TEAM RESULTS
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
                    displayResult.Append($"\n{"Team",-6}  {"fired",-6}  {"H+",-4}  {"H-",-4}  {"Gr",-4}  {"sK",-2}  {"gK",-2}  {"D",-2}     {"fA",-2}  {"fG",-5}  {"Points",-8}\n");
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
                    $"{rrr.ffAir,-2}  " +
                    $"{rrr.ffGround,-5}  " +
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
