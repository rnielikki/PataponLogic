using System.Collections.Generic;

namespace PataRoad.Core.Character
{
    internal static class CharacterTypeDataCollection
    {
        private static readonly Dictionary<CharacterType, CharacterTypeData> _indexes =
            new Dictionary<CharacterType, CharacterTypeData>()
            {
                { CharacterType.Patapon, new CharacterTypeData(CharacterType.Patapon) },
                { CharacterType.Hazoron, new CharacterTypeData(CharacterType.Hazoron) },
                { CharacterType.Others, new CharacterTypeData(CharacterType.Others) },
            };
        internal static CharacterTypeData GetCharacterData(CharacterType type) => _indexes[type];
        internal static CharacterTypeData GetCharacterDataByType(ICharacter character) => character switch
        {
            Patapons.Patapon => _indexes[CharacterType.Patapon],
            Hazorons.Hazoron => _indexes[CharacterType.Hazoron],
            _ => _indexes[CharacterType.Others]
        };
    }
}
