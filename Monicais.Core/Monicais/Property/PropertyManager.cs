
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monicais.Property
{

    [Serializable]
    public sealed class PropertyManager
    {
        private readonly Dictionary<string, PropertyListManager> listMgrs = new Dictionary<string, PropertyListManager>();
        private readonly List<PropertyID> propertyIDs = new List<PropertyID>();
        private readonly HashSet<string> stringIDs = new HashSet<string>();

        internal void AddNewPropertyID(PropertyID pid)
        {
            if (ContainsID(pid.ID))
                Argument.Throw("repeated id");
            stringIDs.Add(pid.ID);
            propertyIDs.Add(pid);
        }

        internal bool ContainsID(string id)
        {
            return stringIDs.Contains(id);
        }

        public int GetPropertyIndex(string key, string pid)
        {
            return this[key][pid].Index;
        }

        public PropertyListManager NewPropertyListManager(string key)
        {
            if (listMgrs.ContainsKey(key))
                Argument.Throw("repeated key");
            PropertyListManager manager = new PropertyListManager(this);
            listMgrs[key] = manager;
            return manager;
        }

        public PropertyListManager this[string key]
        {
            get
            {
                if (listMgrs.ContainsKey(key))
                    return listMgrs[key];
                return null;
            }
        }

        public Dictionary<string, PropertyListManager>.KeyCollection PropertyListNames
        {
            get { return listMgrs.Keys; }
        }
    }

    [Serializable]
    public sealed class PropertyListManager
    {
        private int propertyIDIndex = 0;
        private readonly List<PropertyID> propertyIDs = new List<PropertyID>();
        private readonly PropertyManager propertyMgr;

        internal PropertyListManager(PropertyManager mgr)
        {
            propertyMgr = mgr;
        }

        public void ClearPropertyIDs()
        {
            propertyIDs.Clear();
            propertyIDIndex = 0;
        }

        public PropertyList CreateProperties(IPropertyCreater creater)
        {
            return CreateProperties(new PropertyCreater(creater.Create));
        }

        public PropertyList CreateProperties(PropertyCreater creater)
        {
            return creater(propertyIDs);
        }

        public PropertyID NewPropertyID(string id, string name, string description)
        {
            if (propertyMgr.ContainsID(id))
                Argument.Throw("repeated id");
            PropertyID item = new PropertyID(id, name, description);
            propertyIDs.Add(item);
            item.Index = propertyIDIndex++;
            propertyMgr.AddNewPropertyID(item);
            return item;
        }

        public PropertyID this[string pid]
        {
            get
            {
                return propertyIDs.Find(p => p.ID.Equals(pid));
            }
        }

        public delegate PropertyList PropertyCreater(List<PropertyID> ids);
    }

    public delegate void AfterPropertyListCreate(PropertyList properties);

    public interface IPropertyCreater
    {

        void AddAfterCreate(AfterPropertyListCreate afterCreate);

        PropertyList Create(List<PropertyID> ids);

        void RemoveAfterCreate(AfterPropertyListCreate afterCreate);
    }

    [Serializable]
    public sealed class SimplePropertyCreater : IPropertyCreater
    {

        private AfterPropertyListCreate AfterCreates = pl => { };
        private Dictionary<PropertyID, CreateProperty> creaters = new Dictionary<PropertyID, CreateProperty>();
        private Dictionary<PropertyID, SetRestrictor> parents = new Dictionary<PropertyID, SetRestrictor>();

        public static readonly CreateProperty RestorablePropertyCreater = new CreateProperty(pid => new RestorableProperty(pid));
        public static readonly CreateProperty UnrecoverablePropertyCreater = new CreateProperty(pid => new UnrecoverableProperty(pid));

        public void AddAfterCreate(AfterPropertyListCreate afterCreate)
        {
            if (afterCreate == null)
                ArgumentNull.Throw("AfterCreate");
            AfterCreates += afterCreate;
        }

        public PropertyList Create(List<PropertyID> ids)
        {
            if (ids == null)
                ArgumentNull.Throw("ids");
            Dictionary<PropertyID, IProperty> instance = new Dictionary<PropertyID, IProperty>();
            foreach (PropertyID yid in ids)
                instance.Add(yid, this.creaters[yid](yid));
            foreach (KeyValuePair<PropertyID, SetRestrictor> pair in this.parents)
                pair.Value.SetRestrictor(instance[pair.Key], instance);
            PropertyList properties = new PropertyList(instance.Values.ToArray());
            AfterCreates(properties);
            return properties;
        }

        public static CreateProperty GetRestorablePropertyCreater(int val)
        {
            return pid => new RestorableProperty(pid, val);
        }

        public static CreateProperty GetUnrecoverablePropertyCreater(int val)
        {
            return pid => new UnrecoverableProperty(pid, val);
        }

        public void RemoveAfterCreate(AfterPropertyListCreate afterCreate)
        {
            if (afterCreate == null)
                ArgumentNull.Throw("AfterCreate");
            AfterCreates += afterCreate;
        }

        public void RemoveParentFor(PropertyID child)
        {
            if (child == null)
                ArgumentNull.Throw("child");
            parents.Remove(child);
        }

        public void setCreaterFor(PropertyID pid, CreateProperty creater)
        {
            if (pid == null)
                ArgumentNull.Throw("pid");
            if (creater == null)
                ArgumentNull.Throw("creater");
            creaters.Add(pid, creater);
        }

        public void SetParentFor(PropertyID child, SetRestrictorForProperty setter)
        {
            if (child == null)
                ArgumentNull.Throw("child");
            if (setter == null)
                ArgumentNull.Throw("setRestrictor");
            parents.Add(child, new SetRestrictor_DelegateSetter(setter));
        }

        public void SetParentFor(PropertyID child, PropertyID min, PropertyID max)
        {
            if (child == null)
                ArgumentNull.Throw("child");
            if (min == null)
                ArgumentNull.Throw("min");
            if (max == null)
                ArgumentNull.Throw("max");
            parents.Add(child, new SetRestrictor_Property_Property(min, max));
        }

        public void SetParentFor(PropertyID child, PropertyID min, int max)
        {
            if (child == null)
                ArgumentNull.Throw("child");
            if (min == null)
                ArgumentNull.Throw("min");
            parents.Add(child, new SetRestrictor_Property_Int(min, max));
        }

        public void SetParentFor(PropertyID child, int min, PropertyID max)
        {
            if (child == null)
                ArgumentNull.Throw("child");
            if (max == null)
                ArgumentNull.Throw("max");
            parents.Add(child, new SetRestrictor_Int_Property(min, max));
        }

        [Serializable]
        public delegate IProperty CreateProperty(PropertyID pid);

        internal interface SetRestrictor
        {
            void SetRestrictor(IProperty child, Dictionary<PropertyID, IProperty> instance);
        }

        [Serializable]
        internal sealed class SetRestrictor_DelegateSetter : SetRestrictor
        {
            private SetRestrictorForProperty setter;

            public SetRestrictor_DelegateSetter(SetRestrictorForProperty setter)
            {
                this.setter = setter;
            }

            public void SetRestrictor(IProperty child, Dictionary<PropertyID, IProperty> instance)
            {
                setter(child, instance);
            }
        }

        [Serializable]
        internal sealed class SetRestrictor_Int_Property : SetRestrictor
        {
            private PropertyID max;
            private int min;

            public SetRestrictor_Int_Property(int min, PropertyID pid)
            {
                this.min = min;
                this.max = pid;
            }

            public void SetRestrictor(IProperty child, Dictionary<PropertyID, IProperty> instance)
            {
                child.SetRestrictor(min, instance[max]);
            }
        }

        [Serializable]
        internal sealed class SetRestrictor_Property_Int : SetRestrictor
        {
            private int max;
            private PropertyID min;

            public SetRestrictor_Property_Int(PropertyID pid, int max)
            {
                this.min = pid;
                this.max = max;
            }

            public void SetRestrictor(IProperty child, Dictionary<PropertyID, IProperty> instance)
            {
                child.SetRestrictor(instance[min], max);
            }
        }

        [Serializable]
        internal sealed class SetRestrictor_Property_Property : SetRestrictor
        {
            private PropertyID max;
            private PropertyID min;

            public SetRestrictor_Property_Property(PropertyID min, PropertyID max)
            {
                this.min = min;
                this.max = max;
            }

            public void SetRestrictor(IProperty child, Dictionary<PropertyID, IProperty> instance)
            {
                child.SetRestrictor(instance[min], instance[max]);
            }
        }

        public delegate void SetRestrictorForProperty(IProperty child, Dictionary<PropertyID, IProperty> instance);
    }
}
