
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;

namespace Monicais.Core
{

    public interface IEntity
    {

        int ID { get; }

        PropertyList Properties { get; }

        EntityStatus Status { get; set; }

        IEntity AttachedOn { get; set; }

        void Update();

        void AddAction(MonoAction action);

        void RemoveAction(MonoAction action);

        List<MonoAction> AllActions { get; }

        void AddCurrentAction(MonoAction action);

        void RemoveCurrentAction(MonoAction action);

        List<MonoAction> CurrentActions { get; }

        void Suspend(string actionName, ActionSuspendType suspendType);

        void SuspendFirst(string actionName, ActionSuspendType suspendType);

        void SuspendIf(Predicate<MonoAction> predicate, ActionSuspendType suspendType);

        void SuspendAll(ActionSuspendType suspendType);
    }

    [Serializable]
    public abstract class AbstractEntity : NonNullDisplayable, IEntity
    {

        protected AbstractEntity(int id, string name, PropertyList properties)
            : this(id, name, properties, null)
        { }

        protected AbstractEntity(int id, string name, PropertyList properties,
            EntityStatus status) : base(name)
        {
            if (properties == null)
                ArgumentNull.Throw("properties");
            ID = id;
            Properties = properties;
            this.status = status ?? EntityStatus.NormalStatus;
        }

        protected AbstractEntity(int id, string name, string desc, PropertyList properties)
            : this(id, name, desc, properties, null)
        { }

        protected AbstractEntity(int id, string name, string desc, PropertyList properties,
            EntityStatus status) : base(name, desc)
        {
            if (properties == null)
                ArgumentNull.Throw("properties");
            ID = id;
            Properties = properties;
            this.status = status ?? EntityStatus.NormalStatus;
        }

        public int ID { get; private set; }

        public EntityStatus Status
        {
            get { return status; }
            set
            {
#if DEBUG
                if (value == null)
                    ArgumentNull.Throw("status");
#endif
                EntityStatus lastStatus = status;
                lastStatus.Deactivate(value, this);
                status = value;
                status.Activate(lastStatus, this);
            }
        }
        private EntityStatus status;

        public IEntity AttachedOn { get; set; }

        public PropertyList Properties { get; private set; }

        public void Update()
        {
            status.Update();
            Properties.Update();
            CurrentActions.ForEach(a => a.Update(this));
        }

        public void AddAction(MonoAction action)
        {
            if (action == null)
                ArgumentNull.Throw("action");
            actions.Add(action);
        }

        public void RemoveAction(MonoAction action)
        {
            actions.Remove(action);
        }

        public List<MonoAction> AllActions { get { return actions; } }
        private List<MonoAction> actions;

        public void AddCurrentAction(MonoAction action)
        {
            CurrentActions.Add(action);
        }

        public void RemoveCurrentAction(MonoAction action)
        {
            CurrentActions.Remove(action);
        }

        public List<MonoAction> CurrentActions { get; private set; }

        public void Suspend(string actionName, ActionSuspendType suspendType)
        {
            foreach (var currentAction in CurrentActions)
                if (currentAction.Name.Equals(actionName))
                    currentAction.Suspend(this, suspendType);
        }

        public void SuspendFirst(string actionName, ActionSuspendType suspendType)
        {
            foreach (var currentAction in CurrentActions)
                if (currentAction.Name.Equals(actionName))
                {
                    currentAction.Suspend(this, suspendType);
                    break;
                }
        }

        public void SuspendIf(Predicate<MonoAction> predicate, ActionSuspendType suspendType)
        {
            foreach (var currentAction in CurrentActions)
                if (predicate(currentAction))
                    currentAction.Suspend(this, suspendType);
        }

        public void SuspendAll(ActionSuspendType suspendType)
        {
            if (CurrentActions.Count > 0)
                foreach (MonoAction action in CurrentActions)
                    action.Suspend(this, suspendType);
        }
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

        public static EntityStatus NormalStatus { get; internal set; }
    }

    [Serializable]
    internal class NonActionEntityStatus : EntityStatus
    {
        internal NonActionEntityStatus()
        {
            NormalStatus = this;
        }

        public override void Activate(EntityStatus lastStatus, IEntity entity) { }

        public override void Deactivate(EntityStatus nextStatus, IEntity entity) { }

        public override void Update() { }
    }

    internal class MonoBehaviourFuncs
    {

        // Before first update

        private void Awake() { }

        private void Start() { }

        // Update

        private void Update() { }

        private void FixedUpdate() { }

        private void LateUpdate() { }

        // Destroy

        private void OnDestroy() { }

        // Enable / Disable

        private void OnEnable() { }

        private void OnDisable() { }

        // Application

        private void OnApplicationFocus() { }

        private void OnApplicationPause() { }

        private void OnApplicationQuit() { }

        // Visible / Invisible

        private void OnBecameInvisible() { }

        private void OnBecameVisible() { }

        // Collision

        private void OnCollisionEnter() { }

        private void OnCollisionExit() { }

        private void OnCollisionStay() { }

        private void OnControllerColliderHit() { }

        private void OnParticleCollision() { }

        // Gizmos

        private void OnDrawGizmos() { }

        private void OnDrawGizmosSelected() { }

        // Net Connect / Disconnect

        private void OnFailedToConnect() { }

        private void OnFailedToConnectToMasterServer() { }

        private void OnConnectedToServer() { }

        private void OnDisconnectedFromServer() { }

        private void OnMasterServerEvent() { }

        private void OnNetworkInstantiate() { }

        private void OnPlayerConnected() { }

        private void OnPlayerDisconnected() { }

        private void OnSerializeNetworkView() { }

        private void OnServerInitialized() { }

        // GUI

        private void OnGUI() { }

        // Mouse

        private void OnMouseDown() { }

        private void OnMouseDrag() { }

        private void OnMouseEnter() { }

        private void OnMouseExit() { }

        private void OnMouseOver() { }

        private void OnMouseUp() { }

        private void OnMouseUpAsButton() { }

        // Render

        private void OnPreCull() { }

        private void OnPreRender() { }

        private void OnWillRenderObject() { }

        private void OnPostRender() { }

        private void OnRenderObject() { }

        private void OnRenderImage() { }

        // Trigger

        private void OnTriggerEnter() { }

        private void OnTriggerExit() { }

        private void OnTriggerStay() { }

        // For Debug

        private void Reset() { }

        // Joint Break

        private void OnJointBreak() { }

        // Level

        private void OnLevelWasLoaded() { }
    }
}
