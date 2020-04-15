using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    [Serializable]
    public class ListWrapper
    {
        public List<Node> list;
        public ListWrapper(){
            this.list=new List<Node>();
        }
    }
}
