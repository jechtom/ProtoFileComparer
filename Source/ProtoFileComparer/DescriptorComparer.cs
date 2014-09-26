using Google.ProtocolBuffers.DescriptorProtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    public class DescriptorComparer
    {
        public IEnumerable<object> Compare(FileDescriptorSet d1,FileDescriptorSet d2)
        {
            if (d1 == null)
                throw new ArgumentNullException("d1");

            if (d2 == null)
                throw new ArgumentNullException("d2");

            // expand nested types from files and types
            var d1types = ExpandTypes(d1);
            var d2types = ExpandTypes(d2);

            // compare by name (missing left, missing right, both present)
            var typeCompare = d1types.FullJoinSingleRow(d2types, d => d.Name, d => d).ToArray();

            foreach (var item in typeCompare)
            {
                if (item.Left == null) // missing in left source
                    yield return new MissingType(item.Key, CompareSource.Left);
                else if (item.Right == null) // missing in right source
                    yield return new MissingType(item.Key, CompareSource.Right);
                else // compare type
                    foreach (var diff in CompareTypes(item.Left, item.Right))
                        yield return diff;
            }
        }

        private IEnumerable<object> CompareTypes(DescriptorProto left, DescriptorProto right)
        {
            var fieldsDiff = left.FieldList.FullJoinSingleRow(right.FieldList, f => f.Number, f=>f);
            foreach (var fd in fieldsDiff)
            {
                if (fd.Left == null)
                    yield return new MissingField(right.Name, fd.Right.Name, fd.Right.Number, CompareSource.Left);
                else if (fd.Right == null)
                    yield return new MissingField(left.Name, fd.Left.Name, fd.Left.Number, CompareSource.Right);
                else // compare field
                    foreach (var diff in CompareField(fd.Left, left, fd.Right, right))
	                    yield return diff;
            }
        }

        private IEnumerable<object> CompareField(FieldDescriptorProto left, DescriptorProto leftType, FieldDescriptorProto right, DescriptorProto rightType)
        {
            if (left.Name != right.Name)
                yield return new FieldNameMismatch(leftType.Name, left.Number, left.Name, right.Name);

            if (left.Type != right.Type)
                yield return new FieldMismatch(leftType.Name, left.Name, left.Number, "Type", left.Type, right.Type);

            if (left.TypeName != right.TypeName)
                yield return new FieldMismatch(leftType.Name, left.Name, left.Number, "TypeName", left.TypeName, right.TypeName);

            if (left.DefaultValue != right.DefaultValue)
                yield return new FieldMismatch(leftType.Name, left.Name, left.Number, "DefaultValue", left.DefaultValue, right.DefaultValue);
        }

        private IEnumerable<DescriptorProto> ExpandTypes(FileDescriptorSet set)
        {
            return set.FileList.SelectMany(f => f.MessageTypeList);
        }
    }
}
