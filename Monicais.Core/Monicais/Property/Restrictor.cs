
using System;

namespace Monicais.Property
{

    public interface IRestrictable
    {
        void RemoveRestrictor();
        int Restrict(int val);

        Restrictor Restrictor { set; }
    }

    [Serializable]
    public abstract class Restrictable : IRestrictable
    {

        protected Restrictable() { }

        public bool IsRestricted()
        {
            return restrictor != RestrictorUtil.Unlimited;
        }

        public void RemoveRestrictor()
        {
            RestrictorChangedListener();
            Restrictor = RestrictorUtil.Unlimited;
        }

        public int Restrict(int val)
        {
            return restrictor(val);
        }

        public Restrictor Restrictor
        {
            set
            {
                if (value == null)
                    restrictor = RestrictorUtil.Unlimited;
                else
                    restrictor = value;
                RestrictorChangedListener();
            }
        }

        private Restrictor restrictor = RestrictorUtil.Unlimited;
        public event EventListener RestrictorChangedListener;
    }

    public delegate int Restrictor(int val);

    public static class RestrictorUtil
    {
        public static readonly Restrictor Unlimited = v => v;

        public static Restrictor Between(int min, int max)
        {
            return val => val < min ? min : (val > max ? max : val);
        }
    }
}
