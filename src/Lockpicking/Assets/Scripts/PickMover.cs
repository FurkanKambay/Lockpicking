using Lockpicking.Helpers;
using UnityEngine;

namespace Lockpicking
{
    public class PickMover : MonoBehaviour
    {
        [SerializeField] private float horizontalSensitivity = 1f;
        [SerializeField] private float rotationSensitivity = 1f;
        [SerializeField] private float minXPosition = -20f;
        [SerializeField] private float maxXPosition = 20f;
        [SerializeField] private float minHeadAngle = 45f;
        [SerializeField] private float maxHeadAngle = 180f;

        private new Rigidbody rigidbody;
        private float deltaX, deltaY;

        private void Awake() => rigidbody = GetComponent<Rigidbody>();
        private void Start() => Cursor.lockState = CursorLockMode.Locked;

        private void Update()
        {
            deltaX = Input.GetAxis("Mouse X");
            deltaY = Input.GetAxis("Mouse Y");
        }

        private void FixedUpdate()
        {
            Vector3 position = transform.position;
            float targetX = position.x + (deltaX * horizontalSensitivity * Time.fixedDeltaTime);
            float actualX = Mathf.Clamp(targetX, minXPosition, maxXPosition);
            rigidbody.MovePosition(position.With(x: actualX));

            float rotateAmount = deltaY * rotationSensitivity * Time.fixedDeltaTime;
            Quaternion requestedAngle = rigidbody.rotation * Quaternion.Euler(Vector3.forward * rotateAmount);

            if (requestedAngle.eulerAngles.z > minHeadAngle && requestedAngle.eulerAngles.z < maxHeadAngle)
                rigidbody.MoveRotation(requestedAngle);
        }
    }
}
