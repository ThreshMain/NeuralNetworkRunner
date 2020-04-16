using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NeuralNetwork
{
    [Serializable]
    public class NeuralNetwork:IComparable<NeuralNetwork>
    {
        public Node finalNode;
        public List<ListWrapper> layer = new List<ListWrapper>();
        public int id;
        public double score;
        public Vector2 dead;
        /// <summary>
        /// Constructor for Neural network that copy data from old
        /// </summary>
        /// <param name="nw">Old network</param>
        public NeuralNetwork(NeuralNetwork nw)
        {
            this.score = nw.score;
            finalNode = new Node(nw.finalNode);
            for (int i = 0; i < nw.layer.Count; i++)
            {
                layer.Add(new ListWrapper());
                for (int o = 0; o < nw.layer[i].list.Count; o++)
                {
                    layer[i].list.Add(new Node(nw.layer[i].list[o]));
                }
            }
        }
        /// <summary>
        /// Basic constructor for random network
        /// </summary>
        /// <param name="numberOfLayers">Number of layers</param>
        /// <param name="numberOfInputs">Number of inputs</param>
        public NeuralNetwork(int numberOfLayers,int numberOfInputs)
        {
            finalNode = new Node(numberOfInputs);
            for(int i= 0; i < numberOfLayers; i++)
            {
                layer.Add(new ListWrapper());
                for (int o = 0; o < numberOfInputs; o++)
                {
                    layer[i].list.Add(new Node(numberOfInputs));
                }
            }
        }
        /// <summary>
        /// Getting the resoult for array of inputs
        /// </summary>
        /// <param name="numbers">
        /// Inputs that are passed to the network
        /// </param>
        /// <returns>
        /// Double resoult that is based on the inputs
        /// </returns>
        public double resoult(List<double> numbers)
        {
            return resoultLayer(numbers, 0);
        }
        /// <summary>
        /// Internal method returns resoult of each layer
        /// </summary>
        /// <param name="numbers">Inputs</param>
        /// <param name="layerNumber">Layer number</param>
        /// <returns>Double resoult</returns>
        private double resoultLayer(List<double> numbers,int layerNumber)
        {
            if (layerNumber <= layer.Count - 1)
            {
                List<double> resLayer = new List<double>();
                for (int i = 0; i < numbers.Count; i++)
                {
                    resLayer.Add(layer[layerNumber].list[i].getValue(numbers));
                }
                return resoultLayer(resLayer, layerNumber + 1);
            }
            else
            {
                return finalNode.getValue(numbers);
            }
        }
        /// <summary>
        /// Set random to random part of the networks nodes
        /// </summary>
        /// <param name="v">
        /// 0-1 sets how big part of the networks nodes will be set to random
        /// </param>
        public void setRandomToWeights(double v)
        {
            for (int i = 0; i < layer.Count; i++)
            {
                for (int o = 0; o < layer[i].list.Count; o++)
                {
                    layer[i].list[o].setRandomNums(v);
                }
            }
            finalNode.setRandomNums(v);
        }
        /// <summary>
        /// Add random to all nodes
        /// </summary>
        /// <param name="multiplier">
        /// 0-1 multiplier of the random
        /// </param>
        public void addRadnomToWeights(double multiplier)
        {
            for (int i = 0; i < layer.Count; i++)
            {
                for (int o = 0; o < layer[i].list.Count; o++)
                {
                    layer[i].list[o].addRandomNums(multiplier);
                }
            }
            finalNode.addRandomNums(multiplier);
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
        internal void addRadnomToRandomWeights(double numberToAddTo, double multiplier)
        {
            for (int i = 0; i < layer.Count; i++)
            {
                for (int o = 0; o < layer[i].list.Count; o++)
                {
                    layer[i].list[o].addRandomToRandom(numberToAddTo, multiplier);
                }
            }
            finalNode.addRandomToRandom(numberToAddTo, multiplier);
        }
        public int CompareTo(NeuralNetwork other)
        {
            return other.score.CompareTo(this.score);
        }
    }
}
