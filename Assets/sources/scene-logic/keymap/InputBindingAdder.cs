using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputBindingAdder : MonoBehaviour
    {
        [SerializeField]
        InputActionLoader _loader;
        [SerializeField]
        UnityEngine.UI.Image _image;
        [SerializeField]
        Color _listeningColor;
        Color _defaultColor;
        private void Start()
        {
            _defaultColor = _image.color;
        }
        public void AddBinding()
        {
            _image.color = _listeningColor;

            var action = _loader.CurrentAction;
            action.Disable();

            var rebind = action.PerformInteractiveRebinding()
                .WithRebindAddingNewBinding()
                .WithCancelingThrough("<Keyboard>/escape");

            rebind.Start().OnPotentialMatch(Match).OnComplete(Complete).OnCancel(Cancel);
        }

        private void Match(InputActionRebindingExtensions.RebindingOperation op)
        {
            var path = op.selectedControl.path;
            foreach (var binding in _loader.CurrentAction.bindings)
            {
                if (binding.effectivePath == path)
                {
                    Debug.Log($"found {binding.effectivePath}, canceling");
                    op.Cancel();
                    return;
                }
            }
            if (Core.Global.GlobalData.GlobalInputActions.KeyMapModel.DoPathExist(path))
            {
                op.Cancel();
                return;
            }
            op.Complete();
        }

        private void Complete(InputActionRebindingExtensions.RebindingOperation op)
        {
            var binding = _loader.CurrentAction.GetBindingForControl(op.selectedControl);

            if (binding != null)
            {
                Debug.Log(binding.Value.path + " <- path");
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(binding.Value);
                _loader.Attach(binding.Value);
            }
            else Debug.Log($"binding is null for {op.selectedControl.displayName}, canceling");
            op.Dispose();
            _image.color = _defaultColor;
        }
        private void Cancel(InputActionRebindingExtensions.RebindingOperation op)
        {
            Debug.Log(op.selectedControl.path);
            op.Dispose();
            _image.color = _defaultColor;
        }
    }
}
