namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    [Serializable]
    public sealed class SimplePropertyCreater : IPropertyCreater
    {
        private AfterPropertyListCreate AfterCreates = (<>c.<>9__0_0 ?? (<>c.<>9__0_0 = new AfterPropertyListCreate(<>c.<>9.<.ctor>b__0_0)));
        private Dictionary<PropertyID, CreateProperty> creaters = new Dictionary<PropertyID, CreateProperty>();
        private Dictionary<PropertyID, SetRestrictor> parents = new Dictionary<PropertyID, SetRestrictor>();
        public static readonly CreateProperty RestorablePropertyCreater = new CreateProperty(<>c.<>9.<.cctor>b__24_1);
        public static readonly CreateProperty UnrecoverablePropertyCreater = new CreateProperty(<>c.<>9.<.cctor>b__24_0);

        public void AddAfterCreate(AfterPropertyListCreate afterCreate)
        {
            if (afterCreate == null)
            {
                ArgumentNull.Throw("AfterCreate");
            }
            this.AfterCreates = (AfterPropertyListCreate) Delegate.Combine(this.AfterCreates, afterCreate);
        }

        public PropertyList Create(List<PropertyID> ids)
        {
            if (ids == null)
            {
                ArgumentNull.Throw("ids");
            }
            Dictionary<PropertyID, IProperty> instance = new Dictionary<PropertyID, IProperty>();
            foreach (PropertyID yid in ids)
            {
                instance.Add(yid, this.creaters[yid](yid));
            }
            foreach (KeyValuePair<PropertyID, SetRestrictor> pair in this.parents)
            {
                pair.Value.SetRestrictor(instance[pair.Key], instance);
            }
            PropertyList properties = new PropertyList(instance.Values.ToArray<IProperty>());
            this.AfterCreates(properties);
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
            {
                ArgumentNull.Throw("AfterCreate");
            }
            this.AfterCreates = (AfterPropertyListCreate) Delegate.Remove(this.AfterCreates, afterCreate);
        }

        public void RemoveParentFor(PropertyID child)
        {
            if (child == null)
            {
                ArgumentNull.Throw("child");
            }
            this.parents.Remove(child);
        }

        public void setCreaterFor(PropertyID pid, CreateProperty creater)
        {
            if (pid == null)
            {
                ArgumentNull.Throw("pid");
            }
            if (creater == null)
            {
                ArgumentNull.Throw("creater");
            }
            this.creaters.Add(pid, creater);
        }

        public void SetParentFor(PropertyID child, SetRestrictorForProperty setter)
        {
            if (child == null)
            {
                ArgumentNull.Throw("child");
            }
            if (setter == null)
            {
                ArgumentNull.Throw("setRestrictor");
            }
            this.parents.Add(child, new SetRestrictor_DelegateSetter(setter));
        }

        public void SetParentFor(PropertyID child, PropertyID min, PropertyID max)
        {
            if (child == null)
            {
                ArgumentNull.Throw("child");
            }
            if (min == null)
            {
                ArgumentNull.Throw("min");
            }
            if (max == null)
            {
                ArgumentNull.Throw("max");
            }
            this.parents.Add(child, new SetRestrictor_Property_Property(min, max));
        }

        public void SetParentFor(PropertyID child, PropertyID min, int max)
        {
            if (child == null)
            {
                ArgumentNull.Throw("child");
            }
            if (min == null)
            {
                ArgumentNull.Throw("min");
            }
            this.parents.Add(child, new SetRestrictor_Property_Int(min, max));
        }

        public void SetParentFor(PropertyID child, int min, PropertyID max)
        {
            if (child == null)
            {
                ArgumentNull.Throw("child");
            }
            if (max == null)
            {
                ArgumentNull.Throw("max");
            }
            this.parents.Add(child, new SetRestrictor_Int_Property(min, max));
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SimplePropertyCreater.<>c <>9 = new SimplePropertyCreater.<>c();
            public static AfterPropertyListCreate <>9__0_0;

            internal IProperty <.cctor>b__24_0(PropertyID pid)
            {
                return new UnrecoverableProperty(pid);
            }

            internal IProperty <.cctor>b__24_1(PropertyID pid)
            {
                return new RestorableProperty(pid);
            }

            internal void <.ctor>b__0_0(PropertyList ps)
            {
            }
        }

        [Serializable]
        public delegate IProperty CreateProperty(PropertyID pid);

        internal interface SetRestrictor
        {
            void SetRestrictor(IProperty child, Dictionary<PropertyID, IProperty> instance);
        }

        [Serializable]
        internal sealed class SetRestrictor_DelegateSetter : Monicais.Property.SimplePropertyCreater.SetRestrictor
        {
            private SimplePropertyCreater.SetRestrictorForProperty setter;

            public SetRestrictor_DelegateSetter(SimplePropertyCreater.SetRestrictorForProperty setter)
            {
                this.setter = setter;
            }

            public void SetRestrictor(IProperty child, Dictionary<PropertyID, IProperty> instance)
            {
                this.setter(child, instance);
            }
        }

        [Serializable]
        internal sealed class SetRestrictor_Int_Property : Monicais.Property.SimplePropertyCreater.SetRestrictor
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
                child.SetRestrictor(this.min, instance[this.max]);
            }
        }

        [Serializable]
        internal sealed class SetRestrictor_Property_Int : Monicais.Property.SimplePropertyCreater.SetRestrictor
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
                child.SetRestrictor(instance[this.min], this.max);
            }
        }

        [Serializable]
        internal sealed class SetRestrictor_Property_Property : Monicais.Property.SimplePropertyCreater.SetRestrictor
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
                child.SetRestrictor(instance[this.min], instance[this.max]);
            }
        }

        public delegate void SetRestrictorForProperty(IProperty child, Dictionary<PropertyID, IProperty> instance);
    }
}

