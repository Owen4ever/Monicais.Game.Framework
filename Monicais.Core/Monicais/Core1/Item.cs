namespace Monicais.Core
{
    using Monicais.Property;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Item : NonNullDisplayable, IEntity
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<IAction> <CurrentActions>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PropertyList <Properties>k__BackingField;
        private EntityStatus status;

        public Item(int id, string name, string desc, PropertyList properties) : base(name, desc)
        {
            this.ID = id;
            this.Properties = properties;
            this.status = EntityStatus.NormalStatus;
        }

        public void AddAction(IAction action)
        {
        }

        public void AddCurrentAction(IAction action)
        {
            this.CurrentActions.Add(action);
        }

        public void RemoveAction(IAction action)
        {
        }

        public void RemoveCurrentAction(IAction action)
        {
            this.CurrentActions.Remove(action);
        }

        public void Suspend(string action)
        {
            IAction action2 = this.CurrentActions.Find(a => a.Name.Equals(action));
            if (action2 > null)
            {
                action2.Suspend(this);
            }
        }

        public void SuspendAll()
        {
            if (this.CurrentActions.Count > 0)
            {
                foreach (IAction action in this.CurrentActions)
                {
                    action.Suspend(this);
                }
            }
        }

        public void Update()
        {
        }

        public List<IAction> CurrentActions { get; private set; }

        public int ID { get; private set; }

        public PropertyList Properties { get; private set; }

        public EntityStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                EntityStatus lastStatus = this.status;
                lastStatus.Deactivate(value, this);
                this.status = value;
                this.status.Activate(lastStatus, this);
            }
        }
    }
}

