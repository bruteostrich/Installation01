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

        /// <summary>
        /// Gets number of resources in this resource pool
        /// </summary>
        public int ResourceCount
        {
            get
            {
                return Objects.Count;
            }
        }

        /// <summary>
        /// Indexer to access resource objects quickly
        /// </summary>
        /// <param name="ResourceName">Name of the resource in the collection</param>
        /// <returns></returns>
        public object this[string ResourceName]
        {
            get
            {
                return Objects[ResourceName];
            }
        }

        /// <summary>
        /// Returns a value indicating wether there is a resource with the specified name in this resource pool
        /// </summary>
        /// <param name="ResourceName">Name of specified resource</param>
        /// <returns>True if the resource exists, false if not</returns>
        public bool Contains(string ResourceName)
        {
            return Objects.ContainsKey(ResourceName);
        }

        /// <summary>
        /// Adds resource to this resource pool with the specified name
        /// </summary>
        /// <param name="ResourceObject">Object to add</param>
        /// <param name="ResourceName">Specified name to give to resource</param>
        public void AddResource(object ResourceObject, string ResourceName)
        {
            Objects.Add(ResourceName, ResourceObject);
        }

        /// <summary>
        /// Removes resource from the pool
        /// </summary>
        /// <param name="ResourceName">Name of resource to remove</param>
        public void RemoveResource(string ResourceName)
        {
            Objects.Remove(ResourceName);
        }
        
        public ResourcePool()
        {
            Objects = new Dictionary<string, object>();
        }
    }
}