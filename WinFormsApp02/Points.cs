using System;

namespace RCCombatCalc
{
    public static class Points
    {
        #region SOLO
        public static int CalcSolo(ResultStringClass r, SettingsClass s) // CALCULATE POINTS FOR SOLO RESULTS
        {
            int points = 0;
            points += r.soloKills * s.airKill;
            points += r.groupKills * s.killAssist;
            points += r.hitsAchieved * s.airHit;
            points += r.hitsGround * s.groundHit;
            if (r.killed == 0) points +=  s.stayAlive; 

            return points;
        }
        #endregion

        #region TEAM
        public static int CalcTeam(ResultStringClass r, SettingsClass s) // CALCULATE POINTS FOR TEAM RESULTS
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
        #endregion
    }
}
