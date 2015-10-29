namespace Monicais.Core
{
    using System;

    [Serializable]
    internal class NonActionEntityStatus : EntityStatus
    {
        internal NonActionEntityStatus()
        {
            EntityStatus.NormalStatus = this;
        }

        public override void Activate(EntityStatus lastStatus, IEntity entity)
        {
        }

        public override void Deactivate(EntityStatus nextStatus, IEntity entity)
        {
        }

        public override void Update()
        {
        }
    }
}

