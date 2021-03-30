using System;


namespace RCCombatCalc
{
    public class SettingsClass
    {
        public Boolean usePoints { get; set; }
        public Boolean winCondition { get; set; }
        public int airKill { get; set; }
        public int killAssist { get; set; }
        public int airHit { get; set; }
        public int groundHit { get; set; }
        public int stayAlive { get; set; }        
        public int friendlyAir { get; set; }
        public int friendlyGround { get; set; }
        public int condAir { get; set; }
        public int condGround { get; set; }
        public int winBonus { get; set; }

        public SettingsClass() { }

        public SettingsClass(bool usePoints, bool winCondition, int airKill, int killAssist, int airHit, int groundHit, int stayAlive, int friendlyAir, int friendlyGround, int condAir, int condGround, int winBonus)
        {
            this.usePoints = usePoints;
            this.winCondition = winCondition;
            this.airKill = airKill;
            this.killAssist = killAssist;
            this.airHit = airHit;
            this.groundHit = groundHit;
            this.stayAlive = stayAlive;            
            this.friendlyAir = friendlyAir;
            this.friendlyGround = friendlyGround;
            this.condAir = condAir;
            this.condGround = condGround;
            this.winBonus = winBonus;
        }
    }
}
