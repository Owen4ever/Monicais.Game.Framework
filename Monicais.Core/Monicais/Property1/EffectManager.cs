namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class EffectManager
    {
        private List<EffectID> bothBuffEffects = new List<EffectID>();
        private List<EffectID> buffEffects = new List<EffectID>();
        private List<EffectID> debuffEffects = new List<EffectID>();
        private Dictionary<EffectID, IEffect> effects = new Dictionary<EffectID, IEffect>();
        private List<EffectID> ids = new List<EffectID>();
        private List<EffectID> normalEffects = new List<EffectID>();

        public void AddEffect(EffectType type, IEffect effect)
        {
            if (effect == null)
            {
                ArgumentNull.Throw("effect");
            }
            EffectID iD = effect.ID;
            if (this.effects.ContainsKey(iD))
            {
                AlreadyContain.Throw("effect");
            }
            this.effects.Add(iD, effect);
            switch (type)
            {
                case EffectType.NORMAL:
                    this.normalEffects.Add(iD);
                    break;

                case EffectType.BUFF:
                    this.buffEffects.Add(iD);
                    break;

                case EffectType.DEBUFF:
                    this.debuffEffects.Add(iD);
                    break;

                case EffectType.BOTH_BUFF:
                    this.debuffEffects.Add(iD);
                    this.buffEffects.Add(iD);
                    this.bothBuffEffects.Add(iD);
                    break;

                case EffectType.ALL:
                    this.normalEffects.Add(iD);
                    this.debuffEffects.Add(iD);
                    this.buffEffects.Add(iD);
                    break;
            }
        }

        public EffectID GetEffectID(string id)
        {
            return this.ids.Find(eid => eid.ID == id);
        }

        public List<EffectID> GetEffectIDsViaType(EffectType type)
        {
            switch (type)
            {
                case EffectType.NORMAL:
                    return this.normalEffects;

                case EffectType.BUFF:
                    return this.buffEffects;

                case EffectType.DEBUFF:
                    return this.debuffEffects;

                case EffectType.BOTH_BUFF:
                    return this.bothBuffEffects;

                case EffectType.ALL:
                    return this.ids;
            }
            ArgumentOutOfRange.Throw("UsingType");
            return null;
        }

        public bool RemoveEffect(EffectID effectID)
        {
            if (effectID == null)
            {
                ArgumentNull.Throw("effectID");
            }
            return ((this.ids.Remove(effectID) & this.effects.Remove(effectID)) & (((this.debuffEffects.Remove(effectID) | this.buffEffects.Remove(effectID)) | this.bothBuffEffects.Remove(effectID)) | this.normalEffects.Remove(effectID)));
        }
    }
}

