using PataRoad.Editor;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossSummonManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _bossObject;
        private bool _summoned;

        [SerializeField, Layer]
        private int _bossLayer;
        [SerializeField, Layer]
        private int _bossLayerNoRaycast;
        [SerializeField, Layer]
        private int _bossWeaponLayer;
        [SerializeField, Layer]
        private int _pataponLayer;
        [SerializeField, Layer]
        private int _pataponLayerNoRaycast;
        [SerializeField, Layer]
        private int _pataponWeaponLayer;
        private SummonedBoss _boss;
        private bool _dead;

        public void SendCommand(Rhythm.Command.RhythmCommandModel model)
        {
            if (_summoned && !_dead) _boss.Act(model);
        }
        public void MarkAsDead() => _dead = true;
        public void SummonBoss()
        {
            if (!_summoned)
            {
                _summoned = true;
                var boss = Instantiate(_bossObject, transform);
                boss.transform.position += CharacterEnvironment.Sight * Vector3.left;
                foreach (var child in boss.GetComponentsInChildren<Transform>(true))
                {
                    var childLayer = child.gameObject.layer;
                    if (childLayer == _bossLayer)
                    {
                        child.gameObject.layer = _pataponLayer;
                    }
                    else if (childLayer == _bossLayerNoRaycast)
                    {
                        child.gameObject.layer = _pataponLayerNoRaycast;
                    }
                    else if (childLayer == _bossWeaponLayer)
                    {
                        child.gameObject.layer = _pataponWeaponLayer;
                    }
                }
                foreach (var renderer in boss.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    renderer.sortingLayerName = "summoned";
                }
                boss.SetActive(true);
                _boss = boss.GetComponent<SummonedBoss>();
                _boss.SetManager(this);
            }
        }
    }
}
