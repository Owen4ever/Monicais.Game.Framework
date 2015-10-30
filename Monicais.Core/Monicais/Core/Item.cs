
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;

namespace Monicais.Core
{

    [Serializable]
    public class ItemType : NonNullDisplayable
    {
        public static ItemType BaseItemType = new ItemType();

        private ItemType() : base("") { parentType = null; }

        public ItemType(string name, string description = "") : this(name, description, BaseItemType) { }

        public ItemType(string name, string description, ItemType parent) : base(name, description)
        {
            ParentType = parent;
        }

        public ItemType(string name, ItemType parent) : this(name, "", parent) { }

        public bool IsBaseType { get { return parentType == null; } }

        public ItemType ParentType
        {
            get { return ParentType; }
            set
            {
                if (parentType == null)
                    ArgumentNull.Throw("ParentItmeType");
                else
                    parentType = value;
            }
        }
        private ItemType parentType;

        public IEntity AttachedOn { get; set; }
    }

    [Serializable]
    public class Item : NonNullDisplayable, IEntity
    {

        private List<IAction> actions;
        private EntityStatus status;

        public Item(int id, string name, string desc, PropertyList properties) : base(name, desc)
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

        public void Suspend(string actionName)
        {
            IAction action = CurrentActions.Find(a => a.Name.Equals(actionName));
            if (action == null)
                action.Suspend(this);
        }

        public void SuspendAll()
        {
            if (CurrentActions.Count > 0)
                foreach (IAction action in CurrentActions)
                    action.Suspend(this);
        }

        public IEntity AttachedOn { get; set; }

        public void Update()
        {
            CurrentActions.ForEach(a => a.Update(this));
        }

        public List<IAction> CurrentActions { get; private set; }

        public int ID { get; private set; }

        public PropertyList Properties { get; private set; }

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
