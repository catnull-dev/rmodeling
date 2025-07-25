using System.Collections.Generic;
using UnityEngine;

namespace RModeling.Utils
{
    public class PlanarPlotterObserver : PlotterObserver<PlanarPose>
    {
        private PlotterStatus plotterStatus = PlotterStatus.None;

        private SimpleEndEffectorPlotter effectorPlotter;

        private JointPlotter jointPlotter;

        [SerializeField]
        private string url;

        private NetGraphics netGraphics;

        private float totalTime = 0;

        private void Start()
        {
            netGraphics = new NetGraphics(url);
            effectorPlotter = new SimpleEndEffectorPlotter("endEffector", "time", "end", netGraphics);
            jointPlotter = new JointPlotter("joints", "time", "joints", netGraphics);
        }

        public override void RecordStartMovement()
        {
            plotterStatus = PlotterStatus.Record;
        }

        private void Update()
        {
            if (plotterStatus == PlotterStatus.Record)
            {
                var currentJoints = robot.GetValues();
                var pose = robot.SolveForward(currentJoints);
                totalTime += Time.fixedDeltaTime;
                effectorPlotter.AddData((pose, totalTime));
                jointPlotter.AddData((currentJoints, totalTime));
            }
        }

        public override void RecordEndMovement() {
            effectorPlotter.Apply();
            jointPlotter.Apply();
            plotterStatus = PlotterStatus.None;
            totalTime = 0;
            Debug.Log("RecordEndMovement");
        }
    }

    public abstract class PlotterObserver<PoseType> : MonoBehaviour
    {
        [SerializeField]
        protected Robot<PoseType> robot;

        public abstract void RecordStartMovement();
        public abstract void RecordEndMovement();
    }

    public enum PlotterStatus
    {
        Record,
        None
    }
}