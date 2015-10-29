namespace Monicais.Core
{
    using Monicais.Property;
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class AbstractRole : NonNullDisplayable, IEntity
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<IAction> <CurrentActions>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PropertyList <Properties>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SkillList <Skills>k__BackingField;
        private List<IAction> actions;
        private EntityStatus status;

        protected AbstractRole(int id, string name, string desc, PropertyList properties) : base(name, desc)
        {
            this.ID = id;
            this.Properties = properties;
            this.status = EntityStatus.NormalStatus;
        }

        public void AddAction(IAction action)
        {
            if (action == null)
            {
                ArgumentNull.Throw("action");
            }
            this.actions.Add(action);
        }

        public void AddCurrentAction(IAction action)
        {
            this.CurrentActions.Add(action);
        }

        public void RemoveAction(IAction action)
        {
            this.actions.Remove(action);
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

        public void TestAndDo(IEntity entity, params object[] args)
        {
        }

        public void Update()
        {
            this.status.Update();
            this.Properties.Update();
            this.CurrentActions.ForEach(<>c.<>9__26_0 ?? (<>c.<>9__26_0 = new Action<IAction>(<>c.<>9.<Update>b__26_0)));
        }

        public List<IAction> CurrentActions { get; private set; }

        public int ID { get; private set; }

        public PropertyList Properties { get; private set; }

        public SkillList Skills { get; private set; }

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

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly AbstractRole.<>c <>9 = new AbstractRole.<>c();
            public static Action<IAction> <>9__26_0;

            internal void <Update>b__26_0(IAction a)
            {
                a.Update();
            }
        }
    }
}

