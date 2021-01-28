using System;
using System.Collections.Generic;
using System.Linq;
using Lockpicking.Helpers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Lockpicking
{
    public class TumblerLock : MonoBehaviour
    {
        public float ShearLine;
        public float ShearLineTolerance;

        [SerializeField] private Text textTorque;

        public List<PinPair> PinPairs { get; private set; }
        public int PinCount => PinPairs.Count;
        public PinPair CurrentBindingPin => PinPairs[BindingProgression];

        public int AppliedTorque
        {
            get => appliedTorque;
            private set => textTorque.text = $"Torque: {appliedTorque = value}";
        }

        public int BindingProgression
        {
            get => bindingProgression;
            private set
            {
                bindingProgression = value;
                IsPicked = value >= PinCount;
            }
        }

        public bool IsPicked
        {
            get => isPicked;
            private set
            {
                isPicked = value;
                if (value) Debug.Log("Lock is picked!");
            }
        }

        private int appliedTorque;
        private int bindingProgression;
        private bool isPicked;

        private void OnEnable()
        {
            PinPair[] pairs = transform.GetComponentsInChildren<PinPair>();
            IList<int> randomTorques = Enumerable.Range(1, 10).ToList().Shuffle();

            for (int i = 0; i < pairs.Length; i++)
            {
                PinPair pinPair = pairs[i];
                pinPair.RequiredTorque = randomTorques[i];
                pinPair.KeyPinLength = 0.5f + (Random.Range(1, 7) / 10f);
                pinPair.DriverPinLength = 1f + (Random.Range(1, 5) / 5f);
            }

            PinPairs = pairs.OrderBy(p => p.RequiredTorque).ToList();
            BindingProgression = 0;
            AppliedTorque = 0;
            IsPicked = false;
        }

        private void Update()
        {
            if (!IsPicked && Input.GetKeyDown(KeyCode.D))
                IncreaseTorque();
            else if (Input.GetKeyDown(KeyCode.A))
                DecreaseTorque();
        }

        private void IncreaseTorque()
        {
            int targetTorque = Math.Min(AppliedTorque + 1, 10);

            if (targetTorque < CurrentBindingPin.RequiredTorque)
                AppliedTorque = targetTorque;
            else if (targetTorque == CurrentBindingPin.RequiredTorque && CurrentBindingPin.State == PinState.Set)
            {
                CurrentBindingPin.Set();
                AppliedTorque = targetTorque;
                ++BindingProgression;
            }
        }

        private void DecreaseTorque()
        {
            AppliedTorque = Math.Max(AppliedTorque - 1, 0);

            for (int i = 0; i < PinCount; i++)
            {
                if (AppliedTorque < PinPairs[i].RequiredTorque && i < BindingProgression)
                {
                    PinPairs[i].Reset();
                    BindingProgression = Math.Max(BindingProgression - 1, 0);
                }
            }
        }
    }
}
