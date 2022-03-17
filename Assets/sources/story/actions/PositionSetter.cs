namespace PataRoad.Story.Actions
{
    public class PositionSetter : UnityEngine.MonoBehaviour
    {
        [UnityEngine.SerializeField]
        float _x;
        [UnityEngine.SerializeField]
        float _y;
        public void SetX(float x) => _x = x;
        public void SetY(float y) => _y = y;
        public void SetAsPosition()
        {
            transform.position =
                new UnityEngine.Vector3(_x, _y, transform.position.z);
        }
        public void SetAsLocalPosition()
        {
            transform.localPosition =
                new UnityEngine.Vector3(_x, _y, transform.position.z);
        }
    }
}