# Game Design — CourseQuest VR

## Game Overview

- **Genre:** VR educational quiz / course-recommendation experience
- **Target Audience:** Students exploring AI, Computing, or Cybersecurity as a course/career track
- **Platform:** Meta Quest (standalone VR headset)

## Core Gameplay Loop

1. Welcome video → knowledge-quiz intro
2. **Knowledge phase**: video-presented trivia questions, A/B/C multiple choice, right/wrong scored
   (+2 points to the correct answer's tagged course), wrong answers trigger controller haptic feedback
3. **Preference phase**: video-presented career-style questions, no right/wrong — each option nudges
   toward one of the three course tracks (+1 point)
4. Collective feedback video (based on top-2 scoring courses)
5. Quiz end screen → recommendation video for the single best-matching course, plus an optional
   points/answers summary

## Mechanics

- Scoring is per-course-track (AI / Computing / Cybersecurity), accumulated across both question phases
- Video is the primary question-presentation medium (not just text) — every question and most
  transitions are backed by a `.mp4` played via `VideoPlayer`
- Haptic feedback (`OVRInput.SetControllerVibration`) on incorrect knowledge answers

## Progression

- Single-session, linear 30-question flow — no persistent player leveling or save-file continuation
  between sessions beyond Cloud Save recording past results
- No local account system; each play session is anonymous (Unity Services anonymous sign-in)

## UI/UX

- World-space canvases (not screen-space) throughout, sized for VR headset viewing distance
- TextMeshPro for all text rendering
- Video-first question presentation rather than static text/image quiz UI
