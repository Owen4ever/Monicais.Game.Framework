
using System;
using System.Globalization;

namespace Monicais.Core
{

    public sealed class LocalizationManager
    {
        private CultureInfo locale;
        private LocaleUpdater localizers = () => { };

        public CultureInfo CurrentLocale
        {
            get { return locale; }
            set
            {
                locale = value;
                localizers();
            }
        }
    }

    public delegate void LocaleUpdater();
}
