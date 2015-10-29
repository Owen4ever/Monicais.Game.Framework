
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Monicais.Plugin
{

    public class PluginEnvironment
    {

        private readonly List<IPlugin> plugins = new List<IPlugin>();
        private readonly List<string> preconditions = new List<string>();

        public void AddPlugin(IPlugin plugin)
        {
            if (IsLoaded)
                throw new PluginEnvironmentException("Already loaded");
            IPluginInfo info = plugin.Info;
            if (preconditions.Contains(info.ID))
                preconditions.Remove(info.ID);
            if (info.HasPreconditions)
                preconditions.AddRange(info.Preconditions);
            plugins.Add(plugin);
        }

        public bool CanLoad() { return preconditions.Count == 0; }

        private List<IPlugin> GetLoadingSequence()
        {
            int num = 0;
            List<IPlugin> tmp;
            List<IPlugin> list = new List<IPlugin>(plugins);
            List<IPlugin> ret = new List<IPlugin>(list.FindAll(p => !p.Info.HasPreconditions));
            list.RemoveAll(p => ret.Contains(p));
            for (int precount = 1; list.Count > 0; precount = num)
            {
                tmp = list.FindAll(p => p.Info.Preconditions.Count == precount);
                ret.AddRange(tmp);
                list.RemoveAll(p => tmp.Contains(p));
                num = precount + 1;
            }
            return ret;
        }

        public string LoadAll()
        {
            if (IsLoaded)
                throw new PluginEnvironmentException("Already loaded");
            if (CanLoad())
            {
                List<IPlugin> toLoad = GetLoadingSequence();
                List<IPlugin> loaded = new List<IPlugin>();
                foreach (IPlugin plugin in toLoad)
                {
                    int result = plugin.Load(this);
                    if (!plugin.IsSuccessFlag(result))
                    {
                        loaded.ForEach(pl => pl.Unload(this));
                        return plugin.GetMessageFromResult(result);
                    }
                    loaded.Add(plugin);
                }
                try
                {
                    return null;
                } finally
                {
                    IsLoaded = true;
                }
            }
            StringBuilder builder = new StringBuilder("Integrant plugins: ");
            List<string>.Enumerator enumerator = preconditions.GetEnumerator();
            do
                builder.Append(", ").Append(enumerator.Current);
            while (enumerator.MoveNext());
            return builder.ToString();
        }

        public void RemovePlugin(IPlugin plugin)
        {
            if (IsLoaded)
                throw new PluginEnvironmentException("Not loaded yet");
            string id = plugin.Info.ID;
            if (plugins.Exists(p => p.Info.Preconditions.Contains(id)))
                preconditions.Add(id);
            plugins.Remove(plugin);
        }

        private List<IPlugin> GetUnloadingSequence()
        {
            List<IPlugin> loadingSequence = GetLoadingSequence();
            loadingSequence.Reverse();
            return loadingSequence;
        }

        public string UnloadAll()
        {
            if (IsLoaded)
            {
                List<IPlugin> unloadingSequence = GetUnloadingSequence();
                foreach (IPlugin plugin in unloadingSequence)
                {
                    unloadingSequence.Add(plugin);
                    int result = plugin.Unload(this);
                    if (!plugin.IsSuccessFlag(result)) return plugin.GetMessageFromResult(result);
                }
                try
                {
                    return null;
                } finally
                {
                    IsLoaded = false;
                }
            }
            throw new PluginEnvironmentException("Not loaded yet");
        }

        public bool IsLoaded { get; private set; }
    }

    [Serializable]
    public class PluginEnvironmentException : Exception
    {
        public PluginEnvironmentException() { }

        public PluginEnvironmentException(string message) : base(message) { }

        protected PluginEnvironmentException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public PluginEnvironmentException(string message, Exception inner) : base(message, inner) { }
    }
}
