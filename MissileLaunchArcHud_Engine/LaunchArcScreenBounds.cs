using UnityEngine;

namespace MissileLaunchArcHud_Engine
{
    /// <summary>Screen-space gate: target must lie inside the main launch-arc ring.</summary>
    internal static class LaunchArcScreenBounds
    {
        /// <param name="forTargetRings">Target rings use full circle (no side-arc wedge gaps).</param>
        internal static bool IsTargetInsideMainArc(
            LaunchArcGameApi.LaunchArcSnapshot snap,
            Vector3 hudCenterScreen,
            Vector3 targetScreen,
            bool forTargetRings)
        {
            float radiusPx = GetOuterRadiusPx(snap, forTargetRings);
            if (radiusPx <= 1f)
                return false;

            float dx = targetScreen.x - hudCenterScreen.x;
            float dy = targetScreen.y - hudCenterScreen.y;
            if (dx * dx + dy * dy > radiusPx * radiusPx)
                return false;

            if (forTargetRings || !UsePartialArcGate(snap))
                return true;

            float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
            if (angle < 0f)
                angle += 360f;
            return IsInsidePartialArcWedge(angle);
        }

        internal static float GetOuterRadiusPx(LaunchArcGameApi.LaunchArcSnapshot snap, bool forTargetRings)
        {
            float diameter = HudScreenScale.Px(LaserOuterCircleTemplate.GetReferenceDiameterPx())
                * Mathf.Max(0.05f, snap.CircleScale);
            float fraction = Mathf.Clamp(
                MissileLaunchArcHudPlugin.TargetRingInsideArcFraction.Value,
                0.5f,
                1.08f);
            return diameter * 0.5f * fraction;
        }

        private static bool UsePartialArcGate(LaunchArcGameApi.LaunchArcSnapshot snap)
        {
            if (!MissileLaunchArcHudPlugin.PartialRing.Value)
                return false;
            return snap.CircleScale >= MissileLaunchArcHudPlugin.FullRingBelowScale.Value;
        }

        private static bool IsInsidePartialArcWedge(float angleDeg)
        {
            float span = Mathf.Clamp(MissileLaunchArcHudPlugin.PartialSideArcDegrees.Value, 10f, 89f);
            bool rightSide = angleDeg <= span || angleDeg >= 360f - span;
            bool leftSide = angleDeg >= 180f - span && angleDeg <= 180f + span;
            return rightSide || leftSide;
        }
    }
}
