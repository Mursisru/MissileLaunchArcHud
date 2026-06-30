**Developer:** Mursisru

# Missile Launch Arc HUD

[![Nuclear Option](https://img.shields.io/badge/Game-Nuclear%20Option-blue)](https://store.steampowered.com/app/2168680/Nuclear_Option/) [![BepInEx 5](https://img.shields.io/badge/Loader-BepInEx%205-orange)](https://docs.bepinex.dev/) [![Version](https://img.shields.io/badge/Version-1.3.0-green)]() [![License](https://img.shields.io/badge/License-MIT-lightgrey)](LICENSE)


BepInEx 5 plugin for **Nuclear Option**: draws a **launch arc** circle on the **Flight HUD** for **non-laser** missiles when you have a **target lock**, matching the game's **OUT OF ARC** limit (`TargetRequirements.minAlignment`).

Laser-guided weapons (**AGR-18 / AGR-24**) are skipped — vanilla already draws the dynamic arc circle.

## Install

> [!IMPORTANT]
> **BepInEx 5 (x64) required** - install [BepInEx](https://docs.bepinex.dev/) before this mod.

1. BepInEx 5 x64 for Nuclear Option.
2. Copy `MissileLaunchArcHud_Engine.dll` to `Nuclear Option\BepInEx\plugins\`.
3. Launch once; edit `BepInEx\config\com.at747.missilelauncharchud.cfg` if needed.

## Build

1. Set `<NuclearOptionRoot>` in `MissileLaunchArcHud_Engine\MissileLaunchArcHud_Engine.csproj` if the game is not in the default Steam folder.
2. Open `MissileLaunchArcHud_Engine.slnx`, build **Release**.
3. Copy `MissileLaunchArcHud_Engine\bin\Release\MissileLaunchArcHud_Engine.dll` to `Nuclear Option\BepInEx\plugins\`.

## Config `[LaunchArc]`

| Key | Default | Notes |
|-----|---------|--------|
| `Enabled` | true | Master toggle |
| `RequireTargetLock` | true | Needs known target position |
| `HideForLaserGuided` | true | No duplicate circle on laser rockets |
| `UseDynamicArcFormula` | false | false = `minAlignment` (HUDMissileState); true = laser-style `Min(minAlignment, dist*0.002)` |
| `DimWhenOutOfArc` | true | Fade ring when aim exceeds arc |
| `ColorHtml` | `#59D9FF8C` | Ring color |
| `UpdateHz` | 0 | 0 = every frame |

| `NezApproachRingSizePx` / `NezInnerRingSizePx` | Reference size at **1080p height**; auto-scaled to current resolution |

## Game logic reference

- Arc check: `HUDMissileState` — `maxTargetAngle > minAlignment` → **OUT OF ARC**
- Circle scale (laser): `HUDLaserGuidedState` — `50f / FOV * (arcDeg / 8f)` on `outerCircle`

MIT License — see [LICENSE](LICENSE).

---

## Keywords

nuclear-option, bepinex, harmony, mod, missilelauncharchud, csharp, unity
