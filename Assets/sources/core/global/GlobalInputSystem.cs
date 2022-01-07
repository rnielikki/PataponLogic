using PataRoad.Core.Global.Settings;
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
        private readonly InputAction[] _allNavigatableInput;
        private const string PrefKeyName = "KeyBindings";
        private const string PrefAddedKeyName = "KeyBindings-Added";
        /// <summary>
        /// Represents only added and overriden keys. Contains empty data by default.
        /// </summary>
        public KeymapSettingModel KeyMapModel { get; private set; }
        public InputAction OkAction { get; }
        public InputAction CancelAction { get; }
        internal GlobalInputSystem(PlayerInput input)
        {
            _input = input;
            Load();
            OkAction = _input.actions.FindAction("UI/Submit");
            CancelAction = _input.actions.FindAction("UI/Cancel");
            _defaultsToWaitNextInput = new InputAction[]{
                OkAction,
                CancelAction
            };
            _allNavigatableInput = new InputAction[]
            {
                _defaultsToWaitNextInput[0],
                _defaultsToWaitNextInput[1],
                _input.actions.FindAction("UI/Navigate")
            };
        }
        public void EnableOkCancelInputs() => EnableInputs(_defaultsToWaitNextInput);
        /// <summary>
        /// Enables input, including ok/cancel/navigate.
        /// </summary>
        public void EnableNavigatingInput() => EnableInputs(_allNavigatableInput);

        public void ResumeInput()
        {
            foreach (var input in _allNavigatableInput)
            {
                input.Enable();
            }
        }
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
        /// <summary>
        /// Disables input, including ok/cancel/navigate.
        /// </summary>
        public void DisableNavigatingInput() => DisableInputs(_allNavigatableInput);

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
                if ((bindingMask?.Matches(binding) ?? true) && binding.isPartOfComposite && !bindingNames.ContainsKey(binding.name))
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
        public void Load()
        {
            var raw = PlayerPrefs.GetString(PrefKeyName);
            if (!string.IsNullOrEmpty(raw))
            {
                _input.actions.LoadBindingOverridesFromJson(raw);
            }
            var rawAdded = PlayerPrefs.GetString(PrefAddedKeyName);
            if (!string.IsNullOrEmpty(rawAdded))
            {
                KeyMapModel = JsonUtility.FromJson<KeymapSettingModel>(rawAdded);
            }
            else KeyMapModel = new KeymapSettingModel();

            foreach (var binding in KeyMapModel.NewBindings)
            {
                var action = _input.actions.FindAction(binding.action);
                action.AddBinding(binding);
            }
        }
        public void Save()
        {
            PlayerPrefs.SetString(
                PrefKeyName,
                _input.actions.SaveBindingOverridesAsJson()
            );
            PlayerPrefs.SetString(
                PrefAddedKeyName,
                JsonUtility.ToJson(KeyMapModel)
            );
        }
    }
}
