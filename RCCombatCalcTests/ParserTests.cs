using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCCombatCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCombatCalc.Tests
{
    [TestClass()]
    public class ParserTests
    {
       
        [TestMethod()]
        public void Consistency_GoodLog_True()
        {
            List<ResultStringClass> displayTable = new List<ResultStringClass>();
            Parser testParser = new Parser(displayTable);

            // create some log (correct)
            
            testParser.AddString(new LogStringClass("pilot1", 0, 1, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(2, 10), new HitsFromClass(3, 10) }));
            testParser.AddString(new LogStringClass("pilot2", 0, 1, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(3, 10) }));
            testParser.AddString(new LogStringClass("pilot3", 0, 1, false, 0, 3, new List<HitsFromClass> { new HitsFromClass(2, 10), new HitsFromClass(1, 10) }));

            List<int> consistencyList = new List<int>();
            Assert.AreEqual(true, testParser.ConsistencyCheck(consistencyList));
           
        }

        [TestMethod()]
        public void Consistency_SingleBadID_False()
        {
            List<ResultStringClass> displayTable = new List<ResultStringClass>();
            Parser testParser = new Parser(displayTable);

            // create some log (incorrect)

            testParser.AddString(new LogStringClass("pilot1", 0, 1, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(2, 10), new HitsFromClass(3, 10) }));
            testParser.AddString(new LogStringClass("pilot2", 0, 1, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(33, 10) }));
            testParser.AddString(new LogStringClass("pilot3", 0, 1, false, 0, 3, new List<HitsFromClass> { new HitsFromClass(2, 10), new HitsFromClass(1, 10) }));

            List<int> consistencyList = new List<int>();
            Assert.AreEqual(false, testParser.ConsistencyCheck(consistencyList));

        }

        [TestMethod()]
        public void Consistency_SingleBadID_CheckTable()
        {
            List<ResultStringClass> displayTable = new List<ResultStringClass>();
            Parser testParser = new Parser(displayTable);

            // create some log (incorrect)

            testParser.AddString(new LogStringClass("pilot1", 0, 1, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(2, 10), new HitsFromClass(3, 10) }));
            testParser.AddString(new LogStringClass("pilot2", 0, 1, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(33, 10) }));
            testParser.AddString(new LogStringClass("pilot3", 0, 1, false, 0, 3, new List<HitsFromClass> { new HitsFromClass(2, 10), new HitsFromClass(1, 10) }));

            List<int> consistencyList = new List<int>();
            testParser.ConsistencyCheck(consistencyList);
            
            Assert.AreEqual(33, consistencyList[0]);
        }

        [TestMethod()]
        public void Consistency_MultipleBadIDs_CheckTable()
        {
            List<ResultStringClass> displayTable = new List<ResultStringClass>();
            Parser testParser = new Parser(displayTable);

            // create some log (incorrect)

            testParser.AddString(new LogStringClass("pilot1", 0, 1, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(22, 10), new HitsFromClass(33, 10) }));
            testParser.AddString(new LogStringClass("pilot2", 0, 1, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(33, 10) }));
            testParser.AddString(new LogStringClass("pilot3", 0, 1, false, 0, 3, new List<HitsFromClass> { new HitsFromClass(2, 10), new HitsFromClass(11, 10) }));

            List<int> consistencyList = new List<int>();
            testParser.ConsistencyCheck(consistencyList);

            Assert.AreEqual(22, consistencyList[0]);
            Assert.AreEqual(33, consistencyList[1]);
            Assert.AreEqual(11, consistencyList[2]);
        }


        [TestMethod()]
        public void Parse_SimpleGoodLog_CheckTable()
        {
            List<ResultStringClass> displayTable = new();
            List<ResultStringClass> displayTableCheck = new(); // correct results
            Parser testParser = new Parser(displayTable);

            // create some log (correct)
            // String name, int team, int roundsFired, Boolean isGroundTarget, int health, int gunId, List<HitsFromClass> hitsFrom

            testParser.AddString(new LogStringClass("pilot1", 0, 1000, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(3, 30) }));
            testParser.AddString(new LogStringClass("pilot2", 0, 1000, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(3, 30) }));
            testParser.AddString(new LogStringClass("pilot3", 0, 1000, false, 10, 3, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(1, 10) }));

            // create correct results
            // string name, int team, int sortieNo, int roundsFired, int hitsAchieved, int hitsTaken, int hitsGround, int ffAir, int ffGround, int soloKills, int killed, int groupKills, String killedBy

            displayTableCheck.Add(new ResultStringClass("pilot1", 0, 0, 1000, 20, 50, 0, 0, 0, 0, 1, 0, "group"));
            displayTableCheck.Add(new ResultStringClass("pilot2", 0, 0, 1000, 40, 40, 0, 0, 0, 0, 1, 1, "pilot3"));
            displayTableCheck.Add(new ResultStringClass("pilot3", 0, 0, 1000, 60, 30, 0, 0, 0, 1, 0, 1, "none"));

           List<int> ignoreList = new();

            testParser.Parse(0, ignoreList);

            Assert.AreEqual(displayTable[0], displayTableCheck[0]);
            Assert.AreEqual(displayTable[1], displayTableCheck[1]);
            Assert.AreEqual(displayTable[2], displayTableCheck[2]);


        }

        [TestMethod()]
        public void Parse_LogWIgnore_CheckTable()
        {
            List<ResultStringClass> displayTable = new();
            List<ResultStringClass> displayTableCheck = new(); // correct results
            Parser testParser = new Parser(displayTable);

            // create some log (with bad IDs)
            // String name, int team, int roundsFired, Boolean isGroundTarget, int health, int gunId, List<HitsFromClass> hitsFrom

            testParser.AddString(new LogStringClass("pilot1", 0, 1000, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(3, 30), new HitsFromClass(33, 30) }));
            testParser.AddString(new LogStringClass("pilot2", 0, 1000, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(3, 30), new HitsFromClass(33, 30) }));
            testParser.AddString(new LogStringClass("pilot3", 0, 1000, false, 10, 3, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(1, 10), new HitsFromClass(22, 30) }));

            // create correct results
            // string name, int team, int sortieNo, int roundsFired, int hitsAchieved, int hitsTaken, int hitsGround, int ffAir, int ffGround, int soloKills, int killed, int groupKills, String killedBy

            displayTableCheck.Add(new ResultStringClass("pilot1", 0, 0, 1000, 20, 50, 0, 0, 0, 0, 1, 0, "group"));
            displayTableCheck.Add(new ResultStringClass("pilot2", 0, 0, 1000, 40, 40, 0, 0, 0, 0, 1, 1, "pilot3"));
            displayTableCheck.Add(new ResultStringClass("pilot3", 0, 0, 1000, 60, 30, 0, 0, 0, 1, 0, 1, "none"));

            List<int> ignoreList = new();
            ignoreList.Add(33);
            ignoreList.Add(22);

            testParser.Parse(0, ignoreList);

            Assert.AreEqual(displayTable[0], displayTableCheck[0]);
            Assert.AreEqual(displayTable[1], displayTableCheck[1]);
            Assert.AreEqual(displayTable[2], displayTableCheck[2]);


        }

        [TestMethod()]
        public void Parse_LogWGround_CheckTable()
        {
            List<ResultStringClass> displayTable = new();
            List<ResultStringClass> displayTableCheck = new(); // correct results
            Parser testParser = new Parser(displayTable);

            // create some log (with ground target)
            // String name, int team, int roundsFired, Boolean isGroundTarget, int health, int gunId, List<HitsFromClass> hitsFrom

            testParser.AddString(new LogStringClass("pilot1", 0, 1000, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(3, 30)}));
            testParser.AddString(new LogStringClass("pilot2", 0, 1000, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(3, 30)}));
            testParser.AddString(new LogStringClass("pilot3", 0, 1000, false, 10, 3, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(1, 10)}));
            testParser.AddString(new LogStringClass("ground", 0, 1000, true, 10, 4, new List<HitsFromClass> { new HitsFromClass(1, 11), new HitsFromClass(2, 22) }));

            // create correct results
            // string name, int team, int sortieNo, int roundsFired, int hitsAchieved, int hitsTaken, int hitsGround, int ffAir, int ffGround, int soloKills, int killed, int groupKills, String killedBy

            displayTableCheck.Add(new ResultStringClass("pilot1", 0, 0, 1000, 20, 50, 11, 0, 0, 0, 1, 0, "group"));
            displayTableCheck.Add(new ResultStringClass("pilot2", 0, 0, 1000, 40, 40, 22, 0, 0, 0, 1, 1, "pilot3"));
            displayTableCheck.Add(new ResultStringClass("pilot3", 0, 0, 1000, 60, 30, 0, 0, 0, 1, 0, 1, "none"));

            List<int> ignoreList = new();
            ignoreList.Add(33);
            ignoreList.Add(22);

            testParser.Parse(0, ignoreList);

            Assert.AreEqual(displayTable[0], displayTableCheck[0]);
            Assert.AreEqual(displayTable[1], displayTableCheck[1]);
            Assert.AreEqual(displayTable[2], displayTableCheck[2]);


        }

        [TestMethod()]
        public void Parse_LogWTeams_CheckTable()
        {
            List<ResultStringClass> displayTable = new();
            List<ResultStringClass> displayTableCheck = new(); // correct results
            Parser testParser = new Parser(displayTable);

            // create some log (with team status)
            // String name, int team, int roundsFired, Boolean isGroundTarget, int health, int gunId, List<HitsFromClass> hitsFrom

            testParser.AddString(new LogStringClass("pilot1", 1, 1000, false, 0, 1, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(3, 30) }));
            testParser.AddString(new LogStringClass("pilot2", 1, 1000, false, 0, 2, new List<HitsFromClass> { new HitsFromClass(1, 10), new HitsFromClass(3, 30) }));
            testParser.AddString(new LogStringClass("pilot3", 2, 1000, false, 10, 3, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(1, 10) }));
            testParser.AddString(new LogStringClass("ground", 2, 1000, true, 10, 4, new List<HitsFromClass> { new HitsFromClass(1, 11), new HitsFromClass(3, 22) }));

            // create correct results
            // string name, int team, int sortieNo, int roundsFired, int hitsAchieved, int hitsTaken, int hitsGround, int ffAir, int ffGround, int soloKills, int killed, int groupKills, String killedBy

            displayTableCheck.Add(new ResultStringClass("pilot1", 1, 0, 1000, 10, 50, 11, 10, 0, 0, 1, 0, "group"));
            displayTableCheck.Add(new ResultStringClass("pilot2", 1, 0, 1000, 20, 40, 0, 20, 0, 0, 1, 1, "pilot3"));
            displayTableCheck.Add(new ResultStringClass("pilot3", 2, 0, 1000, 60, 30, 0, 0, 22, 1, 0, 1, "none"));

            List<int> ignoreList = new();
            ignoreList.Add(33);
            ignoreList.Add(22);

            testParser.Parse(0, ignoreList);

            Assert.AreEqual(displayTable[0], displayTableCheck[0]);
            Assert.AreEqual(displayTable[1], displayTableCheck[1]);
            Assert.AreEqual(displayTable[2], displayTableCheck[2]);


        }

    }
}