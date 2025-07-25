using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace RModeling.Utils
{
    interface IPlotter<Data>
    {
        void Apply();
        void AddData(Data data);
    }

    public class SimpleEndEffectorPlotter : IPlotter<(PlanarPose, float)>
    {
        private string graphicName;
        private string xName;
        private string yName;

        private const string X_COLOR = "red";
        private const string Y_COLOR = "blue";
        private const string Z_COLOR = "green";
        private const string TOTAL_COLOR = "black";

        private NetGraphics netGraphics;

        private List<(PlanarPose, float)> points = new List<(PlanarPose, float)>();

        public SimpleEndEffectorPlotter(string graphicName, string xLabel, string yLabel, NetGraphics netGraphics)
        {
            this.graphicName = graphicName;
            this.xName = xLabel;
            this.yName = yLabel;
            this.netGraphics = netGraphics;
        }

        public void AddData((PlanarPose, float) data)
        {
            points.Add(data);
        }

        public void Apply()
        {
            var plotReprX = new PlotRepr(X_COLOR);
            var plotReprY = new PlotRepr(Y_COLOR);
            var plotReprZ = new PlotRepr(Z_COLOR);

            foreach (var point in points)
            {
                var x = point.Item1.X;
                var y = point.Item1.Y;
                var z = point.Item1.Z;

                var time = point.Item2;
                plotReprX.AddData(time, x);
                plotReprY.AddData(time, y);
                plotReprZ.AddData(time, z);
            }

            var graphic = new Graphic(graphicName, new PlotRepr[] { plotReprX, plotReprY, plotReprZ }, xName, yName);
            netGraphics.PlotGraphic(graphic);
        }
    }

    public class JointPlotter : IPlotter<(float[], float)>
    {
        private string graphicName;
        private string xName;
        private string yName;

        private readonly string[] colors = new string[] {
            "red", "blue", "green"
        };

        private NetGraphics netGraphics;

        private List<(float[], float)> points = new List<(float[], float)>();

        public JointPlotter(string graphicName, string xLabel, string yLabel, NetGraphics netGraphics)
        {
            this.graphicName = graphicName;
            this.xName = xLabel;
            this.yName = yLabel;
            this.netGraphics = netGraphics;
        }

        public void AddData((float[], float) data)
        {
            points.Add(data);
        }

        public void Apply()
        {
            var reps = new List<PlotRepr>();

            for (var counter = 0; counter < points[0].Item1.Length; counter++)
            {
                var plotRepr = new PlotRepr(colors[counter]);
                reps.Add(plotRepr);
            }

            foreach (var point in points)
            {
                var jointsData = point.Item1;
                for (var counter = 0; counter < jointsData.Length; counter++)
                {
                    reps[counter].AddData(point.Item2, jointsData[counter]);
                }
            }

            var graphic = new Graphic(graphicName, reps.ToArray(), xName, yName);
            netGraphics.PlotGraphic(graphic);
        }
    }
}