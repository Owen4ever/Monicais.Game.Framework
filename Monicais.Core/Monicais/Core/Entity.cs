
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
        private void Awake() { }

        private void FixedUpdate() { }

        private void LateUpdate() { }

        private void OnApplicationFocus() { }

        private void OnApplicationPause() { }

        private void OnApplicationQuit() { }

        private void OnBecameInvisible() { }

        private void OnBecameVisible() { }

        private void OnCollisionEnter() { }

        private void OnCollisionExit() { }

        private void OnCollisionStay() { }

        private void OnConnectedToServer() { }

        private void OnControllerColliderHit() { }

        private void OnDestroy() { }

        private void OnDisable() { }

        private void OnDisconnectedFromServer() { }

        private void OnDrawGizmos() { }

        private void OnDrawGizmosSelected() { }

        private void OnEnable() { }

        private void OnFailedToConnect() { }

        private void OnFailedToConnectToMasterServer() { }

        private void OnGUI() { }

        private void OnJointBreak() { }

        private void OnLevelWasLoaded() { }

        private void OnMasterServerEvent() { }

        private void OnMouseDown() { }

        private void OnMouseDrag() { }

        private void OnMouseEnter() { }

        private void OnMouseExit() { }

        private void OnMouseOver() { }

        private void OnMouseUp() { }

        private void OnMouseUpAsButton() { }

        private void OnNetworkInstantiate() { }

        private void OnParticleCollision() { }

        private void OnPlayerConnected() { }

        private void OnPlayerDisconnected() { }

        private void OnPostRender() { }

        private void OnPreCull() { }

        private void OnPreRender() { }

        private void OnRenderImage() { }

        private void OnRenderObject() { }

        private void OnSerializeNetworkView() { }

        private void OnServerInitialized() { }

        private void OnTriggerEnter() { }

        private void OnTriggerExit() { }

        private void OnTriggerStay() { }

        private void OnWillRenderObject() { }

        private void Reset() { }

        private void Start() { }

        private void Update() { }
    }
}
