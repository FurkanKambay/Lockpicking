using UnityEngine;

namespace Lockpicking
{
    public class PickMover : MonoBehaviour
    {
        [SerializeField] private float horizontalSensitivity;
        [SerializeField] private float rotationSensitivity;
        [SerializeField] private float minXPosition;
        [SerializeField] private float maxXPosition;
        [SerializeField] private float minHeadAngle;
        [SerializeField] private float maxHeadAngle;

        private new Rigidbody rigidbody;

        private void Awake() => rigidbody = GetComponent<Rigidbody>();
        private void Start() => Cursor.lockState = CursorLockMode.Locked;

        private void FixedUpdate()
        {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");

            Vector3 position = transform.position;
            float targetX = position.x + (deltaX * horizontalSensitivity * Time.fixedDeltaTime);
            float actualX = Mathf.Clamp(targetX, minXPosition, maxXPosition);
            rigidbody.MovePosition(new Vector3(actualX, position.y, position.z));

            float rotateAmount = deltaY * rotationSensitivity * Time.fixedDeltaTime;
            Quaternion requestedAngle = rigidbody.rotation * Quaternion.Euler(Vector3.forward * rotateAmount);

            if (requestedAngle.eulerAngles.z > minHeadAngle && requestedAngle.eulerAngles.z < maxHeadAngle)
                rigidbody.MoveRotation(requestedAngle);
        }
    }
}
