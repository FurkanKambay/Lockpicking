using UnityEngine;

namespace Lockpicking.Helpers
{
    public static class VectorExtensions
    {
        public static Vector3 With(this Vector3 self, float? x = default, float? y = default, float? z = default)
        {
            self.x = x ?? self.x;
            self.y = y ?? self.y;
            self.z = z ?? self.z;
            return self;
        }
    }
}
