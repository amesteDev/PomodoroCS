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
			//if there is a posts.json open it and deserialize it to objects that can be understood by C# and add them to the List
			if (File.Exists(@"poms.json") == true)
			{
				string jsonString = File.ReadAllText(@"poms.json");
				poms = JsonSerializer.Deserialize<List<Pomodoro>>(jsonString);
			}
		}

		public List<Pomodoro> GetPoms()
		{
			return poms;
		}

		public Pomodoro addPomo(Pomodoro pom)
		{
			poms.Add(pom);
			writeToFile();
			return pom;
		}

		private void writeToFile()
		{
			//if there is not a posts.json file, create it first
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

		public List<Pomodoro> afterTwelveCalc(){
			
			List<Pomodoro> afterTwelve = new List<Pomodoro>();
			foreach (Pomodoro pomos in poms)
			{
				if(pomos.Date.Hour > 12 )
				{
					afterTwelve.Add(pomos);
				}

			}
			return afterTwelve;
		}

		public List<Pomodoro> beforeTwelveCalc()
		{
			List<Pomodoro> beforeTwelve = new List<Pomodoro>();
			foreach (Pomodoro pomos in poms)
			{
			if(pomos.Date.Hour <= 12)
			{
				beforeTwelve.Add(pomos);
			}
			}
			return beforeTwelve;
		}
	}
}