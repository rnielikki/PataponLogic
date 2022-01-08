using UnityEngine;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InstructionWindow : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text _statusText;
        private bool _willBeHidden;
        public InstructionWindow Show()
        {
            if (!gameObject.activeSelf) _willBeHidden = false;
            gameObject.SetActive(true);
            return this;
        }
        public InstructionWindow Hide()
        {
            if (_willBeHidden) return this;
            _willBeHidden = true;
            gameObject.SetActive(false);
            return this;
        }
        public InstructionWindow SetText(string text)
        {
            _statusText.text = text;
            return this;
        }
        public InstructionWindow AppendText(string text)
        {
            _statusText.text += "\n" + text;
            return this;
        }
        public InstructionWindow HideAfterTime(float seconds)
        {
            if (_willBeHidden) return this;
            StartCoroutine(HideAfterTimeCoroutine(seconds));
            return this;
        }
        private System.Collections.IEnumerator HideAfterTimeCoroutine(float seconds)
        {
            Show();
            _willBeHidden = true;
            Core.Global.GlobalData.GlobalInputActions.DisableNavigatingInput();
            yield return new WaitForSeconds(seconds);
            Core.Global.GlobalData.GlobalInputActions.EnableNavigatingInput();
            _willBeHidden = false;
            Hide();
        }
    }
}
