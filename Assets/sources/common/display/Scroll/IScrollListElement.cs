namespace PataRoad.Common.GameDisplay
{
    public interface IScrollListElement : UnityEngine.EventSystems.ISelectHandler
    {
        UnityEngine.UI.Selectable Selectable { get; }
        UnityEngine.RectTransform RectTransform { get; }
        int Index { get; }
    }
}
