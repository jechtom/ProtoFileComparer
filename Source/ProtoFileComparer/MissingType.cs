using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    public class MissingType
    {
        public MissingType(string name, CompareSource compareSource)
        {
            this.Name = name;
            this.Source = compareSource;
        }
        public string Name { get; set; }
        public CompareSource Source { get; set; }

        public override string ToString()
        {
            return string.Format("Type [{0}] -> Missing in: {1} source", Name, Source);
        }
    }
}
