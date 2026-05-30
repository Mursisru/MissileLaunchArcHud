# Missile Launch Arc HUD

BepInEx 5 plugin for **Nuclear Option**: launch-arc circle on the **Flight HUD** for **non-laser** missiles with **target lock**, matching vanilla **OUT OF ARC** (`TargetRequirements.minAlignment`).

**Pre-release:** `1.3.0 Build PR-R2P1` · BepInEx semver **1.3.0** · last GitHub release **1.2.0**

Laser-guided weapons (**AGR-18 / AGR-24**) are skipped — vanilla already draws the dynamic arc circle.

## Features (1.3.0)

- **Main arc** — OUT OF ARC limit ring, styled like Flight HUD missile lines (color / alpha / resolution scaling).
- **NEZ (single target)** — approaching ring (calm HUD green) and inner/outer pair inside no-escape zone (red blink when in arc).
- **Multi-target** — with **2+** locked targets, one calm ring on **each** target that is within launch arc and inside the main arc on screen.

## Install

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
| `NezMarkersEnabled` | true | NEZ rings on target (single-target mode) |
| `NezApproachBandMeters` | 2000 | Distance band before NEZ for “approaching” |
| `NezInnerRingSizePx` / `NezApproachRingSizePx` | Reference @ **1080p**; auto-scaled |
| `TargetRingInsideArcHoldSeconds` | 0.35 | Screen gate hold when target crosses main arc edge |
| `MatchFlightHudStyle` | true | Sample live HUD missile line color/alpha |
| `ArcRadiusScale` | 1 | Main arc radius multiplier |

See `BepInEx\config\com.at747.missilelauncharchud.cfg` after first run for the full list.

## Game logic reference

- Arc check: `HUDMissileState` — `maxTargetAngle > minAlignment` → **OUT OF ARC**
- NEZ ranges: `Missile.CalcRange` / `HUDMissileState.CalcWeaponRange`
- Circle scale (laser template): `50f / FOV * (arcDeg / 8f)`

MIT License — see [LICENSE](LICENSE).
