using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.Json;

namespace pomo
{
	public class Pomos
	{
		private List<Pomodoro> poms = new List<Pomodoro>();
		public Pomos()
		{
			//if there is a poms.json open it and deserialize it to objects that can be understood by C# and add them to the List
			if (File.Exists(@"poms.json") == true)
			{
				string jsonString = File.ReadAllText(@"poms.json");
				poms = JsonSerializer.Deserialize<List<Pomodoro>>(jsonString);
			}
		}
		//return complete list of saved pomodoros that are loaded from the JSON above.
		public List<Pomodoro> GetPoms()
		{
			return poms;
		}
		//method to add pomodoros
		public Pomodoro addPomo(Pomodoro pom)
		{
			poms.Add(pom);
			writeToFile();
			return pom;
		}
		//write data to the JSON.
		private void writeToFile()
		{
			//if there is not a poms.json file, create it first
			if (File.Exists(@"poms.json") != true)
			{
				File.Create(@"poms.json").Dispose();
			}
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
			};
			string jsonString = JsonSerializer.Serialize(poms, options);
			File.WriteAllText(@"poms.json", jsonString);
		}
	}
}