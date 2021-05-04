using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCCombatCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RCCombatCalc.BFOutProcessor;

namespace RCCombatCalc.Tests
{
    [TestClass()]
    public class BFOutProcessorTests
    {
        [TestMethod()]
        public void Parse_InfoNotComplete_DefaultValue()
        {
            BFOutProcessor testProcessor = new();
            string msg = "bf parameters info \n ammo: 2000 \n "; // no new string prompt yet
            LogStringClass testLogString = new();
            RequestType req = RequestType.Info;

            testProcessor.Parse(testLogString, req, msg);

            Assert.AreEqual(1800, testProcessor.ammo);

        }

        [TestMethod()]
        public void Parse_Info_AmmoInitial()
        {
            BFOutProcessor testProcessor = new();
            string msg = "bf parameters info \n ammo: 2000 \n sh>"; // new string prompt, complete
            LogStringClass testLogString = new();
            RequestType req = RequestType.Info;

            testProcessor.Parse(testLogString, req, msg);

            Assert.AreEqual(2000, testProcessor.ammo);

        }

        [TestMethod()]
        public void Parse_LogNoHits_CheckString()
        {
            BFOutProcessor testProcessor = new();
            string msg = "Log: \n Session address: 8 \n Life: 100 % \n Ammo: 1300 \n Reset count: 0 \n Hits: 0 \n Num Address Count \n ---- - --- \n sh>"; // new string prompt, complete
            LogStringClass testLogString = new();           
            RequestType req = RequestType.Log;

            testProcessor.Parse(testLogString, req, msg);
            //String name, int team, int roundsFired, Boolean isGroundTarget, int health, int gunId, List<HitsFromClass> hitsFrom
            LogStringClass checkLogString = new();
            checkLogString.gunId = 8;
            checkLogString.roundsFired = 500;
            checkLogString.health = 100;


            Assert.AreEqual(8, testLogString.gunId);
            Assert.AreEqual(500, testLogString.roundsFired);
            Assert.AreEqual(100, testLogString.health);

        }

        [TestMethod()]
        public void Parse_LogSomeHits_CheckString()
        {
            BFOutProcessor testProcessor = new();
            string msg = "Log: \n Session address: 8 \n Life: 100 % \n Ammo: 1300 \n Reset count: 0 \n Hits: 0 \n Num Address Count \n 1. 2 20 \n 2. 3 30 \n sh>"; // new string prompt, complete
            LogStringClass testLogString = new();
            RequestType req = RequestType.Log;

            testProcessor.Parse(testLogString, req, msg);
            //String name, int team, int roundsFired, Boolean isGroundTarget, int health, int gunId, List<HitsFromClass> hitsFrom
            LogStringClass checkLogString = new LogStringClass(null, 0, 500, false, 100, 8, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(3, 30) });

            //Assert.AreEqual(checkLogString, testLogString;
            Assert.AreEqual(checkLogString.hitsFrom, testLogString.hitsFrom);

        }
    }
}