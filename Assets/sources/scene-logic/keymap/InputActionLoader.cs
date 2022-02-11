using PataRoad.Core.Global;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InputActionLoader : MonoBehaviour
    {
        [SerializeField]
        private InputBindingItem _template;
        [SerializeField]
        private RectTransform _attachTarget;
        [SerializeField]
        InputBindingAdder _adder;
        private InputAction _currentAction;

        [SerializeField]
        private UnityEngine.UI.Button _okButton;
        public UnityEngine.UI.Button Button => _okButton;
        [SerializeField]
        private UnityEngine.UI.Button _resetButton;

        [SerializeField]
        private InstructionWindow _instruction;
        [SerializeField]
        private Common.GameDisplay.ScrollList _scrollList;
        public Common.GameDisplay.ScrollList ScrollList => _scrollList;

        [SerializeField]
        private UnityEngine.UI.Selectable _firstSelect;
        [SerializeField]
        private AudioClip _soundOnLoaded;

        [SerializeField]
        private UnityEngine.UI.Toggle _resetToggle;

        public InstructionWindow Instruction => _instruction;
        private ActionToggleItem _actionToggle;
        public ActionToggleItem CurrentActionToggle => _actionToggle;
        private readonly List<InputBindingItem> _items = new List<InputBindingItem>();

        private int _contentCount;
        private bool _isScrollListInitialized;

        private void Start()
        {
            _adder.Init(this);
            _firstSelect.Select();
        }
        public void Load(InputAction action, ActionToggleItem currentItem)
        {
            _currentAction = action;
            _actionToggle = currentItem;
            _adder.SetListeningType(action, currentItem);
            GlobalData.Sound.PlayInScene(_soundOnLoaded);
            Refresh(true);
        }
        internal void Refresh(bool selectFirst)
        {
            _items.Clear();
            foreach (var child in _attachTarget.GetComponentsInChildren<InputBindingItem>())
            {
                Destroy(child.gameObject);
            }
            _scrollList.ForceRebuildLayout();
            _contentCount = 0;
            for (var i = 0; i < _currentAction.bindings.Count; i++)
            {
                var binding = _currentAction.bindings[i];
                if (!binding.isComposite && binding.path != null)
                {
                    var obj = Instantiate(_template, _attachTarget);
                    obj.Init(binding, _currentAction, this, _contentCount);
                    _contentCount++;
                    _items.Add(obj);
                }
            }
            if (!_isScrollListInitialized)
            {
                var first = _items[0];
                _scrollList.Init(first);
                _isScrollListInitialized = true;
            }

            var item = selectFirst ? _items[0] : _items[_items.Count - 1];
            _scrollList.SetMaximumScrollLength(_contentCount - 1, item);
            if (selectFirst) _scrollList.SetToZero();
            else _scrollList.SetToLastForIndexed();
        }
        internal void Attach(InputBinding newBinding)
        {
            var obj = Instantiate(_template, _attachTarget);
            obj.Init(newBinding, _currentAction, this, ++_contentCount);
            _items.Add(obj);
            _scrollList.SetMaximumScrollLength(++_contentCount, obj);
            obj.Selectable.Select();
        }
        internal void MarkAsDeleted(InputBindingItem item)
        {
            var index = _items.IndexOf(item);
            _items.Remove(item);
            for (int i = index; i < _items.Count; i++)
            {
                _items[i].MoveToUp();
            }
            _scrollList.SetMaximumScrollLength(--_contentCount, _items[index - 1]);
            _items[index - 1].Selectable.Select();
        }
        public void Save()
        {
            GlobalData.GlobalInputActions.Save();
            Common.GameDisplay.SceneLoadingAction.Create("Main").ChangeScene();
        }
        internal string ConvertToBindingPath(string ControlPath)
        {
            var path = ControlPath.Split('/', 3);
            return $"<{path[1]}>/{path[2]}";
        }
        public void ResetBindings()
        {
            GlobalData.GlobalInputActions.KeyMapModel.ClearAllBindings();

            if (_resetToggle.isOn) GlobalData.GlobalInputActions.LoadRightPreset();
            else GlobalData.GlobalInputActions.LoadLeftPreset();

            GlobalData.Input.actions.RemoveAllBindingOverrides();

            Refresh(true);
            _resetButton.interactable = false;
        }
    }
}
