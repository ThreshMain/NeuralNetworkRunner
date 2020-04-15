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
        /// <summary>
        /// Copy node
        /// </summary>
        /// <param name="nw">Old node</param>
        public Node(Node nw)
        {
            nodeWeight = nw.nodeWeight;
            weight = new double[nw.weight.Length];
            for (int i = 0; i < nw.weight.Length; i++)
            {
                weight[i]=nw.weight[i];
            }
        }
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="numberOfConnections">
        /// Number of inputs
        /// </param>
        public Node(int numberOfConnections)
        {
            weight = new double[numberOfConnections];
            for(int i = 0; i < numberOfConnections; i++)
            {
                weight[i]=rand.NextDouble()-0.5 * 2.0;
            }
            nodeWeight = (rand.NextDouble() - 0.5)*2.0;
        }
        /// <summary>
        /// Returns value calculated based on the inputs and weights
        /// </summary>
        /// <param name="numbers">
        /// Inputs
        /// </param>
        /// <returns>
        /// Single value
        /// </returns>
        public double getValue(List<double> numbers)
        {
            double value = 0;
            for (int i=0;i< numbers.Count;i++)
            {
                value += numbers[i] * weight[i];
            }
            return value * (nodeWeight / (double) numbers.Count);
        }
        /// <summary>
        /// Add numbers to the weights
        /// </summary>
        /// <param name="numbers">
        /// Array of numbers to add
        /// </param>
        public void addToWeights(double[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                weight[i] = Math.Max(-1,Math.Min(1, weight[i] + numbers[i]));
            }
        }

        /// <summary>
        /// Generate array of random numbers with size of the node inputs
        /// </summary>
        /// <param name="multiplier">
        /// 0-1 multiplier of the random
        /// </param>
        /// <returns>
        /// Array of random numbers
        /// </returns>
        public double[] generateRandomNums(double multiplier)
        {
            double[] random = new double[weight.Length];
            for (int i = 0; i < weight.Length; i++)
            {
                random[i]=(rand.NextDouble() - 0.5) * 2 * multiplier;
            }
            return random;
        }

        /// <summary>
        /// Add random to all nodes
        /// </summary>
        /// <param name="multiplier">
        /// 0-1 multiplier of the random
        /// </param>
        public void addRandomNums(double multiplier)
        {
            double[] random = new double[weight.Length];
            for (int i = 0; i < weight.Length; i++)
            {
                random[i]=(rand.NextDouble() - 0.5) * 4 * multiplier;
            }
            nodeWeight=Math.Max(-1, Math.Min(1, nodeWeight + ((rand.NextDouble() - 0.5) *4* multiplier)));
            addToWeights(random);
        }

        /// <summary>
        /// Set random to random part of the networks nodes
        /// </summary>
        /// <param name="v">
        /// 0-1 sets how big part of the networks nodes will be set to random
        /// </param>
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

        /// <summary>
        /// Add random to random part of the networks nodes
        /// </summary>
        /// <param name="numberToAddTo">
        /// 0-1 sets how big part of the networks nodes will be changed
        /// </param>
        /// <param name="multiplier">
        /// 0-1 multiplier of the random
        /// </param>
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
