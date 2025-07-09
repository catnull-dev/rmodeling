using UnityEngine;

namespace RModeling.Joint
{
    public class RevoluteJoint : Joint<float>
    {
        public override void MoveJoint(float value)
        {
            target.localRotation = Quaternion.AngleAxis(value, axis);
        }
    }
}