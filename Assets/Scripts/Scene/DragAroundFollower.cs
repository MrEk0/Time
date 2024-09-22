using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scene
{
    [RequireComponent(typeof(RectTransform))]
    public class DragAroundFollower : MonoBehaviour, IDragHandler, IEditListener, IConfirmListener
    {
        [SerializeField] private RectTransform _centerPoint;
        [SerializeField] private RectTransform _followTransform;

        private RectTransform _rect;
        private Vector3 _direction;
        private float _dragDistance;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _dragDistance = (_centerPoint.position - transform.position).magnitude;
        }
        
        public void Edit()
        {
            transform.gameObject.SetActive(true);
        }

        public void Confirm()
        {
            transform.gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(_rect, eventData.position, eventData.pressEventCamera, out var worldPoint))
                return;

            var position = _centerPoint.position;

            _direction = worldPoint - position;
            var clampedDirection = ClampToMagnitude(_direction, _dragDistance, _dragDistance);
            var newPosition = new Vector3(position.x + clampedDirection.x, position.y + clampedDirection.y, 0f);

            var angle = Vector2.SignedAngle(_rect.position - position, newPosition - position);

            var tr = _followTransform;
            var trRot = _followTransform.rotation.eulerAngles;
            tr.rotation = Quaternion.Euler(trRot.x, trRot.y, trRot.z + angle);
        }

        private Vector3 ClampToMagnitude(Vector3 vec, float min, float max)
        {
            return vec.normalized * Mathf.Clamp(vec.magnitude, min, max);
        }
    }
}