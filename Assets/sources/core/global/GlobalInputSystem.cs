using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Global
{
    public class GlobalInputSystem
    {
        private readonly PlayerInput _input;
        private readonly InputAction[] _defaultsToWaitNextInput;
        internal GlobalInputSystem(PlayerInput input)
        {
            _input = input;
            _defaultsToWaitNextInput = new InputAction[]{
                _input.actions.FindAction("UI/Submit"),
                _input.actions.FindAction("UI/Cancel")
            };
        }
        public void EnableOkCancelInputs() => EnableInputs(_defaultsToWaitNextInput);

        public void EnableAllInputs()
        {
            foreach (var map in _input.actions.actionMaps)
            {
                EnableInputs(map);
            }
        }

        public void EnableInputs(IEnumerable<InputAction> maps)
        {
            foreach (var input in maps)
            {
                input.Enable();
            }
        }

        public void DisableOkCancelInputs() => DisableInputs(_defaultsToWaitNextInput);

        public void DisableAllInputs()
        {
            foreach (var map in _input.actions.actionMaps)
            {
                DisableInputs(map);
            }
        }

        public void DisableInputs(IEnumerable<InputAction> maps)
        {
            foreach (var input in maps)
            {
                input.Disable();
            }
        }
        public bool TryGetActionBindingName(string actionName, out string name)
        {
            //Note: To make GetBindingDisplayString() work, you MUST select CORRECT BINDING TYPE (keyboard, gamepad...) in input system.
            var actionBindingName = _input.actions.FindAction(actionName)?.GetBindingDisplayString();
            name = actionBindingName;
            return !string.IsNullOrEmpty(actionBindingName);
        }
        public bool TryGetAllActionBindingNames(string actionName,
            out Dictionary<string, string> bindingNames)
        {
            var action = _input.actions.FindAction(actionName);
            bindingNames = new Dictionary<string, string>();
            if (action == null) return false;
            var bindingMask = GetInputBindingMask();

            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if ((bindingMask?.Matches(binding) ?? false) && binding.isPartOfComposite && !bindingNames.ContainsKey(binding.name))
                {
                    bindingNames.Add(binding.name, action.GetBindingDisplayString(i));
                }
            }
            return bindingNames.Count != 0;

            //from InputAction.FindEffectiveBindingMask
            InputBinding? GetInputBindingMask()
            {
                if (action.bindingMask != null) return action.bindingMask;
                else if (action.actionMap.bindingMask != null) return action.actionMap.bindingMask;
                else return action.actionMap.asset.bindingMask;
            }
        }
        /// <summary>
        /// Prevents propagation of the input.
        /// </summary>
        /// <param name="callback">Callback, Expected to ADDING INPUT EVENT</param>
        /// <param name="inputNames">Custom input names. default is "UI/Submit" and "UI/Cancel".</param>
        /// <returns>Coroutine for wating update, depends on if it's fixed update or frame update.</returns>
        public IEnumerator WaitForNextInput(UnityEngine.Events.UnityAction callback, string[] inputNames = null)
        {
            IEnumerable<InputAction> inputs = _defaultsToWaitNextInput;
            if (inputNames != null)
            {
                inputs = inputNames.Select(inp => _input.actions.FindAction(inp));
            }
            foreach (var input in inputs)
            {
                input.Disable();
            }

            //without waiting for next, it just continues... :/
            if (InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsInDynamicUpdate)
            {
                yield return new WaitForEndOfFrame();
            }
            else if (InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsInFixedUpdate)
            {
                yield return new WaitForFixedUpdate();
            }
            callback();

            foreach (var input in inputs)
            {
                input.Enable();
            }
        }
    }
}
