namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [Serializable]
    public sealed class PropertyListManager
    {
        private int propertyIDIndex = 0;
        private readonly List<PropertyID> propertyIDs = new List<PropertyID>();
        private readonly PropertyManager propertyMgr;

        internal PropertyListManager(PropertyManager mgr)
        {
            this.propertyMgr = mgr;
        }

        public void ClearPropertyIDs()
        {
            this.propertyIDs.Clear();
            this.propertyIDIndex = 0;
        }

        public PropertyList CreateProperties(IPropertyCreater creater)
        {
            return this.CreateProperties(new PropertyCreater(creater.Create));
        }

        public PropertyList CreateProperties(PropertyCreater creater)
        {
            return creater(this.propertyIDs);
        }

        public PropertyID NewPropertyID(string id, string name, string description)
        {
            if (this.propertyMgr.ContainsID(id))
            {
                Argument.Throw("repeated id");
            }
            PropertyID item = new PropertyID(id, name, description);
            this.propertyIDs.Add(item);
            int propertyIDIndex = this.propertyIDIndex;
            this.propertyIDIndex = propertyIDIndex + 1;
            item.Index = propertyIDIndex;
            this.propertyMgr.AddNewPropertyID(item);
            return item;
        }

        public PropertyID this[string pid]
        {
            get
            {
                return this.propertyIDs.Find(p => p.ID.Equals(pid));
            }
        }

        public delegate PropertyList PropertyCreater(List<PropertyID> ids);
    }
}

