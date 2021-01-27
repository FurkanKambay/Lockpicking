using UnityEngine;
using UnityEngine.UI;

namespace Lockpicking
{
    public class PinPair : MonoBehaviour
    {
        [SerializeField] private float keyPinLength, driverPinLength;
        [SerializeField] private Text textStatus;

        [HideInInspector] public int PinRadius;
        public float PinPosition => keyPinRigidbody.position.y;

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
        private PinStatus pinStatus;

        private void Awake()
        {
            tumblerLock = GetComponentInParent<TumblerLock>();
            keyPin = transform.GetChild(0).GetChild(0);
            driverPin = transform.GetChild(1).GetChild(0);

            keyPinRigidbody = keyPin.GetComponent<Rigidbody>();
            driverPinRigidbody = driverPin.GetComponent<Rigidbody>();
            keyPinSpring = keyPin.GetComponent<SpringJoint>();
            driverPinSpring = driverPin.GetComponent<SpringJoint>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (pinStatus == PinStatus.Set && this == tumblerLock.CurrentBindingPin)
                {
                    driverPinSpring.connectedBody = null;
                    textStatus.color = Color.green;
                    tumblerLock.SetNextPin();
                }
            }
        }

        private void FixedUpdate()
        {
            float shear = tumblerLock.ShearLine - (keyPinLength / 2f);

            if (Mathf.Abs(shear - PinPosition) < tumblerLock.ShearLineTolerance)
            {
                pinStatus = PinStatus.Set;
                textStatus.text = "set";
            }
            else if (PinPosition < shear)
            {
                pinStatus = PinStatus.Unset;
                textStatus.text = "unset";
            }
            else
            {
                pinStatus = PinStatus.Overset;
                textStatus.text = "overset";
            }
        }

        private void SetupPinLengths()
        {
            Vector3 keyPinPosition = keyPinRigidbody.position;
            Vector3 driverPinPosition = driverPinRigidbody.position;
            Vector3 anchor = keyPinSpring.connectedAnchor;

            float keyY = keyPinLength / 2f;
            float driverY = keyPinLength + ((driverPinLength * .5f) + .05f);

            keyPin.localScale = new Vector3(1f, keyPinLength, 1f);
            keyPin.position = new Vector3(keyPinPosition.x, keyY, keyPinPosition.z);
            keyPinSpring.connectedAnchor = new Vector3(anchor.x, keyY, anchor.z);

            driverPin.localScale = new Vector3(1f, driverPinLength, 1f);
            driverPin.position = new Vector3(driverPinPosition.x, driverY, driverPinPosition.z);
            driverPinSpring.connectedAnchor = new Vector3(anchor.x, driverY, anchor.z);

            driverPinSpring.connectedBody = keyPinRigidbody;
            textStatus.color = Color.red;
        }

        enum PinStatus
        {
            Unset,
            Set,
            Overset
        }
    }
}
