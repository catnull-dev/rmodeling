using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace RModeling.Utils
{
    public class NetGraphics
    {
        private string url;

        private HttpClient client;

        public NetGraphics(string url)
        {
            client = new HttpClient();
            this.url = url;
        }

        public void PlotGraphic(Graphic graphic)
        {
            var content = JsonUtility.ToJson(graphic);
            var task = Task.Run(async () => await PostAsync(content));
        }

        private async Task PostAsync(string content)
        {
            var stringContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, stringContent);
        }
    }

    [Serializable]
    public struct PlotRepr
    {
        public List<double> x_data;
        public List<double> y_data;
        public string color;

        public PlotRepr(string color)
        {
            x_data = new List<double>();
            y_data = new List<double>();
            this.color = color;
        }

        public void AddData(double x, double y)
        {
            x_data.Add(x);
            y_data.Add(y);
        }
    }

    [Serializable]
    public struct Graphic
    {
        public string name;
        public List<PlotRepr> plots;
        public string x_label;
        public string y_label;

        public Graphic(string name, PlotRepr[] plots, string xLabel, string yLabel)
        {
            this.name = name;
            this.plots = plots.ToList();
            this.x_label = xLabel;
            this.y_label = yLabel;
        }
    }
}