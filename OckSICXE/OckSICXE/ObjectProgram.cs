using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OckSICXE
{
    internal class ObjectProgram
    {
        public ObjectProgram()
        {
            HeaderRecord = "";
            TextRecord = new LinkedList<string>();
        }
        public string HeaderRecord { get; set; }
        public LinkedList<string> TextRecord;
        public string EndDirective { get; set; }
        public static string GetHeaderDirective(string Symbol, int StartAddress, int ProgramSize)
        {
            //Console.WriteLine("H^" + Symbol + "^" + StartAddress.ToString("X") + "^"+ ProgramSize.ToString("X"));
            return "H^" + Symbol + "^" + StartAddress.ToString("X") + "^" + ProgramSize.ToString("X");
        }
        public string GetTextRecord(int StartAddress, int TextSize, LinkedList<String> TextRecord)
        {
            string Text = "";
            foreach(string t in TextRecord)
            {
                Text = Text + "^" + t;
            }
            Text = "T^" + StartAddress.ToString("X") + "^" + TextSize.ToString("X") + Text;
            return Text;
        }
    }
}
