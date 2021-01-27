using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lockpicking
{
    public class PinPair : MonoBehaviour
    {
        [SerializeField] private float keyPinLength;
        public int PinRadius;
        public int DriverPinLength = 2;
        [SerializeField] private Text textStatus;

        public float PinPosition => rigidbody.position.y;

        public float KeyPinLength
        {
            get => keyPinLength;
            set
            {
                keyPinLength = value;

                Vector3 position = rigidbody.position;
                Vector3 anchor = spring.connectedAnchor;

                cube.localScale = new Vector3(1f, value, 1f);
                collider.size = cube.localScale;
                transform.position = new Vector3(position.x, value / 2f, position.z);
                spring.connectedAnchor = new Vector3(anchor.x, value / 2f, anchor.z);
            }
        }

        private new Rigidbody rigidbody;
        private new BoxCollider collider;
        private SpringJoint spring;
        private Transform cube;
        private TumblerLock tumblerLock;

        private void Awake()
        {
            rigidbody = transform.GetComponent<Rigidbody>();
            cube = transform.GetChild(0);
            tumblerLock = GetComponentInParent<TumblerLock>();
            spring = rigidbody.GetComponent<SpringJoint>();
            collider = transform.GetComponent<BoxCollider>();
        }

        private void FixedUpdate()
        {
            float shear = (tumblerLock.ShearLine - (keyPinLength / 2f));

            if (Mathf.Abs(shear - PinPosition) < tumblerLock.ShearLineTolerance)
                textStatus.text = "set";
            else if (PinPosition < shear)
                textStatus.text = "unset";
            else
                textStatus.text = "overset";
        }
    }
}
