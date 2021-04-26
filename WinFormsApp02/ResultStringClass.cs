using System;


namespace RCCombatCalc
{
    public class ResultStringClass // DATA WE STORE IN TABLES & FILES
    {
        public String name { get; set; }
        public int team { get; set; }
        public int sortieNo { get; set; }
        public int roundsFired { get; set; }
        public int hitsAchieved { get; set; }
        public int hitsTaken { get; set; }
        public int hitsGround { get; set; }
        public int ffAir { get; set; }
        public int ffGround { get; set; }
        public int soloKills { get; set; }
        public int killed  { get; set; }
        public int groupKills { get; set; }
        public String killedBy { get; set; }


public ResultStringClass(string name, int team, int sortieNo, int roundsFired, int hitsAchieved, int hitsTaken, int hitsGround, int ffAir, int ffGround, int soloKills, int killed, int groupKills, String killedBy)
        {
            this.name = name;
            this.team = team;
            this.sortieNo = sortieNo;
            this.roundsFired = roundsFired;
            this.hitsAchieved = hitsAchieved;
            this.hitsTaken = hitsTaken;
            this.hitsGround = hitsGround;
            this.ffAir = ffAir;
            this.ffGround = ffGround;
            this.soloKills = soloKills;
            this.killed = killed;
            this.groupKills = groupKills;

            this.killedBy = killedBy;
           
        }

        public ResultStringClass ShallowCopy()
        {
            return (ResultStringClass)this.MemberwiseClone();
        }

    }
}
