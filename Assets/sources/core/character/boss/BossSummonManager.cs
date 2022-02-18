using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossSummonManager : MonoBehaviour
    {
        private const string _path = "Characters/Bosses/BossData/";
        private GameObject _resource;
        private SummonedBoss _boss;

        private bool _dead;
        public bool HasBoss { get; private set; }

        private int _summonCount;
        [SerializeField]
        private Transform _summonStatus;
        [SerializeField]
        private GameObject _summonThumbnail;

        public void Init(int dataIndex, int summonCount)
        {
            var item = Items.ItemLoader.GetItem<Items.StringKeyItemData>(Items.ItemType.Key, "Boss", dataIndex);
            if (item == null || summonCount <= 0)
            {
                InitZero();
                return;
            }
            _resource = Resources.Load<GameObject>(_path + item.Data + "/Summon");
            if (_resource == null)
            {
                InitZero();
            }
            else
            {
                HasBoss = true;
                for (int i = 0; i < summonCount; i++)
                {
                    var obj = Instantiate(_summonThumbnail, _summonStatus);
                    obj.GetComponent<UnityEngine.UI.Image>().sprite = item.Image;
                }
                _summonCount = summonCount;
                _dead = true;
            }
        }
        public void InitZero()
        {
            _summonCount = 0;
            _dead = true;
        }
        public void SendCommand(Rhythm.Command.RhythmCommandModel model)
        {
            if (!_dead) _boss.Act(model);
        }
        public void CancelAttack()
        {
            _boss?.BossAttackData?.StopAllAttacking();
        }
        public void MarkAsDead()
        {
            _dead = true;
            _boss = null;
        }
        public void SummonBoss()
        {
            if (_summonCount > 0)
            {
                bool reborn = !_dead && _boss != null;
                if (reborn) _boss.Die(true);
                _summonCount--;
                var boss = Instantiate(_resource, transform);
                boss.transform.position += CharacterEnvironment.OriginalSight * Vector3.left;

                _boss = boss.GetComponent<SummonedBoss>();
                _boss.Init(reborn);
                boss.SetActive(true);
                _boss.SetManager(this);
                Destroy(_summonStatus.GetChild(0).gameObject);
                _dead = false;
            }
        }
    }
}
