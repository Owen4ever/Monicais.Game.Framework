namespace Monicais.Property
{
    using System;
    using System.Runtime.CompilerServices;

    public static class EffectSupportUtil
    {
        public static EffectSupport PreventAll()
        {
            return (<>c.<>9__0_0 ?? (<>c.<>9__0_0 = new EffectSupport(<>c.<>9.<PreventAll>b__0_0)));
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly EffectSupportUtil.<>c <>9 = new EffectSupportUtil.<>c();
            public static EffectSupport <>9__0_0;

            internal bool <PreventAll>b__0_0(IEffect effect)
            {
                return true;
            }
        }
    }
}

