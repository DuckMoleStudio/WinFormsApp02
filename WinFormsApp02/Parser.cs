﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCombatCalc
{
    public class Parser
    {
        public Dictionary<int, ResultStringClass> resultTable;
        public List<LogStringClass> log;
        public List<ResultStringClass> displayTable;

        public Parser(List<ResultStringClass> displayTable) 
        {
            resultTable = new Dictionary<int, ResultStringClass>();
            log = new List<LogStringClass>();
            this.displayTable = displayTable;

        }

        public void AddString(LogStringClass logString) // new single log string
        {
            log.Add(logString);
        }

        public List<int> Parse(int sortieNo, List<int> gunIdIgnoreList) // MAIN PARSER
        {
            // consistency check now!
            List<int> consistencyList = new List<int>();
            Boolean consistency = true;
            
            List<int> gunIdList = new List<int>();
            

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
                          consistencyList.Add(ccHitsFrom.gunId);
                          consistency = false;
                        }

                    }
                }
            if (!consistency) return consistencyList;


                
           

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

            foreach (ResultStringClass newString in resultTable.Values) // add from result table to display table 
            {
                displayTable.Add(newString);
            }
            return consistencyList;
            

        }
    
    }
}