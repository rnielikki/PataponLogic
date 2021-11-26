using PataRoad.Editor;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossSummonManager : MonoBehaviour
    {
        private const string _path = "Characters/Bosses/BossData/";
        private GameObject _resource;
        [SerializeField]
        private int _dataIndex;
        private SummonedBoss _boss;

        [SerializeField]
        private int _summonCount;
        private bool _dead;

        [SerializeField]
        private Transform _summonStatus;
        [SerializeField]
        private GameObject _summonThumbnail;

        private void Awake()
        {
            var item = Items.ItemLoader.GetItem<Items.StringKeyItemData>(Items.ItemType.Key, "Boss", _dataIndex);
            if (item == null || _summonCount <= 0)
            {
                _summonCount = 0;
                _dead = true;
                return;
            }
            _resource = Resources.Load<GameObject>(_path + item.Name + "/Summon");
            if (_resource == null)
            {
                _summonCount = 0;
            }
            else
            {
                for (int i = 0; i < _summonCount; i++)
                {
                    var obj = Instantiate(_summonThumbnail, _summonStatus);
                    obj.GetComponent<UnityEngine.UI.Image>().sprite = item.Image;
                }
            }
            _dead = true;
        }
        public void SendCommand(Rhythm.Command.RhythmCommandModel model)
        {
            if (!_dead) _boss.Act(model);
        }
        public void MarkAsDead() => _dead = true;
        public void SummonBoss()
        {
            if (_summonCount > 0 && _dead)
            {
                _summonCount--;
                var boss = Instantiate(_resource, transform);
                boss.transform.position += CharacterEnvironment.Sight * Vector3.left;
                boss.SetActive(true);
                _boss = boss.GetComponent<SummonedBoss>();
                _boss.SetManager(this);
                Destroy(_summonStatus.GetChild(0).gameObject);
                _dead = false;
            }
        }
    }
}
