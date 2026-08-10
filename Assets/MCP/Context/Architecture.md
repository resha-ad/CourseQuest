# Project Architecture — CourseQuest VR

## System Overview

A VR course-recommendation quiz for Meta Quest headsets. The player answers 30 video-presented
questions (a mix of scored knowledge trivia and unscored career-preference picks), and the app
recommends one of three tracks — AI, Computing, or Cybersecurity — based on accumulated points.

## VR / XR Stack — read before touching XR packages

- Built on the **legacy Oculus Integration** (`Assets/Oculus/`, manually imported) + `com.unity.xr.oculus`
  (the older Unity XR provider) + `com.unity.xr.management`.
- **Do not add `com.meta.xr.sdk.*` packages** (the newer unified Meta XR SDK) without a deliberate,
  full migration. They ship files with GUIDs that collide with the existing `Assets/Oculus` content and
  will corrupt the asset database if installed alongside it. If a Package Manager popup suggests
  upgrading, decline it — the legacy integration is what the shipped app actually runs on.
- The **live shipped app's** VR interaction (quiz UI, controller vibration) is hand-rolled against
  `OVRInput` (button/trigger polling) and `Transform` positions, not XRI interactables — that's the
  existing, working pattern, don't rewrite it without reason.
- `com.unity.inputsystem` and `com.unity.xr.interaction.toolkit` (3.5.0) are now also installed, for use
  in **new** prototype/dev work. `Active Input Handling` (Project Settings → Player) is set to **Both**
  so the new Input System is available without breaking the existing `OVRInput` code — do not change
  this to "Input System Package (New)" only.

## Core Systems

### `SceneManagerController` (`Assets/SceneManager.cs`)
- Singleton, `DontDestroyOnLoad`, lives in `TechLab.unity`
- Exposes `LoadTechLabScene()` / `LoadClassRoomScene()` — all scene transitions go through this

### `SurveyManager` (`Assets/Scripts/SurveyManager.cs`)
- The main quiz engine, lives in `ClassRoom.unity`
- Loads `Assets/Resources/vr_survey_questions.json`, splits into knowledge (right/wrong scored)
  and preference (course-tagged, unscored-correctness) questions
- Drives video playback per question, option-button wiring, scoring, Cloud Save persistence

### `CanvasTransition` (welcome flow, `TechLab.unity`) / `MainCanvasButton` / `AnswerCanvas2Manager` / `ReplayVideo`
- Supporting UI-flow scripts for the welcome video, quiz-end canvas transitions, and answer review

## Data Flow

1. `vr_survey_questions.json` → `SurveyManager.LoadJSON()` → split into knowledge/preference lists
2. Each answer → `AddPointsToCourse()` (knowledge: +2 to correct option's course; preference: +1 to tagged course)
3. End of quiz → `coursePoints` dict compared → best-matching course recommended
4. Answers serialized and saved via Unity Gaming Services **Cloud Save** (anonymous auth — no persistent
   per-user identity across devices/reinstalls)

## Known issue (still open)

The "review your answers" 2-part canvas flow (`AnswerCanvas1`/`AnswerCanvas2` → `PointsCanvas`) is dead:
two buttons are wired via the Unity Inspector to method names that don't exist on the `SurveyManager`
component they're bound to (stale refactor artifacts), and a third calls a `private` method that Unity's
serialized `OnClick` system can't invoke. The only working path to the points/answers summary is the
"Check" button on `QuizEndCanvas`. Fix candidates: re-point the two broken bindings, or make
`SurveyManager.ShowPointsCanvas()` public and wire it via `AddListener` like `checkButton` already is.

## Dependencies (see `Packages/manifest.json` for exact versions)

- `com.unity.services.cloudsave` — save/leaderboard persistence
- `com.unity.xr.oculus` + `com.unity.xr.management` — VR runtime (legacy provider, see above)
- `com.unity.textmeshpro` — all in-scene text
- `com.unity.timeline`, `com.unity.visualscripting` — present, no confirmed active use in live gameplay code
- `com.anklebreaker.unity-mcp` — dev tooling only (AI-assisted editing), not part of the shipped app

## Build Targets

- Android (Meta Quest), Built-in render pipeline, IL2CPP, .NET Standard 2.0
- Shipped scenes: `Assets/TechLab.unity` (landing) + `Assets/Scenes/ClassRoom.unity` (quiz) — check
  `ProjectSettings/EditorBuildSettings.asset` before assuming any other scene is live
