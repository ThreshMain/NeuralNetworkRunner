using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    [Serializable]
    public class NeuralNetwork:IComparable<NeuralNetwork>
    {
        public Node finalNode;
        public int id;
        public List<ListWrapper> layer = new List<ListWrapper>();
        public double score;
        public NeuralNetwork(Node final, List<ListWrapper> layers)
        {
            finalNode=final;
            layer = layers;
        }
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
        public double resoult(List<double> numbers)
        {
            return resoultLayer(numbers, 0);
        }
        public double resoultLayer(List<double> numbers,int layerNumber)
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
        public override string ToString()
        {
            string value = "{\"Main\":{" + finalNode.ToString() + "},\n\"Layers\":[";
            for(int i = 0; i < layer.Count; i++)
            {
                if (i != 0)
                {
                    value += ",";
                }
                value += "{ \"LayerNumber\":" + i + ",\n\"Nodes\":[\n";
                for(int o = 0; o < layer[i].list.Count; o++)
                {
                    if (o != 0)
                    {
                        value += ",\n";
                    }
                    value += "{\n" + layer[i].list[o].ToString() + "\n}";
                }
                value += "]}\n";
            }
            return value + "]}";
        }

        internal void setRandomToWeights(double v)
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

        public void addRadnomToWeights(double multiplaer)
        {
            for (int i = 0; i < layer.Count; i++)
            {
                for (int o = 0; o < layer[i].list.Count; o++)
                {
                    layer[i].list[o].addRandomNums(multiplaer);
                }
            }
            finalNode.addRandomNums(multiplaer);
        }

        internal void addRadnomToRandomWeights(double numberToAddTo, double mulplier)
        {
            for (int i = 0; i < layer.Count; i++)
            {
                for (int o = 0; o < layer[i].list.Count; o++)
                {
                    layer[i].list[o].addRandomToRandom(numberToAddTo, mulplier);
                }
            }
            finalNode.addRandomToRandom(numberToAddTo, mulplier);
        }

        public int CompareTo(NeuralNetwork other)
        {
            return other.score.CompareTo(this.score);
        }
    }
}
