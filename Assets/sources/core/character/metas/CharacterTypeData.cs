using UnityEngine;

namespace PataRoad.Core.Character
{
    internal class CharacterTypeData
    {
        internal CharacterType Type { get; }
        internal LayerMask RaycastLayerMask { get; private set; }
        internal LayerMask AttackTargetLayerMask { get; private set; }
        internal int AttackLayer { get; }
        internal LayerMask SelfLayerMask { get; }
        internal LayerMask SelfLayerMaskNoRayCast { get; }
        internal LayerMask SelfLayerMaskRayCast { get; }
        internal CharacterTypeData(CharacterType type)
        {
            Type = type;
            switch (type)
            {
                case CharacterType.Patapon:
                    RaycastLayerMask = LayerMask.GetMask("structures", "hazorons", "bosses");
                    AttackTargetLayerMask = RaycastLayerMask | LayerMask.GetMask("hazoron.noraycast", "boss.noraycast");
                    AttackLayer = LayerMask.NameToLayer("patapon.attack");
                    SelfLayerMask = LayerMask.GetMask("patapons", "patapon.noraycast");
                    SelfLayerMaskNoRayCast = LayerMask.NameToLayer("patapon.noraycast");
                    SelfLayerMaskRayCast = LayerMask.NameToLayer("patapons");
                    break;
                case CharacterType.Hazoron:
                    RaycastLayerMask = LayerMask.GetMask("patapons", "bosses");
                    AttackTargetLayerMask = RaycastLayerMask | LayerMask.GetMask("patapon.noraycast", "boss.noraycast");
                    AttackLayer = LayerMask.NameToLayer("hazoron.attack");
                    SelfLayerMask = LayerMask.GetMask("hazorons", "hazoron.noraycast");
                    SelfLayerMaskNoRayCast = LayerMask.NameToLayer("hazoron.noraycast");
                    SelfLayerMaskRayCast = LayerMask.NameToLayer("hazorons");
                    break;
                case CharacterType.Others:
                    RaycastLayerMask = LayerMask.GetMask("patapons", "hazorons");
                    AttackTargetLayerMask = RaycastLayerMask | LayerMask.GetMask("patapon.noraycast", "hazoron.noraycast");
                    AttackLayer = LayerMask.NameToLayer("boss.attack");
                    SelfLayerMask = LayerMask.GetMask("bossees", "bosses.noraycast");
                    SelfLayerMaskNoRayCast = LayerMask.NameToLayer("boss.noraycast");
                    SelfLayerMaskRayCast = LayerMask.NameToLayer("bosses");
                    break;
            }
        }
    }
}
