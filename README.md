# ~ PATAPORT - Definition of Demon ~

## Goal

Our goal isn't just simply copying existing Patapon game. It may contain:

* More predictable and skill-based playing than official series
* New strategy and idea
* Very different storyline, but still inherits background and character of official Patapon series.

## Known issue

### Music

Some musics may not in sync. A music with sync hits beat in 0.5s - 2s (Note that not in sync in very small range, but ignore if it's too small so nobody can tell it).

First intro should be about 2 seconds, and for this reason the first sound has delay. But for this work, intro sound file (without other sound like base sound) is needed from original Patapon sound file. If we can find every intro sound, the code will be fixed (it's simple work!).

We didn't find some of voices that cannot be found with PSound : Wuffunfa chakachaka2,
If you found them, you can fill them and send merge request to main :)

### Performance

The performance on fever is gradually down, and curiously, we found the reason is on **text animation**. There's nothing that makes such slow on rhythm base logic or worm movement. It needs to be fixed on `fever-worm-animation` branch.

### Character Animation

Some of them are not smooth enough - we need people who can do animation well. The Patapon movement doesn't need to be exact same as original (For example, Yaripon Chakachaka is different from original but it's fine!) - it's enough to distinguishuable which status they are.

## Code

These will be moved to "documentation" part on wiki.

### Timing System

We use **Frequency** System with FixedUpdate, since FixedUpdate isn't very clean to count with remainer. Frequency is `int` value from 1/FixedUpdateTime. (Actually it uses also "InputInterval" seconds from RhythmEnvironment, but anyway. See more in RhythmEnvironment and RhythmTimer.)

#### Events

We use events for ease of extending. You can add event on Unity Editor - see the *Canvas/MusicSource* game object.

### Guide

* **DO use `RhythmTimer` and `TurnCounter` events** for right timing.
  * Somewhat tricky, but if a method when `RhythmTimer` event is called, you can use next turn event (`TurnCounter.OnNextTurn`) inside `RhythmTimer.OnNextHalfTime`.

* **AVOID using `FixedUpdate()` or `WaitForFixedUpdate()`**. We use FixedUpdate for listening perfect timing (also input listens from FixedUpdate), it's set by **half of default**, which means **FixedUpdate is called two times more than default**. Using directly from FixedUpdate() may gradually reduces performance. You can use `WaitForRhythmTime()` with Coroutine, or can call `RhythmTimer.OnTime()`,  `RhythmTimer.OnHalfTime()` instead if time is zero or half.

## Animation

Drum animation is 1 second - for scenario for "most early-most late". Each patapon command action animation is 2 seconds.
