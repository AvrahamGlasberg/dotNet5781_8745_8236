using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Station
    {
        //
        public int Code { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("Station Code: {0}, Name: {1}", Code, Name);
        }
    }
}
