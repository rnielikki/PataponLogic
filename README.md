# ~ PATAPORT - Definition of Demon ~

## Goal

Our goal isn't just simply copying existing Patapon game. It may contain:

* More predictable and skill-based playing than official series
* New strategy and idea
* Very different storyline, but still inherits background and character of official Patapon series.

## Code

These will be moved to "documentation" part on wiki.

### Timing System

We use **Frequency** System with FixedUpdate, since FixedUpdate isn't very clean to count with remainer. Frequency is `int` value from 1/FixedUpdateTime.

#### Events

We use events for ease of extending. You can add event on Unity Editor - see the *Canvas/MusicSource* game object.

### Known issue

The performance on fever is gradually down, and curiously, we found the reason is on **text animation**. There's nothing that makes such slow on rhythm base logic or worm movement.

### Guide

* **DO use `RhythmTimer` and `TurnCounter` events** for right timing.
  * Somewhat tricky, but if a method when `RhythmTimer` event is called, you can use next turn event (`TurnCounter.OnNextTurn`) inside `RhythmTimer.OnNextHalfTime`.

* **AVOID using `FixedUpdate()` or `WaitForFixedUpdate()`**. We use FixedUpdate for listening perfect timing (also input listens from FixedUpdate), it's set by **half of default**, which means **FixedUpdate is called two times more than default**. Using directly from FixedUpdate() may gradually reduces performance. You can use `WaitForRhythmTime()` with Coroutine, or can call `RhythmTimer.OnTime()`,  `RhythmTimer.OnHalfTime()` instead if time is zero or half.
