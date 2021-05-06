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
        public void Parse_Info_AmmoInitial()
        {
            //BFOutProcessor testProcessor = new();
            string msg = "bf parameters info \n ammo: 2000 \n sh>"; // new string prompt, complete
            BFDataClass dataBF = new();
            RequestType req = RequestType.Info;

            BFOutProcessor.Parse(dataBF, req, msg);

            Assert.AreEqual(2000, dataBF.ammoInit);

        }

        [TestMethod()]
        public void Parse_LogNoHits_CheckString()
        {
            
            string msg = "Log: \n Session address: 8 \n Life: 100 % \n Ammo: 1300 \n Reset count: 0 \n Hits: 0 \n Num Address Count \n ---- - --- \n sh>"; // new string prompt, complete
            BFDataClass dataBF = new();
            RequestType req = RequestType.Log;

            BFOutProcessor.Parse(dataBF, req, msg);
            
            
            Assert.AreEqual(8, dataBF.iD);
            Assert.AreEqual(1300, dataBF.ammoLeft);
            Assert.AreEqual(100, dataBF.health);

        }

        [TestMethod()]
        public void Parse_LogSomeHits_CheckString()
        {
            
            string msg = "Log: \n Session address: 8 \n Life: 100 % \n Ammo: 1300 \n Reset count: 0 \n Hits: 0 \n Num Address Count \n 1. 2 20 \n 2. 3 30 \n sh>"; // new string prompt, complete
           
            BFDataClass dataBF = new();
            RequestType req = RequestType.Log;
            BFOutProcessor.Parse(dataBF, req, msg);

            // int ammoInit, int rate, int iD, int health, int ammoLeft, List<HitsFromClass> hits
            BFDataClass dataBFCheck = new BFDataClass(0, 0, 8, 100, 1300, new List<HitsFromClass> { new HitsFromClass(2, 20), new HitsFromClass(3, 30) });
            
            

            Assert.AreEqual(dataBF, dataBFCheck);
        }
    }
}