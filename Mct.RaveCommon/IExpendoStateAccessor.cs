using System.Collections.Generic;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public interface IExpendoStateAccessor
    {
        IEnumerable<string> Keys { get; }
        IExpendoStateAccessor Set(string name, object value);
        object Get(string name);
        IExpendoStateAccessor Remove(string name);
        IExpendoStateAccessor RemoveAll();
        bool Exists(string name);
    }
}