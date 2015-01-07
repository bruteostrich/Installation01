using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Assets.Code
{
    public sealed class ResourcePool
    {
        private Dictionary<string, object> Objects;

        public int ResourceCount
        {
            get
            {
                return Objects.Count;
            }
        }
        
        public ResourcePool()
        {
            Objects = new Dictionary<string, object>();
        }
    }
}