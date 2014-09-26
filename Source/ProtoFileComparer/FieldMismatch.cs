using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    public class FieldMismatch
    {
        public FieldMismatch(string typeName, string fieldName, int fieldNumber, string propertyName, object leftValue, object rightValue)
        {
            this.TypeName = typeName;
            this.FieldName = fieldName;
            this.FieldNumber = fieldNumber;
            this.PropertyName = propertyName;
            this.LeftValue = leftValue;
            this.RightValue = rightValue;
        }

        public string TypeName { get; set; }
        public int FieldNumber { get; set; }
        public string FieldName { get; set; }
        public string PropertyName { get; set; }
        public object LeftValue { get; set; }
        public object RightValue { get; set; }

        public override string ToString()
        {
            return string.Format("Type [{0}] -> Field [{2}] -> Property [{3}] -> Mismatch; Left = \"{4}\", Right = \"{5}\"", 
                TypeName, 
                FieldNumber,
                FieldName,
                PropertyName,
                LeftValue ?? "NULL",
                RightValue ?? "NULL"
                );
        }
    }
}
