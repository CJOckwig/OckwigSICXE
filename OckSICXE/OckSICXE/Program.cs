using OckSICXE;
using System;
using System.Collections;
/*********************************************************************
        *** NAME : Caleb Ockwig                                            ***
        *** CLASS : CSc 354                                                ***
        *** ASSIGNMENT : 3/4                                               ***
        *** DUE DATE : 10/25                                               ***
        *** INSTRUCTOR : GAMRADT ***
        *********************************************************************
        *** DESCRIPTION : Goes through Pass 1 of the SICXE Assembler process.***
        ********************************************************************/
internal class Program
{
    static void Main(string[] args)
    {
        string SICProgram = (args.Length == 1) ? args[0] : null;
        while (string.IsNullOrEmpty(SICProgram))
        {
            Console.Write("Please Enter the name of the SIC Program to read: ");
            SICProgram = Console.ReadLine();
        }
        string[] OpcodeLines = FileFormat.GetFileLines("OckSICXE" + Path.DirectorySeparatorChar + "opcodes.txt");
        string[] SICProgramLines = FileFormat.GetFileLines("OckSICXE" + Path.DirectorySeparatorChar + SICProgram);
        ArrayList LiteralTable = new ArrayList();
        LinkedList<string> IntermediateFile = new LinkedList<string>();
        BST SymbolTable = new();
        ArrayList OpcodeTable = new();
        foreach (string OpCodeLine in OpcodeLines)
        {
            Opcode Op = new(OpCodeLine);
            OpcodeTable.Add(Op);
        }
        string intermediateFile = SICProgram + ".int";
        int StartIndex = 0;
        int ProgramCounter = 0, NextInstruction = 0; ;
        int TrimmedLineIndex = 0, SymbolIndex = 1, OpcodeIndex = 2, OperandIndex = 3, CommentIndex = 4;
        int ExpressionValue = 0;
        int i;
        bool RFlag = true;
        string[] Arguments = Instruction.FormatLine(SICProgramLines[0]);
        using (StreamWriter writer = new StreamWriter("OckSICXE" + Path.DirectorySeparatorChar +intermediateFile))
        {
            if (Arguments[OpcodeIndex].Equals("START"))
            {
                int.TryParse(Arguments[OperandIndex], out StartIndex);

                string output = FileFormat.FileLine(Arguments, StartIndex, 0);
                NextInstruction = StartIndex;
                writer.WriteLine(output);
                Console.WriteLine(output);
            }
            if (!string.IsNullOrEmpty(Arguments[SymbolIndex]))
            {
                Arguments[SymbolIndex] = Validation.FormatSymbol(Arguments[SymbolIndex]);
                if (!SymbolTable.AddNode(Arguments[SymbolIndex], ProgramCounter, RFlag))
                {
                    Console.WriteLine("Error adding to symbol Table");

                }
            }
            for (i = 1; i < SICProgramLines.Length; i++)
            {
                string opcodeOut = "";
                bool ValidDirective = false;
                Arguments = Instruction.FormatLine(SICProgramLines[i]);
                //is there anything in the line?
                if (!string.IsNullOrEmpty(Arguments[TrimmedLineIndex]))
                {
                    //Populate symbol table if necessary.
                    if (!string.IsNullOrEmpty(Arguments[OperandIndex]))
                    {
                        ExpressionValue = ExVal.EvalExpression(Arguments[OperandIndex], out LiteralTable, LiteralTable, SymbolTable);
                    }
                    if (!string.IsNullOrEmpty(Arguments[SymbolIndex]))
                    {
                        Arguments[SymbolIndex] = Validation.FormatSymbol(Arguments[SymbolIndex]);
                        if (!SymbolTable.AddNode(Arguments[SymbolIndex], ProgramCounter, RFlag))
                        {
                            Console.WriteLine("Error adding to symbol Table");

                        }
                    }

                    //Format 4.
                    opcodeOut = Arguments[OpcodeIndex];
                    if (Arguments[OpcodeIndex].StartsWith("+"))
                    {
                        NextInstruction += 1;
                        Arguments[OpcodeIndex] = Arguments[OpcodeIndex].Trim('+');
                    }
                    foreach (Opcode O in OpcodeTable)
                    {
                        if (O.Mnemonic.Equals(Arguments[OpcodeIndex]))
                        {
                            NextInstruction += O.Format;
                            ValidDirective = true;
                        }
                    }
                    Arguments[OpcodeIndex] = opcodeOut;
                    if (Arguments[OpcodeIndex].Equals("WORD"))
                    {
                        NextInstruction += 3;
                        ValidDirective = true;

                    }
                    else if (Arguments[OpcodeIndex].Equals("RESW"))
                    {
                        NextInstruction += (3 * ExpressionValue);
                        ValidDirective = true;

                    }
                    else if (Arguments[OpcodeIndex].Equals("RESB"))
                    {
                        NextInstruction += ExpressionValue;
                        ValidDirective = true;

                    }
                    else if (Arguments[OpcodeIndex].Equals("BYTE"))
                    {
                        NextInstruction++;
                        ValidDirective = true;

                    }
                    else if ((Arguments[OpcodeIndex].Equals("BASE")))
                    {
                        ValidDirective = true;

                    }
                    else if ((Arguments[OpcodeIndex].Equals("EQU")))
                    {
                        ProgramCounter = ExVal.EvalExpression(Arguments[OperandIndex], out LiteralTable, LiteralTable, SymbolTable);
                        ValidDirective = true;

                    }
                    else if (Arguments[OpcodeIndex].Equals("EXTDEF"))
                    {
                        ValidDirective = true;

                    }
                    else if (Arguments[OpcodeIndex].Equals("EXTREF"))
                    {
                        ValidDirective = true;

                    }
                    else if (Arguments[OpcodeIndex].Equals("END"))
                    {
                        ValidDirective = true;
                    }
                    else if (!ValidDirective)
                    {
                        Console.WriteLine("Illegal instruction: " + Arguments[OpcodeIndex]);

                    }
                    if(ValidDirective)
                    {
                        string output = FileFormat.FileLine(Arguments, ProgramCounter, i);
                        ProgramCounter = NextInstruction;
                        writer.WriteLine(output);
                        IntermediateFile.AddLast(output);
                        Console.WriteLine(output);
                    }

                }
            }
            foreach(Literal Lit in LiteralTable)
            {
            string lineNumber = i.ToString(),  _locationCounter = ProgramCounter.ToString("X");;
            while (lineNumber.Length < 5)
            {
                lineNumber += " ";
            }
            while (_locationCounter.Length < 5)
            {
                _locationCounter = "0" + _locationCounter;
            }
                string literalPrint = lineNumber + _locationCounter + "    *          "  + Lit.Name;
                Console.WriteLine(literalPrint);
                writer.WriteLine(literalPrint);
                IntermediateFile.AddLast(literalPrint);

                i++;
                ProgramCounter+=Lit.Length;
            }
        }
        int ProgramLength = ProgramCounter - StartIndex;
        int menuChoice = 0;
        string menuInput = "";
        do
        {
            menuChoice = 0;
            Console.WriteLine("Enter the the corresponding input to view contents");
            Console.WriteLine("1 - Intermediate file");
            Console.WriteLine("2 - Symbol Table");
            Console.WriteLine("3 - Literal Table");
            Console.WriteLine("4 - Exit");

            menuInput = Console.ReadLine();
            int.TryParse(menuInput, out menuChoice);
            switch (menuChoice)
            {
                case 1:
                    //print intermediate file;
                    Console.Clear();
                    Console.WriteLine("Printing " + intermediateFile + "(intermediate file)");
                    foreach (string line in IntermediateFile)
                    {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("Program length: " + ProgramLength.ToString("X"));
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Symbol    RFlag   MFlag   IFlag   Value");
                    BST.LeftTraverse(SymbolTable.Root);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Literal    Value      Size   Address");
                    foreach (Literal Lit in LiteralTable)
                    {
                        Console.WriteLine(Lit.ToString());
                    }
                    break;
                default:
                    break;
            }
        } while (menuChoice != 4);

    }
}
