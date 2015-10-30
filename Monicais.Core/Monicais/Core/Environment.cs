
using Monicais.Plugin;
using Monicais.Property;
using System;

namespace Monicais.Core
{

    public delegate void OnDispose();

    public static class GlobalEnvironment
    {

        public static void Dispose()
        {
            OnDispose();
            monoEnv.Dispose();
        }

        public static MonoEnvironment MonoEnvironment
        {
            get { return monoEnv; }
            private set
            {
                if (value == null)
                    monoEnv.Dispose();
                else
                {
                    if (monoEnv != null)
                        monoEnv.Dispose();
                    monoEnv = value;
                }
            }
        }
        private static MonoEnvironment monoEnv = new MonoEnvironment();

        public static event OnDispose OnDispose;
    }


    public class MonoEnvironment : IDisposable
    {

        public event OnDispose OnDispose;

        public MonoEnvironment()
        {
            OnDispose = () => { };
            EffectManager = new EffectManager();
            SkillManager = new SkillList();
            PropertyManager = new PropertyManager();
            PluginEnvironment = new PluginEnvironment();
            Attributes = new Attributes();
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
