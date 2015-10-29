namespace Monicais.Property
{
    using System;

    public interface IRestrictable
    {
        void RemoveRestrictor();
        int Restrict(int val);

        Monicais.Property.Restrictor Restrictor { set; }
    }
}

