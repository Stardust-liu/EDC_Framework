using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CustomOdinAttribute{
    public class ClampStringAttribute : PropertyAttribute
    {
        public List<string> list;
        public string[] array;
        public ClampStringAttribute(string[] _array){
            array = _array;
            list = array.ToList();
        }
    }
}
