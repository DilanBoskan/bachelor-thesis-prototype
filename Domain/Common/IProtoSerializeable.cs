using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;
public interface IProtoSerializeable<T, J> {
    abstract static T FromProto(J proto);
    abstract static J ToProto(T value);
}
