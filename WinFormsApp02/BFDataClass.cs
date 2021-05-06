using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCombatCalc
{
    public class BFDataClass // WHAT BATTLEFLY CAN PROVIDE & WHAT WE NEED
    {
        public int ammoInit, rate, iD, health, ammoLeft;
        public List<HitsFromClass> hits;

        public BFDataClass()
        {
        }

        public BFDataClass(int ammoInit, int rate, int iD, int health, int ammoLeft, List<HitsFromClass> hits)
        {
            this.ammoInit = ammoInit;
            this.rate = rate;
            this.iD = iD;
            this.health = health;
            this.ammoLeft = ammoLeft;
            this.hits = hits;
        }
    }
}
