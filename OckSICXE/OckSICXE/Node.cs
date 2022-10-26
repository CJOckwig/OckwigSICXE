using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OckSICXE
{
    public class Node
    {
        public Node(string symbol, bool rFlag, int value, Node left = null, Node right = null, bool mFlag = false, bool iFlag = true)
        {
            Left = left;
            Right = right;
            Symbol = symbol;
            Value = value;
            RFlag = rFlag;
            IFlag = iFlag;
            MFlag = mFlag;
        }

        public Node? Left { get; set; }
        public Node? Right { get; set; }
        public string Symbol { get; set; }
        public int Value { get; set; }
        public bool RFlag { get; set; }
        public bool IFlag { get; set; }
        public bool MFlag { get; set; }
        public void PrintNode()
        {
            string s = "", buffer = "", rBuffer = "   ", mBuffer = "   ", iBuffer = "   ";
            int bufferlength = 10 - Symbol.Length;
            for (int i = 0; i < bufferlength; i++)
                buffer += " ";
            if (RFlag)
            {
                rBuffer += " ";
            }
            if (MFlag)
            {
                mBuffer += " ";
            }
            if (IFlag)
            {
                iBuffer += " ";
            }
            s = Symbol + buffer + RFlag + rBuffer + MFlag + mBuffer + IFlag + iBuffer + Value.ToString("X");
            Console.WriteLine(s);
            return;

        }

    }
}
