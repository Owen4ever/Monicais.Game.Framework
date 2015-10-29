namespace Monicais.Core
{
    using System;

    [Serializable]
    public abstract class ISkillCondition
    {
        private string desc;

        protected ISkillCondition()
        {
        }

        public abstract bool DoAction(IEntity entity);
        public abstract bool Test(IEntity entity);

        public string Description
        {
            get
            {
                return this.desc;
            }
            set
            {
                this.desc = value ?? "";
            }
        }
    }
}

