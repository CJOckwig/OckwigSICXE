using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OckSICXE
{
    internal class Opcode
    {
        public string Mnemonic { get; set; }
        public int Format { get; set; }
        public int Hex { get; set; }

        public Opcode(string inputLine)
        {
            if(string.IsNullOrEmpty(inputLine))
            {
                throw new ArgumentNullException("Input for opcode from opcodes file caused an error. " + inputLine);
            }
            inputLine = inputLine.Trim();
            string[] strings = inputLine.Split(' ');

            Mnemonic = strings[0];

            int temp = 0;
            int.TryParse(strings[2], out temp);
            Format = temp;

            Hex = Convert.ToInt32(strings[1],16);

            //Hex[0] = Byte.Parse("" + ops[0]);
            //Hex[1] = Byte.Parse("" + ops[1]);
        }
        public override string ToString()
        {
            string NameBuffer = Mnemonic;
            while (NameBuffer.Length <= 10)
                NameBuffer += " ";
            string HexBuffer = Hex < 16 ? "0" + Hex.ToString("X") : Hex.ToString("X");
            NameBuffer = NameBuffer + HexBuffer + "    " + Format;

            return NameBuffer;
        }
    }
}
