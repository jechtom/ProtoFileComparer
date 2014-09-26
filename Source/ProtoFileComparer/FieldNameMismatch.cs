using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    public class FieldNameMismatch
    {
        public FieldNameMismatch(string typeName, int fieldNumber, string leftName, string rightName)
        {
            this.TypeName =typeName;
            this.FieldNumber = fieldNumber;
            this.LeftName = leftName;
            this.RightName = rightName;
        }

        public string TypeName { get; set; }
        public int FieldNumber { get; set; }
        public string FieldName { get; set; }
        public string LeftName { get; set; }
        public string RightName { get; set; }

        public override string ToString()
        {
            return string.Format("Type [{0}] -> Field #{1} -> Mismatch name; Left=\"{2}\", Right=\"{3}\"", 
                TypeName, FieldNumber, LeftName, RightName);
        }
    }
}
