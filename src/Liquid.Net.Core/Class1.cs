using System;
using System.Collections.Generic;

namespace Liquid.Net.Core;

/// <summary>
/// Represents a neuron in a liquid neural network.
/// </summary>
public interface INeuron
{
    /// <summary>
    /// Unique identifier for the neuron.
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// Current membrane potential of the neuron.
    /// </summary>
    double Potential { get; }
    
    /// <summary>
    /// Firing threshold for the neuron.
    /// </summary>
    double Threshold { get; set; }
    
    /// <summary>
    /// Indicates whether the neuron is currently active (firing).
    /// </summary>
    bool IsActive { get; }
    
    /// <summary>
    /// Collection of input synapses connecting to this neuron.
    /// </summary>
    IReadOnlyCollection<ISynapse> InputSynapses { get; }
    
    /// <summary>
    /// Collection of output synapses from this neuron.
    /// </summary>
    IReadOnlyCollection<ISynapse> OutputSynapses { get; }
    
    /// <summary>
    /// Updates the neuron's state given a time step.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since last update.</param>
    void Update(double deltaTime);
    
    /// <summary>
    /// Resets the neuron to its initial state.
    /// </summary>
    void Reset();
    
    /// <summary>
    /// Adds an input synapse to this neuron.
    /// </summary>
    /// <param name="synapse">The synapse to add.</param>
    void AddInputSynapse(ISynapse synapse);
    
    /// <summary>
    /// Adds an output synapse from this neuron.
    /// </summary>
    /// <param name="synapse">The synapse to add.</param>
    void AddOutputSynapse(ISynapse synapse);
}
