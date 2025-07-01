using System.Linq;
using UnityEngine;

namespace RModeling
{
    public class PlanarRobot : Robot<RevoluteJoint, float, PlanarPose>
    {
        [SerializeField]
        private float Length1;

        [SerializeField]
        private float Length2;

        [SerializeField]
        private bool configuration;

        private int numberConfiguration;

        public override void MoveJoints(float[] values)
        {
            var jointValuePairs = joints.Zip(values, (joint, value) => (Joint: joint, Value: value));
            foreach (var pair in jointValuePairs)
            {
                pair.Joint.MoveJoint(pair.Value);
            }
        }

        private void Update()
        {
            if (configuration)
            {
                numberConfiguration = 1;
            } else
            {
                numberConfiguration = -1;
            }
        }

        public override float[] SolveInverse(PlanarPose pose)
        {
            var c = Mathf.Sqrt(Mathf.Pow(pose.X, 2) + Mathf.Pow(pose.Y, 2));
            var atan = Mathf.Atan2(pose.Y, pose.X);

            var top = Mathf.Pow(Length1, 2) + Mathf.Pow(c, 2) - Mathf.Pow(Length2, 2);
            var bottom = 2 * Length1 * c;

            var addTop = Mathf.Pow(Length1, 2) + Mathf.Pow(Length2, 2) - Mathf.Pow(c, 2);
            Debug.Log("Add top is " + addTop + "; c is " + c);
            var addBottom = 2 * Length2 * Length1;

            var q1 = Mathf.Rad2Deg * ((numberConfiguration * Mathf.Acos(top / bottom) + atan));
            var q2 = -180 + Mathf.Rad2Deg * (numberConfiguration * Mathf.Acos(addTop / addBottom));

            q1 = WrapAngle(q1);
            q2 = WrapAngle(q2);

            return new float[] { q1, q2 };
        }

        public override PlanarPose SolveForward(float[] joints)
        {
            throw new System.NotImplementedException();
        }

        float WrapAngle(float angle) => angle - 360f * Mathf.Floor((angle + 180f) / 360f);
    }
} 