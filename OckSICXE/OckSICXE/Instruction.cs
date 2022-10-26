using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OckSICXE
{
     public class Instruction
    {
/*********************************************************************
        *** NAME : Caleb Ockwig                                            ***
        *** CLASS : CSc 354                                                ***
        *** ASSIGNMENT : 2                                                 ***
        *** DUE DATE : 10/5                                                ***
        *** INSTRUCTOR : GAMRADT ***
        *********************************************************************
        *** DESCRIPTION : Splits the line into arguments based             ***
        ***                on what information can be collected in lines.  ***
        ********************************************************************/        
        public static string[] FormatLine(string input)
        {
            
            string _trimmedLine = "";
            string _comment = "";
            string _opcode = "";
            string _operand = "";
            string _symbol = "";
            string[] ops = { };

            if(!string.IsNullOrEmpty(input))
            {
                if(input.Contains(';'))
                {
                    _comment = input.Substring(input.IndexOf(';') + 1);
                    input = input[..input.IndexOf(';')];
                }
                if (!string.IsNullOrEmpty(input))
                {
                    ops = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    if (ops.Length == 3)
                    {
                        _symbol = ops[0].Substring(0, ops[0].Length-1);
                        _opcode = ops[1];
                        _operand = ops[2];
                    }
                    else if(ops.Length == 2)
                    {
                        _opcode = ops[0];
                        _operand = ops[1];
                    }
                    else
                    {
                        //error or something?
                    }
                    _trimmedLine = input;

                }
            }
            string[] Arguments =  { _trimmedLine, _symbol, _opcode, _operand, _comment};
            return Arguments;

        }

    }
}
