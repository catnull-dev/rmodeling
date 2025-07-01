using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RModeling
{
    public abstract class Joint<Type> : MonoBehaviour
    {
        [SerializeField]
        protected Transform target;

        [SerializeField]
        protected Vector3 axis;

        public abstract void MoveJoint(Type value);
    }

    public abstract class Robot<Joint, JointType, Pose> : MonoBehaviour
    where Joint : Joint<JointType>
    where JointType : struct
    where Pose : struct
    {
        [SerializeField]
        protected List<Joint> joints = new List<Joint>();
        public abstract void MoveJoints(JointType[] values);
        public abstract JointType[] SolveInverse(Pose pose);
        public abstract Pose SolveForward(JointType[] joints);
    }

    public abstract class Target<Robot, Joint, JointType, Pose> : MonoBehaviour
    where Robot : Robot<Joint, JointType, Pose>
    where Joint : Joint<JointType>
    where JointType : struct
    where Pose : struct
    {
        [SerializeField]
        public Robot controller;

        [SerializeField]
        protected Transform target;
    }
}