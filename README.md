# ~ PATAPORT - Definition of Demon ~

## Code

These will be moved to "documentation" part on wiki.

### Timing System

We use **Frequency** System with FixedUpdate, since FixedUpdate isn't very clean to count with remainer. Frequency is `int` value from 1/FixedUpdateTime.

#### Events

We use events for ease of extending. You can add event on Unity Editor - see the *Canvas/MusicSource* game object.

### Guide

> **AVOID using `FixedUpdate()` or `WaitForFixedUpdate()`**. We use FixedUpdate for listening perfect timing (also input listens from FixedUpdate), it's set by **half of default**, which means **FixedUpdate is called two times more than default**. Using directly from FixedUpdate() may gradually reduces performance. You can use `WaitForRhythmTime()` with Coroutine, or can call `RhythmTimer.OnTime()`,  `RhythmTimer.OnHalfTime()` instead if time is zero or half.
