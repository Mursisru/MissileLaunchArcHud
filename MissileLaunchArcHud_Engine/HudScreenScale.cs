using UnityEngine;

namespace MissileLaunchArcHud_Engine
{
    /// <summary>
    /// Scales HUD pixel sizes to the current game resolution (same convention as vanilla
    /// <c>HUDMissileState</c> / <c>HUDBombingState</c>: reference height 1080 px).
    /// Config ring sizes are defined at that reference and multiplied by <see cref="HeightScale"/>.
    /// </summary>
    internal static class HudScreenScale
    {
        /// <summary>Vanilla NO HUD reference height (see <c>PlayerSettings.hmdHeight</c>, bombing CCIP line).</summary>
        internal const float ReferenceHeight = 1080f;

        private static int _cachedWidth = -1;
        private static int _cachedHeight = -1;
        private static float _heightScale = 1f;

        internal static float HeightScale
        {
            get
            {
                int w = Screen.width;
                int h = Screen.height;
                if (w != _cachedWidth || h != _cachedHeight)
                {
                    _cachedWidth = w;
                    _cachedHeight = h;
                    _heightScale = h > 0 ? Mathf.Max(0.25f, h / ReferenceHeight) : 1f;
                }

                return _heightScale;
            }
        }

        /// <summary>Convert a reference-pixel value (at 1080p height) to current screen pixels.</summary>
        internal static float Px(float referencePixels) => referencePixels * HeightScale;
    }
}
