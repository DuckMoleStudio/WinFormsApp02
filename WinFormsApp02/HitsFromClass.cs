using System;


namespace RCCombatCalc
{
    public class HitsFromClass
    {
        public int gunId;
        public int hits;

        public HitsFromClass() { }

        public HitsFromClass(int id, int hits) 
        {
            this.gunId = id;
            this.hits = hits;
        }
    
    }
}
