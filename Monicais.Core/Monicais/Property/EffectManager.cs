
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;

namespace Monicais.Property
{

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
                ArgumentNull.Throw("effect");
            EffectID iD = effect.ID;
            if (effects.ContainsKey(iD))
                AlreadyContain.Throw("effect");
            effects.Add(iD, effect);
            switch (type)
            {
                case EffectType.NORMAL:
                    normalEffects.Add(iD);
                    break;

                case EffectType.BUFF:
                    buffEffects.Add(iD);
                    break;

                case EffectType.DEBUFF:
                    debuffEffects.Add(iD);
                    break;

                case EffectType.BOTH_BUFF:
                    debuffEffects.Add(iD);
                    buffEffects.Add(iD);
                    bothBuffEffects.Add(iD);
                    break;

                case EffectType.ALL:
                    normalEffects.Add(iD);
                    debuffEffects.Add(iD);
                    buffEffects.Add(iD);
                    break;
            }
        }

        public EffectID GetEffectID(string id)
        {
            return ids.Find(eid => eid.ID == id);
        }

        public List<EffectID> GetEffectIDsViaType(EffectType type)
        {
            switch (type)
            {
                case EffectType.NORMAL:
                    return normalEffects;

                case EffectType.BUFF:
                    return buffEffects;

                case EffectType.DEBUFF:
                    return debuffEffects;

                case EffectType.BOTH_BUFF:
                    return bothBuffEffects;

                case EffectType.ALL:
                    return ids;
            }
            ArgumentOutOfRange.Throw("UsingType");
            return null;
        }

        public bool RemoveEffect(EffectID effectID)
        {
            if (effectID == null)
                ArgumentNull.Throw("effectID");
            return ((ids.Remove(effectID) & effects.Remove(effectID))
                & (((debuffEffects.Remove(effectID) | buffEffects.Remove(effectID))
                | bothBuffEffects.Remove(effectID)) | normalEffects.Remove(effectID)));
        }
    }
}

