using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputBindingAdder : MonoBehaviour
    {
        InputAction _action;
        [SerializeField]
        UnityEngine.UI.Image _image;
        [SerializeField]
        Color _listeningColor;
        Color _defaultColor;

        [SerializeField]
        GameObject _status;
        [SerializeField]
        UnityEngine.UI.Text _statusText;

        InputActionLoader _loader;
        private UnityEngine.Events.UnityAction ListenBinding { get; set; }
        private void Start()
        {
            _defaultColor = _image.color;
        }
        public void Init(InputActionLoader loader) => _loader = loader;
        public void SetListeningType(InputAction action)
        {
            _action = action;
            if (action.type == InputActionType.Value)
            {
                switch (action.expectedControlType)
                {
                    case "Axis":
                        ListenBinding = ListenBindingFor1D;
                        break;
                    case "Vector 2":
                        ListenBinding = ListenBindingFor2D;
                        break;
                    default:
                        Debug.LogError(action.expectedControlType + " not supported");
                        throw new System.NotImplementedException();
                }
            }
            else
            {
                ListenBinding = ListenBindingDefault;
            }
        }
        public void AddBinding()
        {
            _image.color = _listeningColor;
            _action.Disable();
            ListenBinding();
        }
        private void ListenBindingDefault()
        {
            _status.SetActive(true);
            _statusText.text = "Press Key to assign";
            var rebind = _action.PerformInteractiveRebinding()
                .WithRebindAddingNewBinding()
                .WithCancelingThrough("<Keyboard>/escape");

            rebind.Start().OnPotentialMatch(Match).OnComplete(Complete).OnCancel(Cancel);
        }
        private void ListenBindingFor1D()
        {
            var rebind0 = GetOperation();
            var rebind1 = GetOperation();
            var queue = new System.Collections.Generic.Queue<(string, InputActionRebindingExtensions.RebindingOperation)>();
            queue.Enqueue(("Press Left or Up (negative) to assign", rebind0));
            queue.Enqueue(("Press Right or Down (positive) to assign", rebind1));
            ChainListen(queue, new System.Collections.Generic.List<InputControl>(), After1DAxis, true);
            void After1DAxis(System.Collections.Generic.List<InputControl> results)
            {
                var composite = _action.AddCompositeBinding("1DAxis")
                    .With("negative", ConvertToBindingPath(results[0].path))
                    .With("positive", ConvertToBindingPath(results[1].path));
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(_action.bindings[composite.bindingIndex + 1]);
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(_action.bindings[composite.bindingIndex + 2]);
                _action.AddBinding();
                _loader.Refresh();
            }
        }

        private void ListenBindingFor2D()
        {
            var rebindUp = GetOperation();
            var rebindDown = GetOperation();
            var rebindLeft = GetOperation();
            var rebindRight = GetOperation();
            var queue = new System.Collections.Generic.Queue<(string, InputActionRebindingExtensions.RebindingOperation)>();
            queue.Enqueue(("Press Up Key to assign", rebindUp));
            queue.Enqueue(("Press Down Key to assign", rebindDown));
            queue.Enqueue(("Press Left Key to assign", rebindLeft));
            queue.Enqueue(("Press Right Key to assign", rebindRight));
            ChainListen(queue, new System.Collections.Generic.List<InputControl>(), After2DVector, true);

            void After2DVector(System.Collections.Generic.List<InputControl> results)
            {
                var composite = _action.AddCompositeBinding("2DVector")
                    .With("up", ConvertToBindingPath(results[0].path))
                    .With("down", ConvertToBindingPath(results[1].path))
                    .With("left", ConvertToBindingPath(results[2].path))
                    .With("right", ConvertToBindingPath(results[3].path));
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(_action.bindings[composite.bindingIndex + 1]);
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(_action.bindings[composite.bindingIndex + 2]);
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(_action.bindings[composite.bindingIndex + 3]);
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(_action.bindings[composite.bindingIndex + 4]);
                _loader.Refresh();
            }
        }
        private void ChainListen(System.Collections.Generic.Queue<(string, InputActionRebindingExtensions.RebindingOperation)> operations,
            System.Collections.Generic.List<InputControl> results,
            UnityEngine.Events.UnityAction<System.Collections.Generic.List<InputControl>> callback,
            bool first = false)
        {
            if (!first)
            {
                var (_, operation) = operations.Dequeue();
                if (operation != null)
                {
                    results.Add(operation.selectedControl);
                    operation.Dispose();
                }
            }
            else _status.SetActive(true);

            if (operations.Count == 0)
            {
                _status.SetActive(false);
                _image.color = _defaultColor;
                callback(results);
                return;
            }
            var (label, current) = operations.Peek();
            _statusText.text = label;
            current.Start().OnPotentialMatch(Match).OnComplete((_) => ChainListen(operations, results, callback)).OnCancel(Cancel);
        }

        private InputActionRebindingExtensions.RebindingOperation GetOperation()
        {
            return new InputActionRebindingExtensions.RebindingOperation()
                .OnApplyBinding(ApplyBinding)
                .WithRebindAddingNewBinding()
                .WithCancelingThrough("<Keyboard>/escape");
        }
        private void ApplyBinding(InputActionRebindingExtensions.RebindingOperation op, string path)
        {
            //not now, for transaction
        }

        private void Match(InputActionRebindingExtensions.RebindingOperation op)
        {
            var path = op.selectedControl.path;
            foreach (var control in _action.controls)
            {
                if (control.path == path)
                {
                    op.Cancel();
                }
            }
            op.Complete();
        }

        private void Complete(InputActionRebindingExtensions.RebindingOperation op)
        {
            var binding = _action.GetBindingForControl(op.selectedControl);
            if (binding != null)
            {
                _loader.Attach(binding.Value);
                Core.Global.GlobalData.GlobalInputActions.KeyMapModel.AddNewBinding(binding.Value);
            }
            else Debug.Log($"binding is null for {op.selectedControl.displayName}, canceling");

            op.Dispose();
            _status.SetActive(false);
            _image.color = _defaultColor;
        }
        private string ConvertToBindingPath(string ControlPath)
        {
            var path = ControlPath.Split('/', 3);
            return $"<{path[1]}>/{path[2]}";
        }

        private void Cancel(InputActionRebindingExtensions.RebindingOperation op)
        {
            Debug.Log("canceled : " + op.selectedControl.path);
            _status.SetActive(false);
            op.Dispose();
            _image.color = _defaultColor;
        }
    }
}
