using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace MissileLaunchArcHud_Engine
{
    [BepInPlugin(PluginGuid, PluginName, AppVersion.BepInSemVer)]
    public sealed class MissileLaunchArcHudPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.at747.missilelauncharchud";
        public const string PluginName = "Missile Launch Arc HUD";

        internal static ConfigEntry<bool> Enabled { get; private set; }
        internal static ConfigEntry<bool> RequireTargetLock { get; private set; }
        internal static ConfigEntry<bool> HideForLaserGuided { get; private set; }
        internal static ConfigEntry<float> ArcRadiusScale { get; private set; }
        internal static ConfigEntry<bool> MatchFlightHudStyle { get; private set; }
        internal static ConfigEntry<float> FlightHudAlphaInArcMul { get; private set; }
        internal static ConfigEntry<float> FlightHudAlphaOutOfArcMul { get; private set; }
        internal static ConfigEntry<float> CalmArcAlphaInZone { get; private set; }
        internal static ConfigEntry<float> CalmArcAlphaOutOfArc { get; private set; }
        internal static ConfigEntry<bool> UseHudColorForCalmArc { get; private set; }
        internal static ConfigEntry<bool> DrawBehindHud { get; private set; }
        internal static ConfigEntry<bool> PartialRing { get; private set; }
        internal static ConfigEntry<float> FullRingBelowScale { get; private set; }
        internal static ConfigEntry<float> RingInnerRadius01 { get; private set; }
        internal static ConfigEntry<float> RingOuterRadius01 { get; private set; }
        internal static ConfigEntry<float> PartialSideArcDegrees { get; private set; }
        internal static ConfigEntry<float> UpdateHz { get; private set; }

        internal static ConfigEntry<bool> NezMarkersEnabled { get; private set; }
        internal static ConfigEntry<float> NezApproachBandMeters { get; private set; }
        internal static ConfigEntry<float> NezApproachRingSizePx { get; private set; }
        internal static ConfigEntry<float> NezApproachRingAlpha { get; private set; }
        internal static ConfigEntry<float> NezInnerRingSizePx { get; private set; }
        internal static ConfigEntry<float> NezInnerOuterGapPx { get; private set; }
        internal static ConfigEntry<string> NezRedColorHtml { get; private set; }
        internal static ConfigEntry<float> NezBlinkHz { get; private set; }
        internal static ConfigEntry<float> NezBlinkAlphaMax { get; private set; }
        internal static ConfigEntry<float> TargetRingInsideArcFraction { get; private set; }
        internal static ConfigEntry<float> TargetRingInsideArcHoldSeconds { get; private set; }

        private Harmony _harmony;

        private void Awake()
        {
            Enabled = Config.Bind("LaunchArc", "Enabled", true,
                "Draw launch-arc circle on Flight HUD for non-laser missiles.");
            RequireTargetLock = Config.Bind("LaunchArc", "RequireTargetLock", false,
                "Only show when at least one target has a known position.");
            HideForLaserGuided = Config.Bind("LaunchArc", "HideForLaserGuided", true,
                "Do not draw for laser-guided weapons (vanilla already shows a circle).");
            ArcRadiusScale = Config.Bind("LaunchArc", "ArcRadiusScale", 1.16f,
                "HUD ring size multiplier (vanilla laser formula is ×1; tune to match OUT OF ARC).");
            UseHudColorForCalmArc = Config.Bind("LaunchArc", "UseHudColorForCalmArc", true,
                "Tint calm rings with PlayerSettings HUD color.");
            MatchFlightHudStyle = Config.Bind("LaunchArc", "MatchFlightHudStyle", true,
                "Match ring color/material/alpha to live missile HUD lines (ladder, dist span, max/min ticks). Falls back to laser arc 0.5 alpha. PlayerSettings HUD color is NOT used for vanilla lines.");
            FlightHudAlphaInArcMul = Config.Bind("LaunchArc", "FlightHudAlphaInArcMul", 1f,
                "Multiplier on sampled Flight HUD alpha while inside launch arc.");
            FlightHudAlphaOutOfArcMul = Config.Bind("LaunchArc", "FlightHudAlphaOutOfArcMul", 1f,
                "Multiplier on sampled Flight HUD alpha when OUT OF ARC.");
            CalmArcAlphaInZone = Config.Bind("LaunchArc", "CalmArcAlphaInZone", 0.42f,
                "Legacy fixed alpha in-zone when MatchFlightHudStyle is false.");
            CalmArcAlphaOutOfArc = Config.Bind("LaunchArc", "CalmArcAlphaOutOfArc", 0.34f,
                "Legacy fixed alpha out-of-arc when MatchFlightHudStyle is false.");
            DrawBehindHud = Config.Bind("LaunchArc", "DrawBehindHud", true,
                "Draw ring behind other Flight HUD elements (missile ladder, hints).");
            UpdateHz = Config.Bind("LaunchArc", "UpdateHz", 0f,
                "Max update rate (0 = every frame).");

            PartialRing = Config.Bind("LaunchArc.RingShape", "PartialRing", true,
                "Side arcs only (parentheses); gaps at top and bottom.");
            FullRingBelowScale = Config.Bind("LaunchArc.RingShape", "FullRingBelowScale", 0.42f,
                "If HUD scale is below this, draw a full circle instead of side arcs.");
            RingInnerRadius01 = Config.Bind("LaunchArc.RingShape", "RingInnerRadius01", 0.924f,
                "Ring inner edge (0–1). Classic OK: 0.925 / 0.928; default + slight: 0.924 / 0.929.");
            RingOuterRadius01 = Config.Bind("LaunchArc.RingShape", "RingOuterRadius01", 0.929f,
                "Ring outer edge (0–1). Must be greater than RingInnerRadius01.");
            PartialSideArcDegrees = Config.Bind("LaunchArc.RingShape", "PartialSideArcDegrees", 20f,
                "Half-width of each side arc in degrees (10–89). Lower = wider gaps top/bottom.");

            NezMarkersEnabled = Config.Bind("LaunchArc.NEZ", "NezMarkersEnabled", true,
                "War Thunder–style NEZ rings on the target + red blinking arc inside NEZ.");
            NezApproachBandMeters = Config.Bind("LaunchArc.NEZ", "NezApproachBandMeters", 1000f,
                "Show red target ring from NEZ distance up to NEZ + this band (meters).");
            NezApproachRingSizePx = Config.Bind("LaunchArc.NEZ", "NezApproachRingSizePx", 8f,
                "Unused — approach ring uses NezInnerRingSizePx (kept for old cfg).");
            NezApproachRingAlpha = Config.Bind("LaunchArc.NEZ", "NezApproachRingAlpha", 0.95f,
                "Legacy approach-ring alpha when MatchFlightHudStyle is false.");
            NezInnerRingSizePx = Config.Bind("LaunchArc.NEZ", "NezInnerRingSizePx", 30f,
                "NEZ target ring diameter @1080 (approach + inner; outer = this + gap).");
            NezInnerOuterGapPx = Config.Bind("LaunchArc.NEZ", "NezInnerOuterGapPx", 6f,
                "Gap between inner/outer NEZ rings at reference height (1080 px); auto-scaled.");
            NezRedColorHtml = Config.Bind("LaunchArc.NEZ", "NezRedColorHtml", "#FF2020",
                "HTML color for NEZ warning (approach ring, blink rings, arc inside NEZ).");
            NezBlinkHz = Config.Bind("LaunchArc.NEZ", "NezBlinkHz", 2.5f,
                "Blink frequency when inside NEZ (Hz).");
            NezBlinkAlphaMax = Config.Bind("LaunchArc.NEZ", "NezBlinkAlphaMax", 1f,
                "Legacy NEZ blink ON alpha when MatchFlightHudStyle is false.");
            TargetRingInsideArcFraction = Config.Bind("LaunchArc.NEZ", "TargetRingInsideArcFraction", 1.02f,
                "Target ring gate: radius as fraction of main arc (full circle, not side-arc gaps).");
            TargetRingInsideArcHoldSeconds = Config.Bind("LaunchArc.NEZ", "TargetRingInsideArcHoldSeconds", 0.28f,
                "Keep target rings visible briefly after leaving arc bounds (maneuvers).");

            RingInnerRadius01.SettingChanged += OnRingShapeConfigChanged;
            RingOuterRadius01.SettingChanged += OnRingShapeConfigChanged;
            PartialSideArcDegrees.SettingChanged += OnRingShapeConfigChanged;
            PartialRing.SettingChanged += OnRingShapeConfigChanged;

            LaunchArcRingSprite.InvalidateCache();
            MigrateLegacyConfigValues();

            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll(typeof(MissileLaunchArcHudPlugin).Assembly);

            Logger.LogInfo($"{PluginName} {AppVersion.DisplayVersion} loaded.");
        }

        private static void MigrateLegacyConfigValues()
        {
            if (NezInnerRingSizePx.Value < 29f)
                NezInnerRingSizePx.Value = 30f;

            if (NezInnerOuterGapPx.Value < 5.5f)
                NezInnerOuterGapPx.Value = 6f;

            float band = RingOuterRadius01.Value - RingInnerRadius01.Value;
            if (band >= 0.012f)
            {
                RingInnerRadius01.Value = 0.924f;
                RingOuterRadius01.Value = 0.929f;
            }
            else if (Mathf.Abs(RingInnerRadius01.Value - 0.925f) < 0.001f
                     && Mathf.Abs(RingOuterRadius01.Value - 0.928f) < 0.001f)
            {
                RingInnerRadius01.Value = 0.924f;
                RingOuterRadius01.Value = 0.929f;
            }
        }

        private static void OnRingShapeConfigChanged(object sender, System.EventArgs e)
        {
            LaunchArcRingSprite.InvalidateCache();
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
            LaunchArcRingSprite.InvalidateCache();
            LaunchArcGameApi.ResetRangeCache();
        }

        [HarmonyPatch(typeof(FlightHud), "Awake")]
        private static class FlightHudAwakePatch
        {
            private static void Postfix(FlightHud __instance)
            {
                if (__instance == null)
                    return;
                if (__instance.GetComponent<LaunchArcHudController>() == null)
                    __instance.gameObject.AddComponent<LaunchArcHudController>();
            }
        }
    }
}
