using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// JsonUtils can not serializate List<List<Object>> 
    /// becouse List<T> is not serializable
    /// </summary>
    [Serializable]
    public class ListWrapper
    {
        public List<Node> list;
        public ListWrapper(){
            this.list=new List<Node>();
        }
    }
}
