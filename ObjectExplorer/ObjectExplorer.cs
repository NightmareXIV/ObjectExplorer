using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons;
using System;

namespace ObjectExplorer
{
    public class ObjectExplorer : IDalamudPlugin
    {
        public string Name => "ObjectExplorer";
        internal ObjectExplorer P;
        internal WindowSystem ws;
        internal Gui gui;

        public ObjectExplorer(DalamudPluginInterface pi)
        {
            P = this;
            ECommons.ECommons.Init(pi, Module.ObjectFunctions);
        }

        public void Dispose()
        {

            P = null;
        }
    }
}
