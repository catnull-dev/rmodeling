using UnityEngine;
using RModeling.Controller;
using RModeling.Robots;

namespace RModeling
{
    public class Executor : MonoBehaviour
    {
        [SerializeField]
        private Controller.Controller controller;

        [SerializeField]
        private PlanarRobot planarRobot;

        private void Start()
        {
            controller = GetComponent<Controller.Controller>();
            var target1 = new PlanarPose();
            target1.X = 1.175f;
            target1.Y = 0.195f;
            target1.Z = 0.0f;

            var target2 = new PlanarPose();
            target2.X = 0.175f;
            target2.Y = 0.195f;
            target2.Z = 0.0f;

            var linearMovement1 = new PlanarSimpleLinearCommand(planarRobot, target1);
            var linearMovement2 = new PlanarSimpleLinearCommand(planarRobot, target2);

            controller.AddCommand(linearMovement1);
            controller.AddCommand(linearMovement2);

            controller.Run();
        }
    }
}