namespace Monicais.Property
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal sealed class ValidValue
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsValid>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Value>k__BackingField;

        public ValidValue() : this(false, 0)
        {
        }

        public ValidValue(bool isValid, int val)
        {
            this.IsValid = isValid;
            this.Value = val;
        }

        public void Invalidate()
        {
            this.IsValid = false;
        }

        public void Validate(int val)
        {
            this.Value = val;
            this.IsValid = true;
        }

        public bool IsValid { get; private set; }

        public int Value { get; private set; }
    }
}

