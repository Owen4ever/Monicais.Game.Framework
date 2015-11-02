
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;

namespace Monicais.Core
{

    [Serializable]
    public abstract class AbstractRole : NonNullDisplayable, IEntity
    {

        private List<IAction> actions;
        private EntityStatus status;

        protected AbstractRole(int id, string name, string desc, PropertyList properties) : base(name, desc)
        {
            ID = id;
            Properties = properties;
            status = EntityStatus.NormalStatus;
        }

        public void AddAction(IAction action)
        {
            if (action == null)
                ArgumentNull.Throw("action");
            actions.Add(action);
        }

        public void RemoveAction(IAction action)
        {
            actions.Remove(action);
        }

        public void AddCurrentAction(IAction action)
        {
            CurrentActions.Add(action);
        }

        public void RemoveCurrentAction(IAction action)
        {
            CurrentActions.Remove(action);
        }

        public void Suspend(string name)
        {
            IAction action = CurrentActions.Find(a => a.Name.Equals(name));
            if (action != null)
                action.Suspend(this);
        }

        public void SuspendAll()
        {
            if (CurrentActions.Count > 0)
                foreach (IAction action in CurrentActions)
                    action.Suspend(this);
        }

        public IEntity AttachedOn { get; set; }

        public void TestAndDo(IEntity entity, params object[] args)
        {
        }

        public void Update()
        {
            status.Update();
            Properties.Update();
            CurrentActions.ForEach(a => a.Update(this));
        }

        public List<IAction> CurrentActions { get; private set; }

        public int ID { get; private set; }

        public PropertyList Properties { get; private set; }

        public SkillList Skills { get; private set; }

        public EntityStatus Status
        {
            get { return status; }
            set
            {
                EntityStatus lastStatus = status;
                lastStatus.Deactivate(value, this);
                status = value;
                status.Activate(lastStatus, this);
            }
        }
    }
}
