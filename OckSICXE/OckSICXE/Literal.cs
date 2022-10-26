using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OckSICXE
{
    internal class Literal
    {
        public Literal(string Name = "", string Value = "", int Length = 0, int Address = 0, char Type = ' ')
        {
            this.Name = Name;
            this.Value = Value;
            this.Length = Length;
            this.Address = Address;
            this.Type = Type;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Length { get; set; }
        public int Address { get; set; }
        public char Type { get; set; }
        public override string ToString()
        {
            string NameBuffer = Name, ValueBuffer = Value;
            while (NameBuffer.Length <= 10)
                NameBuffer += " ";
            while (ValueBuffer.Length <= 10)
                ValueBuffer += " ";
            string output = NameBuffer + ValueBuffer + Length + "      " + Address;
            return output;
        }
        //test

    }
}
