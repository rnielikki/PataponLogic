using UnityEngine;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class InstructionWindow : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text _statusText;
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
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
        public void HideAfterTime(float seconds)
            => StartCoroutine(HideAfterTimeCoroutine(seconds));
        private System.Collections.IEnumerator HideAfterTimeCoroutine(float seconds)
        {
            Show();
            Core.Global.GlobalData.GlobalInputActions.DisableNavigatingInput();
            yield return new WaitForSeconds(seconds);
            Hide();
            Core.Global.GlobalData.GlobalInputActions.EnableNavigatingInput();
        }
    }
}
