using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OckSICXE
{
    internal class FileFormat
    {
        /*********************************************************************
        *** NAME : Caleb Ockwig                                            ***
        *** CLASS : CSc 354                                                ***
        *** ASSIGNMENT : 2                                                 ***
        *** DUE DATE : 10/5                                                ***
        *** INSTRUCTOR : GAMRADT ***
        *********************************************************************
        *** DESCRIPTION : Formats lines to look uniform in file          ***
        ********************************************************************/
        public static string FileLine(string[] arguments, int ProgramCounter, int i)
        {
            string output = "", _locationCounter = ProgramCounter.ToString("X");
            string _symbolString = arguments[1], _opCodeString = arguments[2];
            string _operandString = arguments[3], _comment = arguments[4]; ;
            string lineNumber = i.ToString();

            while (lineNumber.Length < 5)
            {
                lineNumber += " ";
            }
            while (_locationCounter.Length < 5)
            {
                _locationCounter = "0" + _locationCounter;
            }
            while (_symbolString.Length <= 10)
            {
                _symbolString += " ";
            }
            while (_opCodeString.Length <= 10)
            {
                _opCodeString += " ";
            }
            while (_operandString.Length <= 10)
            {
                _operandString += " ";
            }

            output = lineNumber + _locationCounter + "    " + _symbolString + _opCodeString + _operandString + _comment;
            return output;
        }
        public static string[] GetFileLines(string filename)
        {
            string[] lines = { };
            try
            {
                lines = File.ReadAllLines(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return lines;
        }
    }
}
