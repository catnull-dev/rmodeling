using UnityEngine;

namespace RModeling
{
    public class RevoluteJoint : Joint<float>
    {
        public override void MoveJoint(float value)
        {
            target.localRotation = Quaternion.AngleAxis(value, axis);
        }
    }
}