using UnityEngine;

namespace PataRoad.Core.Items
{
    public abstract class ItemDropData : ScriptableObject
    {
        [SerializeField]
        protected int _timeToExist;
        public int TimeToExist => _timeToExist;
        [SerializeField]
        private bool _infiniteExistingTime;
        public bool DoNotDestroy => _infiniteExistingTime;
        [SerializeField]
        [Tooltip("0-1 probability value. only works with DropRandom()")]
        protected float _chanceToDrop;
        public float ChanceToDrop => _chanceToDrop;
        [SerializeField]
        UnityEngine.Events.UnityEvent _events;
        public virtual UnityEngine.Events.UnityEvent Events => _events;

        [SerializeField]
        protected AudioClip _sound;
        public AudioClip Sound => _sound;
    }
}
