namespace MissileLaunchArcHud_Engine
{
    /// <summary>
    /// at747 versioning — keep in sync with BepInSemVer, DisplayVersion, AssemblyInfo, CHANGELOG.
    /// [BepInPlugin] attribute: <see cref="BepInSemVer"/> only (semver). Log/CHANGELOG: <see cref="DisplayVersion"/>.
    /// </summary>
    internal static class AppVersion
    {
        /// <summary>Target semver for the next GitHub release (fixed during DEV / PR-R).</summary>
        public const string ReleaseBase = "1.3.0";

        /// <summary>Semver only — required by BepInEx [BepInPlugin]; dev string breaks plugin load.</summary>
        public const string BepInSemVer = ReleaseBase;

        /// <summary>DEV in Engine; PR-R in Desktop\GITHUB local mirror after robocopy.</summary>
        public const string VersionChannel = "PR-R";

        public const int CycleBuildNumber = 2;

        /// <summary>Program / pre-release iteration.</summary>
        public const string ChangeLetters = "P";

        public const int SubNumber = 1;

        public const string DisplayVersion = "1.3.0 Build PR-R2P1";
    }
}
