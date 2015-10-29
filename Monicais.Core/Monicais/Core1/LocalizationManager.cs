namespace Monicais.Core
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public sealed class LocalizationManager
    {
        private CultureInfo locale;
        private LocaleUpdater localizers = (<>c.<>9__0_0 ?? (<>c.<>9__0_0 = new LocaleUpdater(<>c.<>9.<.ctor>b__0_0)));

        public CultureInfo CurrentLocale
        {
            get
            {
                return this.locale;
            }
            set
            {
                this.locale = value;
                this.localizers();
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly LocalizationManager.<>c <>9 = new LocalizationManager.<>c();
            public static LocaleUpdater <>9__0_0;

            internal void <.ctor>b__0_0()
            {
            }
        }
    }
}

