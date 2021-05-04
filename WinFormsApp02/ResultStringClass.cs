using System;


namespace RCCombatCalc
{
    public class ResultStringClass : IEquatable<ResultStringClass>
    // DATA WE STORE IN TABLES & FILES
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
        public int killed { get; set; }
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

        public override bool Equals(object obj)
        {
            return Equals(obj as ResultStringClass);
        }

        public bool Equals(ResultStringClass other)
        {
            return other != null &&
                   name == other.name &&
                   team == other.team &&
                   sortieNo == other.sortieNo &&
                   roundsFired == other.roundsFired &&
                   hitsAchieved == other.hitsAchieved &&
                   hitsTaken == other.hitsTaken &&
                   hitsGround == other.hitsGround &&
                   ffAir == other.ffAir &&
                   ffGround == other.ffGround &&
                   soloKills == other.soloKills &&
                   killed == other.killed &&
                   groupKills == other.groupKills &&
                   killedBy == other.killedBy;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(name);
            hash.Add(team);
            hash.Add(sortieNo);
            hash.Add(roundsFired);
            hash.Add(hitsAchieved);
            hash.Add(hitsTaken);
            hash.Add(hitsGround);
            hash.Add(ffAir);
            hash.Add(ffGround);
            hash.Add(soloKills);
            hash.Add(killed);
            hash.Add(groupKills);
            hash.Add(killedBy);
            return hash.ToHashCode();
        }
        

    }
}
