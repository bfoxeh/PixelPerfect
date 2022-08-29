using System;
using System.Diagnostics;
using Dalamud.Configuration;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface;
using Dalamud.Plugin;
using ImGuiNET;
using Num = System.Numerics;

namespace PixelPerfect
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class PixelPerfect : IDalamudPlugin
    {
        public string Name => "Pixel Perfect";
        private readonly DalamudPluginInterface _pi;
        private readonly CommandManager _cm;
        private readonly ClientState _cs;
        private readonly Framework _fw;
        private readonly GameGui _gui;
        private readonly Condition _condition;
        private readonly PartyList _partylist;
        private readonly Config _configuration;

        private bool _config;
        private bool _combat;
        private bool _instance;

        //hbdot
        private bool _enabled;
        private Num.Vector4 _col = new Num.Vector4(1f, 1f, 1f, 1f);
        private float _hbdotradius = 2f;

        //ring
        private bool _ring;
        private Num.Vector4 _colRing = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _radius = 10f;
        private int _segments = 100;
        private float _thickness = 5f;

        //ring2
        private bool _ring2;
        private Num.Vector4 _colRing2 = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _radius2 = 10f;
        private int _segments2 = 100;
        private float _thickness2 = 5f;

        //ring3
        private bool _ring3;
        private Num.Vector4 _colRing3 = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _radius3 = 10f;
        private int _segments3 = 100;
        private float _thickness3 = 5f;

        //ring4
        private bool _ring4;
        private Num.Vector4 _colRing4 = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _radius4 = 10f;
        private int _segments4 = 100;
        private float _thickness4 = 5f;

        //ring5
        private bool _ring5;
        private Num.Vector4 _colRing5 = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _radius5 = 10f;
        private int _segments5 = 100;
        private float _thickness5 = 5f;

        //hitbox ring
        private bool _hbring;
        private Num.Vector4 _hbcolRing = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _hbradius = 10f;
        private int _hbsegments = 100;
        private float _hbthickness = 5f;

        //party hitbox dot
        private bool _partyenabled;
        private Num.Vector4 _pdotcol = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _pdotthickness = 2f;

        //party hitbox ring
        private bool _phbring;
        private Num.Vector4 _phbcolRing = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _phbradius = 10f;
        private int _phbsegments = 100;
        private float _phbthickness = 5f;

        //party hitbox ring rolecolor options
        private bool _rolecolors;
        private Num.Vector4 _melecolor = new Num.Vector4(255.0f, 0.0f, 0.0f, 128f);
        private Num.Vector4 _physrangecolor = new Num.Vector4(255.0f, 0.0f, 255.0f, 128.0f);
        private Num.Vector4 _magicrangecolor = new Num.Vector4(255.0f, 255.0f, 0.0f, 128.0f);
        private Num.Vector4 _tankcolor = new Num.Vector4(0.0f, 0.0f, 255.0f, 128.0f);
        private Num.Vector4 _healercolor = new Num.Vector4(0.0f, 255.0f, 0.0f, 0.0f);

        //party member ring
        private bool _partyring1;
        private Num.Vector4 _partycolRing1 = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        private float _partyradius1 = 3f;
        private int _partysegments1 = 100;
        private float _partythickness1 = 5f;

        //compass stuff
        private bool _north, _east, _south, _west, _player;
        private bool _nchev, _echev, _schev, _wchev, _pchev;
        private bool _nline, _eline, _sline, _wline, _pline;

        private float _nchevLength = 0.5f, _echevLength = 0.5f, _schevLength = 0.5f, _wchevLength = 0.5f, _pchevLength = 0.5f;
        private float _nchevOffset = 0.5f, _echevOffset = 0.5f, _schevOffset = 0.43f, _wchevOffset = 0.5f, _pchevOffset = 0.5f;
        private float _nchevRad = 0.5f, _echevRad = 0.5f, _schevRad = 0.5f, _wchevRad = 0.5f, _pchevRad = 0.3f;
        private float _nchevThicc = 5f, _echevThicc = 5f, _schevThicc = 5f, _wchevThicc = 5f, _pchevThicc = 5f;

        private Num.Vector4 _nchevCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _echevCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _schevCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _wchevCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _pchevCol = new Num.Vector4(1f, 1f, 1f, 1f);

        private float _nlineOffset = 0.6f, _elineOffset = 0.6f, _slineOffset = 0.6f, _wlineOffset = 0.6f, _plineOffset = 0.6f;
        private float _nlineLength = 1f, _elineLength = 1f, _slineLength = 1f, _wlineLength = 1f, _plineLength = 1f;
        private float _nlineThicc = 10f, _elineThicc = 10f, _slineThicc = 10f, _wlineThicc = 10f, _plineThicc = 10f;

        private Num.Vector4 _nlineCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _elineCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _slineCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _wlineCol = new Num.Vector4(1f, 1f, 1f, 1f);
        private Num.Vector4 _plineCol = new Num.Vector4(1f, 1f, 1f, 1f);

        //save config after 100 ticks
        private int dirtyHack = 0;


        public PixelPerfect(
            DalamudPluginInterface pluginInterface,
            CommandManager commandManager,
            ClientState clientState,
            Framework framework,
            GameGui gameGui,
            Condition condition,
            PartyList partylist
        )
        {
            _pi = pluginInterface;
            _cm = commandManager;
            _cs = clientState;
            _fw = framework;
            _gui = gameGui;
            _condition = condition;
            _partylist = partylist;

            _configuration = pluginInterface.GetPluginConfig() as Config ?? new Config();

            _combat = _configuration.Combat;
            _instance = _configuration.Instance;

            _enabled = _configuration.Enabled;
            _col = _configuration.Col;
            _hbdotradius = _configuration.HBdotradius;

            _north = _configuration.North;
            _east = _configuration.East;
            _south = _configuration.South;
            _west = _configuration.West;
            _player = _configuration.Player;

            _hbring = _configuration.HBRing;
            _hbthickness = _configuration.HBThickness;
            _hbcolRing = _configuration.HBColRing;
            _hbsegments = _configuration.HBSegments;
            _hbradius = _configuration.HBRadius;

            _ring = _configuration.Ring;
            _thickness = _configuration.Thickness;
            _colRing = _configuration.ColRing;
            _segments = _configuration.Segments;
            _radius = _configuration.Radius;


            _ring2 = _configuration.Ring2;
            _thickness2 = _configuration.Thickness2;
            _colRing2 = _configuration.ColRing2;
            _segments2 = _configuration.Segments2;
            _radius2 = _configuration.Radius2;

            _ring3 = _configuration.Ring3;
            _thickness3 = _configuration.Thickness3;
            _colRing3 = _configuration.ColRing3;
            _segments3 = _configuration.Segments3;
            _radius3 = _configuration.Radius3;

            _ring4 = _configuration.Ring4;
            _thickness4 = _configuration.Thickness4;
            _colRing4 = _configuration.ColRing4;
            _segments4 = _configuration.Segments4;
            _radius4 = _configuration.Radius4;

            _ring5 = _configuration.Ring5;
            _thickness5 = _configuration.Thickness5;
            _colRing5 = _configuration.ColRing5;
            _segments5 = _configuration.Segments5;
            _radius5 = _configuration.Radius5;

            _nchev = _configuration.NChev;
            _nchevLength = _configuration.NChevLength;
            _nchevOffset = _configuration.NChevOffset;
            _nchevRad = _configuration.NChevRad;
            _nchevThicc = _configuration.NChevThicc;
            _nchevCol = _configuration.NChevCol;

            _echev = _configuration.EChev;
            _echevLength = _configuration.EChevLength;
            _echevOffset = _configuration.EChevOffset;
            _echevRad = _configuration.EChevRad;
            _echevThicc = _configuration.EChevThicc;
            _echevCol = _configuration.EChevCol;

            _schev = _configuration.SChev;
            _schevLength = _configuration.SChevLength;
            _schevOffset = _configuration.SChevOffset;
            _schevRad = _configuration.SChevRad;
            _schevThicc = _configuration.SChevThicc;
            _schevCol = _configuration.SChevCol;

            _wchev = _configuration.WChev;
            _wchevLength = _configuration.WChevLength;
            _wchevOffset = _configuration.WChevOffset;
            _wchevRad = _configuration.WChevRad;
            _wchevThicc = _configuration.WChevThicc;
            _wchevCol = _configuration.WChevCol;

            _pchev = _configuration.PChev;
            _pchevLength = _configuration.PChevLength;
            _pchevOffset = _configuration.PChevOffset;
            _pchevRad = _configuration.PChevRad;
            _pchevThicc = _configuration.PChevThicc;
            _pchevCol = _configuration.PChevCol;

            _nline = _configuration.NLine;
            _nlineOffset = _configuration.NLineOffset;
            _nlineLength = _configuration.NLineLength;
            _nlineThicc = _configuration.NLineThicc;
            _nlineCol = _configuration.NLineCol;

            _eline = _configuration.ELine;
            _elineOffset = _configuration.ELineOffset;
            _elineLength = _configuration.ELineLength;
            _elineThicc = _configuration.ELineThicc;
            _elineCol = _configuration.ELineCol;

            _sline = _configuration.SLine;
            _slineOffset = _configuration.SLineOffset;
            _slineLength = _configuration.SLineLength;
            _slineThicc = _configuration.SLineThicc;
            _slineCol = _configuration.SLineCol;

            _wline = _configuration.WLine;
            _wlineOffset = _configuration.WLineOffset;
            _wlineLength = _configuration.WLineLength;
            _wlineThicc = _configuration.WLineThicc;
            _wlineCol = _configuration.WLineCol;

            _pline = _configuration.PLine;
            _plineOffset = _configuration.PLineOffset;
            _plineLength = _configuration.PLineLength;
            _plineThicc = _configuration.PLineThicc;
            _plineCol = _configuration.PLineCol;

            _phbring = _configuration.PhbRing;
            _phbcolRing = _configuration.PhbColRing;
            _phbradius = _configuration.PhbRadius;
            _phbsegments = _configuration.PhbSegments;
            _phbthickness = _configuration.PhbThickness;

            _partyenabled = _configuration.Penabled;
            _pdotcol = _configuration.PdotCol;

            _partyring1 = _configuration.PRing1;
            _partycolRing1 = _configuration.PColRing1;
            _partyradius1 = _configuration.PRadius1;
            _partysegments1 = _configuration.PSegments1;
            _partythickness1 = _configuration.PThickness1;

            _rolecolors = _configuration.RoleColors;
            _melecolor  = _configuration.MeleColor;
            _physrangecolor = _configuration.PhysRangeColor;
            _magicrangecolor = _configuration.MagicRangeColor;
            _tankcolor = _configuration.TankColor;
            _healercolor = _configuration.HealerColor;


            pluginInterface.UiBuilder.Draw += DrawWindow;
            pluginInterface.UiBuilder.OpenConfigUi += ConfigWindow;
            commandManager.AddHandler("/pp", new CommandInfo(Command)
            {
                HelpMessage = "Pixel Perfect config." +
                              "\nArguments of 'ring', 'ring2', 'north' will enable/disable those features."
            });
        }

        private void ConfigWindow()
        {
            _config = true;
        }

        public void Dispose()
        {
            _pi.UiBuilder.Draw -= DrawWindow;
            _pi.UiBuilder.OpenConfigUi -= ConfigWindow;
            _cm.RemoveHandler("/pp");
        }

        ///<GUI Code>///
        private void DrawWindow()
        {
            if (_config)
            {
                ImGui.SetNextWindowSize(new Num.Vector2(300, 500), ImGuiCond.FirstUseEver);
                ImGui.Begin("Pixel Perfect Config", ref _config);

                ImGui.Checkbox("Combat Only", ref _combat);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Only show all of this during combat");
                }
                ImGui.SameLine();
                ImGui.Checkbox("Instance Only", ref _instance);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Only show all of this during instances (like dungeons, raids etc)");
                }

                ImGui.Separator();
                ImGui.Checkbox("Hitbox Dot", ref _enabled);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("A visual representation of your hitbox");
                }
                if (_enabled)
                {
                    ImGui.SameLine();
                    ImGui.ColorEdit4("Hitbox Colour", ref _col, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The colour of the hitbox");
                    }
                    ImGui.DragFloat("HB dot thickness", ref _hbdotradius);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the Dot");
                    }
                }
                ImGui.Checkbox("Hitbox Ring", ref _hbring);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show another ring around your character");
                }
                if (_hbring)
                {
                    ImGui.DragFloat("HB ring thickness", ref _hbthickness);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("HB smoothness", ref _hbsegments);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("HB Ring Color", ref _hbcolRing, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The colour of the ring");
                    }
                }
                ImGui.Separator();
                ImGui.Checkbox("N Stuff", ref _north);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Options for N facing objects");
                }

                if (_north)
                {
                    ImGui.SameLine();
                    ImGui.Checkbox("NLine", ref _nline);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("N facing line");
                    }
                    ImGui.SameLine();
                    ImGui.Checkbox("NChev", ref _nchev);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("N arrowhead");
                    }
                    if (_nline)
                    {
                        ImGui.DragFloat("N Line Offset", ref _nlineOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How far from your hitbox to start the N line");
                        }
                        ImGui.DragFloat("N Line Length", ref _nlineLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How long the line N is");
                        }
                        ImGui.DragFloat("N Line Thickness", ref _nlineThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the N line is");
                        }
                        ImGui.ColorEdit4("N Line Colour", ref _nlineCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the N line");
                        }
                    }
                    if (_nchev)
                    {
                        ImGui.DragFloat("N Chevron Offset", ref _nchevOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Chevron pointing N");
                        }
                        ImGui.DragFloat("N Chevron Length", ref _nchevLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("N Chevron Lenght");
                        }
                        ImGui.DragFloat("N Chevron Width", ref _nchevRad);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("N Chevron Width");
                        }
                        ImGui.DragFloat("N Chevron Thickness", ref _nchevThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the N Chevron is");
                        }
                        ImGui.ColorEdit4("N Chevron Colour", ref _nchevCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the N Chevron");
                        }
                    }
                }
                ImGui.Checkbox("E Stuff", ref _east);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Options for E facing objects");
                }

                if (_east)
                {
                    ImGui.SameLine();
                    ImGui.Checkbox("ELine", ref _eline);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("E facing line");
                    }
                    ImGui.SameLine();
                    ImGui.Checkbox("EChev", ref _echev);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("E arrowhead");
                    }
                    if (_eline)
                    {
                        ImGui.DragFloat("E Line Offset", ref _elineOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How far from your hitbox to start the E line");
                        }
                        ImGui.DragFloat("E Line Length", ref _elineLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How long the line E is");
                        }
                        ImGui.DragFloat("E Line Thickness", ref _elineThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the E line is");
                        }
                        ImGui.ColorEdit4("E Line Colour", ref _elineCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the E line");
                        }
                    }
                    if (_echev)
                    {
                        ImGui.DragFloat("E Chevron Offset", ref _echevOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Chevron pointing E");
                        }
                        ImGui.DragFloat("E Chevron Length", ref _echevLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("E Chevron Lenght");
                        }
                        ImGui.DragFloat("E Chevron Width", ref _echevRad);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("E Chevron Width");
                        }
                        ImGui.DragFloat("E Chevron Thickness", ref _echevThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the E Chevron is");
                        }
                        ImGui.ColorEdit4("E Chevron Colour", ref _echevCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the E Chevron");
                        }
                    }
                }
                ImGui.Checkbox("S Stuff", ref _south);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Options for S facing objects");
                }

                if (_south)
                {
                    ImGui.SameLine();
                    ImGui.Checkbox("SLine", ref _sline);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("S facing line");
                    }
                    ImGui.SameLine();
                    ImGui.Checkbox("SChev", ref _schev);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("S arrowhead");
                    }
                    if (_sline)
                    {
                        ImGui.DragFloat("S Line Offset", ref _slineOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How far from your hitbox to start the S line");
                        }
                        ImGui.DragFloat("S Line Length", ref _slineLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How long the line S is");
                        }
                        ImGui.DragFloat("S Line Thickness", ref _slineThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the S line is");
                        }
                        ImGui.ColorEdit4("S Line Colour", ref _slineCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the S line");
                        }
                    }
                    if (_schev)
                    {
                        ImGui.DragFloat("S Chevron Offset", ref _schevOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Chevron pointing S");
                        }
                        ImGui.DragFloat("S Chevron Length", ref _schevLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("S Chevron Lenght");
                        }
                        ImGui.DragFloat("S Chevron Width", ref _schevRad);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("S Chevron Width");
                        }
                        ImGui.DragFloat("S Chevron Thickness", ref _schevThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the S Chevron is");
                        }
                        ImGui.ColorEdit4("S Chevron Colour", ref _schevCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the S Chevron");
                        }
                    }
                }
                ImGui.Checkbox("W Stuff", ref _west);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Options for W facing objects");
                }

                if (_west)
                {
                    ImGui.SameLine();
                    ImGui.Checkbox("WLine", ref _wline);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("W facing line");
                    }
                    ImGui.SameLine();
                    ImGui.Checkbox("WChev", ref _wchev);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("W arrowhead");
                    }
                    if (_wline)
                    {
                        ImGui.DragFloat("W Line Offset", ref _wlineOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How far from your hitbox to start the W line");
                        }
                        ImGui.DragFloat("N Line Length", ref _wlineLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How long the line W is");
                        }
                        ImGui.DragFloat("W Line Thickness", ref _wlineThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the W line is");
                        }
                        ImGui.ColorEdit4("W Line Colour", ref _wlineCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the W line");
                        }
                    }
                    if (_wchev)
                    {
                        ImGui.DragFloat("W Chevron Offset", ref _wchevOffset);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Chevron pointing W");
                        }
                        ImGui.DragFloat("W Chevron Length", ref _wchevLength);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("W Chevron Lenght");
                        }
                        ImGui.DragFloat("W Chevron Width", ref _wchevRad);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("W Chevron Width");
                        }
                        ImGui.DragFloat("W Chevron Thickness", ref _wchevThicc);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("How thicc the W Chevron is");
                        }
                        ImGui.ColorEdit4("W Chevron Colour", ref _wchevCol, ImGuiColorEditFlags.NoInputs);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("The color of the W Chevron");
                        }
                    }
                }

                //

                ImGui.Separator();

                ImGui.Checkbox("PlayerLine", ref _pline);
                {
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Show a line character facing");
                    }
                }
                ImGui.SameLine();
                ImGui.Checkbox("PlayerChevron", ref _pchev);
                {
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Show a chevron character facing");
                    }
                }
                if (_pline)
                {
                    ImGui.DragFloat("player Line Offset", ref _plineOffset);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How far from your hitbox to start the line");
                    }
                    ImGui.DragFloat("player Line Length", ref _plineLength);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How long the line is");
                    }
                    ImGui.DragFloat("player Line Thickness", ref _plineThicc);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How thicc the line is");
                    }
                    ImGui.ColorEdit4("player Line Colour", ref _plineCol, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of the line");
                    }
                }
                if (_pchev)
                {
                    ImGui.DragFloat(" Player Chevron Offset", ref _pchevOffset);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Chevron pointing Player");
                    }
                    ImGui.DragFloat("Player Chevron Length", ref _pchevLength);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Chevron Lenght");
                    }
                    ImGui.DragFloat("Player Chevron Width", ref _pchevRad);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Chevron Width");
                    }
                    ImGui.DragFloat("Player Chevron Thickness", ref _pchevThicc);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How thicc the Chevron is");
                    }
                    ImGui.ColorEdit4("Player Chevron Colour", ref _pchevCol, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of the Chevron");
                    }
                }
                ImGui.Separator();
                ImGui.Checkbox("PartyDot", ref _partyenabled);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show hitbox dot under party members");
                }
                ImGui.SameLine();
                ImGui.Checkbox("P HB Ring", ref _phbring);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show hitbox ring around party members");
                }
                ImGui.SameLine();
                ImGui.Checkbox("P Ring", ref _partyring1);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show a ring around party members");
                }
                if (_partyenabled)
                {
                    ImGui.ColorEdit4("P Hitbox Color", ref _pdotcol, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The colour of the hitbox");
                    }
                    ImGui.SameLine();
                    ImGui.Checkbox("Role Colors", ref _rolecolors);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("Sets party hitbox to role colors");
                    }
                    ImGui.DragFloat("PHB dot thickness", ref _pdotthickness);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the party Dot");
                    }

                }
                if (_rolecolors)
                {
                    ImGui.ColorEdit4("Mele Color", ref _melecolor, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of mele hitbox");
                    }
                    ImGui.ColorEdit4("P Ranged Color", ref _physrangecolor, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of P Ranged hitbox");
                    }
                    ImGui.ColorEdit4("M Ranged Color", ref _magicrangecolor, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of M Ranged hitbox");
                    }
                    ImGui.ColorEdit4("Tank Color", ref _tankcolor, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of Tank hitbox");
                    }
                    ImGui.ColorEdit4("Healer Color", ref _healercolor, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of Healer hitbox");
                    }
                }
                if (_phbring)
                {
                    ImGui.DragFloat("PHB ring thickness", ref _phbthickness);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("PHB smoothness", ref _phbsegments);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("PHB Ring Color", ref _phbcolRing, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The colour of the ring");
                    }
                }
                if (_partyring1)
                {
                    ImGui.DragFloat("PartyRingRad", ref _partyradius1);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The radius of the ring");
                    }
                    ImGui.DragFloat("PartyThickness", ref _partythickness1);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("PartySmoothness", ref _partysegments1);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("Party Ring Colour", ref _partycolRing1, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of the Party ring");
                    }
                }
                ImGui.Separator();
                ImGui.Checkbox("Ring", ref _ring);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show a ring around your character");
                }
                if (_ring)
                {
                    ImGui.DragFloat("Yalms", ref _radius);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The radius of the ring");
                    }
                    ImGui.DragFloat("Thickness", ref _thickness);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("Smoothness", ref _segments);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("Ring Colour", ref _colRing, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of the ring");
                    }
                }
                ImGui.Separator();
                ImGui.Checkbox("Ring 2", ref _ring2);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show another ring around your character");
                }
                if (_ring2)
                {
                    ImGui.DragFloat("Yalms 2", ref _radius2);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The radius of the ring");
                    }
                    ImGui.DragFloat("Thickness 2", ref _thickness2);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("Smoothness 2", ref _segments2);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("Ring Colour 2", ref _colRing2, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of the ring");
                    }
                }

                ImGui.Separator();
                ImGui.Checkbox("Ring 3", ref _ring3);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show another ring around your character");
                }
                if (_ring3)
                {
                    ImGui.DragFloat("Yalms 3", ref _radius3);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The radius of the ring");
                    }
                    ImGui.DragFloat("Thickness 3", ref _thickness3);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("Smoothness 3", ref _segments3);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("Ring Colour 3", ref _colRing3, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of the ring");
                    }
                }

                ImGui.Separator();
                ImGui.Checkbox("Ring 4", ref _ring4);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show another ring around your character");
                }
                if (_ring4)
                {
                    ImGui.DragFloat("Yalms 4", ref _radius4);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The radius of the ring");
                    }
                    ImGui.DragFloat("Thickness 4", ref _thickness4);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("Smoothness 4", ref _segments4);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("Ring Colour 4", ref _colRing4, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The color of the ring");
                    }
                }

                ImGui.Separator();
                ImGui.Checkbox("Ring 5", ref _ring5);
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Show another ring around your character");
                }
                if (_ring5)
                {
                    ImGui.DragFloat("Yalms 5", ref _radius5);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The radius of the ring");
                    }
                    ImGui.DragFloat("Thickness 5", ref _thickness5);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The thiccness of the ring");
                    }
                    ImGui.DragInt("Smoothness 5", ref _segments5);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("How many segments to make the ring out of");
                    }
                    ImGui.ColorEdit4("Ring Color 5", ref _colRing5, ImGuiColorEditFlags.NoInputs);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("The colour of the ring");
                    }
                }



                if (ImGui.Button("Save and Close Config"))
                {
                    SaveConfig();
                    _config = false;
                }


                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0x005E5BFF);
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, 0xDD000000 | 0x005E5BFF);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0x005E5BFF);


                if (ImGui.Button("Buy Haplo a Hot Chocolate"))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://ko-fi.com/haplo",
                        UseShellExecute = true
                    });
                }


                ImGui.PopStyleColor(3);
                ImGui.End();



                if (dirtyHack > 100)
                {
                    SaveConfig();
                    dirtyHack = 0;
                }

                dirtyHack++;
            }

            ///<logic>///

            if (_cs.LocalPlayer == null) return;

            if (_combat)
            {
                if (!_condition[ConditionFlag.InCombat])
                {
                    return;
                }
            }

            if (_instance)
            {
                if (!_condition[ConditionFlag.BoundByDuty])
                {
                    return;
                }

            }

            var actor = _cs.LocalPlayer;

            if (!_gui.WorldToScreen(
                new Num.Vector3(actor.Position.X, actor.Position.Y, actor.Position.Z),
                out var pos)) return;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(0, 0));
            ImGuiHelpers.ForceNextWindowMainViewport();
            ImGuiHelpers.SetNextWindowPosRelativeMainViewport(new Num.Vector2(0, 0));
            ImGui.Begin("Ring",
                ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoBackground);
            ImGui.SetWindowSize(ImGui.GetIO().DisplaySize);

            if (_enabled)
            {

                ImGui.GetWindowDrawList().AddCircleFilled(
                    new Num.Vector2(pos.X, pos.Y),
                    _hbdotradius,
                    ImGui.GetColorU32(_col),
                    100);
            }

            if (_ring)
            {
                DrawRingWorld(_cs.LocalPlayer, _radius, _segments, _thickness,
                    ImGui.GetColorU32(_colRing));
            }

            if (_ring2)
            {
                DrawRingWorld(_cs.LocalPlayer, _radius2, _segments2, _thickness2,
                    ImGui.GetColorU32(_colRing2));
            }

            if (_ring3)
            {
                DrawRingWorld(_cs.LocalPlayer, _radius3, _segments3, _thickness3,
                    ImGui.GetColorU32(_colRing3));
            }

            if (_ring4)
            {
                DrawRingWorld(_cs.LocalPlayer, _radius4, _segments4, _thickness4,
                    ImGui.GetColorU32(_colRing4));
            }

            if (_ring5)
            {
                DrawRingWorld(_cs.LocalPlayer, _radius5, _segments5, _thickness5,
                    ImGui.GetColorU32(_colRing5));
            }

            if (_hbring)
            {
                DrawRingWorld(_cs.LocalPlayer, _cs.LocalPlayer.HitboxRadius, _hbsegments, _hbthickness,
                    ImGui.GetColorU32(_hbcolRing));
            }

            if (_north)
            {
                if (_nline)
                {
                    _gui.WorldToScreen(new Num.Vector3(
                                actor.Position.X + ((_nlineLength + _nlineOffset) * (float)Math.Sin(Math.PI)),
                                actor.Position.Y,
                                actor.Position.Z + ((_nlineLength + _nlineOffset) * (float)Math.Cos(Math.PI))
                            ),
                            out Num.Vector2 lineTip);

                    _gui.WorldToScreen(new Num.Vector3(
                            actor.Position.X + (_nlineOffset * (float)Math.Sin(Math.PI)),
                            actor.Position.Y,
                            actor.Position.Z + (_nlineOffset * (float)Math.Cos(Math.PI))
                        ),
                        out Num.Vector2 lineOffset);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(lineTip.X, lineTip.Y), new Num.Vector2(lineOffset.X, lineOffset.Y),
                        ImGui.GetColorU32(_nlineCol), _nlineThicc);
                }
                if (_nchev)
                {
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X + (_nchevRad / 2), actor.Position.Y, actor.Position.Z - _nchevOffset), out Num.Vector2 chevOffset1);
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X - (_nchevRad / 2), actor.Position.Y, actor.Position.Z - _nchevOffset), out Num.Vector2 chevOffset2);

                    _gui.WorldToScreen(new Num.Vector3(
                        actor.Position.X + ((_nchevOffset + _nchevLength) * (float)Math.Sin(Math.PI)),
                        actor.Position.Y,
                        actor.Position.Z + ((_nchevOffset + _nchevLength) * (float)Math.Cos(Math.PI))
                    ),
                    out Num.Vector2 chevTip);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset1.X, chevOffset1.Y),
                        ImGui.GetColorU32(_nchevCol), _nchevThicc);
                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset2.X, chevOffset2.Y),
                        ImGui.GetColorU32(_nchevCol), _nchevThicc);
                }
            }

            if (_east)
            {
                if (_eline)
                {
                    _gui.WorldToScreen(new Num.Vector3(
                                actor.Position.X + (_elineLength + _elineOffset),
                                actor.Position.Y,
                                actor.Position.Z
                            ),
                            out Num.Vector2 lineTip);

                    _gui.WorldToScreen(new Num.Vector3(
                            actor.Position.X + (_elineOffset),
                            actor.Position.Y,
                            actor.Position.Z
                        ),
                        out Num.Vector2 lineOffset);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(lineTip.X, lineTip.Y), new Num.Vector2(lineOffset.X, lineOffset.Y),
                        ImGui.GetColorU32(_elineCol), _elineThicc);
                }
                if (_echev)
                {
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X + _echevOffset, actor.Position.Y, actor.Position.Z + (_echevRad / 2)), out Num.Vector2 chevOffset1);
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X + _echevOffset, actor.Position.Y, actor.Position.Z - (_echevRad / 2)), out Num.Vector2 chevOffset2);

                    _gui.WorldToScreen(new Num.Vector3(
                        actor.Position.X + (_echevOffset + _echevLength),
                        actor.Position.Y,
                        actor.Position.Z
                    ),
                    out Num.Vector2 chevTip);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset1.X, chevOffset1.Y),
                        ImGui.GetColorU32(_echevCol), _echevThicc);
                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset2.X, chevOffset2.Y),
                        ImGui.GetColorU32(_echevCol), _echevThicc);
                }
            }

            if (_south)
            {
                if (_sline)
                {
                    _gui.WorldToScreen(new Num.Vector3(
                                actor.Position.X,
                                actor.Position.Y,
                                actor.Position.Z + (_slineLength + _slineOffset)
                            ),
                            out Num.Vector2 lineTip);

                    _gui.WorldToScreen(new Num.Vector3(
                            actor.Position.X,
                            actor.Position.Y,
                            actor.Position.Z + (_slineOffset)
                        ),
                        out Num.Vector2 lineOffset);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(lineTip.X, lineTip.Y), new Num.Vector2(lineOffset.X, lineOffset.Y),
                        ImGui.GetColorU32(_slineCol), _slineThicc);
                }
                if (_schev)
                {
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X + (_schevRad / 2), actor.Position.Y, actor.Position.Z + _schevOffset), out Num.Vector2 chevOffset1);
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X - (_schevRad / 2), actor.Position.Y, actor.Position.Z + _schevOffset), out Num.Vector2 chevOffset2);

                    _gui.WorldToScreen(new Num.Vector3(
                        actor.Position.X,
                        actor.Position.Y,
                        actor.Position.Z + (_schevOffset + _schevLength)
                    ),
                    out Num.Vector2 chevTip);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset1.X, chevOffset1.Y),
                        ImGui.GetColorU32(_schevCol), _schevThicc);
                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset2.X, chevOffset2.Y),
                        ImGui.GetColorU32(_schevCol), _schevThicc);
                }
            }

            if (_west)
            {
                if (_wline)
                {
                    _gui.WorldToScreen(new Num.Vector3(
                                actor.Position.X - (_wlineLength + _wlineOffset),
                                actor.Position.Y,
                                actor.Position.Z
                            ),
                            out Num.Vector2 lineTip);

                    _gui.WorldToScreen(new Num.Vector3(
                            actor.Position.X - (_wlineOffset),
                            actor.Position.Y,
                            actor.Position.Z
                        ),
                        out Num.Vector2 lineOffset);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(lineTip.X, lineTip.Y), new Num.Vector2(lineOffset.X, lineOffset.Y),
                        ImGui.GetColorU32(_wlineCol), _wlineThicc);
                }
                if (_wchev)
                {
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X - _wchevOffset, actor.Position.Y, actor.Position.Z + (_wchevRad / 2)), out Num.Vector2 chevOffset1);
                    _gui.WorldToScreen(new Num.Vector3(actor.Position.X - _wchevOffset, actor.Position.Y, actor.Position.Z - (_wchevRad / 2)), out Num.Vector2 chevOffset2);

                    _gui.WorldToScreen(new Num.Vector3(
                        actor.Position.X - (_wchevOffset + _wchevLength),
                        actor.Position.Y,
                        actor.Position.Z
                    ),
                    out Num.Vector2 chevTip);

                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset1.X, chevOffset1.Y),
                        ImGui.GetColorU32(_wchevCol), _wchevThicc);
                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset2.X, chevOffset2.Y),
                        ImGui.GetColorU32(_wchevCol), _wchevThicc);
                }
            }

            if (_pline)
            {

                _gui.WorldToScreen(new Num.Vector3(
                        actor.Position.X + (_plineOffset * (float)Math.Sin(actor.Rotation)),
                        actor.Position.Y,
                        actor.Position.Z + (_plineOffset * (float)Math.Cos(actor.Rotation))
                    ),
                    out Num.Vector2 lineOffset);

                _gui.WorldToScreen(new Num.Vector3(
                        actor.Position.X + ((_plineLength + _plineOffset) * (float)Math.Sin(actor.Rotation)),
                        actor.Position.Y,
                        actor.Position.Z + ((_plineLength + _plineOffset) * (float)Math.Cos(actor.Rotation))
                    ),
                    out Num.Vector2 lineTip);


                ImGui.GetWindowDrawList().AddLine(new Num.Vector2(lineTip.X, lineTip.Y), new Num.Vector2(lineOffset.X, lineOffset.Y), ImGui.GetColorU32(_plineCol), _plineThicc);
            }

            if (_pchev)
            {
                _gui.WorldToScreen(new Num.Vector3(actor.Position.X + _pchevOffset * (float)Math.Sin(actor.Rotation + _pchevRad),
                                                   actor.Position.Y,
                                                   actor.Position.Z + _pchevOffset * (float)Math.Cos(actor.Rotation + _pchevRad)),
                                                   out Num.Vector2 chevOffset1);

                _gui.WorldToScreen(new Num.Vector3(actor.Position.X + _pchevOffset * (float)Math.Sin(actor.Rotation - _pchevRad),
                                                   actor.Position.Y,
                                                   actor.Position.Z + _pchevOffset * (float)Math.Cos(actor.Rotation - _pchevRad)),
                                                   out Num.Vector2 chevOffset2);

                _gui.WorldToScreen(new Num.Vector3(
                        actor.Position.X + ((_pchevLength + _pchevOffset) * (float)Math.Sin(actor.Rotation)),
                        actor.Position.Y,
                        actor.Position.Z + ((_pchevLength + _pchevOffset) * (float)Math.Cos(actor.Rotation))
                    ),
                    out Num.Vector2 chevTip);

                ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset1.X, chevOffset1.Y), ImGui.GetColorU32(_pchevCol), _pchevThicc);
                ImGui.GetWindowDrawList().AddLine(new Num.Vector2(chevTip.X, chevTip.Y), new Num.Vector2(chevOffset2.X, chevOffset2.Y), ImGui.GetColorU32(_pchevCol), _pchevThicc);

            }

            if (_partyenabled)
            {
                if (_partylist != null)
                {
                    if (_partylist.Length > 0)
                    {
                        PartyMember pmember0 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(0));
                        if (pmember0.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember0.Position.X, pmember0.Position.Y, pmember0.Position.Z), out Num.Vector2 pm0pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm0pos.X, pm0pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }

                        PartyMember pmember1 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(1));
                        if (pmember1.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember1.Position.X, pmember1.Position.Y, pmember1.Position.Z), out Num.Vector2 pm1pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm1pos.X, pm1pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }
                    }
                    if (_partylist.Length > 1)
                    {
                        PartyMember pmember2 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(2));
                        if (pmember2.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember2.Position.X, pmember2.Position.Y, pmember2.Position.Z), out Num.Vector2 pm2pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm2pos.X, pm2pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }
                    }
                    if (_partylist.Length > 2)
                    {
                        PartyMember pmember3 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(3));
                        if (pmember3.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember3.Position.X, pmember3.Position.Y, pmember3.Position.Z), out Num.Vector2 pm3pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm3pos.X, pm3pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }
                    }
                    if (_partylist.Length > 3)
                    {
                        PartyMember pmember4 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(4));
                        if (pmember4.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember4.Position.X, pmember4.Position.Y, pmember4.Position.Z), out Num.Vector2 pm4pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm4pos.X, pm4pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }
                    }
                    if (_partylist.Length > 4)
                    {
                        PartyMember pmember5 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(5));
                        if (pmember5.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember5.Position.X, pmember5.Position.Y, pmember5.Position.Z), out Num.Vector2 pm5pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm5pos.X, pm5pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }
                    }
                    if (_partylist.Length > 5)
                    {
                        PartyMember pmember6 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(6));
                        if (pmember6.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember6.Position.X, pmember6.Position.Y, pmember6.Position.Z), out Num.Vector2 pm6pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm6pos.X, pm6pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }
                    }
                    if (_partylist.Length > 6)
                    {
                        PartyMember pmember7 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(7));
                        if (pmember7.Name.ToString() != actor.Name.ToString())
                        {
                            _gui.WorldToScreen(new Num.Vector3(pmember7.Position.X, pmember7.Position.Y, pmember7.Position.Z), out Num.Vector2 pm7pos);
                            ImGui.GetWindowDrawList().AddCircleFilled(new Num.Vector2(pm7pos.X, pm7pos.Y), _pdotthickness, ImGui.GetColorU32(_pdotcol), 100);
                        }
                    }

                }
            }

            if (_phbring)
            {
                if (_partylist != null)
                {
                    if (_partylist.Length > 0)
                    {
                        PartyMember pmember0 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(0));
                        if (pmember0.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember0.ClassJob.ToString().Contains("Paladin") ||
                                    pmember0.ClassJob.ToString().Contains("Dark") ||
                                    pmember0.ClassJob.ToString().Contains("Gun") ||
                                    pmember0.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember0, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember0.ClassJob.ToString().Contains("White") ||
                                    pmember0.ClassJob.ToString().Contains("Sch") ||
                                    pmember0.ClassJob.ToString().Contains("Sage") ||
                                    pmember0.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember0, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember0.ClassJob.ToString().Contains("Monk") ||
                                    pmember0.ClassJob.ToString().Contains("Drag") ||
                                    pmember0.ClassJob.ToString().Contains("Nin") ||
                                    pmember0.ClassJob.ToString().Contains("Sam") ||
                                    pmember0.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember0, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember0.ClassJob.ToString().Contains("Bard") ||
                                    pmember0.ClassJob.ToString().Contains("Mach") ||
                                    pmember0.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember0, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember0.ClassJob.ToString().Contains("Black") || pmember0.ClassJob.ToString().Contains("Summ") || pmember0.ClassJob.ToString().Contains("Red") || pmember0.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember0, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember0, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }

                        PartyMember pmember1 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(1));
                        if (pmember1.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember1.ClassJob.ToString().Contains("Paladin") ||
                                    pmember1.ClassJob.ToString().Contains("Dark") ||
                                    pmember1.ClassJob.ToString().Contains("Gun") ||
                                    pmember1.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember1, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember1.ClassJob.ToString().Contains("White") ||
                                    pmember1.ClassJob.ToString().Contains("Sch") ||
                                    pmember1.ClassJob.ToString().Contains("Sage") ||
                                    pmember1.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember1, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember1.ClassJob.ToString().Contains("Monk") ||
                                    pmember1.ClassJob.ToString().Contains("Drag") ||
                                    pmember1.ClassJob.ToString().Contains("Nin") ||
                                    pmember1.ClassJob.ToString().Contains("Sam") ||
                                    pmember1.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember1, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember1.ClassJob.ToString().Contains("Bard") ||
                                    pmember1.ClassJob.ToString().Contains("Mach") ||
                                    pmember1.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember1, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember1.ClassJob.ToString().Contains("Black") ||
                                    pmember1.ClassJob.ToString().Contains("Summ") ||
                                    pmember1.ClassJob.ToString().Contains("Red") ||
                                    pmember1.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember1, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember1, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }
                    }
                    if (_partylist.Length > 1)
                    {

                        PartyMember pmember2 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(2));
                        if (pmember2.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember2.ClassJob.ToString().Contains("Paladin") ||
                                    pmember2.ClassJob.ToString().Contains("Dark") ||
                                    pmember2.ClassJob.ToString().Contains("Gun") ||
                                    pmember2.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember2, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember2.ClassJob.ToString().Contains("White") ||
                                    pmember2.ClassJob.ToString().Contains("Sch") ||
                                    pmember2.ClassJob.ToString().Contains("Sage") ||
                                    pmember2.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember2, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember2.ClassJob.ToString().Contains("Monk") ||
                                    pmember2.ClassJob.ToString().Contains("Drag") ||
                                    pmember2.ClassJob.ToString().Contains("Nin") ||
                                    pmember2.ClassJob.ToString().Contains("Sam") ||
                                    pmember2.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember2, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember2.ClassJob.ToString().Contains("Bard") ||
                                    pmember2.ClassJob.ToString().Contains("Mach") ||
                                    pmember2.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember2, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember2.ClassJob.ToString().Contains("Black") ||
                                    pmember2.ClassJob.ToString().Contains("Summ") ||
                                    pmember2.ClassJob.ToString().Contains("Red") ||
                                    pmember2.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember2, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember2, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }
                    }
                    if (_partylist.Length > 2)
                    {

                        PartyMember pmember3 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(3));
                        if (pmember3.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember3.ClassJob.ToString().Contains("Paladin") ||
                                    pmember3.ClassJob.ToString().Contains("Dark") ||
                                    pmember3.ClassJob.ToString().Contains("Gun") ||
                                    pmember3.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember3, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember3.ClassJob.ToString().Contains("White") ||
                                    pmember3.ClassJob.ToString().Contains("Sch") ||
                                    pmember3.ClassJob.ToString().Contains("Sage") ||
                                    pmember3.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember3, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember3.ClassJob.ToString().Contains("Monk") ||
                                    pmember3.ClassJob.ToString().Contains("Drag") ||
                                    pmember3.ClassJob.ToString().Contains("Nin") ||
                                    pmember3.ClassJob.ToString().Contains("Sam") ||
                                    pmember3.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember3, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember3.ClassJob.ToString().Contains("Bard") ||
                                    pmember3.ClassJob.ToString().Contains("Mach") ||
                                    pmember3.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember3, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember3.ClassJob.ToString().Contains("Black") ||
                                    pmember3.ClassJob.ToString().Contains("Summ") ||
                                    pmember3.ClassJob.ToString().Contains("Red") ||
                                    pmember3.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember3, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember3, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }
                    }
                    if (_partylist.Length > 3)
                    {
                        PartyMember pmember4 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(4));
                        if (pmember4.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember4.ClassJob.ToString().Contains("Paladin") ||
                                    pmember4.ClassJob.ToString().Contains("Dark") ||
                                    pmember4.ClassJob.ToString().Contains("Gun") ||
                                    pmember4.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember4, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember4.ClassJob.ToString().Contains("White") ||
                                    pmember4.ClassJob.ToString().Contains("Sch") ||
                                    pmember4.ClassJob.ToString().Contains("Sage") ||
                                    pmember4.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember4, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember4.ClassJob.ToString().Contains("Monk") ||
                                    pmember4.ClassJob.ToString().Contains("Drag") ||
                                    pmember4.ClassJob.ToString().Contains("Nin") ||
                                    pmember4.ClassJob.ToString().Contains("Sam") ||
                                    pmember4.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember4, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember4.ClassJob.ToString().Contains("Bard") ||
                                    pmember4.ClassJob.ToString().Contains("Mach") ||
                                    pmember4.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember4, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember4.ClassJob.ToString().Contains("Black") ||
                                    pmember4.ClassJob.ToString().Contains("Summ") ||
                                    pmember4.ClassJob.ToString().Contains("Red") ||
                                    pmember4.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember4, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember4, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }
                    }
                    if (_partylist.Length > 4)
                    {

                        PartyMember pmember5 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(5));
                        if (pmember5.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember5.ClassJob.ToString().Contains("Paladin") ||
                                    pmember5.ClassJob.ToString().Contains("Dark") ||
                                    pmember5.ClassJob.ToString().Contains("Gun") ||
                                    pmember5.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember5, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember5.ClassJob.ToString().Contains("White") ||
                                    pmember5.ClassJob.ToString().Contains("Sch") ||
                                    pmember5.ClassJob.ToString().Contains("Sage") ||
                                    pmember5.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember5, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember5.ClassJob.ToString().Contains("Monk") ||
                                    pmember5.ClassJob.ToString().Contains("Drag") ||
                                    pmember5.ClassJob.ToString().Contains("Nin") ||
                                    pmember5.ClassJob.ToString().Contains("Sam") ||
                                    pmember5.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember5, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember5.ClassJob.ToString().Contains("Bard") ||
                                    pmember5.ClassJob.ToString().Contains("Mach") ||
                                    pmember5.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember5, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember5.ClassJob.ToString().Contains("Black") ||
                                    pmember5.ClassJob.ToString().Contains("Summ") ||
                                    pmember5.ClassJob.ToString().Contains("Red") ||
                                    pmember5.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember5, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember5, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }
                    }
                    if (_partylist.Length > 5)
                    {

                        PartyMember pmember6 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(6));
                        if (pmember6.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember6.ClassJob.ToString().Contains("Paladin") ||
                                    pmember6.ClassJob.ToString().Contains("Dark") ||
                                    pmember6.ClassJob.ToString().Contains("Gun") ||
                                    pmember6.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember6, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember6.ClassJob.ToString().Contains("White") ||
                                    pmember6.ClassJob.ToString().Contains("Sch") ||
                                    pmember6.ClassJob.ToString().Contains("Sage") ||
                                    pmember6.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember6, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember6.ClassJob.ToString().Contains("Monk") ||
                                    pmember6.ClassJob.ToString().Contains("Drag") ||
                                    pmember6.ClassJob.ToString().Contains("Nin") ||
                                    pmember6.ClassJob.ToString().Contains("Sam") ||
                                    pmember6.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember6, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember6.ClassJob.ToString().Contains("Bard") ||
                                    pmember6.ClassJob.ToString().Contains("Mach") ||
                                    pmember6.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember6, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember6.ClassJob.ToString().Contains("Black") ||
                                    pmember6.ClassJob.ToString().Contains("Summ") ||
                                    pmember6.ClassJob.ToString().Contains("Red") ||
                                    pmember6.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember6, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember6, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }
                    }
                    if (_partylist.Length > 6)
                    {
                        PartyMember pmember7 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(7));
                        if (pmember7.Name.ToString() != actor.Name.ToString())
                        {
                            if (_rolecolors)
                            {
                                if (pmember7.ClassJob.ToString().Contains("Paladin") ||
                                    pmember7.ClassJob.ToString().Contains("Dark") ||
                                    pmember7.ClassJob.ToString().Contains("Gun") ||
                                    pmember7.ClassJob.ToString().Contains("War"))
                                {
                                    DrawRingPlayerWorld(pmember7, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_tankcolor));
                                }
                                else if (pmember7.ClassJob.ToString().Contains("White") ||
                                    pmember7.ClassJob.ToString().Contains("Sch") ||
                                    pmember7.ClassJob.ToString().Contains("Sage") ||
                                    pmember7.ClassJob.ToString().Contains("Ast"))
                                {
                                    DrawRingPlayerWorld(pmember7, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_healercolor));
                                }
                                else if (pmember7.ClassJob.ToString().Contains("Monk") ||
                                    pmember7.ClassJob.ToString().Contains("Drag") ||
                                    pmember7.ClassJob.ToString().Contains("Nin") ||
                                    pmember7.ClassJob.ToString().Contains("Sam") ||
                                    pmember7.ClassJob.ToString().Contains("Reap"))
                                {
                                    DrawRingPlayerWorld(pmember7, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_melecolor));
                                }
                                else if (pmember7.ClassJob.ToString().Contains("Bard") ||
                                    pmember7.ClassJob.ToString().Contains("Mach") ||
                                    pmember7.ClassJob.ToString().Contains("Dance"))
                                {
                                    DrawRingPlayerWorld(pmember7, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_physrangecolor));
                                }
                                else if (pmember7.ClassJob.ToString().Contains("Black") ||
                                    pmember7.ClassJob.ToString().Contains("Summ") ||
                                    pmember7.ClassJob.ToString().Contains("Red") ||
                                    pmember7.ClassJob.ToString().Contains("Blue"))
                                {
                                    DrawRingPlayerWorld(pmember7, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_magicrangecolor));
                                }
                            }
                            else
                            {
                                DrawRingPlayerWorld(pmember7, _cs.LocalPlayer.HitboxRadius, _phbsegments, _phbthickness, ImGui.GetColorU32(_phbcolRing));
                            }
                        }
                    }
                }
            }

            if (_partyring1)
            {
                if (_partylist != null)
                {
                    if (_partylist.Length > 0)
                    {

                        PartyMember pmember0 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(0));
                        if (pmember0.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember0, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }

                        PartyMember pmember1 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(1));
                        if (pmember1.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember1, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }
                    }
                    if (_partylist.Length > 1)
                    {

                        PartyMember pmember2 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(2));
                        if (pmember2.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember2, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }
                    }
                    if (_partylist.Length > 2)
                    {

                        PartyMember pmember3 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(3));
                        if (pmember3.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember3, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }
                    }
                    if (_partylist.Length > 3)
                    {

                        PartyMember pmember4 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(4));
                        if (pmember4.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember4, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }
                    }
                    if (_partylist.Length > 4)
                    {

                        PartyMember pmember5 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(5));
                        if (pmember5.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember5, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }
                    }
                    if (_partylist.Length > 5)
                    {

                        PartyMember pmember6 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(6));
                        if (pmember6.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember6, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }
                    }
                    if (_partylist.Length > 6)
                    {

                        PartyMember pmember7 = _partylist.CreatePartyMemberReference(_partylist.GetPartyMemberAddress(7));
                        if (pmember7.Name.ToString() != actor.Name.ToString())
                        {
                            DrawRingPlayerWorld(pmember7, _partyradius1, _partysegments1, _partythickness1, ImGui.GetColorU32(_partycolRing1));
                        }
                    }

                }
            }

            ImGui.End();
            ImGui.PopStyleVar();
        }


        private void Command(string command, string arguments)
        {
            if (arguments == "ring")
            {
                _ring = !_ring;
            }
            else if (arguments == "ring2")
            {
                _ring2 = !_ring2;
            }
            else if (arguments == "ring3")
            {
                _ring3 = !_ring3;
            }
            else if (arguments == "ring4")
            {
                _ring4 = !_ring4;
            }
            else if (arguments == "ring5")
            {
                _ring5 = !_ring5;
            }

            else if (arguments == "north")
            {
                _north = !_north;
            }
            else
            {
                _config = !_config;
            }
            SaveConfig();
        }

        /// <Config>///
        private void SaveConfig()
        {
            _configuration.Enabled = _enabled;
            _configuration.Col = _col;
            _configuration.HBdotradius = _hbdotradius;

            _configuration.Combat = _combat;
            _configuration.Instance = _instance;

            _configuration.ColRing = _colRing;
            _configuration.HBColRing = _hbcolRing;

            _configuration.Ring = _ring;
            _configuration.Thickness = _thickness;
            _configuration.Segments = _segments;
            _configuration.Radius = _radius;

            _configuration.Ring2 = _ring2;
            _configuration.Thickness2 = _thickness2;
            _configuration.Segments2 = _segments2;
            _configuration.Radius2 = _radius2;

            _configuration.Ring3 = _ring3;
            _configuration.Thickness3 = _thickness3;
            _configuration.Segments3 = _segments3;
            _configuration.Radius3 = _radius3;

            _configuration.Ring4 = _ring4;
            _configuration.Thickness4 = _thickness4;
            _configuration.Segments4 = _segments4;
            _configuration.Radius4 = _radius4;

            _configuration.Ring5 = _ring5;
            _configuration.Thickness5 = _thickness5;
            _configuration.Segments5 = _segments5;
            _configuration.Radius5 = _radius5;


            _configuration.HBRing = _hbring;
            _configuration.HBThickness = _hbthickness;
            _configuration.HBSegments = _hbsegments;
            _configuration.HBRadius = _hbradius;

            _configuration.North = _north;
            _configuration.East = _east;
            _configuration.South = _south;
            _configuration.West = _west;
            _configuration.Player = _player;

            _configuration.NLine = _nline;
            _configuration.ELine = _eline;
            _configuration.SLine = _sline;
            _configuration.WLine = _wline;
            _configuration.PLine = _pline;

            _configuration.NChev = _nchev;
            _configuration.EChev = _echev;
            _configuration.SChev = _schev;
            _configuration.WChev = _wchev;
            _configuration.PChev = _pchev;

            _configuration.NChevLength = _nchevLength;
            _configuration.NChevOffset = _nchevOffset;
            _configuration.NChevRad = _nchevRad;
            _configuration.NChevThicc = _nchevThicc;
            _configuration.NChevCol = _nchevCol;

            _configuration.EChevLength = _echevLength;
            _configuration.EChevOffset = _echevOffset;
            _configuration.EChevRad = _echevRad;
            _configuration.EChevThicc = _echevThicc;
            _configuration.EChevCol = _echevCol;

            _configuration.SChevLength = _schevLength;
            _configuration.SChevOffset = _schevOffset;
            _configuration.SChevRad = _schevRad;
            _configuration.SChevThicc = _schevThicc;
            _configuration.SChevCol = _schevCol;

            _configuration.WChevLength = _wchevLength;
            _configuration.WChevOffset = _wchevOffset;
            _configuration.WChevRad = _wchevRad;
            _configuration.WChevThicc = _wchevThicc;
            _configuration.WChevCol = _wchevCol;

            _configuration.PChevLength = _pchevLength;
            _configuration.PChevOffset = _pchevOffset;
            _configuration.PChevRad = _pchevRad;
            _configuration.PChevThicc = _pchevThicc;
            _configuration.PChevCol = _pchevCol;

            _configuration.NLineCol = _nlineCol;
            _configuration.NLineThicc = _nlineThicc;
            _configuration.NLineOffset = _nlineOffset;
            _configuration.NLineLength = _nlineLength;

            _configuration.ELineCol = _elineCol;
            _configuration.ELineThicc = _elineThicc;
            _configuration.ELineOffset = _elineOffset;
            _configuration.ELineLength = _elineLength;

            _configuration.SLineCol = _slineCol;
            _configuration.SLineThicc = _slineThicc;
            _configuration.SLineOffset = _slineOffset;
            _configuration.SLineLength = _slineLength;

            _configuration.WLineCol = _wlineCol;
            _configuration.WLineThicc = _wlineThicc;
            _configuration.WLineOffset = _wlineOffset;
            _configuration.WLineLength = _wlineLength;

            _configuration.PLineCol = _plineCol;
            _configuration.PLineThicc = _plineThicc;
            _configuration.PLineOffset = _plineOffset;
            _configuration.PLineLength = _plineLength;

            _configuration.PhbRing = _phbring;
            _configuration.PhbColRing = _phbcolRing;
            _configuration.PhbRadius = _phbradius;
            _configuration.PhbSegments = _phbsegments;
            _configuration.PhbThickness = _phbthickness;

            _configuration.Penabled = _partyenabled;
            _configuration.PdotCol = _pdotcol;
            _configuration.PdotThickness = _pdotthickness;

            _configuration.PRing1 = _partyring1;
            _configuration.PColRing1 = _partycolRing1;
            _configuration.PRadius1 = _partyradius1;
            _configuration.PSegments1 = _partysegments1;
            _configuration.PThickness1 = _partythickness1;

            _configuration.RoleColors = _rolecolors;
            _configuration.MeleColor = _melecolor;
            _configuration.PhysRangeColor = _physrangecolor;
            _configuration.MagicRangeColor = _magicrangecolor;
            _configuration.TankColor = _tankcolor;
            _configuration.HealerColor = _healercolor;

            _pi.SavePluginConfig(_configuration);
        }

        private void DrawRingWorld(Dalamud.Game.ClientState.Objects.Types.Character actor, float radius, int numSegments, float thicc, uint colour)
        {
            if (numSegments > 100)
            {
                numSegments = 100;
            }
            var seg = numSegments / 2;
            for (var i = 0; i <= numSegments; i++)
            {

                _gui.WorldToScreen(new Num.Vector3(
                    actor.Position.X + (radius * (float)Math.Sin((Math.PI / seg) * i)),
                    actor.Position.Y,
                    actor.Position.Z + (radius * (float)Math.Cos((Math.PI / seg) * i))
                    ),
                    out Num.Vector2 pos);

                ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(pos.X, pos.Y));
            }
            ImGui.GetWindowDrawList().PathStroke(colour, ImDrawFlags.None, thicc);
        }

        private void DrawRingPlayerWorld(PartyMember pmem, float radius, int numSegments, float thicc, uint colour)
        {
            bool b;
            if (numSegments > 100)
            {
                numSegments = 100;
            }
            var seg = numSegments / 2;
            for (var i = 0; i <= numSegments; i++)
            {
                b = _gui.WorldToScreen(new Num.Vector3(
                    pmem.Position.X + (radius * (float)Math.Sin((Math.PI / seg) * i)),
                    pmem.Position.Y,
                    pmem.Position.Z + (radius * (float)Math.Cos((Math.PI / seg) * i))
                    ),
                    out Num.Vector2 pos);
                ImGui.GetWindowDrawList().PathLineTo(new Num.Vector2(pos.X, pos.Y));
                if (b is false)//try to not draw offscreen character rings, odd rings
                {
                    break;
                }
            }
            ImGui.GetWindowDrawList().PathStroke(colour, ImDrawFlags.None, thicc);
        }
    }


    public class Config : IPluginConfiguration
    {
        public int Version { get; set; } = 0;
        public bool Combat { get; set; } = true;
        //public bool Circle { get; set; }
        public bool Instance { get; set; }

        public bool Enabled { get; set; } = true;
        public Num.Vector4 Col { get; set; } = new Num.Vector4(1f, 1f, 1f, 1f);
        public float HBdotradius { get; set; } = 2f;

        public bool Ring { get; set; }
        public Num.Vector4 ColRing { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public int Segments { get; set; } = 100;
        public float Thickness { get; set; } = 10f;
        public float Radius { get; set; } = 2f;

        public bool Ring2 { get; set; }
        public Num.Vector4 ColRing2 { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public int Segments2 { get; set; } = 100;
        public float Thickness2 { get; set; } = 10f;
        public float Radius2 { get; set; } = 2f;

        public bool Ring3 { get; set; }
        public Num.Vector4 ColRing3 { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public int Segments3 { get; set; } = 100;
        public float Thickness3 { get; set; } = 10f;
        public float Radius3 { get; set; } = 2f;

        public bool Ring4 { get; set; }
        public Num.Vector4 ColRing4 { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public int Segments4 { get; set; } = 100;
        public float Thickness4 { get; set; } = 10f;
        public float Radius4 { get; set; } = 2f;

        public bool Ring5 { get; set; }
        public Num.Vector4 ColRing5 { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public int Segments5 { get; set; } = 100;
        public float Thickness5 { get; set; } = 10f;
        public float Radius5 { get; set; } = 2f;

        public bool HBRing { get; set; }
        public Num.Vector4 HBColRing { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public int HBSegments { get; set; } = 100;
        public float HBThickness { get; set; } = 10f;
        public float HBRadius { get; set; } = 2f;

        public bool North { get; set; } = false;
        public bool NLine { get; set; } = false;
        public bool NChev { get; set; } = false;
        public float NChevLength { get; set; } = 1f;
        public float NChevOffset { get; set; } = 1f;
        public float NChevRad { get; set; } = 0.5f;
        public float NChevThicc { get; set; } = 5f;
        public Num.Vector4 NChevCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public float NLineLength { get; set; } = 1f;
        public float NLineThicc { get; set; } = 5f;
        public float NLineOffset { get; set; } = 0.5f;
        public Num.Vector4 NLineCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);


        public bool East { get; set; } = false;
        public bool ELine { get; set; } = false;
        public bool EChev { get; set; } = false;
        public float EChevLength { get; set; } = 1f;
        public float EChevOffset { get; set; } = 1f;
        public float EChevRad { get; set; } = 0.5f;
        public float EChevThicc { get; set; } = 5f;
        public Num.Vector4 EChevCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public float ELineLength { get; set; } = 1f;
        public float ELineThicc { get; set; } = 5f;
        public float ELineOffset { get; set; } = 0.5f;
        public Num.Vector4 ELineCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);

        public bool South { get; set; } = false;
        public bool SLine { get; set; } = false;
        public bool SChev { get; set; } = false;
        public float SChevLength { get; set; } = 1f;
        public float SChevOffset { get; set; } = 1f;
        public float SChevRad { get; set; } = 0.5f;
        public float SChevThicc { get; set; } = 5f;
        public Num.Vector4 SChevCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public float SLineLength { get; set; } = 1f;
        public float SLineThicc { get; set; } = 5f;
        public float SLineOffset { get; set; } = 0.5f;
        public Num.Vector4 SLineCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);

        public bool West { get; set; } = false;
        public bool WLine { get; set; } = false;
        public bool WChev { get; set; } = false;
        public float WChevLength { get; set; } = 1f;
        public float WChevOffset { get; set; } = 1f;
        public float WChevRad { get; set; } = 0.5f;
        public float WChevThicc { get; set; } = 5f;
        public Num.Vector4 WChevCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public float WLineLength { get; set; } = 1f;
        public float WLineThicc { get; set; } = 5f;
        public float WLineOffset { get; set; } = 0.5f;
        public Num.Vector4 WLineCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);


        public bool Player { get; set; } = false;
        public bool PLine { get; set; } = false;
        public bool PChev { get; set; } = false;
        public float PChevLength { get; set; } = 1f;
        public float PChevOffset { get; set; } = 1f;
        public float PChevRad { get; set; } = 0.5f;
        public float PChevThicc { get; set; } = 5f;
        public Num.Vector4 PChevCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public float PLineLength { get; set; } = 1f;
        public float PLineThicc { get; set; } = 5f;
        public float PLineOffset { get; set; } = 0.5f;
        public Num.Vector4 PLineCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);


        public bool PhbRing { get; set; } = false;
        public Num.Vector4 PhbColRing { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public float PhbRadius { get; set; } = 0.5f;
        public int PhbSegments { get; set; } = 3;
        public float PhbThickness { get; set; } = 5f;

        public bool Penabled { get; set; } = false;
        public Num.Vector4 PdotCol { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);

        public float PdotThickness { get; set; } = 2f;

        public bool PRing1 { get; set; } = false;
        public Num.Vector4 PColRing1 { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public float PRadius1 { get; set; } = 0.5f;
        public int PSegments1 { get; set; } = 3;
        public float PThickness1 { get; set; } = 5f;


        public bool RoleColors { get; set; } = false;
        public Num.Vector4 MeleColor { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public Num.Vector4 PhysRangeColor { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public Num.Vector4 MagicRangeColor { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public Num.Vector4 TankColor { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);
        public Num.Vector4 HealerColor { get; set; } = new Num.Vector4(0.4f, 0.4f, 0.4f, 0.5f);


    }
#pragma warning restore CA1416 // Validate platform compatibility
}