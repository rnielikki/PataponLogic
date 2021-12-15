namespace PataRoad.Common.Navigator
{
    /// <summary>
    /// Has one-many relationship and <see cref="ICustomNavigationGroup"/> is many.
    /// </summary>
    public interface ICustomNavigator
    {
        /// <summary>
        /// Called when (expected "after") first initialization of all navigators.
        /// </summary>
        public void InitializeSelection();
        /// <summary>
        /// Move on navigator input.
        /// </summary>
        /// <param name="sender">Customizable (default GameObject) sender from< <see cref="ActionEventMap"/>./param>
        /// <param name="context">Input System Context that contains navigation value.</param>
        public void Move(UnityEngine.Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context);
    }
}
