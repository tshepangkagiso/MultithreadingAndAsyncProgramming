using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section_5
{
    class AsyncAwaitTaskOperations
    {


        public static void TaskHttpCall()
        {
            const string url = "https://pokeapi.co/api/v2/pokemon";
            using var client = new HttpClient();
            var task = client.GetStringAsync(url);

            task.ContinueWith(t =>
            {
                if (t.IsFaulted && t.Exception.InnerExceptions != null)
                {
                    foreach (var ex in t.Exception.InnerExceptions)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    var result = t.Result; //blocking
                    var doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;
                    JsonElement results = root.GetProperty("results");
                    JsonElement firstPokemon = results[0];

                    Console.WriteLine($"First Pokemon: {firstPokemon.GetProperty("name")}");
                    Console.WriteLine($"First url: {firstPokemon.GetProperty("url")}");
                }
            });

            //main thread
            Console.WriteLine("This is the end of the program.");
            Console.ReadLine();
        }


        public async static void TaskHttpCallAsync()
        {
            const string url = "https://pokeapi.co/api/v2/pokemon";
            using var client = new HttpClient();
            var task = client.GetStringAsync(url);
            var result = await task; // non-blocking

            var doc = JsonDocument.Parse(result);
            JsonElement root = doc.RootElement;
            JsonElement results = root.GetProperty("results");
            JsonElement firstPokemon = results[0];

            Console.WriteLine($"First Pokemon: {firstPokemon.GetProperty("name")}");
            Console.WriteLine($"First url: {firstPokemon.GetProperty("url")}");

            Console.WriteLine("This is the end of the program.");
        }
    }
}
