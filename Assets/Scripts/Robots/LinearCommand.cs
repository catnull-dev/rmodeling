using RModeling.Controller;
using System.Numerics;

namespace RModeling.Robots
{
    public abstract class SimpleLinearCommand<Pose> : ICommand
    {
        protected Pose currentPose;

        protected Pose targetPose;

        protected CommandStatus status;

        protected Robot<Pose> robot;

        protected float progress = 0;

        protected float totalTime = 10;

        public SimpleLinearCommand(Robot<Pose> robot, Pose target) {
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

    public class PlanarSimpleLinearCommand : SimpleLinearCommand<PlanarPose>
    {
        public PlanarSimpleLinearCommand(Robot<PlanarPose> robot, PlanarPose target) : base(robot, target)
        {
        }

        public override CommandStatus Execute(float deltaTime)
        {
            if (progress >= 1)
            {
                status = CommandStatus.Done;
            } else
            {
                status = CommandStatus.Executing;
            }

            var startVector = new Vector3(currentPose.X, currentPose.Y, currentPose.Z);
            var endVector = new Vector3(targetPose.X, targetPose.Y, targetPose.Z);

            var interpolatedPosition = Vector3.Lerp(startVector, endVector, progress);
            var interpolatedPlanarPosition = new PlanarPose();
            interpolatedPlanarPosition.X = interpolatedPosition.X;
            interpolatedPlanarPosition.Y = interpolatedPosition.Y;
            interpolatedPlanarPosition.Z = interpolatedPosition.Z;

            var joints = robot.SolveInverse(interpolatedPlanarPosition);

            if (joints != null)
            {
                robot.MoveJoints(joints);
            } else
            {
                status = CommandStatus.Failed;
            }

            progress += deltaTime / totalTime;
            return status;
        }
    }
}