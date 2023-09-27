using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InsaneScatterbrain.Extensions;
using UnityEngine;

namespace InsaneScatterbrain.ScriptGraph
{
    /// <summary>
    /// Base node for any node that provides (gives) data. In other words, any node that has one more out ports.
    /// </summary>
    [Serializable]
    public abstract class ProviderNode : ScriptNode, IProviderNode
    {
        [SerializeReference] private List<OutPort> outPorts = new List<OutPort>();

        private ReadOnlyCollection<OutPort> readOnlyOutPorts;

        /// <inheritdoc cref="IProviderNode.OutPorts"/>
        public ReadOnlyCollection<OutPort> OutPorts => readOnlyOutPorts ?? (readOnlyOutPorts = outPorts.AsReadOnly());

        private Dictionary<string, OutPort> outPortsByName;
        
        /// <summary>
        /// Gets out ports by name.
        /// </summary>
        private Dictionary<string, OutPort> OutPortsByName
        {
            get
            {
                if (outPortsByName != null) return outPortsByName;
                
                // If the out ports haven't been indexed by name yet, do it now.
                outPortsByName = new Dictionary<string, OutPort>();
                foreach (var outPort in outPorts)
                {
                    outPortsByName.Add(outPort.Name, outPort); 
                }

                return outPortsByName;
            }
        }
        
        /// <inheritdoc cref="IProviderNode.OnOutPortAdded"/>
        public event Action<OutPort> OnOutPortAdded;
        
        /// <inheritdoc cref="IConsumerNode.OnInPortRemoved"/>
        public event Action<OutPort> OnOutPortRemoved;
        
        /// <inheritdoc cref="IConsumerNode.OnInPortRemoved"/>
        public event Action<OutPort, string, string> OnOutPortRenamed;

        /// <inheritdoc cref="IConsumerNode.OnInPortMoved"/>
        public event Action<OutPort, int> OnOutPortMoved;
        
        [Obsolete("Processor nodes needn't be dependant on a graph anymore. You can use the parameterless constructor instead. This one will be removed with version 2.0.")]
        protected ProviderNode(ScriptGraphGraph graph) : base(graph)
        {
        }

        protected ProviderNode()
        {
        }

        /// <inheritdoc cref="IProviderNode.GetOutPort"/>
        public OutPort GetOutPort(string name)
        {
            if (!OutPortsByName.ContainsKey(name))
            {
                throw new PortNotFoundException(name);
            }
            return OutPortsByName[name];
        }

        /// <summary>
        /// Adds an out port with the given name of the given type.
        /// </summary>
        /// <param name="name">The port's name.</param>
        /// <param name="type">The port's data type.</param>
        /// <param name="owner">
        /// The node that is marked as the owner. If this is null that will be this node
        /// (which is probably what you want).
        /// </param>
        /// <param name="port">The existing port.</param>
        /// <returns>The new out port.</returns>
        public OutPort AddOut(string name, Type type, IScriptNode owner = null, OutPort port = null)
        {
            if (port != null && port.Name != name)
            {
                OutPortsByName.Remove(port.Name);
                port.Name = name;
                OutPortsByName.Add(port.Name, port);
            }

            if (OutPortsByName.ContainsKey(name))
            {
                var existingPort = OutPortsByName[name];
                if (existingPort.Type == type)
                {
                    // Port already exists and the type matches. Re-add it to the list, to make sure the order is
                    // the same as how they are defined in OnLoadInputPorts.
                    outPorts.Remove(existingPort);
                    outPorts.Add(existingPort);
                    
                    OnOutPortAdded?.Invoke(existingPort);
                    return OutPortsByName[name];
                }

                // Port already exists, but the type doesn't match, remove it, so a new one with the correct type can be added.
                OutPortsByName.Remove(name);
                outPorts.Remove(existingPort);
            }
            
            if (owner == null) owner = Node;
            
            var outPort = OutPort.Create(name, type, owner);
            outPorts.Add(outPort);
            OutPortsByName.Add(outPort.Name, outPort);
            
            OnOutPortAdded?.Invoke(outPort); 
            
            return outPort;
        }

        /// <inheritdoc cref="AddOut"/>
        public OutPort AddOut<T>(string name, IScriptNode owner = null)
        {
            return AddOut(name, typeof(T), owner);
        }
        
        /// <inheritdoc cref="IProviderNode.RemoveOut"/>
        public void RemoveOut(string name)
        {
            if (!OutPortsByName.ContainsKey(name)) return;
            
            var outPort = OutPortsByName[name];
            outPort.DisconnectAll();
            outPorts.Remove(outPort);
            outPortsByName.Remove(name);
            OnOutPortRemoved?.Invoke(outPort);
        }
        
        /// <inheritdoc cref="IProviderNode.RenameOut"/>
        public void RenameOut(string oldName, string newName)
        {
            var outPort = OutPortsByName[oldName];
            outPort.Name = newName;
            outPortsByName?.Remove(oldName);
            OutPortsByName.Add(newName, outPort);
            OnOutPortRenamed?.Invoke(outPort, oldName, newName);
        }

        /// <inheritdoc cref="IProviderNode.MoveOut"/>
        public void MoveOut(int oldIndex, int newIndex)
        {
            var outPort = outPorts[oldIndex];
            outPorts.RemoveAt(oldIndex);
            outPorts.Insert(newIndex, outPort);
            OnOutPortMoved?.Invoke(outPort, newIndex);
        }

        public void ClearPorts()
        {
            var names = OutPortsByName.Keys.ToList();
            foreach (var name in names)
            {
                RemoveOut(name);
            }
        }

        /// <inheritdoc cref="IProviderNode.OnLoadOutputPorts"/>
        public virtual void OnLoadOutputPorts()
        {
            var outPortFields = Node.GetType().GetAllPrivateFields()
                .Where(field => typeof(OutPort).IsAssignableFrom(field.FieldType));
            
            foreach (var field in outPortFields)
            {
                var outPortAttribute = field.GetAttribute<OutPortAttribute>();
                
                if (outPortAttribute == null) continue;
                
                var port = field.GetValue(Node) as OutPort;
                
                port = AddOut(outPortAttribute.Name, outPortAttribute.Type, port: port);
                field.SetValue(Node, port);
            }
        }
        
        /// <summary>
        /// In case of proxy classes (such as ProcessorNode uses) it's useful to explicitly set the node's type.
        /// </summary>
        protected virtual IProviderNode Node => this;
    }
}