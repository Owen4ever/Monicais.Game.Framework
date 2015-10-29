namespace Monicais.Core
{
    using Monicais.ThrowHelper;
    using System;
    using System.Runtime.CompilerServices;

    public class DefaultSkillCondition : ISkillCondition
    {
        private Action @do;
        private Action test;

        public DefaultSkillCondition(Action test, Action @do, string description)
        {
            if (test == null)
            {
                ArgumentNull.Throw("test");
            }
            if (@do == null)
            {
                ArgumentNull.Throw("do");
            }
            this.test = test;
            this.@do = @do;
            base.Description = description;
        }

        public override bool DoAction(IEntity entity)
        {
            return this.@do(entity);
        }

        public override bool Test(IEntity entity)
        {
            return this.test(entity);
        }

        public delegate bool Action(IEntity entity);
    }
}

