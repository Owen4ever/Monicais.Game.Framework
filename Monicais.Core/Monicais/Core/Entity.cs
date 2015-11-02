
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

        IEntity AttachedOn { get; set; }

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
