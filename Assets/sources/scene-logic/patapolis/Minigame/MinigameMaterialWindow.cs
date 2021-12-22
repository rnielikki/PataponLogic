using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class MinigameMaterialWindow : MonoBehaviour
    {
        MinigameSelectionWindow _parent;
        [SerializeField]
        GameObject _template;
        [SerializeField]
        Transform _contentTarget;
        [SerializeField]
        UnityEngine.UI.VerticalLayoutGroup _childTargetToRefresh;
        [SerializeField]
        UnityEngine.UI.VerticalLayoutGroup _targetToRefresh;
        private MinigameSelectionButton _lastSelection;
        [SerializeField]
        private UnityEngine.UI.Button _okButton;
        [SerializeField]
        Common.Navigator.HorizontalNavigationGroup _navGroup;
        MaterialLoader[] _materialLoaders;

        public void Open(MinigameSelectionWindow parent, MinigameSelectionButton model)
        {
            _lastSelection = model;
            _parent = parent;
            _parent.gameObject.SetActive(false);
            gameObject.SetActive(true);
            StartCoroutine(
            Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                foreach (var group in model.MaterialGroups)
                {
                    var obj = Instantiate(_template, _contentTarget);
                    obj.GetComponent<MaterialLoader>().Init(group, this);
                }
                //Canvas update ------------------
                Canvas.ForceUpdateCanvases();
                _childTargetToRefresh.enabled = false;
                _targetToRefresh.enabled = false;
                _childTargetToRefresh.enabled = true;
                _targetToRefresh.enabled = true;

                _navGroup.LateInit();
                _materialLoaders = _contentTarget.GetComponentsInChildren<MaterialLoader>();
                foreach (var materialLoader in _materialLoaders)
                {
                    materialLoader.LateInit();
                }
                GetComponentsInChildren<UnityEngine.UI.Selectable>().First(item => item.enabled).Select();
            }));
        }
        public void Close(bool ok = false)
        {
            _parent.gameObject.SetActive(true);
            gameObject.SetActive(false);
            foreach (Transform child in _contentTarget)
            {
                Destroy(child.gameObject);
            }
            if (!ok)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_lastSelection.gameObject);
            }
        }
        //lol nested so much, I don't usually do this but
        internal void RemoveOne(MaterialLoader loader, CommonSceneLogic.ItemDisplay itemDisplay)
        {
            foreach (var materialLoader in _materialLoaders)
            {
                if (materialLoader.Group == loader.Group)
                {
                    foreach (var display in materialLoader.AllDisplays)
                    {
                        if (display.Item == itemDisplay.Item)
                        {
                            display.UpdateText(display.Amount - 1);
                            if (display.Amount == 0)
                            {
                                display.MarkAsDisable();
                                if (display == itemDisplay) //select something else
                                {
                                    var target = loader.GetNextSelectionTarget(itemDisplay);
                                    foreach (var matLoader in _materialLoaders)
                                    {
                                        target = matLoader.GetNextSelectionTarget();
                                        if (target != null)
                                        {
                                            target.Selectable.Select();
                                            return;
                                        }
                                    }
                                    _okButton.Select();
                                }
                            }
                        }
                    }
                }
            }
        }
        internal void RestoreOne(MaterialLoader loader, CommonSceneLogic.ItemDisplay itemDisplay)
        {
            foreach (var materialLoader in _materialLoaders)
            {
                if (materialLoader.Group == loader.Group)
                {
                    foreach (var display in materialLoader.AllDisplays)
                    {
                        if (display.Item == itemDisplay.Item)
                        {
                            if (display.Amount == 0) display.MarkAsEnable();
                            display.UpdateText(display.Amount + 1);
                        }
                    }
                }
            }
        }
        public void LoadGame()
        {
            // UI/Start to start.
        }
    }
}
