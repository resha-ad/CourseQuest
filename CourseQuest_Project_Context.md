# CourseQuest VR — Full Project Context (for planning/thesis discussion)

> This document is a factual snapshot of an existing Unity VR project, compiled for use as context in an AI planning session (Claude Chat / Cowork) to brainstorm enhancements for a final-year college thesis. It covers purpose, flow, mechanics, tools, objects/screens, and known technical debt.

---

## 1. What the project is

**Product name (in ProjectSettings):** `CourseQuest VR`
**Platform:** Meta Quest (standalone Android VR headset), built with Unity, using `OVRCameraRig` (Oculus Integration) for head/controller tracking.

**Concept:** A VR educational quiz experience. The live/current build is a **30-question video-based quiz** that does two things at once:
- **Knowledge questions** — general CS/tech/security trivia, right/wrong scored (e.g. "What does HTML stand for?", "What is Phishing?").
- **Preference questions** — career-style multiple choice with no right/wrong answer, each option nudges the user toward one of three tracks: **AI, Computing, Cybersecurity**.

At the end, the app shows a **collective feedback video**, calculates the user's best-matching course track, plays a **personalized recommendation video**, and displays a **points/answers summary**. So functionally, this is a **VR course/career-recommendation quiz**, presented question-by-question via short pre-rendered videos plus A/B/C multiple-choice buttons.

**Important nuance for framing the thesis:** the codebase also contains two other, non-live quiz subsystems (see §7) — a **cybersecurity-awareness narrative quiz** and a **nutrition/dietary/health self-assessment** — plus leftover **non-VR desktop "3D tour" navigation code** (`CameraController.cs`, `MenuController.cs`). These are not random third-party cruft: **they're from the student's own earlier project**, which was edited/merged into this one. They were kept rather than deleted specifically because it wasn't clear whether other parts of the working app depended on them — a reasonable caution, not a mistake. That means before removing or refactoring any of it, the real dependency graph should be checked (which GameObjects in each scene actually have these scripts attached, and whether any *other* live script references them) rather than assuming they're safe to delete outright. The **actually-shipping flow is the course-recommendation quiz** (`SurveyManager`), not nutrition or cybersecurity — but deciding what to formally do with the other two (retire cleanly vs. merge as alternate modes vs. keep as documented legacy) is itself a legitimate scoping decision for the planning session.

---

## 2. Scenes

| Scene | File | Notes |
|---|---|---|
| ClassRoom (main/current) | `Assets/Scenes/ClassRoom.unity` | The live quiz scene, 26 root GameObjects, VR classroom environment with robot avatar props |
| TechLab (referenced) | *(loaded by name via `SceneManagerController`)* | Landing/welcome scene the quiz transitions from/back to |
| CastleVR, SciFiVR, demo22, SampleScene | `Assets/Scenes/*.unity` | Unused/demo environment scenes from asset packs, not part of the live flow |

**ClassRoom.unity root GameObjects (26):**
UIHelpers, OVRCameraRig, MainVideoCanvas, WelcomeVideoCanvas, KnowledgeIntroCanvas, PointsCanvas, Q1VideoCanvas, RecommendationCanvas, PrefernceIntroCanvas *(sic — typo in name)*, FeedbackCanvas, SurveyManager, CollectiveFeedbackCanvas, AudioManager, Main Camera, Directional Light, Classroom Touchpad, MainCanvasManager, AnswerCanvas1, QuizEndCanvas, AnswerCanvas2, AnswerCanvasManager, QuestionDisplayManager, mrc_nm_aveline_robot_result_1, roboKawaii_last, monitor corupted bot, CartoonRobot.

The VR classroom environment itself is built from a mix of asset-store packs (`Assets/Room`, `Assets/school`, `Modular Castle`, `Sci-Fi Styled Modular Pack`, `Altar_Ruins_FREE`, `Fantasy Skybox FREE`, `Grass And Flowers Pack 1`, etc.) plus several purchased/downloaded robot character models (`aveline-robot`, `cute-home-robot`, `medium-cute-robo-drone`, `robot-dog`, `turbobolt-the-cartoon-robot`) used as classroom decoration/avatars.

---

## 3. Screen-by-screen user flow (live/active system)

Driven mainly by `Assets/Scripts/SurveyManager.cs`, with scene transitions centralized in a singleton `SceneManagerController` (`Assets/SceneManager.cs`, `DontDestroyOnLoad`, exposes `LoadTechLabScene()` / `LoadClassRoomScene()`).

