using System;


namespace RCCombatCalc
{
    public class HitsFromClass : IEquatable<HitsFromClass>
    {
        public int gunId;
        public int hits;

        public HitsFromClass() { }

        public HitsFromClass(int id, int hits) 
        {
            this.gunId = id;
            this.hits = hits;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HitsFromClass);
        }

        public bool Equals(HitsFromClass other)
        {
            return other != null &&
                   gunId == other.gunId &&
                   hits == other.hits;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(gunId, hits);
        }
    }
}
