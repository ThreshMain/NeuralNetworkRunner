using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    [Serializable]
    public class AiLearning
    {
        List<NeuralNetwork> old = null;
        public List<NeuralNetwork> networks = new List<NeuralNetwork>();
        public int numberOfNetworks,numberOfLayers, numberOfInputs;
        public AiLearning(int numberOfSteps, int numberOfLayers, int numberOfInputs)
        {
            numberOfNetworks = numberOfSteps;
            this.numberOfLayers = numberOfLayers;
            this.numberOfInputs = numberOfInputs;
            for (int i = 0; i < numberOfNetworks; i++)
            {
                networks.Add(new NeuralNetwork(numberOfLayers, numberOfInputs));
                networks[i].id = i;
            }
        }
        public void learn(int newOnes, int copiesOfTheBest)
        {
            if (old == null)
            {
                Debug.Log("init");
                old = networks;
            }
            List<NeuralNetwork> save = networks;
            networks = new List<NeuralNetwork>();

            int numberTakesFromOld = 0;
            for (int i = 0; i < old.Count; i++)
            {
                if(old[i].score > save[i].score)
                {
                    numberTakesFromOld++;
                    //Debug.Log(old[i].score+","+ save[i].score);
                    networks.Add(new NeuralNetwork(old[i]));
                    save[i] = old[i];
                }
                else
                {
                    networks.Add(new NeuralNetwork(save[i]));
                }
                networks[i].id = i;
            }

            double saveAvg = 0, nowAvg = 0, oldAvg = 0;
            for (int i = 0; i < old.Count; i++)
            {
                saveAvg += save[i].score;
                nowAvg += networks[i].score;
                oldAvg += old[i].score;
            }
            //Debug.Log(numberTakesFromOld + ", s-" + saveAvg / (double)old.Count + ", n-" + nowAvg / (double)old.Count + ", o-" + oldAvg / (double)old.Count);

            networks.Sort();
            double best = networks[0].score;
            //Debug.Log(best );

            for (int i = 1; i < numberOfNetworks; i++)
            {
                if (i + newOnes + copiesOfTheBest < numberOfNetworks)
                {
                    networks[i].addRadnomToRandomWeights(0.5,0.05*(i/ numberOfNetworks));
                }
                else
                {
                    if (i + copiesOfTheBest < numberOfNetworks)
                    {
                        int idSave = networks[i].id;
                        networks[i] = new NeuralNetwork(numberOfLayers, numberOfInputs);
                        networks[i].id = idSave;
                    }
                    else
                    {
                        int idSave = networks[i].id;
                        networks[i] = new NeuralNetwork(networks[0]);
                        networks[i].id = idSave;
                        networks[i].addRadnomToWeights(0.015);
                    }
                }
            }
            networks.Sort((a, b) => a.id.CompareTo(b.id));
            foreach(NeuralNetwork n in networks)
            {
                n.score = 0;
            }
            old = save;
        }
        public override string ToString()
        {
            string value = "{\"networks\":[";
            for (int i = 0; i < networks.Count; i++)
            {
                if (i != 0)
                {
                    value += ",";
                }
                value += "{\"network_" + i + "\":" + networks[i].ToString() + "}";
            }
            value += "]}";
            return value;
        }
        public string Save()
        {
            return "Pojebano si dostal";
        }
    }
}
