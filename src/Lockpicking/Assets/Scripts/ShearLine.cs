using UnityEngine;

namespace Lockpicking
{
    [ExecuteInEditMode]
    public class ShearLine : MonoBehaviour
    {
        private TumblerLock tumblerLock;

        private void Awake() => tumblerLock = GetComponent<TumblerLock>();

        private void Update()
        {
            Vector3 position = transform.position;
            Debug.DrawLine(
                    start: position + new Vector3(-1f, tumblerLock.ShearLine, 0f),
                    end: position + new Vector3(tumblerLock.transform.childCount * 1.1f, tumblerLock.ShearLine, 0f),
                    color: Color.red);
        }
    }
}
