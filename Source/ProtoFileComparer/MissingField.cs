using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    public class MissingField
    {
        public MissingField(string typeName, string fieldName, int fieldNumber, CompareSource compareSource)
        {
            this.TypeName =typeName;
            this.FieldName = fieldName;
            this.FieldNumber = fieldNumber;
            this.Source = compareSource;
        }

        public string TypeName { get; set; }
        public int FieldNumber { get; set; }
        public string FieldName { get; set; }
        public CompareSource Source { get; set; }

        public override string ToString()
        {
            return string.Format("Type [{0}] -> Field #{1} ([{2}]) -> Missing in: {3} source", TypeName, FieldNumber, FieldName, Source);
        }
    }
}
