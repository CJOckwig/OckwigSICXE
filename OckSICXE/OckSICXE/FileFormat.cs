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
        public static string[] InterFileFormat(string line)
        {
            string[] operands = {"","","","","" };
            // 0 line number
            // 1 PC Counter
            // 2 Symbol
            // 3 opcode
            // 4 expressions

            if (!string.IsNullOrEmpty(line))
            {
                string[] s = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if(s.Length == 5)
                {
                    operands[0] = s[0];
                    operands[1] = s[1];
                    operands[2] = s[2];
                    operands[3] = s[3];
                    operands[4] = s[4];
                }else if(s.Length == 4)
                {
                    operands[0] = s[0];
                    operands[1] = s[1];
                    //operands[2] is ""
                    operands[3] = s[2];
                    operands[4] = s[3];
                }
                //add for single opcode cases
                int ExpressionType = ExVal.CheckLine(operands[4]);

            }


            return operands;
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
        public static int middleFormat(bool X, bool B, bool P, bool E)
        {
            //8, 4, 2, 1
            int val = 0;
            if(X) 
            val+=8;
            if(B)
            val+=4;
            if(P)
            val+=2;
            if(E)
            val+=1;
            
            return val;
            
        }
    }
}
