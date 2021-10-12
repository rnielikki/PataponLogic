# ~ PATAPORT - Definition of Demon ~

## Goal

Our goal isn't just simply copying existing Patapon game. We'll try to achieve in this game:

* For **both who haven't played Patapon and who played them**, which also means we'll prepare tutorial and difficulty system for the game in the future.
* More predictable and skill-based playing than official series
* New strategy and idea
* Very different storyline, but still inherits background and character of official Patapon series.

Complexity (scale) is maybe somewhere between Patapon2 and Patapon3.

## Known issue

### Music

Some musics may not in sync. A music with sync hits beat in 0.5s - 2s (Note that not in sync in very small range, but ignore if it's too small so nobody can tell it).

First intro should be about 2 seconds, and for this reason the first sound has delay. But for this work, intro sound file (without other sound like base sound) is needed from original Patapon sound file. If we can find every intro sound, the code will be fixed (it's simple work!).

We didn't find some of voices that cannot be found with PSound : Wuffunfa chakachaka2, Some voices in Zunzun...
If you found them, you can fill them and send merge request to main :)

### Character Animation

Some of them are not smooth enough - we need people who can do animation well. The Patapon movement doesn't need to be exact same as original (For example, Yaripon Chakachaka is different from original but it's fine!) - it's enough to distinguishuable which status they are.

## Code

These will be moved to "documentation" part on wiki.

### Timing System

We use **Frequency** System with FixedUpdate, since FixedUpdate isn't very clean to count with remainer. Frequency is `int` value from 1/FixedUpdateTime. (Actually it uses also "InputInterval" seconds from RhythmEnvironment, but anyway. See more in RhythmEnvironment and RhythmTimer.)

#### Events

We use events for ease of extending. You can add event on Unity Editor - see the *Canvas/MusicSource* game object.

## Guide

Currently we don't accept any feature - we're focusing on realising existing specs and fixing bugs.

You can contribute by:

* Programming
* Graphic design (UI/UX design. Do UI design only if you can continuosly contribute, for consistency.)
* Documentation
* Testing
* Spreading this project

Where to see what should do:

1. See the Project tab and select the ticket that you can do, from "tasks" part.
2. Assign yourself by leaving post to the thread in corresponding issue. Move ticket to the "doing" part. *We don't recommend to assign too much task alone. 'OK, One step at a time', also Patapon said.*.
3. Fork corresponding version repository (in the issue) to your repository.
4. Work in your forked branch. If you cannot fix, take out the assignment back to "tasks". **Note: For progress, we'll take back to "tasks" and unassign you if you don't response in 2 weeks. If task takes long, just let us know how's going, even with few words.** (but also we don't want to hear "I was busy so I didn't do at all", which is better to give chance to others).
5. Send Pull Request from your repository to the corresponding version repository.

How to contribute by leaving issue:

1. See on the issue tab. Search first.
2. If you cannot find, select template and fill it with the template guideline. Please follow the guideline, without them we cannot do/accept anything.

## DO and DON'Ts

* **DO use `RhythmTimer` and `TurnCounter` events** for right timing.
  * Somewhat tricky, but if a method when `RhythmTimer` event is called, you can use next turn event (`TurnCounter.OnNextTurn`) inside `RhythmTimer.OnNextHalfTime`.

* **AVOID using `FixedUpdate()` or `WaitForFixedUpdate()`**. We use FixedUpdate for listening perfect timing (also input listens from FixedUpdate), it's set by **half of default**, which means **FixedUpdate is called two times more than default**. Using directly from FixedUpdate() may gradually reduces performance. You can use `WaitForRhythmTime()` with Coroutine, or can call `RhythmTimer.OnTime()`,  `RhythmTimer.OnHalfTime()` instead if time is zero or half.

* **AVOID scene editing**. The scenes are **most easy part to get merge conflict**. Don't edit scene if possible. Also some changes with the scenes will be removed for this reason. Notify on the corresponding issue thread, if scene change is needed, and we'll put `Scene change needed` flag.

## Animation

Drum animation is 1 second - for scenario for "most early-most late". Each patapon command action animation is 2 seconds.
