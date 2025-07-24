using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RModeling
{
    public abstract class Joint : MonoBehaviour
    {
        [SerializeField]
        protected Transform target;

        [SerializeField]
        protected Vector3 axis;

        [SerializeField]
        private float currentValue;

        public float CurrentJointValue
        {
            get => currentValue;
            set => currentValue = value;
        }
        
        public abstract void MoveJoint(float value);
    }

    public abstract class Robot<Pose> : MonoBehaviour
    {
        [SerializeField]
        protected List<Joint> joints = new List<Joint>();
        public abstract void MoveJoints(float[] values);
        public abstract float[] SolveInverse(Pose pose);
        public abstract Pose SolveForward(float[] joints);
        public abstract float[] GetValues();
    }

    public abstract class Target<Pose> : MonoBehaviour
    {
        [SerializeField]
        public Robot<Pose> controller;

        [SerializeField]
        protected Transform target;

        [SerializeField]
        protected Transform forwardTarget;
    }
}