1. **Launch / intro video** — `MainCanvasButton.cs` activates `KnowledgeIntroCanvas` on start. `StartSurvey()` stops the intro video and shows `WelcomeVideoCanvas`.
2. **Welcome → Knowledge intro** — `WelcomeCanvasManager.GoToQuestionCanvas()` swaps `WelcomeVideoCanvas` off, `KnowledgeIntroCanvas` on.
3. **Scene transition** — `CanvasTransition.TransitionToClassRoom()` loads the ClassRoom scene, then finds `SurveyManager` and calls `StartKnowledgeQuestions()`.
4. **Knowledge question phase** — For each question: plays a per-question video (`Q1.mp4`…`Q30.mp4` from `Assets/StreamingAssets/`) via `VideoPlayer.Prepare()`, populates up to 3 option buttons (A/B/C) from JSON, scores the pick, disables buttons, advances after a short delay.
5. **Preference intro → preference question phase** — same video+button mechanism, but options represent a career pull rather than right/wrong.
6. **Collective feedback video** — after all questions, computes the top-2 scoring course tracks and plays one of six pre-rendered `Preference_Feedback_<Course1>_<Course2>.mp4` videos.
7. **Quiz end screen** — shows result options: view recommendation, go home, or check points/answers; also fires a confetti effect and triggers a cloud save.
8. **Recommendation** — plays the single best-matching course's `<Course>_Recommendation.mp4` (AI / Computing / Cybersecurity).
9. **Points/answers summary** — displays raw answer list plus AI/Computing/Cybersecurity point totals as text; triggers another cloud save.
10. **Answers review (2-part) — CONFIRMED BROKEN, not reachable in practice.** `AnswerCanvas1`/`AnswerCanvas2` were meant to present a two-part "review your answers" screen leading into Points, but verified directly against the scene file (`Assets/Scenes/ClassRoom.unity`): nothing in any live script ever activates `answersCanvasPart1` (it's set inactive at startup and never re-enabled), and two buttons in the scene are wired via the Unity Inspector to call `OnContinueButtonClicked` / `OnContinueToPreference` on the `SurveyManager` component — **neither method exists on `SurveyManager`** (only a same-named method exists on the separate `AnswerCanvas2Manager` script, an unrelated component). A third button ("ContinueButton") is wired to call `SurveyManager.ShowPointsCanvas()` directly, but that method is `private`, and Unity's serialized OnClick bindings can only invoke public methods — so that binding is silently a no-op too. **Net effect: this entire review chain is dead code from an unfinished refactor.**
11. **Points/answers summary — reachable, but easy to miss.** The *only* working path into `PointsCanvas` is the **"Check" button on `QuizEndCanvas`**, wired correctly via C# (`AddListener`, not Inspector reflection — bypasses the bug above). Critically, `QuizEndCanvas` presents **View Result / Home / Check as three parallel, mutually exclusive options**, not a sequence — clicking "View Result" (the natural choice) jumps straight to Recommendation and the user never sees Points/answers at all.
12. **Return home / restart** — either a graceful reset (`ReturnToMainVideo()` + `ResetQuizData()`, zeroes scores/answers for a fresh run) or a blunt full scene reload (`RestartGame.cs`). In normal play, this is what's reached right after Recommendation, since Recommendation itself has no code-driven forward path and QuizEndCanvas's branches don't chain.

---

## 4. Core mechanics

**Question data (live system):** `Assets/Resources/vr_survey_questions.json`, loaded via `Resources.Load<TextAsset>` + `JsonUtility`. 30 questions total — Q1–5 & Q16–25 are "preference" type (3 options, each tagged with a target course), Q6–15 & Q26–30 are "knowledge" type (exactly one correct option, has an associated feedback video). Schema:
```json
{
  "questions": [
    {
      "questionID": 1, "questionText": "...", "videoFile": "Q1.mp4",
      "questionType": "preference | knowledge",
      "feedbackVideo": "Feedback_Q4.mp4",   // knowledge only
      "options": [
        { "optionID": "A", "text": "...", "course": "AI|Computing|Cybersecurity|null",
          "explanation": "...", "isCorrect": true }
      ]
    }
  ]
}
```

**Scoring:** Knowledge questions award +2 points to the correct option's course track on a correct answer; preference questions always award +1 point to their tagged course track. Wrong knowledge answers highlight correct vs. wrong options and trigger controller vibration (haptic feedback via `OVRInput.SetControllerVibration`).

**Data persistence:** Unity Gaming Services **Cloud Save**, with **anonymous sign-in** (`AuthenticationService.SignInAnonymouslyAsync()` — no real user accounts, so no persistent per-user identity across devices/reinstalls). Answers are saved as a JSON-serialized response list under a randomly generated key. No local save/offline fallback exists on-device (a `#if UNITY_EDITOR`-only debug file write exists for local dev testing).

**Leaderboard:** `LeaderboardManager.cs` loads all Cloud Save keys project-wide, parses each as an integer score, sorts descending, and shows the top 7. A companion `VulnerableManager.cs` does the same but ascending (bottom 7 — a "lowest scores" board).

---

## 5. VR interaction

- Controller input via `OVRInput` — trigger/button presses on canvas UI buttons (standard Unity `Button.onClick` on world-space canvases, via `OVRCameraRig` + Unity `EventSystem`), plus explicit haptic feedback (`OVRInput.SetControllerVibration`) on wrong answers.
- `Classroom Touchpad` is a scene prop; its interactive behavior (if any) comes from Oculus Integration's own components rather than custom app code.
- No teleportation/locomotion system evident in the reviewed scripts — the experience appears to be a **stationary, seated/standing quiz** rather than a room-scale explorable environment.

---

## 6. Audio / feedback

- Background/UI audio via a dedicated `AudioManager` scene object.
- Per-question right/wrong answer audio stings (`SurveyManager`, randomized clips).
- Score-based result audio + image reveal (`ScoreAudioManager`) and a "typewriter" text-reveal effect for feedback text (`TypewriterEffect.cs`) exist in the codebase but are **not currently wired into the live flow** (their trigger calls are commented out) — see §7.
- An Android native Text-to-Speech wrapper (`AndroidTextToSpeech.cs`) exists but only speaks a hardcoded demo sentence and isn't connected to real quiz content.

---

## 7. Tools / SDKs (from `Packages/manifest.json`)

| Category | Package |
|---|---|
| Cloud backend | `com.unity.services.cloudsave` 3.2.2 (+ Unity Authentication, anonymous sign-in) |
| VR/XR | `com.unity.xr.management` 4.5.0, `com.unity.xr.oculus` 4.4.0 (legacy Oculus provider — **not** the newer Meta XR All-in-One SDK), plus a manually-imported `Assets/Oculus` Integration folder (Interaction, AudioManager, LipSync) |
| UI/Text | `com.unity.textmeshpro` 3.0.7, `com.unity.ugui` |
| Video/Audio | `com.unity.modules.video`, `com.unity.modules.audio`, related UnityWebRequest modules |
| Other | `com.unity.timeline` 1.7.6, `com.unity.visualscripting` 1.9.4 (present, no confirmed active use), `com.unity.collab-proxy` 2.6.0 (Plastic SCM) |
| Dev tooling only | Unity MCP Editor plugin packages (`com.ivanmurzak.unity.mcp` + companions) — enables AI-assisted Unity editing, not part of the shipped app |

No Cinemachine package in the actual project dependencies (camera is a plain `Camera` + `OVRCameraRig`, not virtual cameras).

---

## 8. Known technical debt / consolidation opportunities

This section exists specifically because a thesis project benefits from being able to say "I identified X, Y, Z and addressed them" — these are concrete, verifiable issues found by reading the code, not speculation.

**Three parallel, non-interoperating quiz engines exist in the same project:**
1. `SurveyManager.cs` — the live 30-question JSON-driven knowledge/preference quiz (course recommendation).
2. `MyQuestionManager.cs` + `MyQuestions.cs` — an earlier 15-question cybersecurity-narrative quiz (pass/fail scoring, its own save format).
3. `QuestionManager.cs` + `NutritionQuestions.cs` + `DietaryPreferences.cs` + `FeedbackManager.cs` — a nutrition/health/physical-activity self-assessment (category-based point scoring, no right/wrong).

They have incompatible save schemas, and the two leaderboard scripts expect a data shape that **neither current save path actually produces** (would likely fail parsing real data).

**Duplicate/dead implementations of the same responsibility:**
- `QuestionDisplayManager.cs` — an entire file that's 100% commented out, an earlier version of what `SurveyManager` now does.
- `QuestionLoader.cs` — a third, live but seemingly-orphaned implementation of question-loading/option-wiring, with its own hardcoded partial feedback-video map.
- `LeaderboardManager.cs` / `VulnerableManager.cs` — structurally identical except sort direction; candidates for merging into one parameterized component.
- Multiple independent "video finished → advance" implementations (`VideoFinishManager`, `VideoManager`, `VideoTransition`, plus `SurveyManager`'s own inline handling) instead of one reusable component.

**At least 6 placeholder/empty scaffold scripts** committed directly under `Assets/` (`NewBehaviourScript.cs`, `FPS.cs`, `MainCanvasScript.cs`, `scrior.cs`, `vr bb.cs`, `console to text.cs`) — dead weight from Unity's default script template, never cleaned up.

**Confirmed (not inferred) UI dead-end bug — verified against both the C# source and the serialized scene bindings in `ClassRoom.unity`:**
- The "review your answers" flow (`AnswerCanvas1`→`AnswerCanvas2`→Points) never actually activates on-screen; two of its buttons are wired via the Unity Inspector to method names (`OnContinueButtonClicked`, `OnContinueToPreference`) that **don't exist on the `SurveyManager` component they're bound to** (stale references from a refactor — one of those method names does exist, but on a different, unrelated script). A third button calls a `private` method directly, which Unity's serialized event system can't invoke at all. Net result: users can only ever reach the Points/answers summary via the "Check" button on `QuizEndCanvas`, and only if they don't click "View Result" or "Home" first — which is why the typical play-through goes straight from Recommendation to Home/Restart. This is a real, fixable bug (re-point the two broken bindings, make `ShowPointsCanvas` public or wire it via code like `checkButton` already is), not a design choice.

**Other specific bugs found:**
- Two content bugs in `MyQuestions.cs` (duplicate "C)" option labels in question 5; mislabeled "B)" in question 7).
- `ScoreAudioManager`'s score bands overlap (75–100 and 40–80 ranges conflict).
- `QuestionManager`/`DietaryPreferences` skip "Category 3" entirely in their category numbering (removed at some point, never renumbered).
- `[System.Obsolete]` attribute misapplied to several actively-used async methods (likely accidental).
- `CameraController.cs` wraps its whole `Update()` in `if (true) { ... }` — dead conditional from disabled/re-enabled debug code.
- `VideoManager.cs` reaches into `MyQuestionManager`'s public `score` field directly from a different script — fragile cross-script coupling instead of events/callbacks.
- Anonymous-only auth means no persistent user identity — leaderboard entries can't be tied to a real returning user, and a `currentLevel = 420129` magic-number auto-increment scheme in `MyQuestionManager.SaveData()` is race-condition-prone.
- No offline/local fallback if Cloud Save fails on-device (Quest).
- `CameraController.cs` and `MenuController.cs` contain leftover desktop-mouse-drag camera control and a commented-out raycast "next site" navigation system — remnants of what looks like an earlier **non-VR desktop/3D-tour prototype** of the same concept.

---

## 9. Suggested angles to raise with the planning AI

(Purely to prompt discussion — not a decision, just context that makes the brainstorm concrete.)
- Which of the three quiz subsystems is "the product" going forward — consolidate or formally retire the other two?
- Real user accounts / persistent identity vs. current anonymous-only Cloud Save (affects leaderboard integrity, progress tracking, multi-session use — all strong thesis "system design" talking points).
- Data layer cleanup: unify save schema so the leaderboard actually reads what the quiz writes.
- Locomotion/interaction depth: currently a stationary quiz — is room-scale interaction, object manipulation, or multiplayer/classroom-shared mode in scope?
- Accessibility: the TTS wrapper exists but is unused — could be genuinely wired up for accessibility credit in a thesis.
- Analytics/instructor-facing dashboard: cloud data currently isn't summarized anywhere except a personal leaderboard — an instructor/admin view could be a strong "contribution" chapter.
- Content authoring: questions are currently hand-edited JSON/hardcoded C# — a simple in-editor or web authoring tool would be a solid engineering contribution.
- Code health as a thesis chapter: the technical-debt findings above (dead code removal, unified video-transition component, merged leaderboard component) are legitimate refactor/methodology material, not just cleanup.

---

*Compiled by reading the project's C# scripts, scene structure, JSON question data, and `Packages/manifest.json` directly — not from assumptions. Use this as grounding context; treat §9 as conversation starters, not conclusions.*
