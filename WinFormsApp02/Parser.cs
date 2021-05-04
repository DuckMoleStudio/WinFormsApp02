using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCombatCalc
{
    public class Parser
    {
        #region VARS
        public Dictionary<int, ResultStringClass> resultTable;
        public List<LogStringClass> log;
        public List<ResultStringClass> displayTable;
        #endregion

        #region CONSTRUCTOR
        public Parser(List<ResultStringClass> displayTable) 
        {
            resultTable = new Dictionary<int, ResultStringClass>();
            log = new List<LogStringClass>();
            this.displayTable = displayTable;
        }
        #endregion

        #region ADD A STRING TO LOG (BEFORE PARSING)
        public void AddString(LogStringClass logString) // new single log string
        {
            log.Add(logString);
        }
        #endregion

        #region CONSISTENCY
        public Boolean ConsistencyCheck(List<int> consistencyList) // CHECK FOR INVALID GUN IDS IN HITSFROM, FORM A LIST
        {
            // consistency check now!
            
            Boolean consistency = true;

            List<int> gunIdList = new List<int>();


            foreach (LogStringClass ccString in log) // collect all present IDs
            {
                gunIdList.Add(ccString.gunId);
            }
            //gunIdList.AddRange(log.Select(x => x.gunId));

            foreach (LogStringClass ccString in log)
            {
                foreach (HitsFromClass ccHitsFrom in ccString.hitsFrom)
                {
                    if (!gunIdList.Contains(ccHitsFrom.gunId) && !consistencyList.Contains(ccHitsFrom.gunId)) // found unknown gunId, must process
                    {
                        consistencyList.Add(ccHitsFrom.gunId);
                        consistency = false;
                    }

                }
            }

            //List<int> allLogIds = log.SelectMany(log => log.hitsFrom, (log, hitsFrom) => hitsFrom.gunId).Distinct().ToList();
            //List<int> wrongIds = allLogIds.Except(gunIdList).ToList();
            //consistencyList.AddRange(wrongIds);
            //consistencyList = consistencyList.Distinct().ToList();
            //consistency = wrongIds.Count == 0;

            return consistency;

        }
        #endregion

        #region MAIN PARSER
        public void Parse(int sortieNo, List<int> gunIdIgnoreList) // MAIN PARSER
        {
            
            // init table, general
            foreach (LogStringClass ccString in log)
            {
                if (!ccString.isGroundTarget)
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
                    

                }

                if (ccString.health == 0)    // was killed? assign killer(s)...
                {
                    resultTable[ccString.gunId].killed = 1;
                    resultTable[ccString.gunId].killedBy = "group";

                    foreach (HitsFromClass ccHitsFrom in ccString.hitsFrom)
                    {
                        if (ccHitsFrom.hits >= (resultTable[ccString.gunId].hitsTaken * 0.7) && !gunIdIgnoreList.Contains(ccHitsFrom.gunId)) // solo kill, hardwired criteria
                        {
                            resultTable[ccHitsFrom.gunId].soloKills += 1;
                            resultTable[ccString.gunId].killedBy = resultTable[ccHitsFrom.gunId].name;
                        }
                        else
                        {
                            if (ccHitsFrom.hits > (resultTable[ccString.gunId].hitsTaken * 0.3) && !gunIdIgnoreList.Contains(ccHitsFrom.gunId)) // group kill, hardwired criteria
                            { resultTable[ccHitsFrom.gunId].groupKills += 1; }
                        }
                    }
                }
            }

            foreach (ResultStringClass newString in resultTable.Values) // add from result table to display table 
            {
                displayTable.Add(newString);
            }
            //displayTable.AddRange(resultTable.Values);

        }
        #endregion

    }
}
