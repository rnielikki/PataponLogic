namespace PataRoad.Core.Character.Animal
{
    /// <summary>
    /// Referenced by <see cref="AnimalBehaviour"/>.
    /// </summary>
    interface IAnimalData
    {
        /// <summary>
        /// Attack type of the animal.
        /// </summary>
        public Equipments.Weapons.AttackType AttackType { get; }
        /// <summary>
        /// Default sight on clear weather.
        /// </summary>
        public float Sight { get; }

        public Stat Stat { get; }

        /// <summary>
        /// Lock <see cref="OnTarget()"/> action by setting this to <c>true</c>.
        /// </summary>
        public bool PerformingAction { get; }
        public void OnDamaged();
        public void OnTarget();
        public void InitFromParent(AnimalBehaviour parent);
        public void StopAttacking();
        public bool CanMove();
    }
}
