using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lockpicking
{
    public class TumblerLock : MonoBehaviour
    {
        public float ShearLine;
        public float ShearLineTolerance;

        public List<PinPair> PinPairs => pinPairs;
        public int PinCount => pinPairs.Count;
        public PinPair CurrentBindingPin => pinPairs[bindingOrder[currentBindingPin]];
        public bool IsPicked { get; private set; }

        private List<PinPair> pinPairs;
        private int[] bindingOrder;
        private int currentBindingPin;

        private void OnEnable()
        {
            pinPairs = transform.GetComponentsInChildren<PinPair>().ToList();

            foreach (PinPair t in pinPairs)
            {
                t.PinRadius = GenerateUnseenRandom();
                t.KeyPinLength = 0.5f + (Random.Range(1, 10) / 10f);
                t.DriverPinLength = 1f + (Random.Range(1, 5) / 5f);
            }

            bindingOrder = pinPairs.OrderByDescending(p => p.PinRadius)
                    .Select(p => pinPairs.IndexOf(p))
                    .ToArray();
        }

        public void SetNextPin()
        {
            if (IsPicked) return;

            if (currentBindingPin + 1 >= PinCount)
            {
                IsPicked = true;
                Debug.Log("Lock is picked!");
                return;
            }

            ++currentBindingPin;
        }

        private int GenerateUnseenRandom()
        {
            int number = 0;

            while (pinPairs.Any(p => p.PinRadius == number))
                number = Random.Range(1, 10);

            return number;
        }
    }
}
