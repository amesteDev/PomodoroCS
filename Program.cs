using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;

/*
Pomodoro Timer by Morgan Andersson.
Done as part of the course Programmering i C#, given at Mittuniversitet 2020.
*/

namespace pomo
{
	class Program
	{

		static void Main(string[] args)
		{
			//setup DateTime for use in the app.
			DateTime localdate = DateTime.Now;
			int i = 0;
			while (true)
			{
				//simple menu-system, same approach used throughout the app with switch-statements.
				Console.CursorVisible = false;
				Console.Clear();
				Console.WriteLine("Välkommen, gör val enligt nedan");
				Console.WriteLine("1: Starta ny pomodorosession\n");
				Console.WriteLine("2: Se statistik\n");
				Console.WriteLine("q: Stäng applikationen\n");
				string input = Console.ReadLine().ToLower();
				switch (input)
				{
					case "1":
						Console.Clear();
						Console.CursorVisible = true;
						Pomodoro pomo = new Pomodoro();
						bool isNumber = false;
						int number;

						Console.WriteLine("Dags att ställa in dagens pomodoro\n");
						do
						{
							Console.WriteLine("How long should the workperiods be? (20-30 minutes) \n");
							string sessionLength = Console.ReadLine().ToLower();
							//TryParse to make sure that there are numbers entered.
							isNumber = Int32.TryParse(sessionLength, out number);
							if (isNumber && number >= 20 && number <= 30)
							{
								pomo.LengthOfWork = number;
							}
							else
							{
								isNumber = false;
								Console.WriteLine("Please enter a valid number between 20 and 30\n");
							}
						} while (!isNumber);

						Console.WriteLine("Title of this sprint of work?");
						string title = Console.ReadLine().ToLower();
						pomo.Title = title;

						do
						{
							Console.WriteLine("Viloperioderna? (3-5 minuter) \n");
							string restLength = Console.ReadLine().ToLower();
							isNumber = Int32.TryParse(restLength, out number);
							if (isNumber && number >= 1 && number <= 5)
							{
								pomo.LengthOfBreak = number;
								pomo.Date = localdate;
							}
							else
							{
								isNumber = false;
								Console.WriteLine("Du måste ange ett tal mellan 3 och 5! \n");
							}
						} while (!isNumber);
						Console.WriteLine("Starting pomodoro timer with theese settings: ");
						Console.WriteLine("Title: {0}", pomo.Title);
						Console.WriteLine("Focustime: {0}", pomo.LengthOfWork);
						Console.WriteLine("Breaktime: {0}", pomo.LengthOfBreak);
						Console.WriteLine("Date and time of this worksprint: {0}", pomo.Date);
						Console.WriteLine("Press any key to start");
						Console.ReadKey();
						startWorking(pomo);

						break;
					case "2":
						Console.Clear();
						//new isntance of the Pomos class.
						Pomos pom = new Pomos();

						Console.WriteLine("Totalt antal genomförda pomodoros: " + pom.GetPoms().Count);

						Console.CursorVisible = false;
						Console.Clear();
						Console.WriteLine("Välkommen, gör val enligt nedan");
						Console.WriteLine("1: Last week\n");
						Console.WriteLine("2: Last month\n");
						Console.WriteLine("3: All time\n");
						Console.WriteLine("q: Back to main menu\n");
						string inp = Console.ReadLine().ToLower();
						switch (inp)
						{
							case "1":

								Console.WriteLine("Last week: ");
								//loop out every Pomodor-object that is stored in the JSON.Based on the date it was stored with. (last 7 days)
								foreach (Pomodoro poms in pom.GetPoms())
								{
									if ((localdate - poms.Date).TotalDays < 7)
									{
										Console.WriteLine("[" + i++ + "] Datum: " + poms.Date + " Score: " + poms.Score + poms.LengthOfWork + " - " + poms.LengthOfBreak);
									}
								}
								Console.WriteLine("q: Press any key to return \n");
								Console.ReadLine().ToLower();
								break;
							case "2":

								Console.WriteLine("Last month: ");
								//loop out every Pomodor-object that is stored in the JSON. Based on the date it was stored with. (last 30 days)
								foreach (Pomodoro poms in pom.GetPoms())
								{
									if ((localdate - poms.Date).TotalDays < 30)
									{
										Console.WriteLine("[" + i++ + "] Datum: " + poms.Date + " Score: " + poms.Score + poms.LengthOfWork + " - " + poms.LengthOfBreak);
									}
								}
								Console.WriteLine("q: Press any key to return \n");
								Console.ReadLine().ToLower();
								break;
							case "3":
							//loop out ALL Pomodoro-object that is stored in the JSON.
								foreach (Pomodoro poms in pom.GetPoms())
								{
									Console.WriteLine("[" + i++ + "] Datum: " + poms.Date + " Score: " + poms.Score + poms.LengthOfWork + " - " + poms.LengthOfBreak);
								}
								Console.WriteLine("Press any key to return \n");
								Console.ReadLine().ToLower();
								break;
						}
						break;
					case "q":
						//exit
						Environment.Exit(0);
						break;
				}
			}
		}

