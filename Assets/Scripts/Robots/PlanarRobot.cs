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
            if (values != null)
            {
                var jointValuePairs = joints.Zip(values, (joint, value) => (Joint: joint, Value: value));
                foreach (var pair in jointValuePairs)
                {
                    pair.Joint.MoveJoint(pair.Value);
                }
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
            var addBottom = 2 * Length2 * Length1;

            var q1 = Mathf.Rad2Deg * ((numberConfiguration * Mathf.Acos(top / bottom) + atan));
            var q2 = -180 + Mathf.Rad2Deg * (numberConfiguration * Mathf.Acos(addTop / addBottom));

            if (float.IsNaN(q1) || float.IsNaN(q2))
            {
                return null;
            }

            q1 = WrapAngle(q1);
            q2 = WrapAngle(q2);

            return new float[] { q1, q2 };
        }

        public override PlanarPose SolveForward(float[] joints)
        {
            var t1 = Matrix4x4.identity * Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(joints[0], new Vector3(0, 0, 1)), Vector3.one);
            var t2 = t1 * Matrix4x4.TRS(new Vector3(0.717f, 0, 0), Quaternion.identity, Vector3.one);
            var t3 = t2 * Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(joints[1], new Vector3(0, 0, 1)), Vector3.one);
            var t4 = t3 * Matrix4x4.TRS(new Vector3(0.717f, 0, 0), Quaternion.identity, Vector3.one);
            
            var planarPose = new PlanarPose();
            planarPose.X = t4.GetPosition().x;
            planarPose.Y = t4.GetPosition().y;
            planarPose.Z = t4.GetPosition().z;
            
            return planarPose;
        }

        private float WrapAngle(float angle) => angle - 360f * Mathf.Floor((angle + 180f) / 360f);
    }
} 