using System;
using System.Text.RegularExpressions;


namespace RCCombatCalc
{
    public static class BFOutProcessor
    {
        public enum RequestType { Other, Info, Log };

       
        public static void Parse(BFDataClass dataBF, RequestType req, string msg) 
        {
                        
              
                
                string[] digits = Regex.Split(msg, @"\D+");

                switch (req) // process depending on request, currently processing only "log read" request
                {
                    case RequestType.Info:

                        //should we need some settings
                        dataBF.ammoInit = Int32.Parse(digits[1]); // like initial ammo

                        break;

                    case RequestType.Log:                                //log core for 1.08, should log structure change -- CORRECT HERE
                        
                        dataBF.iD = Int32.Parse(digits[1]);
                        dataBF.health = Int32.Parse(digits[2]);
                        dataBF.ammoLeft = Int32.Parse(digits[3]);
                        int cc = 6;
                        while (digits[cc] != "") 
                        {
                            HitsFromClass pair = new HitsFromClass(Int32.Parse(digits[cc + 1]), Int32.Parse(digits[cc + 2]));
                            dataBF.hits.Add(pair);
                            cc += 3;
                        }
                                                
                        break;

                    default:
                        //maybe smth else
                        break;

                }
                
           
        }
    }
}
