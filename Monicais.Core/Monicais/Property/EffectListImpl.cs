
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Monicais.Property
{

    [Serializable]
    internal class RestorableEffectList : IEffectList
    {
        private int count;
        private List<EffectNode>[] effects;

        public RestorableEffectList()
        {
            effects = EffectPriority.NewEffectLists<EffectNode>();
        }

        protected RestorableEffectList(SerializationInfo info, StreamingContext context)
        {
            effects = EffectPriority.NewEffectLists<EffectNode>();
            count = info.GetInt32("COUNT");
            for (int i = 0; i < Count; ++i)
                effects[i] = (List<EffectNode>) info.GetValue(string.Format("LIST:{0:X00}", i), typeof(List<EffectNode>));
        }

        public override void AddEffect(IEffect effect)
        {
            effects[effect.ID.Priority].Add(new EffectNode(effect));
            ++count;
        }

        public override int Calculate(Attributes propertyAttrs, int val)
        {
            if (!Empty)
                foreach (List<EffectNode> list in effects)
                    list.ForEach(node => node.Effect.Affect(propertyAttrs, node.Attributes, ref val));
            return val;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("COUNT", count);
            for (int i = 0; i < count; ++i)
                info.AddValue(string.Format("LIST:{0:X00}", i), effects[i]);
        }

        public override int RemoveEffect(EffectID eid)
        {
            int num = effects[eid.Priority].RemoveAll(node => node.ID == eid);
            count -= num;
            return num;
        }

        public override bool Update()
        {
            if (Empty)
                return false;
            bool flag = false;
            foreach (List<EffectNode> list in effects)
                foreach (var node in list)
                    flag |= node.Effect.Update(node.Attributes);
            return flag;
        }

        public override int Count
        {
            get { return count; }
        }
    }

    [Serializable]
    internal struct EffectNode
    {
        private IEffect effect;
        private Attributes attrs;
        public EffectNode(IEffect effect)
        {
            this.effect = effect;
            this.attrs = effect.DefaultAttributes;
        }

        public EffectNode(IEffect effect, Attributes attrs)
        {
            this.effect = effect;
            this.attrs = attrs;
        }

        private EffectNode(SerializationInfo info, StreamingContext context)
        {
            effect = (IEffect) info.GetValue("EFFECT", typeof(IEffect));
            attrs = (Attributes) info.GetValue("ATTRIBUTES", typeof(Attribute));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("EFFECT", effect);
            info.AddValue("ATTRIBUTES", attrs);
        }

        public IEffect Effect
        {
            get { return effect; }
        }
        public EffectID ID
        {
            get { return Effect.ID; }
        }
        public Attributes Attributes
        {
            get { return attrs; }
        }
    }

    [Serializable]
    internal class UnrecoverableEffectList : IEffectList
    {
        private int count;
        private List<EffectNode>[] effects;

        public UnrecoverableEffectList()
        {
            effects = EffectPriority.NewEffectLists<EffectNode>();
            count = 0;
        }

        protected UnrecoverableEffectList(SerializationInfo info, StreamingContext context)
        {
            effects = EffectPriority.NewEffectLists<EffectNode>();
            count = info.GetInt32("COUNT");
            for (int i = 0; i < count; ++i)
                effects[i] = (List<EffectNode>) info.GetValue(string.Format("LIST:{0:X00}", i), typeof(List<EffectNode>));
        }

        public override void AddEffect(IEffect effect)
        {
            EffectPriority priority = effect.ID.Priority;
            if (priority == EffectPriority.IMMEDIATE)
                effects[priority].Add(new EffectNode(effect, true));
            else
                effects[priority].Add(new EffectNode(effect, false));
            int num = count + 1;
            count = num;
        }

        public override int Calculate(Attributes propertyAttrs, int val)
        {
            if (!Empty)
                foreach (List<EffectNode> list in effects)
                    count -= list.RemoveAll(node =>
                    {
                        node.Effect.Affect(propertyAttrs, node.Attributes, ref val);
                        return node.Immediate;
                    });
            return val;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("COUNT", count);
            for (int i = 0; i < count; ++i)
                info.AddValue(string.Format("LIST:{0:X00}", i), effects[i]);
        }

        public override int RemoveEffect(EffectID eid)
        {
            int num = effects[eid.Priority].RemoveAll(node => node.ID == eid);
            this.count -= num;
            return num;
        }

        public override bool Update()
        {
            if (Empty)
                return false;
            bool flag = false;
            foreach (List<EffectNode> list in this.effects)
                foreach (var node in list)
                    flag |= node.Effect.Update(node.Attributes);
            return flag;
        }

        public override int Count
        {
            get { return count; }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct EffectNode
        {
            private IEffect effect;
            private Attributes attrs;
            private bool disposable;
            public EffectNode(IEffect effect, bool disposable)
            {
                this.effect = effect;
                this.attrs = effect.DefaultAttributes;
                this.disposable = disposable;
            }

            public EffectNode(IEffect effect, Attributes attrs, bool disposable)
            {
                this.effect = effect;
                this.attrs = attrs;
                this.disposable = disposable;
            }

            public bool Immediate
            {
                get
                {
                    return disposable;
                }
            }
            public IEffect Effect
            {
                get
                {
                    return effect;
                }
            }
            public EffectID ID
            {
                get
                {
                    return Effect.ID;
                }
            }
            public Attributes Attributes
            {
                get
                {
                    return attrs;
                }
            }
        }
    }
}
