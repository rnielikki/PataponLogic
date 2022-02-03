using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    [System.Serializable]
    public class MinigameTurn
    {
        [SerializeField]
        private MinigameDrumType[] _drums;
        public MinigameDrumType[] Drums => _drums;

        [SerializeField]
        [Tooltip("If you use donchaka sound, you can skip this")]
        private AudioClip _sound;
        public AudioClip Sound => _sound;
        public MinigameTurn GenerateRandomDrums()
        {
            var drums = new MinigameDrumType[_drums.Length];
            for (int i = 0; i < _drums.Length; i++)
            {
                drums[i] = _drums[i] == MinigameDrumType.Empty ? MinigameDrumType.Empty : GetRandomDrum();
            }
            var turn = new MinigameTurn();
            turn._drums = drums;
            return turn;
            MinigameDrumType GetRandomDrum()
            {
                var range = UnityEngine.Random.Range(0f, 1f);
                if (range < 0.25f) return MinigameDrumType.Pata;
                else if (range < 0.65f) return MinigameDrumType.Pon;
                else if (range < 0.85f) return MinigameDrumType.Chaka;
                else return MinigameDrumType.Don;
            }
        }
        public static MinigameTurn[] SwapDrum(MinigameTurn[] turns)
        {
            var newDrums = RemapDrum();
            var newTurns = new MinigameTurn[turns.Length];
            for (int i = 0; i < turns.Length; i++)
            {
                newTurns[i] = turns[i].ReplaceDrums(newDrums);
            }
            return newTurns;
            MinigameDrumType[] RemapDrum()
            {
                var newDrumMap = new MinigameDrumType[5]
                {
                    MinigameDrumType.Pata,
                    MinigameDrumType.Pon,
                    MinigameDrumType.Chaka,
                    MinigameDrumType.Don,
                    MinigameDrumType.Empty
                };
                for (int i = 0; i < 4; i++)
                {
                    int j = Random.Range(0, 3);
                    var temp = newDrumMap[j];
                    newDrumMap[j] = newDrumMap[i];
                    newDrumMap[i] = temp;
                }
                return newDrumMap;
            }
        }
        private MinigameTurn ReplaceDrums(MinigameDrumType[] map)
        {
            var newDrums = new MinigameDrumType[_drums.Length];
            for (int i = 0; i < _drums.Length; i++)
            {
                var oldDrum = _drums[i];
                newDrums[i] = _drums[i] == MinigameDrumType.Empty ? MinigameDrumType.Empty : map[(int)oldDrum];
            }
            var turn = new MinigameTurn();
            turn._drums = newDrums;
            return turn;
        }
    }
}
