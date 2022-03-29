namespace PataRoad.SceneLogic.Patapolis
{
    public interface IMaterialSelectionButton
    {
        public string[] MaterialGroups { get; }
        public UnityEngine.UI.Button Button { get; }
    }
}