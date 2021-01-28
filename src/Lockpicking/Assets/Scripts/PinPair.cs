using Lockpicking.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Lockpicking
{
    public class PinPair : MonoBehaviour
    {
        [SerializeField] private Text textStatus;

        public PinState State { get; private set; }
        internal int RequiredTorque;

        public float KeyPinLength
        {
            get => keyPinLength;
            set
            {
                keyPinLength = value;
                SetupPinLengths();
            }
        }

        public float DriverPinLength
        {
            get => driverPinLength;
            set
            {
                driverPinLength = value;
                SetupPinLengths();
            }
        }

        private Transform keyPin, driverPin;
        private Rigidbody keyPinRigidbody, driverPinRigidbody;
        private SpringJoint keyPinSpring, driverPinSpring;

        private TumblerLock tumblerLock;
        private float keyPinLength, driverPinLength;
        private float KeyPinPosition => keyPinRigidbody.position.y;

        private void Awake()
        {
            tumblerLock = GetComponentInParent<TumblerLock>();
            keyPin = transform.GetChild(0);
            driverPin = transform.GetChild(1);

            keyPinRigidbody = keyPin.GetComponent<Rigidbody>();
            driverPinRigidbody = driverPin.GetComponent<Rigidbody>();
            keyPinSpring = keyPin.GetComponent<SpringJoint>();
            driverPinSpring = driverPin.GetComponent<SpringJoint>();
        }

        private void FixedUpdate()
        {
            float shear = tumblerLock.ShearLine - (keyPinLength / 2f);

            if (Mathf.Abs(shear - KeyPinPosition) < tumblerLock.ShearLineTolerance)
            {
                State = PinState.Set;
                textStatus.text = "set";
            }
            else if (KeyPinPosition < shear)
            {
                State = PinState.Unset;
                textStatus.text = "unset";
            }
            else
            {
                State = PinState.Overset;
                textStatus.text = "overset";
            }
        }

        public void Set()
        {
            textStatus.color = Color.green;

            driverPinSpring.connectedBody = null;
            driverPin.position = driverPinRigidbody.position.With(y: tumblerLock.ShearLine + (driverPinLength / 2f));
        }

        public void Reset()
        {
            textStatus.color = Color.red;

            float driverY = KeyPinPosition + (keyPinLength / 2f) + (driverPinLength / 2f) + .04f;
            driverPin.position = driverPinRigidbody.position.With(y: driverY);
            driverPinSpring.connectedBody = keyPinRigidbody;
            driverPinSpring.connectedAnchor = Vector3.up * driverY;
        }

        private void SetupPinLengths()
        {
            keyPin.localScale = new Vector3(.6f, keyPinLength, .6f);
            driverPin.localScale = new Vector3(.6f, driverPinLength, .6f);

            float keyY = keyPinLength / 2f;
            keyPin.position = keyPinRigidbody.position.With(y: keyY);
            keyPinSpring.connectedAnchor = keyPinSpring.connectedAnchor.With(y: keyY);

            Reset();
        }
    }

    public enum PinState
    {
        Unset,
        Set,
        Overset
    }
}
