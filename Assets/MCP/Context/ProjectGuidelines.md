# Project Guidelines — CourseQuest VR

## Naming Conventions

- Classes / methods: PascalCase
- Fields: camelCase, `public` and Inspector-exposed by default (not `[SerializeField] private`) — this is the existing convention in the live code (`SurveyManager`, `CanvasTransition`, etc.), keep new work consistent with it rather than switching styles mid-project
- Scene objects: PascalCase, descriptive (`QuizEndCanvas`, `KnowledgeIntroCanvas`, etc.)

## Code Style

- Heavy `Debug.Log` usage is the existing convention for traceability through async flows (Cloud Save calls, scene transitions, question state) — keep this pattern for new gameplay code, it's genuinely useful for VR debugging where you can't easily attach a debugger mid-session
- Async Unity lifecycle methods (`async void Start()`) are used for Cloud Save / Unity Services initialization — follow the existing try/catch pattern around `UnityServices.InitializeAsync()` / `AuthenticationService` calls if adding more

## Project Structure

- Most gameplay scripts currently live directly under `Assets/` rather than `Assets/Scripts/` — that's existing layout, not a convention to copy. **New work should go in its own clearly-named subfolder** (e.g. `Assets/Prototypes/<name>/` for throwaway prototypes, kept separate from shipping code)
- `Assets/Resources/vr_survey_questions.json` — the live quiz's question data (JSON, loaded via `Resources.Load<TextAsset>`)
- `Assets/Scenes/ClassRoom.unity` + `Assets/TechLab.unity` — the two scenes that actually ship (see `ProjectSettings/EditorBuildSettings.asset`); everything else in `Assets/Scenes/` is disabled/unused
- `Assets/MCP/Context/` — this folder; keep it updated as the project evolves, it's what gives any AI agent connecting via MCP its bearings without re-deriving everything from scratch

## Git Workflow

- Single `master` branch, no feature-branch workflow currently in use
- Commit messages: plain and functional, describing what changed technically — not narrative/historical. Git history here is for developer reference only, not a public-facing changelog
- Large binaries (textures, models, audio, video, native plugins) are tracked via Git LFS — check `.gitattributes` before adding new binary asset types

## Unity Version / Platform

- Unity **2022.3.34f1** (LTS)
- Render pipeline: **Built-in** (not URP/HDRP)
- Scripting backend: **IL2CPP**, API compatibility: **.NET Standard 2.0**
- Build target: **Android** (Meta Quest)