		static void startWorking(Pomodoro inPom)
		{
			//this is the code for the actual timer.
			Console.Clear();
			Stopwatch stopWatch = new Stopwatch();
			int pomoCount = 0;
			bool onBreak = false;
			stopWatch.Start();
			while (pomoCount < 4)
			{
				//timespan/stopwatch used here to time working and break-timers
				TimeSpan ts = stopWatch.Elapsed;
				if (!onBreak)
				{
					Console.Write("\rTime to work!");
					string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
					ts.Hours, ts.Minutes, ts.Seconds + 1);
					Console.Write("\rArbetad tid: " + elapsedTime);
					//sleep for 1 second and then check if the time elapsed matches the time specified by the user. same approach used for both work-sprints and rest-periods.
					Thread.Sleep(1000);
					if (stopWatch.Elapsed.TotalMilliseconds > inPom.LengthOfWork * 10000)
					{
						pomoCount++;
						Console.Clear();
						Console.Beep();
						stopWatch.Reset();
						stopWatch.Start();
						onBreak = true;
					}
				}
				if ((onBreak) && (pomoCount <= 3))
				{
					ts = stopWatch.Elapsed;
					string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
					ts.Hours, ts.Minutes, ts.Seconds + 1);
					Console.Write("\rRastad tid: " + elapsedTime);
					Thread.Sleep(1000);
					if (stopWatch.Elapsed.TotalMilliseconds > inPom.LengthOfBreak * 10000)
					{
						Console.Clear();
						Console.Beep();
						Console.WriteLine("Dags att börja jobba igen, tryck på valfri tangent!");
						Console.ReadKey();
						stopWatch.Reset();
						stopWatch.Start();
						onBreak = false;
					}
				}
			}
			savePrompt(inPom);

		}
		static void savePrompt(Pomodoro inPom)
		{
			Pomos pom = new Pomos();
			Console.WriteLine("Du har nu kört 4 varv!");
			bool isNumber = false;
			int number;
			do
			{
				//allows the user to grade the completed pomodor-session.
				Console.WriteLine("What is the score for this sprint of work? (1-10)\n");
				string score = Console.ReadLine().ToLower();
				isNumber = Int32.TryParse(score, out number);
				if (isNumber && number >= 1 && number <= 10)
				{
					inPom.Score = number;
				}
				else
				{
					isNumber = false;
					Console.WriteLine("Du måste ange ett tal mellan 1 och 10! \n");
				}
			} while (!isNumber);
			//prompts the user to save the information.
			Console.WriteLine("Ok to save this? Y/n");
			Console.WriteLine(inPom.LengthOfWork + " " + inPom.LengthOfBreak + " " + inPom.Date + " " + inPom.Title + " " + inPom.Score);
			bool waiting = true;
			ConsoleKeyInfo cki;
			while (waiting)
			{
				cki = Console.ReadKey(true);
				if (cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.Y)
				{
					pom.addPomo(inPom);
					waiting = false;
				}
				if (cki.Key == ConsoleKey.Escape || cki.Key == ConsoleKey.N)
				{
					waiting = false;
				}
			}
		}
	}
}
