using System;
using System.Collections.Generic;


namespace RCCombatCalc
{
    public class LogStringClass : IEquatable<LogStringClass>
    // DATA WE ACQUIRE FROM BF SYSTEM AND PARSE TO ResultStringClass
    {
        public String name;
        public int team;
        public int roundsFired;
        public Boolean isGroundTarget;
        public int health;

        public int gunId;
        public List<HitsFromClass> hitsFrom;

        public LogStringClass() 
        {
            this.hitsFrom = new List<HitsFromClass>();

        }

        public LogStringClass(String name, int team, int roundsFired, Boolean isGroundTarget, int health, int gunId, List<HitsFromClass> hitsFrom) 
        {
            this.name = name;
            this.team = team;
            this.roundsFired = roundsFired;
            this.isGroundTarget = isGroundTarget;
            this.health = health;
            this.gunId = gunId;
            this.hitsFrom = hitsFrom;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LogStringClass);
        }

        public bool Equals(LogStringClass other)
        {
            
           return other != null &&
           name == other.name &&
           team == other.team &&
           roundsFired == other.roundsFired &&
           isGroundTarget == other.isGroundTarget &&
           health == other.health &&
           gunId == other.gunId &&
           EqualityComparer<List<HitsFromClass>>.Default.Equals(hitsFrom, other.hitsFrom);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, team, roundsFired, isGroundTarget, health, gunId, hitsFrom);
        }
    }
}
