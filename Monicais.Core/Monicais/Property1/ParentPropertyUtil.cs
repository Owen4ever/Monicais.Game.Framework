namespace Monicais.Property
{
    using System;
    using System.Runtime.CompilerServices;

    public static class ParentPropertyUtil
    {
        public static void SetRestrictor(this IProperty child, IProperty min, IProperty max)
        {
            child.Restrictor = delegate (int val) {
                int finalValue = min.FinalValue;
                if (val < finalValue)
                {
                    return finalValue;
                }
                int num2 = max.FinalValue;
                if (val > num2)
                {
                    return num2;
                }
                return val;
            };
            EventListener el_invalid = () => child.Invalidate();
            EventListener el_remove = null;
            el_remove = delegate {
                min.RemoveInvalidationListener(el_invalid);
                max.RemoveInvalidationListener(el_invalid);
                child.RestrictorChangedListener -= el_remove;
            };
            child.RestrictorChangedListener += el_remove;
            min.AddInvalidationListener(el_invalid);
            max.AddInvalidationListener(el_invalid);
        }

        public static void SetRestrictor(this IProperty child, IProperty min, int max)
        {
            child.Restrictor = delegate (int val) {
                if (val > max)
                {
                    return max;
                }
                int finalValue = min.FinalValue;
                if (val < finalValue)
                {
                    return finalValue;
                }
                return val;
            };
            EventListener el_invalid = () => child.Invalidate();
            EventListener el_remove = null;
            el_remove = delegate {
                min.RemoveInvalidationListener(el_invalid);
                child.RestrictorChangedListener -= el_remove;
            };
            child.RestrictorChangedListener += el_remove;
            min.AddInvalidationListener(el_invalid);
        }

        public static void SetRestrictor(this IProperty child, int min, IProperty max)
        {
            child.Restrictor = delegate (int val) {
                if (val < min)
                {
                    return min;
                }
                int finalValue = max.FinalValue;
                if (val > finalValue)
                {
                    return finalValue;
                }
                return val;
            };
            EventListener el_invalid = () => child.Invalidate();
            EventListener el_remove = null;
            el_remove = delegate {
                max.RemoveInvalidationListener(el_invalid);
                child.RestrictorChangedListener -= el_remove;
            };
            child.RestrictorChangedListener += el_remove;
            max.AddInvalidationListener(el_invalid);
        }
    }
}

