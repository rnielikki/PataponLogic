namespace PataRoad.Story.Actions
{
    class StoryCameraMover : UnityEngine.MonoBehaviour
    {
        private SceneLogic.Patapolis.CameraSmoothMover _cameraMover;
        private void Init()
        {
            _cameraMover = UnityEngine.Camera.main.GetComponent<SceneLogic.Patapolis.CameraSmoothMover>();
            if (_cameraMover == null)
            {
                _cameraMover = UnityEngine.Camera.main.gameObject
                    .AddComponent<SceneLogic.Patapolis.CameraSmoothMover>();
            }
        }
        public void MoveTo(float xPosition)
        {
            if (_cameraMover == null) Init();
            _cameraMover.MoveTo(xPosition);
        }
    }
}
