using System;
using System.Text.RegularExpressions;


namespace RCCombatCalc
{
    public class BFOutProcessor
    {
        public enum RequestType { Other, Info, Log };

        //to me moved our of class
        public string buffer = "";
        //to me moved our of class
        public int ammo = 1800; // in case we call for log before getting settings value

        //may be it should be split t0:
        // 1. public LogStringClass ParseLogString(string msg)
        // 2. public int ParseInfo(string msg) 
        // or may be even public InfoString ParseInfo(string msg) in case we'll need something else, not just ammo

        public void Parse(LogStringClass logString, RequestType req, string msg) 
        {
            //this should be moved out of class. Parser should just parse.
            buffer += msg;
            //if (buffer.[buffer.Length - 1] == '>') // we may parse, or we may not (if buffer not complete), therefore this is NOT static
            if (buffer.EndsWith('>'))
            {
                string bf_out = buffer;
                buffer = "";
                string[] digits = Regex.Split(bf_out, @"\D+");

                switch (req) // process depending on request, currently processing only "log read" request
                {
                    case RequestType.Info:

                        //should we need some settings
                        ammo = Int32.Parse(digits[1]);

                        break;

                    case RequestType.Log:                                //log core for 1.08, should log structure change -- CORRECT HERE
                        
                        logString.gunId = Int32.Parse(digits[1]);
                        logString.health = Int32.Parse(digits[2]);
                        logString.roundsFired = ammo - Int32.Parse(digits[3]);
                        int cc = 6;
                        while (digits[cc] != "") 
                        {
                            HitsFromClass pair = new HitsFromClass(Int32.Parse(digits[cc + 1]), Int32.Parse(digits[cc + 2]));
                            logString.hitsFrom.Add(pair);
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
}
