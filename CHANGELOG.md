# Changelog

## 1.3.1 — 2026-05-30

### Performance
- Throttled HUD refresh; cached vanilla range sync (0.5s); cached FlightHud canvas lookup.

## 1.3.0 Build DEV2P1 вЂ” 2026-05-30

### Process
- Synced to **GITHUB local** as **PR-R2P1** (new dev cycle after robocopy).

## 1.3.0 Build DEV1VPM15 вЂ” 2026-05-30

### Added
- **Multi-target rings:** if **2+** targets locked, one calm HUD ring on **each** target that is within launch arc (angle в‰¤ `minAlignment` + inside main arc on screen). Single-target NEZ behavior unchanged.

## 1.3.0 Build DEV1VP14 вЂ” 2026-05-30

### Changed
- **NEZ target rings Г—1.5:** inner **30** px @1080, gap **6** px (outer **36** px); migrate if inner **&lt;29** px.

## 1.3.0 Build DEV1VP13 вЂ” 2026-05-30

### Changed
- **NEZ target rings:** inner **20** px @1080, gap **4** px (outer **24** px); migrate if inner **&lt;16** px (was stuck at **12** in cfg).

## 1.3.0 Build DEV1VP12 вЂ” 2026-05-30

### Fixed
- **Big arc:** removed forced min band 0.022 (cfg **0.925/0.928** was bloating to fat stroke). Default **0.924/0.929** вЂ” classic OK + slight.
- **NEZ rings:** **12** px @1080, gap **2.5** (outer **14.5**); migrate if inner **&lt;9** px.

## 1.3.0 Build DEV1VP11 вЂ” 2026-05-30

### Fixed
- **Big arc:** stroke clamp **0.022вЂ“0.034** (was min **0.07** в†’ В«СЃСѓРїРµСЂ С‚РѕР»СЃС‚РѕРµВ»); defaults **0.902 / 0.928**.
- **NEZ target rings:** defaults **5.5** px @1080, gap **1.5** px; on load migrate old cfg (**>7** px inner, **>3** gap, thick arc band).

## 1.3.0 Build DEV1VP10 вЂ” 2026-05-30

### Fixed
- **Approaching (1 ring on target):** HUD green/calm tint, not red; same diameter as inner NEZ ring (**8** px @1080), not **52** px.
- **Small ring size:** removed **16** px floor and **8** px gap floor that blocked shrink; defaults **8** / **2.5** px gap (outer **~10.5** px).
- **Big OUT OF ARC ring:** sprite bake enforces min band **0.07** (visible stroke even if cfg has old thin values); defaults **0.858 / 0.928**.

## 1.3.0 Build DEV1VP9 вЂ” 2026-05-30

### Changed
- **Big arc stroke Г—2 again:** `RingInnerRadius01` / `RingOuterRadius01` **0.921 / 0.933** (band **0.012**).
- **NEZ pair on target Г·1.5:** inner **13.33** px, gap **4.67** px @1080 (outer **~18** px).

## 1.3.0 Build DEV1VP8 вЂ” 2026-05-30

### Changed
- **Ring stroke Г—2:** main arc `RingInnerRadius01`/`RingOuterRadius01` **0.924 / 0.930**; NEZ target ring sprites doubled band width.
- **NEZ pair on target:** inner diameter **20** px @1080 (was **30**); inner/outer gap **7** px (was **10**) вЂ” outer ring **27** px (was **40**).

## 1.3.0 Build DEV1VP7 вЂ” 2026-05-30

### Changed
- **Flight HUD style:** sample live **`HUDMissileState`** line images (ladder, dist span, max/min ticks) for **color + material + alpha** (incl. `CanvasGroup` chain). Fallback: laser **`outerCircle`** dim alpha **0.5**. `MatchFlightHudStyle` replaces alpha-only sampling; `PlayerSettings.hudColor` is fallback when style match is off.

## 1.3.0 Build DEV1VP6 вЂ” 2026-05-30

### Fixed
- **Plugin not loading:** BepInEx rejects non-semver in `[BepInPlugin]` вЂ” attribute uses **`1.3.0`** only; full dev string in log / `AssemblyInformationalVersion`.

## 1.3.0 Build DEV1VP5 вЂ” 2026-05-30

### Fixed
- **Invisible rings:** `MatchFlightHudAlpha` no longer tints calm rings from `waterline` RGB (always PlayerSettings HUD color); ignore near-zero sampled HUD alpha and fall back to legacy calm alpha / **0.5**; prefer missile ladder alpha over `waterline`.

## 1.3.0 Build DEV1VP4 вЂ” 2026-05-30

Unreleased dev toward GitHub **1.3.0** (last public release **1.2.0**).

### Changed
- **Flight HUD alpha:** `MatchFlightHudAlpha` (default **on**) вЂ” big arc and NEZ rings use the same opacity as visible Flight HUD lines (`waterline`, missile range ladder, fallback laser arc **0.5**). Optional `FlightHudAlphaInArcMul` / `OutOfArcMul`.
- **Resolution scaling:** main arc and NEZ rings scale with **`Screen.height`** (reference **1080** px).

### Fixed
- **Color/alpha mismatch** between main arc and NEZ target rings: arc no longer keeps the vanilla laser HUD **material**; both use default UI tinting via **`ApplyRingColor`**.
- **Approaching NEZ:** target ring uses red + **`NezApproachRingAlpha`** again (arc stays calm HUD color).

### Program
- **at747 versioning:** `AppVersion.cs`; `PluginVersion` = full dev string (`вЂ¦ Build DEVвЂ¦`), not bare semver bump per edit.

## 1.2.0 вЂ” 2026-05-19

- Calm launch-arc tint from PlayerSettings HUD color; separate calm in/out arc opacity.
- NEZ (No Escape Zone) via `Missile.CalcRange` вЂ” same as vanilla `HUDMissileState`.
- Approaching NEZ (default 1 km band): steady red ring on target; arc stays calm-colored.
- Inside NEZ: synchronized blinking red inner/outer target rings + blinking red arc ring.

## 1.0.0 вЂ” 2026-05-16

- Initial release: Flight HUD launch-arc ring for **non-laser** missiles with target lock.
- Arc limit from `WeaponInfo.targetRequirements.minAlignment` (same as vanilla **OUT OF ARC**).
- Circle scale matches vanilla laser `outerCircle` formula.
- Skips `WeaponInfo.laserGuided` weapons.
