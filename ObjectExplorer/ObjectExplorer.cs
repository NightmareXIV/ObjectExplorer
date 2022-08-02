using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons;
using ECommons.Schedulers;
using System;

namespace ObjectExplorer
{
    public class ObjectExplorer : IDalamudPlugin
    {
        public string Name => "ObjectExplorer";
        internal WindowSystem ws;
        internal Gui gui;
        internal static ObjectExplorer P;

        public ObjectExplorer(DalamudPluginInterface pi)
        {
            P = this;
            ECommons.ECommons.Init(pi, Module.ObjectFunctions);
            new TickScheduler(delegate
            {
                ws = new();
                gui = new();
                ws.AddWindow(gui);
                Svc.PluginInterface.UiBuilder.Draw += ws.Draw;
            });
        }

        public void Dispose()
        {
            Svc.PluginInterface.UiBuilder.Draw -= ws.Draw;
            ECommons.ECommons.Dispose();
            P = null;
        }
    }
}
