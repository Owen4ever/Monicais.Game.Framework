namespace Monicais.Property
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    [Serializable]
    public abstract class Restrictable : IRestrictable
    {
        private Monicais.Property.Restrictor restrictor = RestrictorUtil.Unlimited;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventListener RestrictorChangedListener;

        protected Restrictable()
        {
        }

        public bool IsRestricted()
        {
            return (this.restrictor != RestrictorUtil.Unlimited);
        }

        public void RemoveRestrictor()
        {
            this.RestrictorChangedListener();
            this.Restrictor = RestrictorUtil.Unlimited;
        }

        public int Restrict(int val)
        {
            return this.restrictor(val);
        }

        public Monicais.Property.Restrictor Restrictor
        {
            set
            {
                if (value == null)
                {
                    this.Restrictor = RestrictorUtil.Unlimited;
                }
                else
                {
                    this.restrictor = value;
                }
                this.RestrictorChangedListener();
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Restrictable.<>c <>9 = new Restrictable.<>c();
            public static EventListener <>9__0_0;

            internal void <.ctor>b__0_0()
            {
            }
        }
    }
}

