namespace Monicais.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class GlobalEnvironment
    {
        private static Monicais.Core.MonoEnvironment monoEnv = new Monicais.Core.MonoEnvironment();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event Monicais.Core.OnDispose OnDispose;

        public static void Dispose()
        {
            OnDispose();
            monoEnv.Dispose();
        }

        public static Monicais.Core.MonoEnvironment MonoEnvironment
        {
            get
            {
                return monoEnv;
            }
            private set
            {
                if (value == null)
                {
                    monoEnv.Dispose();
                }
                else
                {
                    if (monoEnv > null)
                    {
                        monoEnv.Dispose();
                    }
                    monoEnv = value;
                }
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly GlobalEnvironment.<>c <>9 = new GlobalEnvironment.<>c();

            internal void <.cctor>b__8_0()
            {
            }
        }
    }
}

