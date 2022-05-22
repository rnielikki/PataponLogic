using PataRoad.Core.Character;
using UnityEditor;
using UnityEngine;

namespace PataRoad.EditMode
{
    [CustomEditor(typeof(TriggerParticleEmitter))]
    public class TriggerParticleEditorHandle : Editor
    {
        // Custom in-scene UI for when ExampleScript
        // component is selected.
        public void OnSceneGUI()
        {
            var emitter = target as TriggerParticleEmitter;
            var pos = emitter.transform.position;

            var radMin = emitter.AngleMin * Mathf.Deg2Rad;
            var minRadPos = new Vector3(Mathf.Cos(radMin), Mathf.Sin(radMin), 0).normalized;

            //range
            Handles.color = Color.yellow;
            Handles.DrawLine(pos - (Vector3)emitter.PositionOffset,
                pos + (Vector3)emitter.PositionOffset);

            //speed
            Handles.color = Color.cyan;
            Handles.DrawWireArc(pos, Vector3.forward, minRadPos,
                emitter.AngleMax - emitter.AngleMin, emitter.MinSpeed * emitter.Duration);
            Handles.DrawWireArc(pos, Vector3.forward, minRadPos,
                emitter.AngleMax - emitter.AngleMin, emitter.MaxSpeed * emitter.Duration);
            if (emitter.TwoSided)
            {
                Handles.DrawWireArc(pos, Vector3.forward, -minRadPos,
                    emitter.AngleMax - emitter.AngleMin, emitter.MinSpeed * emitter.Duration);
                Handles.DrawWireArc(pos, Vector3.forward, -minRadPos,
                    emitter.AngleMax - emitter.AngleMin, emitter.MaxSpeed * emitter.Duration);
            }
        }
    }
}