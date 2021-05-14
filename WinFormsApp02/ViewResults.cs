using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCombatCalc
{
    public static class ViewResults  // generate & show result tables
    {

        #region SHOW SOLO
        public static void ShowSolo(List<ResultStringClass> displayTable, SettingsClass settings) // SOLO RESULTS
        {


        int ccSortieNo = 0;
        List<ResultStringClass> displayTableTotal = new();

        StringBuilder displayResult = new StringBuilder("** SOLO RESULTS **"); // Output text for "Show Results"



            foreach (ResultStringClass r in displayTable) // single sorties
            {
                if (r.sortieNo != ccSortieNo) // make header for new sortie
                {
                    displayResult.Append($"\n\n{"== SORTIE"} {r.sortieNo} {" =="}");
                    displayResult.Append($"\n{"Name",-30}  " +
                        $"{"fired",-6}  " +
                        $"{"H+",-4}  " +
                        $"{"H-",-4}  " +
                        $"{"Gr",-4}  " +
                        $"{"sK",-2}  " +
                        $"{"gK",-2}  " +
                        $"{"D",-2}     " +
                        $"{"fA",-2}  " +
                        $"{"fG",-5}  " +
                        $"{"Killed by",-30} " +
                        $"{"Points", -8}\n");
                    
                    ccSortieNo = r.sortieNo;
                }

               
                AppendStringSolo(displayResult, r, settings);

                Boolean newPilot = true;
                foreach (ResultStringClass rr in displayTableTotal) // combined results
                {
                    if (rr.name == r.name)
                    {
                        Accumulate(rr, r);
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
                AppendStringSolo(displayResult, r, settings);    
            }

            displayResult.Append($"\n\n\n");
            FormDisplayResults displayForm = new FormDisplayResults(displayResult);
            displayForm.Show();  // dialog unneccessary, may spawn many o'em
                                 
        }
        #endregion

        #region SHOW TEAM
        public static void ShowTeam(List<ResultStringClass> displayTable, SettingsClass settings) // TEAM RESULTS
        {
            List<ResultStringClass> displayTeamTable = new();
            List<ResultStringClass> displayTeamTableTotal = new();

            // Fill these bloody tables first

            foreach (ResultStringClass r in displayTable)
            {
                Boolean newTeamSortie = true;
                foreach (ResultStringClass rr in displayTeamTable) // sortie results, 1st bloody table
                {
                    if ((rr.team == r.team) && (r.sortieNo == rr.sortieNo)) // check by BOTH team & sortie
                    {
                        Accumulate(rr, r);
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
                        Accumulate(rr, r);
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
                if (rrr.sortieNo != ccSortieNo) // make TEAM header for new sortie
                {
                    displayResult.Append($"\n\n{"== SORTIE"} {rrr.sortieNo} {" =="}");
                    displayResult.Append($"\n{"Team",-6}  " +
                        $"{"fired",-6}  " +
                        $"{"H+",-4}  " +
                        $"{"H-",-4}  " +
                        $"{"Gr",-4}  " +
                        $"{"sK",-2}  " +
                        $"{"gK",-2}  " +
                        $"{"D",-2}     " +
                        $"{"fA",-2}  " +
                        $"{"fG",-5}  " +
                        $"{"Points",-8}\n");
                    ccSortieNo = rrr.sortieNo;
                }

                AppendStringTeam(displayResult, rrr, settings);
            }

            displayResult.Append($"\n\n\n{"== TOTAL =="}\n");

            foreach (ResultStringClass rrr in displayTeamTableTotal) // combined results
            {
                AppendStringTeam(displayResult, rrr, settings);
            }

            displayResult.Append($"\n\n\n");

            FormDisplayResults displayForm = new FormDisplayResults(displayResult);
            displayForm.Show();  // dialog unneccessary, may spawn many o'em           
        }

        #endregion

        public static void Accumulate(ResultStringClass rAccumulate, ResultStringClass rIncrement) // FOR COMBINED TABLES
        {
            rAccumulate.roundsFired += rIncrement.roundsFired;
            rAccumulate.hitsAchieved += rIncrement.hitsAchieved;
            rAccumulate.hitsTaken += rIncrement.hitsTaken;
            rAccumulate.hitsGround += rIncrement.hitsGround;
            rAccumulate.ffAir += rIncrement.ffAir;
            rAccumulate.ffGround += rIncrement.ffGround;
            rAccumulate.soloKills += rIncrement.soloKills;
            rAccumulate.killed += rIncrement.killed;
            rAccumulate.groupKills += rIncrement.groupKills;
        }

        public static void AppendStringSolo(StringBuilder result, ResultStringClass r, SettingsClass settings) // ADD DISPLAY STRING FOR SOLO
        {
            result.Append($"\n{r.name,-30}  " +
                $"{r.roundsFired,-6}  " +
                $"{r.hitsAchieved,-4}  " +
                $"{r.hitsTaken,-4}  " +
                $"{r.hitsGround,-4}  " +
                $"{r.soloKills,-2}  " +
                $"{r.groupKills,-2}  " +
                $"{r.killed,-2}     " +
                $"{r.ffAir,-2}  " +
                $"{r.ffGround,-5}  " +
                $"{r.killedBy,-30} " +
                $"{CalculatePoints(r, settings),-8}");

        }

        public static void AppendStringTeam(StringBuilder result, ResultStringClass r, SettingsClass settings) // ADD DISPLAY STRING FOR SOLO
        {
            result.Append($"\n" +
                    $"{r.team,-6}  " +
                    $"{r.roundsFired,-6}  " +
                    $"{r.hitsAchieved,-4}  " +
                    $"{r.hitsTaken,-4}  " +
                    $"{r.hitsGround,-4}  " +
                    $"{r.soloKills,-2}  " +
                    $"{r.groupKills,-2}  " +
                    $"{r.killed,-5}  " +
                    $"{r.ffAir,-2}  " +
                    $"{r.ffGround,-5}  " +
                    $"{CalculatePoints(r, settings),-8}");
        }

        public static int CalculatePoints(ResultStringClass r, SettingsClass s) // CALCULATE POINTS 
        {
            int points = 0;
            points += r.soloKills * s.airKill;
            points += r.groupKills * s.killAssist;
            points += r.hitsAchieved * s.airHit;
            points += r.hitsGround * s.groundHit;
            if (r.killed == 0) points += s.stayAlive;
            points -= r.ffAir * s.friendlyAir;
            points -= r.ffGround * s.friendlyGround;

            return points;
        }
    }
}
