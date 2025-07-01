using UnityEngine;

namespace RModeling
{
    public class PlanarTarget : Target<Robot<RevoluteJoint, float, PlanarPose>, RevoluteJoint, float, PlanarPose>
    {
        private void Update()
        {
            var planarPose = new PlanarPose();
            planarPose.X = target.position.x;
            planarPose.Y = target.position.y;
            planarPose.Z = target.position.z;

            var gens = controller.SolveInverse(planarPose);
            controller.MoveJoints(gens);
        }
    }

    public struct PlanarPose
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}