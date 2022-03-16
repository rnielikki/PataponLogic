using PataRoad.Core.Character.Hazorons;

/// <summary>
/// Hazorons but being damaged only with specific Status effect.
/// </summary>
namespace PataRoad.Core.Map.Levels
{
    public class StatusOnlyHazoron : Hazoron
    {
        [UnityEngine.SerializeField]
        Character.StatusEffectType _effectTypeToDamage;
        [UnityEngine.SerializeField]
        private Character.CharacterSoundsCollection _sounds;
        public override Character.CharacterSoundsCollection Sounds => _sounds;
        public override bool TakeDamage(int damage)
        {
            if (StatusEffectManager.CurrentStatusEffect == _effectTypeToDamage)
            {
                base.TakeDamage(damage);
                return true;
            }
            else return false;
        }
    }
}