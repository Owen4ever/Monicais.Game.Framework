namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public sealed class PropertyManager
    {
        private readonly Dictionary<string, PropertyListManager> listMgrs = new Dictionary<string, PropertyListManager>();
        private readonly List<PropertyID> propertyIDs = new List<PropertyID>();
        private readonly HashSet<string> stringIDs = new HashSet<string>();

        internal void AddNewPropertyID(PropertyID pid)
        {
            if (this.ContainsID(pid.ID))
            {
                Argument.Throw("repeated id");
            }
            this.stringIDs.Add(pid.ID);
            this.propertyIDs.Add(pid);
        }

        internal bool ContainsID(string id)
        {
            return this.stringIDs.Contains(id);
        }

        public int GetPropertyIndex(string key, string pid)
        {
            return this[key][pid].Index;
        }

        public PropertyListManager NewPropertyListManager(string key)
        {
            if (this.listMgrs.ContainsKey(key))
            {
                Argument.Throw("repeated key");
            }
            PropertyListManager manager = new PropertyListManager(this);
            this.listMgrs[key] = manager;
            return manager;
        }

        public PropertyListManager this[string key]
        {
            get
            {
                if (this.listMgrs.ContainsKey(key))
                {
                    return this.listMgrs[key];
                }
                return null;
            }
        }

        public Dictionary<string, PropertyListManager>.KeyCollection PropertyListNames
        {
            get
            {
                return this.listMgrs.Keys;
            }
        }
    }
}

