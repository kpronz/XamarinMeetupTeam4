using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.Core
{
    public interface IDataModel
    {
        Guid CorrelationID { get; set; }
        DateTime TimeStamp { get; set; }
        bool MarkedForDelete { get; set; }
    }
    public class DataModel : ObservableObject, IDataModel, ICloneable, IDisposable
    {
        public Guid CorrelationID { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool MarkedForDelete { get; set; }

        public object Clone()
        {
            var obj = Activator.CreateInstance(this.GetType());
            foreach (var prop in this.GetType().GetProperties())
                prop.SetValue(obj, prop.GetValue(this));
            return obj;

        }

        public virtual void Dispose() { }

    }
}
