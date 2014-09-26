using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    public class FullJoinGroup<TValue, TKey>
    {
        public TValue Left { get; set; }
        public TValue Right { get; set; }
        public TKey Key { get; set; }
    }
}
