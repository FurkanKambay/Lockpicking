using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lockpicking
{
    public class TumblerLock : MonoBehaviour
    {
        public float ShearLine;
        public float ShearLineTolerance;

        public int PinCount => pinPairs.Count;

        private List<PinPair> pinPairs;
        private int[] bindingOrder;

        private void OnEnable()
        {
            pinPairs = transform.GetComponentsInChildren<PinPair>().ToList();

            foreach (PinPair t in pinPairs)
            {
                t.PinRadius = GenerateUnseenRandom();
                t.KeyPinLength = 1 + (Random.Range(0, 10) / 10f);
            }

            bindingOrder = pinPairs.OrderByDescending(p => p.PinRadius)
                    .Select(p => pinPairs.IndexOf(p))
                    .ToArray();
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
