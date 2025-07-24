using UnityEngine;

namespace RModeling.Joints
{
    public class RevoluteJoint : Joint
    {
        public override void MoveJoint(float value)
        {
            this.CurrentJointValue = value;
            target.localRotation = Quaternion.AngleAxis(value, axis);
        }
    }
}