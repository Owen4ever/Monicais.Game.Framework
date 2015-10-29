namespace Monicais.Property
{
    using System;

    public static class EventListenerUtil
    {
        public static void SetOriginalFromFinal(IProperty listener, IProperty @object)
        {
            @object.AddInvalidationListener(() => listener.OriginalValue = @object.FinalValue);
        }

        public static void SetOriginalFromFinalIfEqual(IProperty listener, IProperty @object)
        {
            @object.AddInvalidationListener(delegate {
                int finalValue = @object.FinalValue;
                if (listener.FinalValue == finalValue)
                {
                    listener.OriginalValue = finalValue;
                }
            });
        }

        public static void SetOriginalFromFinalIfGreater(IProperty listener, IProperty @object)
        {
            @object.AddInvalidationListener(delegate {
                int finalValue = @object.FinalValue;
                if (listener.FinalValue > finalValue)
                {
                    listener.OriginalValue = finalValue;
                }
            });
        }

        public static void SetOriginalFromFinalIfGreaterEqual(IProperty listener, IProperty @object)
        {
            @object.AddInvalidationListener(delegate {
                int finalValue = @object.FinalValue;
                if (listener.FinalValue >= finalValue)
                {
                    listener.OriginalValue = finalValue;
                }
            });
        }

        public static void SetOriginalFromFinalIfLess(IProperty listener, IProperty @object)
        {
            @object.AddInvalidationListener(delegate {
                int finalValue = @object.FinalValue;
                if (listener.FinalValue < finalValue)
                {
                    listener.OriginalValue = finalValue;
                }
            });
        }

        public static void SetOriginalFromFinalIfLessEqual(IProperty listener, IProperty @object)
        {
            @object.AddInvalidationListener(delegate {
                int finalValue = @object.FinalValue;
                if (listener.FinalValue <= finalValue)
                {
                    listener.OriginalValue = finalValue;
                }
            });
        }
    }
}

