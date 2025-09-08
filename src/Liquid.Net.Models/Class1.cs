using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.Net.Core;

namespace Liquid.Net.Models;

/// <summary>
/// Basic implementation of a liquid neuron with leaky integrate-and-fire dynamics.
/// </summary>
public class BasicNeuron : INeuron
{
    private readonly List<ISynapse> _inputSynapses;
    private readonly List<ISynapse> _outputSynapses;

    public BasicNeuron(double threshold = 1.0)
    {
        Id = Guid.NewGuid();
        Threshold = threshold;
        Potential = 0.0;
        _inputSynapses = new List<ISynapse>();
        _outputSynapses = new List<ISynapse>();
    }

    public Guid Id { get; }
    public double Potential { get; set; }
    public double Threshold { get; set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<ISynapse> InputSynapses => _inputSynapses.AsReadOnly();
    public IReadOnlyCollection<ISynapse> OutputSynapses => _outputSynapses.AsReadOnly();

    /// <summary>
    /// Membrane time constant for leak integration.
    /// </summary>
    public double TimeConstant { get; set; } = 10.0;

    public void Update(double deltaTime)
    {
        // Leaky integration: exponential decay of membrane potential
        var decay = Math.Exp(-deltaTime / TimeConstant);
        Potential *= decay;

        // Sum input currents from synapses
        var inputCurrent = _inputSynapses.Sum(synapse => 
            synapse.PresynapticNeuron.IsActive ? synapse.Weight : 0.0);
        
        // Integrate current into membrane potential
        Potential += inputCurrent * deltaTime;

        // Check for spike generation
        if (Potential >= Threshold)
        {
            IsActive = true;
            Potential = 0.0; // Reset after spike
        }
        else
        {
            IsActive = false;
        }
    }

    public void Reset()
    {
        Potential = 0.0;
        IsActive = false;
    }

    public void AddInputSynapse(ISynapse synapse)
    {
        if (synapse.PostsynapticNeuron != this)
            throw new ArgumentException("Synapse postsynaptic neuron must be this neuron");
        
        _inputSynapses.Add(synapse);
    }

    public void AddOutputSynapse(ISynapse synapse)
    {
        if (synapse.PresynapticNeuron != this)
            throw new ArgumentException("Synapse presynaptic neuron must be this neuron");
        
        _outputSynapses.Add(synapse);
    }
}
