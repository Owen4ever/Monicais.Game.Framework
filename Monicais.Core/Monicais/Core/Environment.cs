
using Monicais.Plugin;
using Monicais.Property;
using System;

namespace Monicais.Core
{

    public delegate void OnDispose();

    public class MonoEnvironment : IDisposable
    {

        public event OnDispose OnDispose;

        public MonoEnvironment()
        {
            this.OnDispose = <> c.<> 9__0_0 ?? (<> c.<> 9__0_0 = new Monicais.Core.OnDispose(<> c.<> 9.<.ctor > b__0_0));
            this.EffectManager = new Monicais.Property.EffectManager();
            this.SkillManager = new SkillList();
            this.PropertyManager = new Monicais.Property.PropertyManager();
            this.PluginEnvironment = new Monicais.Plugin.PluginEnvironment();
            this.Attributes = new Monicais.Property.Attributes();
        }

        public MonoEnvironment(MonoEnvironment env)
        {
            OnDispose = env.OnDispose;
            EffectManager = env.EffectManager;
            SkillManager = env.SkillManager;
            PropertyManager = env.PropertyManager;
            PluginEnvironment = env.PluginEnvironment;
            Attributes = env.Attributes;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            OnDispose();
        }

        public Attributes Attributes { get; private set; }

        public EffectManager EffectManager { get; private set; }

        public PluginEnvironment PluginEnvironment { get; private set; }

        public PropertyManager PropertyManager { get; private set; }

        public SkillList SkillManager { get; private set; }
    }
}
