using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    [Serializable]
    public class Node
    {
        public double[] weight;
        public double nodeWeight;
        public static Random rand = new Random(14);
        public Node(double[] weight, double nodeWeight)
        {
            this.weight = weight;
            this.nodeWeight = nodeWeight;
        }
        public Node(Node nw)
        {
            nodeWeight = nw.nodeWeight;
            weight = new double[nw.weight.Length];
            for (int i = 0; i < nw.weight.Length; i++)
            {
                weight[i]=nw.weight[i];
            }
        }
        public Node(int numberOfConnections)
        {
            weight = new double[numberOfConnections];
            for(int i = 0; i < numberOfConnections; i++)
            {
                weight[i]=rand.NextDouble()-0.5 * 2.0;
            }
            nodeWeight = (rand.NextDouble() - 0.5)*2.0;
        }
        public double getValue(List<double> numbers)
        {
            double value = 0;
            for (int i=0;i< numbers.Count;i++)
            {
                value += numbers[i] * weight[i];
            }
            return value * (nodeWeight / (double) numbers.Count);
        }
        public void addToWeights(double[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                weight[i] = Math.Max(-1,Math.Min(1, weight[i] + numbers[i]));
            }
        }
        public double[] generateRandomNums(double multiplaer)
        {
            double[] random = new double[weight.Length];
            for (int i = 0; i < weight.Length; i++)
            {
                random[i]=(rand.NextDouble() - 0.5) * 2 * multiplaer;
            }
            return random;
        }
        public void addRandomNums(double multiplaer)
        {
            double[] random = new double[weight.Length];
            for (int i = 0; i < weight.Length; i++)
            {
                random[i]=(rand.NextDouble() - 0.5) * 4 * multiplaer;
            }
            nodeWeight=Math.Max(-1, Math.Min(1, nodeWeight + ((rand.NextDouble() - 0.5) *4* multiplaer)));
            addToWeights(random);
        }
        public override string ToString()
        {
            string value = "\"weight\":"+ nodeWeight+ ",\n\"weights\":[";
            for (int i = 0; i < weight.Length; i++)
            {
                if (i != 0)
                {
                    value += ",";
                }
                value += "{\"id_" + i + "\":" + weight[i] + "}";
            }
            value += "]";
            return value;

        }

        internal void setRandomNums(double v)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < weight.Length; i++)
            {
                numbers.Add(i);
            }
            for (int i = 0; i < weight.Length*v; i++)
            {
                int randomIndex = (int) rand.NextDouble() * numbers.Count;
                int number=numbers[randomIndex];
                numbers.RemoveAt(randomIndex);
                weight[number] = (rand.NextDouble() - 0.5) * 2;
            }
            nodeWeight = Math.Max(-1, Math.Min(1, nodeWeight + ((rand.NextDouble() - 0.5) * v)));
        }

        internal void addRandomToRandom(double numberToAddTo,double multiplier)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < weight.Length; i++)
            {
                numbers.Add(i);
            }
            for (int i = 0; i < weight.Length * numberToAddTo; i++)
            {
                int randomIndex = (int)rand.NextDouble() * numbers.Count;
                int number = numbers[randomIndex];
                numbers.RemoveAt(randomIndex);
                weight[number] = Math.Max(-1, Math.Min(1, weight[number] + (rand.NextDouble() - 0.5) * multiplier*2));
            }
            nodeWeight = Math.Max(-1, Math.Min(1, nodeWeight + ((rand.NextDouble() - 0.5) * 2)));
        }
    }
}
