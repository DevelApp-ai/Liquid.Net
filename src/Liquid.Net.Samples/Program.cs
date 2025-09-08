using System;
using Liquid.Net.Core;
using Liquid.Net.Models;

namespace Liquid.Net.Samples;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Liquid.Net - Liquid Neural Network Demo");
        Console.WriteLine("=====================================");
        
        // Create a simple network with two neurons
        var neuron1 = new BasicNeuron(threshold: 1.0);
        var neuron2 = new BasicNeuron(threshold: 0.8);
        
        // Create a synapse connecting them
        var synapse = new BasicSynapse(neuron1, neuron2, initialWeight: 0.7);
        
        // Connect the synapse to the neurons
        neuron1.AddOutputSynapse(synapse);
        neuron2.AddInputSynapse(synapse);
        
        Console.WriteLine($"Created network with {2} neurons and {1} synapse");
        Console.WriteLine($"Neuron 1 ID: {neuron1.Id}");
        Console.WriteLine($"Neuron 2 ID: {neuron2.Id}");
        Console.WriteLine($"Synapse weight: {synapse.Weight}");
        
        // Simulate network activity
        Console.WriteLine("\nSimulating network dynamics...");
        
        for (int step = 0; step < 10; step++)
        {
            // Manually stimulate neuron 1 on some steps
            if (step % 3 == 0)
            {
                // Simulate external input by setting potential above threshold
                neuron1.Potential = 1.2; // Above threshold
            }
            
            // Update neurons
            neuron1.Update(0.1);
            neuron2.Update(0.1);
            
            Console.WriteLine($"Step {step + 1}: Neuron1 Active: {neuron1.IsActive}, " +
                            $"Neuron1 Potential: {neuron1.Potential:F3}, " +
                            $"Neuron2 Active: {neuron2.IsActive}, " +
                            $"Neuron2 Potential: {neuron2.Potential:F3}");
        }
        
        Console.WriteLine("\nDemo completed. The Liquid.Net framework is working!");
        Console.WriteLine("Next steps:");
        Console.WriteLine("- Add more sophisticated neuron models");
        Console.WriteLine("- Implement learning algorithms");
        Console.WriteLine("- Create larger network topologies");
        Console.WriteLine("- Add visualization capabilities");
    }
}
