using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons;
using ECommons.Configuration;
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
        public Config Config;

        public ObjectExplorer(IDalamudPluginInterface pi)
        {
            P = this;
            ECommonsMain.Init(pi, this, Module.ObjectFunctions);
            new TickScheduler(delegate
            {
                Config = EzConfig.Init<Config>();
                ws = new();
                gui = new();
                ws.AddWindow(gui);
                Svc.PluginInterface.UiBuilder.Draw += ws.Draw;
                Svc.Commands.AddHandler("/oe", new(delegate { ToggleWindow(); }));
                Svc.PluginInterface.UiBuilder.OpenConfigUi += delegate { ToggleWindow(); };
            });
        }

        public void ToggleWindow()
        {
            gui.IsOpen = !gui.IsOpen;
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
