namespace Monicais.Property
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    internal class UnrecoverableEffectList : IEffectList
    {
        private int count;
        private List<EffectNode>[] effects;

        public UnrecoverableEffectList()
        {
            this.effects = Priority.NewEffectLists<EffectNode>();
            this.count = 0;
        }

        protected UnrecoverableEffectList(SerializationInfo info, StreamingContext context)
        {
            int num2;
            this.effects = Priority.NewEffectLists<EffectNode>();
            this.count = 0;
            this.count = info.GetInt32("COUNT");
            for (int i = 0; i < this.count; i = num2)
            {
                this.effects[i] = (List<EffectNode>) info.GetValue(string.Format("LIST:{0:X00}", i), typeof(List<EffectNode>));
                num2 = i + 1;
            }
        }

        public override void AddEffect(IEffect effect)
        {
            Priority priority = effect.ID.Priority;
            if (priority == Priority.IMMEDIATE)
            {
                this.effects[(int) priority].Add(new EffectNode(effect, true));
            }
            else
            {
                this.effects[(int) priority].Add(new EffectNode(effect, false));
            }
            int num = this.count + 1;
            this.count = num;
        }

        public override int Calculate(Attributes propertyAttrs, int val)
        {
            Predicate<EffectNode> <>9__0;
            if (!base.Empty)
            {
                foreach (List<EffectNode> list in this.effects)
                {
                    this.count -= list.RemoveAll(<>9__0 ?? (<>9__0 = delegate (EffectNode node) {
                        node.Effect.Affect(propertyAttrs, node.Attributes, ref val);
                        return node.Immediate;
                    }));
                }
            }
            return val;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            int num2;
            info.AddValue("COUNT", this.count);
            for (int i = 0; i < this.count; i = num2)
            {
                info.AddValue(string.Format("LIST:{0:X00}", i), this.effects[i]);
                num2 = i + 1;
            }
        }

        public override int RemoveEffect(EffectID eid)
        {
            int num = this.effects[(int) eid.Priority].RemoveAll(node => node.ID == eid);
            this.count -= num;
            return num;
        }

        public override bool Update()
        {
            if (base.Empty)
            {
                return false;
            }
            bool flag = false;
            foreach (List<EffectNode> list in this.effects)
            {
                flag |= list.Any<EffectNode>(<>c.<>9__8_0 ?? (<>c.<>9__8_0 = new Func<EffectNode, bool>(<>c.<>9.<Update>b__8_0)));
            }
            return flag;
        }

        public override int Count
        {
            get
            {
                return this.count;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly UnrecoverableEffectList.<>c <>9 = new UnrecoverableEffectList.<>c();
            public static Func<UnrecoverableEffectList.EffectNode, bool> <>9__8_0;

            internal bool <Update>b__8_0(UnrecoverableEffectList.EffectNode node)
            {
                return node.Effect.Update(node.Attributes);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct EffectNode
        {
            private IEffect effect;
            private Monicais.Property.Attributes attrs;
            private bool disposable;
            public EffectNode(IEffect effect, bool disposable)
            {
                this.effect = effect;
                this.attrs = effect.DefaultAttributes;
                this.disposable = disposable;
            }

            public EffectNode(IEffect effect, Monicais.Property.Attributes attrs, bool disposable)
            {
                this.effect = effect;
                this.attrs = attrs;
                this.disposable = disposable;
            }

            public bool Immediate
            {
                get
                {
                    return this.disposable;
                }
            }
            public IEffect Effect
            {
                get
                {
                    return this.effect;
                }
            }
            public EffectID ID
            {
                get
                {
                    return this.Effect.ID;
                }
            }
            public Monicais.Property.Attributes Attributes
            {
                get
                {
                    return this.attrs;
                }
            }
        }
    }
}

