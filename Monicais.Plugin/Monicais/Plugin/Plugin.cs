
using System;
using System.Collections.Generic;

namespace Monicais.Plugin
{

    public interface IPluginInfo
    {
        string Author { get; }

        string Description { get; }

        string FullName { get; }

        bool HasPreconditions { get; }

        string ID { get; }

        IList<string> Preconditions { get; }

        string SimpleName { get; }

        string Virsion { get; }
    }

    public interface IPlugin
    {
        string GetMessageFromResult(int result);
        bool IsSuccessFlag(int result);
        int Load(PluginEnvironment env);
        int Unload(PluginEnvironment env);

        IPluginInfo Info { get; }
    }

    public delegate int PluginOperate(PluginEnvironment env);

    public delegate string PluginMessage(int result);

    public delegate bool PluginTestSuccessFlag(int result);
}
