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
        string path = "OckSICXE" + Path.DirectorySeparatorChar;
        string SICProgram = (args.Length == 1) ? args[0] : null;
        while (string.IsNullOrEmpty(SICProgram))
        {
            Console.Write("Please Enter the name of the SIC Program to read: ");
            SICProgram = Console.ReadLine();
        }
        string[] OpcodeLines = FileFormat.GetFileLines(path + "opcodes.txt");
        string[] SICProgramLines = FileFormat.GetFileLines(path + SICProgram);

        ArrayList LiteralTable = new ArrayList();
        LinkedList<string> IntermediateFile = new LinkedList<string>();
        LinkedList<string> IntermediateLine = new LinkedList<string>();
        BST SymbolTable = new();
        ArrayList OpcodeTable = new();
        foreach (string OpCodeLine in OpcodeLines)
        {
            Opcode Op = new(OpCodeLine);
            OpcodeTable.Add(Op);
        }
        string intermediateFile = SICProgram.Substring(0, SICProgram.IndexOf('.'));
        int StartIndex = 0;
        int ProgramCounter = 0, NextInstruction = 0; ;
        int TrimmedLineIndex = 0, SymbolIndex = 1, OpcodeIndex = 2, OperandIndex = 3, CommentIndex = 4;
        int ExpressionValue = 0;
        int i;
        bool RFlag = true;
        string[] Arguments = Instruction.FormatLine(SICProgramLines[0]);
        using (StreamWriter writer = new StreamWriter(path + intermediateFile + ".int"))
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
                        bool tempbit = true;
                        ExpressionValue = ExVal.EvalExpression(Arguments[OperandIndex], out LiteralTable, LiteralTable, SymbolTable, out tempbit, out tempbit, out tempbit);
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

                        NextInstruction += ExpressionValue;
                        ValidDirective = true;

                    }
                    else if ((Arguments[OpcodeIndex].Equals("BASE")))
                    {
                        ValidDirective = true;

                    }
                    else if ((Arguments[OpcodeIndex].Equals("EQU")))
                    {
                        bool tempbit = true;
                        ProgramCounter = ExVal.EvalExpression(Arguments[OperandIndex], out LiteralTable, LiteralTable, SymbolTable, out tempbit, out tempbit, out tempbit);
                        SymbolTable.Search(Arguments[SymbolIndex]).Value = ProgramCounter;
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
                    if (ValidDirective)
                    {
                        string output = FileFormat.FileLine(Arguments, ProgramCounter, i);
                        ProgramCounter = NextInstruction;

                        writer.WriteLine(output);
                        IntermediateFile.AddLast(output);
                        Console.WriteLine(output);
                    }

                }
            }
            foreach (Literal Lit in LiteralTable)
            {
                string lineNumber = i.ToString(), _locationCounter = ProgramCounter.ToString("X"); ;
                while (lineNumber.Length < 5)
                {
                    lineNumber += " ";
                }
                while (_locationCounter.Length < 5)
                {
                    _locationCounter = "0" + _locationCounter;
                }
                string literalPrint = lineNumber + _locationCounter + "    *          " + Lit.Name;
                //Console.WriteLine(literalPrint);
                writer.WriteLine(literalPrint);
                IntermediateFile.AddLast(literalPrint);

                i++;
                ProgramCounter += Lit.Length;
            }
            writer.Close();
        }




        // END      PASS 1

        // STARTING PASS 2

        string[] pass2Lines = { };
        try
        {
            pass2Lines = File.ReadAllLines(path + intermediateFile + ".int");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        //FIX the intermediate file
        using (StreamWriter writerObj = new StreamWriter(path + intermediateFile + ".obj"))
        {
            using (StreamWriter writerInt = new StreamWriter(path + intermediateFile + ".txt"))
            {
                string fullLine = pass2Lines[0];
                string[] line = FileFormat.InterFileFormat(pass2Lines[0]);
                // 0 line number
                // 1 PC Counter
                // 2 Symbol
                // 3 opcode
                // 4 expressions
                // 5 comment
                bool NBit = false, IBit = false, XBit = false, BBit = false, PBit = false, EBit = false;
                bool NewTextRecord = true;
                string TextRecord = "";

                string bits;
                string objectCodeFull = "";
                int objectCode = 0, addressType = 0, FormatType = 0, TargetAddress = 0, Displacement = 0, mid = 0;
                int textLength = 0;
                NextInstruction = 0;
                LinkedList<string> TextRecords = new LinkedList<string>();
                line = FileFormat.InterFileFormat(pass2Lines[0]);
                writerObj.WriteLine("H^" + line[2] + "^" + line[4] + "^" + ProgramCounter);


                for (int k = 1; k < pass2Lines.Length; k++)
                {
                    fullLine = pass2Lines[k];
                    objectCodeFull = "";
                    line = FileFormat.InterFileFormat(pass2Lines[k]);
                    FormatType = 0;
                    TargetAddress = 0;
                    EBit = false;
                    //if each line is not a comment line
                    if (line[3].StartsWith("+"))
                    {
                        EBit = true;
                        line[3] = line[3].Trim('+');
                        textLength++;
                        NextInstruction++;
                    }
                    foreach (Opcode O in OpcodeTable)
                    {
                        if (O.Mnemonic.Equals(line[3]))
                        {
                            NextInstruction += O.Format;
                            FormatType = O.Format;
                            objectCode = O.Hex;
                            textLength += O.Format;
                        }
                    }
                    if (line[OpcodeIndex].Equals("WORD"))
                    {
                        NextInstruction += 3;
                        NewTextRecord = true;

                    }
                    else if (line[OpcodeIndex].Equals("RESW"))
                    {
                        NewTextRecord = true;

                    }
                    else if (line[OpcodeIndex].Equals("RESB"))
                    {
                        NewTextRecord = true;

                    }
                    else if (line[OpcodeIndex].Equals("BYTE"))
                    {
                        NewTextRecord = true;

                    }
                    else if (line[OpcodeIndex].Equals("BASE"))
                    {
                        NewTextRecord = true;
                    }
                    else if ((line[OpcodeIndex].Equals("EQU")))
                    {
                        NewTextRecord = true;

                    }
                    else if (line[OpcodeIndex].Equals("EXTDEF"))
                    {
                        NewTextRecord = true;
                    }
                    else if (line[OpcodeIndex].Equals("EXTREF"))
                    {
                        NewTextRecord = true;
                    }
                    else if (line[OpcodeIndex].Equals("END"))
                    {
                        NewTextRecord = true;
                    }
                    addressType = ExVal.CheckLine(line[4]);
                    if (FormatType >= 3)
                    {
                        //public static int EvalExpression(string Expression, out ArrayList LiteralTable, ArrayList LitTableIn, BST SymbolTable)
                        TargetAddress = ExVal.EvalExpression(line[4], out LiteralTable, LiteralTable, SymbolTable, out NBit, out IBit, out XBit);
                        objectCode += addressType;

                        //Base register here
                        if (FormatType == 4)
                        {
                            BBit = false;
                            PBit = false;
                            Displacement = TargetAddress;
                            //Put together text record

                        }
                        else
                        {
                            PBit = true;
                            BBit = false;
                            Console.WriteLine(TargetAddress + "-" + NextInstruction);
                            Displacement = TargetAddress - NextInstruction;
                        }
                        mid = FileFormat.middleFormat(XBit, BBit, PBit, EBit);
                        objectCodeFull = objectCode.ToString("X") + mid.ToString("X") + Displacement.ToString("X");
                        if (NewTextRecord)
                        {
                            TextRecord = "T^" + line[1] + "^" + TextRecord;//add size
                            writerObj.WriteLine(TextRecord);
                            TextRecords.AddLast(objectCodeFull);
                            TextRecord = "^" + objectCodeFull;
                        }
                    }
                    else if (FormatType == 2)
                    {

                        TextRecord = objectCode.ToString("X");
                        //split the registers in expression. X is already trimmed out? maybe? No.

                        if (NewTextRecord)
                        {
                            TextRecord = "T^" + textLength + "^" + line[1] + "^" + TextRecord;
                            TextRecord = "^" + objectCodeFull;
                        }
                    }
                    fullLine = fullLine + objectCodeFull;
                    writerInt.WriteLine(fullLine);

                }

            }
            writerObj.WriteLine("E^000000");

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
            Console.WriteLine("4 - Text File");
            Console.WriteLine("5 - Object File");
            Console.WriteLine("6 - Exit");

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

