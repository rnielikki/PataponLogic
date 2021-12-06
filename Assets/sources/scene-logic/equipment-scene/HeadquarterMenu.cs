namespace PataRoad.SceneLogic.EquipmentScene
{
    public class HeadquarterMenu : SummaryMenu<SummaryElement>
    {
        private void Awake()
        {
            Init();
            _activeNavs = GetComponentsInChildren<SummaryElement>(true);
        }
        private void OnEnable()
        {
            MarkIndex(_index);
            _actionEvent.enabled = true;
        }
    }
}
