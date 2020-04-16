using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    [Serializable]
    public class AiLearning
    {
        public List<NeuralNetwork> old = null;
        public List<NeuralNetwork> networks = new List<NeuralNetwork>();
        public int numberOfNetworks,numberOfLayers, numberOfInputs;
        /// <summary>
        /// Default constructor for AiLearning with random generated Networks
        /// </summary>
        /// <param name="numberOfNetworks">Number of networks (Players)</param>
        /// <param name="numberOfLayers">Number of layers for each network</param>
        /// <param name="numberOfInputs">Number of Inputs for each layer of each network</param>
        public AiLearning(int numberOfNetworks, int numberOfLayers, int numberOfInputs)
        {
            this.numberOfNetworks = numberOfNetworks;
            this.numberOfLayers = numberOfLayers;
            this.numberOfInputs = numberOfInputs;
            for (int i = 0; i < numberOfNetworks; i++)
            {
                networks.Add(new NeuralNetwork(numberOfLayers, numberOfInputs));
                networks[i].id = i;
            }
        }
        /// <summary>
        /// Default learn method for my AI
        /// </summary>
        /// <param name="newOnes">Number of totaly new Random generated networks</param>
        /// <param name="copiesOfTheBest">Number of copyes of the network with best score</param>
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

           
            networks.Sort();
            double best = networks[0].score;
            for (int i = 1; i < numberOfNetworks; i++)
            {
                if (i + newOnes + copiesOfTheBest < numberOfNetworks)
                {
                    // zde si muze kazdy hrat s pomery jen by meli zustat mezi 0-1
                    networks[i].addRadnomToRandomWeights(0.5,0.5*(i/ numberOfNetworks));
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
                        // zde si muze kazdy hrat s pomery jen by meli zustat mezi 0-1
                        networks[i].addRadnomToRandomWeights(0.5, 0.015);
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
    }
}
