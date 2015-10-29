
using Monicais.Property;
using System;
using System.Collections.Generic;

namespace Monicais.Core
{

    public interface IEntity
    {
        void AddAction(IAction action);

        void AddCurrentAction(IAction action);

        void RemoveAction(IAction action);

        void RemoveCurrentAction(IAction action);

        void Suspend(string action);

        void SuspendAll();

        void Update();

        List<IAction> CurrentActions { get; }

        int ID { get; }

        PropertyList Properties { get; }

        EntityStatus Status { get; set; }
    }

    [Serializable]
    public abstract class EntityStatus
    {

        private static EntityStatus NORMAL_STATUS = new NonActionEntityStatus();

        protected EntityStatus() { }

        public abstract void Activate(EntityStatus lastStatus, IEntity entity);

        public abstract void Deactivate(EntityStatus nextStatus, IEntity entity);

        public bool IsNormal() { return this == NormalStatus; }

        public abstract void Update();

        public static EntityStatus NormalStatus
        {
            get; internal set;
        }
    }
}

