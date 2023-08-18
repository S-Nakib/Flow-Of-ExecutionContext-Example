using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowOfExecutionContext;

public static class TaskRunner
{
    public static async Task UpdateExecutionContextExample()
    {
        Console.WriteLine("\nThis example shows what happens when some tasU updates/reassigns some data on ExecutionContext\n");

        //NestedData is a static AsyncLocal Type of the DummyData Class, so the value will be stored on ExecutionContext 
        DummyData.NestedData.Value = new NestedData()
        {
            Id = 1,
            Type = "First"
        };

        Console.WriteLine($"\nOn the parent Task, Id: {DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");

        var child1 = Task.Run(async () =>
        {
            //Updating/Reassigning ExecutionContext's Data
            DummyData.NestedData.Value = new NestedData()
            {
                Id = 2,
                Type = "Second"
            };

            Console.WriteLine($"\nInside Task c1. ExecutionContext's Data is Updated/Reassigned. Id: " +
                              $"{DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");

            var child11 = Task.Run(() =>
            {
                Console.WriteLine($"\nInside task c11 which is a child of task c1. c11 will get the update made by c1.\n" +
                                  $"Since it is a child of c1, it gets the same updated ExecutionContext.\n" +
                                  $"Id: {DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");
            });
            await child11;
        });

        await child1;

        var child2 = Task.Run(() =>
        {
            Console.WriteLine($"\nUpdate of ExecutionContext in c1 task doesn't affect c2.\n" +
                              $"Because ExecutionContext is immutable and c2 isn't a child of c1.\n" +
                              $"Id: {DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");
        });

        await Task.WhenAll(child1, child2);
    }


    public static async Task MutateExecutionContextExample()
    {
        Console.WriteLine("\nThis example shows what happens when some task mutates the data on ExecutionContext\n");

        //NestedData is a static AsyncLocal Type of the DummyData Class, so the value will be stored on ExecutionContext 
        DummyData.NestedData.Value = new NestedData()
        {
            Id = 1,
            Type = "First"
        };

        Console.WriteLine($"\nOn the parent Task, Id: {DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");
        

        var child1 = Task.Run(async() =>
        {
            //Mutating ExecutionContext's Data
            DummyData.NestedData.Value.Id = 2;
            DummyData.NestedData.Value.Type = "Second";

            Console.WriteLine($"\nInside Task c1. ExecutionContext's Data is Mutated,\n" +
                              $"Id: {DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");

            var child11 = Task.Run(() =>
            {
                Console.WriteLine($"\nInside task c11 which is a child of task c1. c11 will get the Mutation made by c1.\n" +
                                  $"Since it is a child of c1, it gets the same Mutated ExecutionContext.\n" +
                                  $"Id: {DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");
            });
            await child11;
        });

        await child1;

        var child2 = Task.Run(() =>
        {
            Console.WriteLine($"\nMutation of ExecutionContext's Data in c1 affect c2.\n" +
                              $"Because, though ExecutionContext is immutable the DummyData Class isn't.\n" +
                              $"Id: {DummyData.NestedData.Value.Id}, Type: {DummyData.NestedData.Value.Type}\n");
        });

        await Task.WhenAll(child1, child2);
    }
}