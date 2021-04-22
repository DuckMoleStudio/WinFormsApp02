using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace RCCombatCalc
{
    public class BFOutProcessor
    {
        public enum RequestType { Other, Info, Log };
        public string buffer;
        public string bf_out;
        
        public BFOutProcessor() 
        {
            this.buffer = "";
            this.bf_out = "";
        }

        public void Parse(LogStringClass logString, RequestType req, string msg) 
        {
            buffer += msg;
            if (buffer[buffer.Length - 1] == '>') // we may parse, or we may not (if buffer not complete), therefore this is NOT static
            {
                bf_out = buffer;
                buffer = "";
                switch (req)
                {
                    case RequestType.Info:
                        
                        //should we need some settings
                        break;

                    case RequestType.Log:                                //log core for 1.08, should log structure change -- CORRECT HERE
                        string[] digits = Regex.Split(bf_out, @"\D+");
                        logString.gunId = Int32.Parse(digits[1]);
                        logString.health = Int32.Parse(digits[2]);
                        logString.roundsFired = Int32.Parse(digits[3]);
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
