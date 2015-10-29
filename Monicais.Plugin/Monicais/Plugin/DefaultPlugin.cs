
using Monicais.ThrowHelper;
using System.Collections.Generic;

namespace Monicais.Plugin
{

    public class DefaultPluginInfo : IPluginInfo
    {

        public DefaultPluginInfo(string id, string fullName, string simpleName, string virsion, string author)
            : this(id, fullName, simpleName, virsion, author, null, null) { }

        public DefaultPluginInfo(string id, string fullName, string simpleName,
            string virsion, string author, List<string> preconditions)
            : this(id, fullName, simpleName, virsion, author, null, preconditions) { }

        public DefaultPluginInfo(string id, string fullName, string simpleName,
            string virsion, string author, string description)
            : this(id, fullName, simpleName, virsion, author, description, null) { }

        public DefaultPluginInfo(string id, string fullName, string simpleName,
            string virsion, string author, string description, List<string> preconditions)
        {
            if (string.IsNullOrWhiteSpace(id))
                ArgumentNull.Throw("id");
            if (string.IsNullOrWhiteSpace(fullName))
                ArgumentNull.Throw("fullName");
            if (string.IsNullOrWhiteSpace(simpleName))
                ArgumentNull.Throw("simpleName");
            if (string.IsNullOrWhiteSpace(virsion))
                ArgumentNull.Throw("virsion");
            if (string.IsNullOrWhiteSpace(author))
                ArgumentNull.Throw("author");
            ID = id;
            FullName = fullName;
            SimpleName = simpleName;
            Virsion = virsion;
            Author = author;
            if (string.IsNullOrWhiteSpace(description))
                Description = string.Empty;
            else
                Description = description;
            if (preconditions == null || preconditions.Count == 0)
                Preconditions = new List<string>().AsReadOnly();
            else
                Preconditions = preconditions.AsReadOnly();
            HasPreconditions = Preconditions.Count == 0;
        }

        public string Author { get; private set; }

        public string Description { get; private set; }

        public string FullName { get; private set; }

        public bool HasPreconditions { get; private set; }

        public string ID { get; private set; }

        public IList<string> Preconditions { get; private set; }

        public string SimpleName { get; private set; }

        public string Virsion { get; private set; }
    }

    public class DefaultPlugin : IPlugin
    {
        private PluginTestSuccessFlag isSuccessFlag;
        private PluginOperate load;
        private PluginMessage msg;
        private PluginOperate unload;

        public DefaultPlugin(IPluginInfo info, PluginOperate load, PluginOperate unload,
            PluginTestSuccessFlag isSuccessFlag, PluginMessage getMsg)
        {
            if (info == null)
                ArgumentNull.Throw("info");
            if (load == null)
                ArgumentNull.Throw("load");
            if (unload == null)
                ArgumentNull.Throw("unload");
            if (isSuccessFlag == null)
                ArgumentNull.Throw("isSuccessFlag");
            if (getMsg == null)
                ArgumentNull.Throw("getMsg");
            this.Info = info;
            this.load = load;
            this.unload = unload;
            this.isSuccessFlag = isSuccessFlag;
            this.msg = getMsg;
        }

        public string GetMessageFromResult(int result)
        {
            return msg(result);
        }

        public bool IsSuccessFlag(int result)
        {
            return isSuccessFlag(result);
        }

        public int Load(PluginEnvironment env)
        {
            return load(env);
        }

        public int Unload(PluginEnvironment env)
        {
            return unload(env);
        }

        public IPluginInfo Info { get; private set; }
    }
}
