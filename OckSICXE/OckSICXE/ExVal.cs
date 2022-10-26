using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Collections;

namespace OckSICXE
{
    internal class ExVal //Expression Evaluation
    {

        /*********************************************************************
        *** NAME : Caleb Ockwig                                            ***
        *** CLASS : CSc 354                                                ***
        *** ASSIGNMENT : 2                                                 ***
        *** DUE DATE : 10/5                                                ***
        *** INSTRUCTOR : GAMRADT ***
        *********************************************************************
        *** DESCRIPTION : Evaluates all of the expressions in a given file.***
        ********************************************************************/
        public static int EvalExpression(string Expression, out ArrayList LiteralTable, ArrayList LitTableIn, BST SymbolTable)
        {
            string outputExpress = Expression;
            string[] Symbols = { };
            int ExpValue = 0;
            string value = "";
            bool IndexBit = false, NBit = false, IBit = false, RFlag = false, Error = false;
            if (Expression.Length > 2)
            {
                if (Expression.Substring(Expression.Length - 2).Equals(",X"))
                {
                    IndexBit = true;
                    Expression = Expression.TrimEnd('X');
                    Expression = Expression.TrimEnd(',');
                }
            }
            switch (CheckLine(Expression))
            {
                case 1://literal

                    Error = true;
                    char[] charLit = Expression.ToCharArray();
                    char LitType = charLit[1];
                    if (LitType == 'C' || LitType == 'c')
                    {

                        int size = 0;
                        for (int i = 3; i < charLit.Length - 1; i++)
                        {
                            value += Convert.ToByte(charLit[i]).ToString("x2");
                            size++;
                        }
                        Literal Lit = new Literal(Expression, value, size, LitTableIn.Count, LitType);
                        LitTableIn.Add(Lit); //adds if doesn't exist
                    }
                    else if (LitType == 'X' || LitType == 'x')
                    {
                        if (Expression.Length % 2 == 1)
                        {
                            value = "0";
                        }
                        for (int i = 3; i < charLit.Length - 1; i++)
                        {
                            value += charLit[i];
                        }
                        int size = value.Length / 2;
                        Literal Lit = new(Expression, value, size, LitTableIn.Count, LitType);
                        LitTableIn.Add(Lit);
                    }

                    break;
                case 2:// indirect @ 
                    NBit = true;
                    IBit = false;
                    Error = IndexBit;
                    if (Error)
                    {
                        Console.WriteLine(outputExpress + " cannot use indirect + index register");
                    }
                    else
                    {
                        if (Expression.Contains('+'))
                        {
                            ExpValue = ExpressionValue(Expression, '+', out RFlag, SymbolTable, out Error);
                        }
                        else if (Expression.Contains("-"))
                        {
                            ExpValue = ExpressionValue(Expression, '-', out RFlag, SymbolTable, out Error);
                        }
                        else
                        {
                            ExpValue = ExpressionValue(Expression, ' ', out RFlag, SymbolTable, out Error);
                        }
                    }


                    break;
                case 3://immediate addressing #
                    NBit = false;
                    IBit = true;
                    Error = IndexBit;
                    if (Error)
                    {
                        Console.WriteLine(outputExpress + " cannot use immediate + index register");
                    }
                    else
                    {
                        if (Expression.Contains('+'))
                        {
                            ExpValue = ExpressionValue(Expression, '+', out RFlag, SymbolTable, out Error);

                        }
                        else if (Expression.Contains("-"))
                        {
                            ExpValue = ExpressionValue(Expression, '-', out RFlag, SymbolTable, out Error);
                        }
                        else
                        {
                            ExpValue = ExpressionValue(Expression, ' ', out RFlag, SymbolTable, out Error);
                        }

                    }
                    break;

                case 4://simple
                    IBit = true;
                    NBit = true;
                    if (Expression.Contains('+'))
                    {
                        ExpValue = ExpressionValue(Expression, '+', out RFlag, SymbolTable, out Error);


                    }
                    else if (Expression.Contains("-"))
                    {
                        ExpValue = ExpressionValue(Expression, '-', out RFlag, SymbolTable, out Error);
                    }
                    else
                    {
                        ExpValue = ExpressionValue(Expression, ' ', out RFlag, SymbolTable, out Error);
                    }
                    break;
            }
            LiteralTable = LitTableIn;
            return ExpValue;
        }
        /********************************************************************
       *** FUNCTION Expression Value***
       *********************************************************************
       *** DESCRIPTION : Parses the given input to determine the value     ***
       ***               from the string either searching the symbol table ***
       ***               or parsing the input and setting the RFlag Value  ***
       *** INPUT ARGS : string Expression, char Operand, out bool RFlag,   ***
                        BST SymbolTable, out bool Error                    ***
       *** OUTPUT ARGS : none                                              ***
       *** IN/OUT ARGS : bool RFlag, bool Error                            ***
       *** RETURN : int value (returned value)                             ***
       ********************************************************************/
        private static int ExpressionValue(string Expression, char Operand, out bool RFlag, BST SymbolTable, out bool Error)
        {
            int value = 0;
            RFlag = true;
            Error = false;
            string[] Symbols = { };
            string expressionTemp = Expression;

            if (Operand != ' ')
            {
                Symbols = Expression.Split(Operand);
            }
            else
            {
                Expression = Expression.Trim('#');
                Expression = Expression.Trim('@');
                if (Regex.IsMatch(Expression, "^[a-zA-Z]+$"))
                {
                    RFlag = SymbolTable.Search(Expression).RFlag;
                    value = SymbolTable.Search(Expression).Value;
                }
                else
                {
                    if (int.TryParse(Expression, out int number))
                    {
                        RFlag = false;
                        return number;
                    }
                }
            }

            if (Symbols.Length == 1)
            {

            }
            else if (Symbols.Length == 2)
            {
                bool RFlag1 = false, RFlag2 = false;
                int number1 = 0, number2 = 0;
                //finding value of first operand
                if (Regex.IsMatch(Symbols[0].First().ToString(), "^[a-zA-Z]+$"))
                {
                    RFlag1 = SymbolTable.Search(Symbols[0]).RFlag;
                    number1 = SymbolTable.Search(Symbols[0]).Value;
                }
                else
                {
                    RFlag1 = false;
                    int.TryParse(Symbols[0], out number1);
                }
                //Finding Value of second operand
                if (Regex.IsMatch(Symbols[1].First().ToString(), "^[a-zA-Z]+$"))
                {
                    RFlag2 = SymbolTable.Search(Symbols[1]).RFlag;
                    number2 = SymbolTable.Search(Symbols[1]).Value;
                }
                else
                {
                    RFlag2 = false;
                    int.TryParse(Symbols[1], out number2);
                }
                if (Operand == '+')
                {
                    if (RFlag1 == true && RFlag2 == true)
                    {
                        Error = true;//Absolute + relative
                        Console.WriteLine("ERROR " + expressionTemp + "- Invalid RFlag Values: " + Symbols[0] + " " + RFlag1 + " " + Operand + " " + Symbols[1] + " " + RFlag2);
                    }
                    value = number1 + number2;

                }
                else if (Operand == '-')
                {
                    if (RFlag1 == false && RFlag2 == true)
                    {
                        Error = true;//Absolute + relative
                    }
                    value = number1 - number2;
                }
                if (RFlag1 == true && RFlag2 == true || RFlag1 == false && RFlag2 == false)
                {
                    RFlag = false;
                }

            }


            return value;
        }
        /********************************************************************
        *** FUNCTION CheckLine***
        *********************************************************************
        *** DESCRIPTION : Determines what kind of addressing the          ***
                         expression gives                                 ***
        *** INPUT ARGS : String line                                      ***
        *** OUTPUT ARGS : none                                            ***
        *** IN/OUT ARGS : none                                            ***
        *** RETURN : int (indicates what it is later)                     ***
        ********************************************************************/
        public static int CheckLine(string line)
        {
            int ExpressionType = -1;
            if (line.First().Equals('='))
            {//Literal
                ExpressionType = 1;
            }
            else if (line.First().Equals('@'))
            {//Indirect Addressing
                ExpressionType = 2;

            }
            else if (line.First().Equals('#') || Regex.IsMatch(line, "^[0-9]+$"))
            {//Immediate Addressing
                ExpressionType = 3;
            }
            else if (Regex.IsMatch(line.First().ToString(), "^[a-zA-Z]+$"))
            {
                ExpressionType = 4;
            }

            return ExpressionType;
        }
        /********************************************************************
        *** FUNCTION PrintExp ***
        *********************************************************************
        *** DESCRIPTION : Prints the expression with an intuitive output  ***
        *** INPUT ARGS  : String expression, int value, bool relocatable, ***
        ***               Bool NBit, bool IBit, bool XBit                 ***
        *** OUTPUT ARGS : none                                            ***
        *** IN/OUT ARGS : none                                            ***
        *** RETURN      : none                                            ***
        ********************************************************************/
        public static void PrintExp(string Expression, int Value, bool Relocatable, bool NBit, bool IBit, bool XBit)
        {
            string ValueStr = Value.ToString();
            string relocatableStr = "", NBitStr = "", IBitStr = "", XBitStr = "";
            int bufferlength = 15 - Expression.Length;
            for (int i = 0; i < bufferlength; i++)
                Expression += " ";
            bufferlength = 10 - ValueStr.Length;
            for (int i = 0; i < bufferlength; i++)
                ValueStr += " ";
            relocatableStr = Relocatable ? "RELATIVE      " : relocatableStr = "ABSOLUTE      ";
            NBitStr = NBit ? "1       " : "0       ";
            IBitStr = IBit ? "1       " : "0       ";
            XBitStr = XBit ? "1       " : "0       ";
            Console.WriteLine(Expression + ValueStr + relocatableStr + NBitStr + IBitStr + XBitStr);

        }
    }

}
