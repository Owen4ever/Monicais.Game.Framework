namespace Monicais.Property
{
    using System;
    using System.Runtime.CompilerServices;

    public static class RestrictorUtil
    {
        public static readonly Restrictor Unlimited = new Restrictor(<>c.<>9.<.cctor>b__2_0);

        public static Restrictor Between(int min, int max)
        {
            return val => ((val < min) ? ((Restrictor) min) : ((Restrictor) ((val > max) ? ((Restrictor) max) : ((Restrictor) val))));
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly RestrictorUtil.<>c <>9 = new RestrictorUtil.<>c();

            internal int <.cctor>b__2_0(int val)
            {
                return val;
            }
        }
    }
}

