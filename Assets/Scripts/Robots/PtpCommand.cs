using RModeling.Controller;
using RModeling.Robots;
using UnityEngine;

namespace RModeling
{
    public abstract class SimplePtpCommand<Pose> : ICommand
    {
        protected Pose currentPose;

        protected Pose targetPose;

        protected CommandStatus status;

        protected Robot<Pose> robot;

        protected float progress = 0;

        protected float totalTime = 10;

        public SimplePtpCommand(Robot<Pose> robot, Pose target)
        {
            this.robot = robot;
            this.targetPose = target;
        }

        public virtual void Init()
        {
            var currentJoints = robot.GetValues();
            currentPose = robot.SolveForward(currentJoints);
        }

        public abstract CommandStatus Execute(float deltaTime);
    }

    public class PlanarSimplePtpCommand : SimplePtpCommand<PlanarPose>
    {
        public PlanarSimplePtpCommand(PlanarRobot robot, PlanarPose target) : base(robot, target)
        {
        }

        public override CommandStatus Execute(float deltaTime)
        {
            if (progress >= 1)
            {
                status = CommandStatus.Done;
            }
            else
            {
                status = CommandStatus.Executing;
            }

            var startJoints = robot.SolveInverse(currentPose);
            var endJoints = robot.SolveInverse(targetPose);

            if (startJoints == null || endJoints == null)
            {
                status = CommandStatus.Failed;
            }

            var interpolatedJoint1 = Mathf.Lerp(startJoints[0], endJoints[0], progress);
            var interpolatedJoint2 = Mathf.Lerp(startJoints[1], endJoints[1], progress);

            robot.MoveJoints(new float[] { interpolatedJoint1, interpolatedJoint2 });

            progress += deltaTime / totalTime;
            return status;
        }
    }
}