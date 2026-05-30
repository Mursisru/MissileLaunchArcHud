using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace MissileLaunchArcHud_Engine
{
    internal static class HudScreenPlacement
    {
        internal static Transform ResolveMarkersParent(Transform flightHudCanvasFallback)
        {
            CombatHUD combatHud = SceneSingleton<CombatHUD>.i;
            if (combatHud != null)
            {
                Image marker = FindPrimaryTargetMarkerImage(combatHud, null);
                if (marker != null)
                {
                    Canvas canvas = marker.GetComponentInParent<Canvas>();
                    if (canvas != null)
                        return canvas.transform;
                    if (marker.rectTransform.parent != null)
                        return marker.rectTransform.parent;
                }
            }

            if (combatHud != null && combatHud.targetDesignator != null)
            {
                Canvas canvas = combatHud.targetDesignator.GetComponentInParent<Canvas>();
                if (canvas != null)
                    return canvas.transform;
            }

            return flightHudCanvasFallback;
        }

        internal static bool TryGetTargetHudPosition(CombatHUD combatHud, Unit target, Camera worldCamera, out Vector3 screenPosition)
        {
            return TryGetPrimaryTargetHudPosition(combatHud, target, worldCamera, out screenPosition);
        }

        internal static bool TryGetPrimaryTargetHudPosition(CombatHUD combatHud, Unit primaryTarget, Camera worldCamera, out Vector3 screenPosition)
        {
            screenPosition = default;
            if (combatHud == null || primaryTarget == null)
                return false;

            Image marker = FindPrimaryTargetMarkerImage(combatHud, primaryTarget);
            if (marker != null && marker.enabled && marker.gameObject.activeInHierarchy)
            {
                screenPosition = marker.rectTransform.position;
                return true;
            }

            if (worldCamera == null)
                return false;

            GlobalPosition gp = primaryTarget.GlobalPosition();
            Vector3 sp = worldCamera.WorldToScreenPoint(gp.ToLocalPosition());
            if (sp.z <= 0f)
                return false;

            screenPosition = new Vector3(sp.x, sp.y, 0f);
            return true;
        }

        private static Image FindPrimaryTargetMarkerImage(CombatHUD combatHud, Unit primaryTarget)
        {
            var markersObj = AccessTools.Field(typeof(CombatHUD), "markers").GetValue(combatHud);
            var markers = markersObj as IList;
            if (markers == null || markers.Count == 0)
                return null;

            if (primaryTarget != null)
            {
                for (int i = 0; i < markers.Count; i++)
                {
                    var hudMarker = markers[i] as HUDUnitMarker;
                    if (hudMarker == null || hudMarker.unit != primaryTarget)
                        continue;
                    if (hudMarker.image != null)
                        return hudMarker.image;
                }
            }

            for (int i = 0; i < markers.Count; i++)
            {
                var hudMarker = markers[i] as HUDUnitMarker;
                if (hudMarker?.image == null || !hudMarker.image.enabled)
                    continue;
                return hudMarker.image;
            }

            return null;
        }
    }
}
