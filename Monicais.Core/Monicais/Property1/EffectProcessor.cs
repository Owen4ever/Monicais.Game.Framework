﻿namespace Monicais.Property
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public delegate void EffectProcessor(Attributes propertyAttrs, Attributes effectAttrs, ref int val);
}
