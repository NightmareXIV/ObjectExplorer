using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommons.DalamudServices.Legacy;

namespace ObjectExplorer
{
    internal unsafe class Gui : Window
    {
        int playersSort = 0;
        string playersFilter = string.Empty;

        public Gui() : base(P.Name)
        {
            this.SizeConstraints = new()
            {
                MinimumSize = new(300, 300),
                MaximumSize = new(float.MaxValue, float.MaxValue)
            };
        }

        public override void Draw()
        {
            if (Svc.ClientState.LocalPlayer != null)
            {
                IPlayerCharacter[] players = Svc.Objects.Where(x => x is IPlayerCharacter && x.Address != Svc.ClientState.LocalPlayer.Address).Select(x => (IPlayerCharacter)x).ToArray();
                if (playersSort == 1)
                {
                    players = players.OrderBy(x => x.Name.ToString()).ToArray();
                }
                else if (playersSort == 2)
                {
                    players = players.OrderBy(x => x.HomeWorld.Value.Name.ToString()).ToArray();
                }
                else if (playersSort == 3)
                {
                    players = players.OrderBy(x => Vector3.Distance(x.Position, Svc.ClientState.LocalPlayer.Position)).ToArray();
                }
                else if (playersSort == 4)
                {
                    players = players.OrderBy(x => x.CompanyTag.ToString()).ToArray();
                }
                ImGui.Text($"Total players: {players.Length}");
                ImGui.SameLine();
                ImGui.Text("Sort: ");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100f.Scale());
                ImGui.Combo("##pSort", ref playersSort, new string[] { "No sort", "Name", "Home world", "Distance", "FC" }, 5);
                ImGui.SameLine();
                ImGui.Text("Search: ");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                ImGui.InputText("##searchPlayer", ref playersFilter, 100);
                ImGui.BeginChild("##playerschild");
                ImGui.BeginTable("##logObjects", 4, ImGuiTableFlags.BordersInner | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit);
                ImGui.TableSetupColumn("Player name", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("Home world");
                ImGui.TableSetupColumn("Distance");
                ImGui.TableSetupColumn("FC");
                ImGui.TableHeadersRow();
                var i = 0;
                foreach (var p in players)
                {
                    if (string.IsNullOrEmpty(playersFilter)
                        || p.Name.ToString().Contains(playersFilter, StringComparison.OrdinalIgnoreCase)
                        || p.HomeWorld.Value.Name.ToString().Contains(playersFilter, StringComparison.OrdinalIgnoreCase)
                        || p.CompanyTag.ToString().Contains(playersFilter, StringComparison.OrdinalIgnoreCase)
                        )
                    {
                        i++;
                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();
                        var friend = ((FFXIVClientStructs.FFXIV.Client.Game.Character.Character*)p.Address)->IsFriend;
                        if (friend)
                        {
                            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudOrange);
                        }
                        if (ImGui.Selectable($"{p.Name}##{i}"))
                        {
                            if (((FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)p.Address)->GetIsTargetable())
                            {
                                Svc.Targets.SetTarget(p);
                            }
                            else
                            {
                                Svc.Toasts.ShowError($"{p.Name} can not be targeted.");
                            }
                        }
                        if (friend)
                        {
                            ImGui.PopStyleColor();
                        }
                        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                        {
                            if (!Svc.Commands.ProcessCommand($"/sf !!{p.Name}"))
                            {
                                Svc.Toasts.ShowError("Splatoon plugin must be installed");
                            }
                        }
                        ImGui.TableNextColumn();
                        ImGui.Text($"{p.HomeWorld.Value.Name}");
                        ImGui.TableNextColumn();
                        ImGui.Text($"{Vector3.Distance(Svc.ClientState.LocalPlayer.Position, p.Position):F1}");
                        ImGui.TableNextColumn();
                        ImGui.Text($"{p.CompanyTag}");
                    }
                }
                ImGui.EndTable();
                ImGui.EndChild();
            }
            else
            {
                ImGui.Text("Character does not exists");
            }
        }
    }
}
