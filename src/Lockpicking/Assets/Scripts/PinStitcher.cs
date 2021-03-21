using Lockpicking.Helpers;
using UnityEngine;

namespace Lockpicking
{
    public class PinStitcher : MonoBehaviour
    {
        public float ActualHeight;
        public void Stitch(float pinHeight, bool isKeyPin)
        {
            Transform top = transform.GetChild(0);
            Transform cylinder = transform.GetChild(1);
            Transform bottom = transform.GetChild(2);
            cylinder.localScale = cylinder.localScale.With(y: pinHeight);
            top.localPosition = top.localPosition.With(y: pinHeight + .1f);
            bottom.localPosition = bottom.localPosition.With(y: -pinHeight - (isKeyPin ? .2f : .1f));

            ActualHeight = pinHeight + .1f + (isKeyPin ? .2f : .1f);
        }
    }
}
