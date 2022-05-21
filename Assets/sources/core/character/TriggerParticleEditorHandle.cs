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

            var rad = emitter.AngleMin * Mathf.Deg2Rad;
            Handles.DrawLine(pos - (Vector3)emitter.PositionOffset,
                pos + (Vector3)emitter.PositionOffset);
            Handles.DrawWireArc(pos, Vector3.forward,
                new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0).normalized,
                emitter.AngleMax - emitter.AngleMin, 1);
        }
    }
}