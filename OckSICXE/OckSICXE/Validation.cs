using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OckSICXE
{
    internal class Validation
    {

        /********************************************************************
        *** FUNCTION CheckSymbol ***
        *********************************************************************
        *** DESCRIPTION : Checks if the given string is valid to be a symbol ***
        ***               Must be alphanumeric starting with a letter
        ***               Must be less than 12 chars
        ***               Any issues will add a corresponding value to the errorcode
        *** INPUT ARGS : string symbol ***
        *** OUTPUT ARGS : byte errorcode ***
        *** IN/OUT ARGS : <list of all input/output argument names> ***
        *** RETURN : <return type and return value name> ***
         ********************************************************************/
        public static byte CheckSymbol(string symbol)
        {
            byte errorCode = 0;
            if (symbol.Length > 12)
            {
                errorCode += 1;
            }

            if (!Regex.IsMatch(symbol, "^[a-zA-Z0-9]+$"))
            {
                errorCode += 2;
            }

            if (!char.IsLetter(symbol.FirstOrDefault()))
            {
                errorCode += 4;
            }

            return errorCode;
        }
        /********************************************************************
        *** FUNCTION FormatSymbol                                         ***
        *********************************************************************
        *** DESCRIPTION : creates a substring of the passed in symbol     ***
        *** INPUT ARGS : string symbol                                    ***
        *** OUTPUT ARGS : string symbol                                   ***
        *** IN/OUT ARGS : <list of all input/output argument names>       ***
        *** RETURN : symbol                                               ***
        ********************************************************************/
        public static string FormatSymbol(string symbol)
        {
            if(symbol.Contains(';'))
            {
                symbol = symbol.Substring(0, symbol.Length - 1);
            }
            if (symbol.Length > 6)
            {
                symbol = symbol.Substring(0, 6);
            }
            return symbol;
        }
        /********************************************************************
        *** FUNCTION CheckRFlag                                           ***
        *********************************************************************
        *** DESCRIPTION : Chacks if string flag is in a valid format      ***
        *                 ("true", "1" evaluates as 1,                    ***
        *                  "False, "0" evaluates as 0.                    ***
        *                  any other input is invalid which returns -1    ***
        *** INPUT ARGS : string symbol                                    ***
        *** OUTPUT ARGS : string symbol                                   ***
        *** IN/OUT ARGS : <list of all input/output argument names>       ***
        *** RETURN : int flag                                             ***
        ********************************************************************/
        public static int CheckRFlag(string rFlag)
        {
            if (rFlag.Equals("true") || rFlag.Equals("1"))
                return 1;
            else if (rFlag.Equals("false") || rFlag.Equals("0"))
                return 0;
            else
                //-1 will indicate that the given input was invalid
                return -1;
        }
        /********************************************************************
        *** FUNCTION ChackValue                                           ***
        *********************************************************************
        *** DESCRIPTION : Checks if the given string can be parsed        ***
        *                as an integer                                    ***
        *** INPUT ARGS : string value                                     ***
        *** OUTPUT ARGS : bool valid                                      ***
        *** IN/OUT ARGS :                                                 ***
        *** RETURN : boolean valid                                        ***
        ********************************************************************/
        public static bool CheckValue(string value)
        {
            if (int.TryParse(value, out _))
            {
                return true;
            }

            return false;
        }
        /********************************************************************
        *** FUNCTION FormatValue                                          ***
        *********************************************************************
        *** DESCRIPTION : Parses the given string into an int.            ***
        *         Made this into a method to keep main function cleaner   ***
        *** INPUT ARGS : string value ***
        *** OUTPUT ARGS : int val ***
        *** IN/OUT ARGS : <list of all input/output argument names> ***
        *** RETURN : int ***
        ********************************************************************/
        public static int FormatValue(string value)
        {
            int val;
            int.TryParse(value, out val);
            return val;
        }
        /********************************************************************
        *** FUNCTION ParseLine                                            ***
        *********************************************************************
        *** DESCRIPTION : creates a substring of the passed in symbol     ***
        *** INPUT ARGS : string symbol                                    ***
        *** OUTPUT ARGS : string symbol                                   ***
        *** IN/OUT ARGS : none                                            ***
        *** RETURN : symbol                                               ***
        ********************************************************************/
        public static string[] ParseLine(String line)
        {
            //ABCDEFgh: true 100
            line = line.Trim(' ');
            line = line.Replace(':', ' ');
            string[] result = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // result[0] = Symbol
            // result[1] = rFlag
            // result[2] = value
            return result;
        }
        /*
        * error lookup
        * 1 = symbol > 12
        * 2 = not alphanumeric
        * 4 = first symbol not a letter
        * 8 = not valid rFlag
        * 16 = value not valid
        * 32 = multiple (set mFlag to true)
        */
        /********************************************************************
        *** FUNCTION ShowError ***
        *********************************************************************
        *** DESCRIPTION : shows the list of errors                        ***
        *** INPUT ARGS : byte errorCode, string symbol, string[] line     ***
        *** OUTPUT ARGS : none                                            ***
        *** IN/OUT ARGS : <list of all input/output argument names>       ***
        *** RETURN : symbol                                               ***
        ********************************************************************/
        public static void ShowError(byte errorCode, string symbol, string[] line)
        {
            if (errorCode % 2 == 1)
                Console.WriteLine("ERROR - " + symbol + " has a symbol greater than 12 characters");
            errorCode /= 2;
            if (errorCode % 2 == 1)
                Console.WriteLine("ERROR - " + symbol + " is not alphanumeric");
            errorCode /= 2;
            if (errorCode % 2 == 1)
                Console.WriteLine("ERROR - " + symbol + " first symbol must be a letter");
            errorCode /= 2;
            if (errorCode % 2 == 1)
                Console.WriteLine("ERROR - " + symbol + " does not have a valid RFlag");
            errorCode /= 2;
            if (errorCode % 2 == 1)
                Console.WriteLine("ERROR - " + symbol + " does not have a valid value. " + line[2]);
        }



    }
}
