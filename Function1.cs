using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;

namespace StackCalculatorAzureFxn
{
    #region Default Code
    /*
    public static class Function1
    {
        static Stack<decimal> stack = new Stack<decimal>();

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
    */
    #endregion

    public class StackCalculator
    {
        Stack<decimal> stack = new Stack<decimal>();
        StringBuilder result = new StringBuilder();

        public string Calculate(string[] commands)
        {
            foreach (var command in commands)
            {
                var splits = command.Split(' ');

                if (splits.Length > 0)
                {
                    switch (splits[0])
                    {
                        case "PUSH":
                            Push(splits);
                            break;
                        case "POP":
                            Pop();
                            break;
                        case "PRINT":
                            Print();
                            break;
                        case "PRINTALL":
                            PrintAll();
                            break;
                        case "ADD":
                            Add();
                            break;
                        case "SUBTRACT":
                            Subtract();
                            break;
                        case "MULTIPLY":
                            Multiply();
                            break;
                        case "DIVIDE":
                            Divide();
                            break;
                        default:
                            Console.WriteLine("Unknown command");
                            break;
                    }
                }
            }
            return result.ToString();
        }

        void Push(string[] splits)
        {
            if (splits.Length > 1)
            {
                var number = decimal.Parse(splits[1]);
                stack.Push(number);
            }
        }
        void Pop()
        {
            if (stack.Count > 0)
            {
                var popped = stack.Pop();
                result.AppendLine(popped.ToString());
            }
        }
        void Print()
        {
            if (stack.Count > 0)
            {
                result.AppendLine(stack.Peek().ToString());
            }
        }
        void PrintAll()
        {
            if (stack.Count > 0)
            {
                for(int i = 0; i < stack.Count; i++)
                {
                    var stackArray = stack.ToArray();
                    result.AppendLine("#" + (i + 1) + ": " + stackArray[i].ToString());
                }
            }
        }
    
        void Add()
        {
            if (stack.Count > 1)
            {
                var pop1 = stack.Pop();
                var pop2 = stack.Pop();
                var result = pop2 + pop1;
                stack.Push(result);
            }
            else
            {
                result.AppendLine("Error: could not Add - stack only has " + stack.Count + " items.");
            }
        }
        void Subtract()
        {
            if (stack.Count > 1)
            {
                var pop1 = stack.Pop();
                var pop2 = stack.Pop();
                var result = pop2 - pop1;
                stack.Push(result);
            }
            else
            {
                result.AppendLine("Error: could not Subtract - stack only has " + stack.Count + " items.");
            }
        }
        void Multiply()
        {
            if (stack.Count > 1)
            {
                var pop1 = stack.Pop();
                var pop2 = stack.Pop();
                var result = pop2 * pop1;
                stack.Push(result);
            }
            else
            {
                result.AppendLine("Error: could not Multiply - stack only has " + stack.Count + " items.");
            }
        }
        void Divide()
        {
            if (stack.Count > 1)
            {
                var pop1 = stack.Pop();
                var pop2 = stack.Pop();
                var result = pop2 / pop1;
                stack.Push(result);
            }
            else
            {
                result.AppendLine("Error: could not Divide - stack only has " + stack.Count + " items.");
            }
        }
    }

    public static class JennyStackCalculator
    {
        [FunctionName("JennyStackCalculator")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(req.Body))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    Console.WriteLine(line);
                    lines.Add(line);
                }
            }

            var calculator = new StackCalculator();
            var response = calculator.Calculate(lines.ToArray());

            return new OkObjectResult(response);
        }
    }



}
