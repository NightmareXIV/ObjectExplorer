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
            ECommonsMain.Init(pi, this, Module.ObjectFunctions);
            new TickScheduler(delegate
            {
                ws = new();
                gui = new();
                ws.AddWindow(gui);
                Svc.PluginInterface.UiBuilder.Draw += ws.Draw;
                Svc.Commands.AddHandler("/oe", new(delegate { gui.IsOpen = true; }));
                Svc.PluginInterface.UiBuilder.OpenConfigUi += delegate { gui.IsOpen = true; };
            });
        }

        public void Dispose()
        {
            Svc.Commands.RemoveHandler("/oe");
            Svc.PluginInterface.UiBuilder.Draw -= ws.Draw;
            ECommonsMain.Dispose();
            P = null;
        }
    }
}
