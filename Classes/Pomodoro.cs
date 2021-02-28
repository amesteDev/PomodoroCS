using System;

namespace pomo
{
	public class Pomodoro
	{

		private int id;
		public int Id		{
			set { this.id = value; }
			get { return this.id; }
		}

		private int lengthOfWork;
		public int LengthOfWork
		{
			set { this.lengthOfWork = value; }
			get { return this.lengthOfWork; }
		}

		private int lengthOfBreak;
		public int LengthOfBreak
		{
			set { this.lengthOfBreak = value; }
			get { return this.lengthOfBreak; }
		}

		private DateTime date;
		public DateTime Date
		{
			set { this.date = value; }
			get { return this.date; }
		}

		private string title;
		public string Title
		{
			set { this.title = value; }
			get { return this.title; }
		}

		private int score;
		public int Score
		{
			set { this.score = value; }
			get { return this.score; }
		}

	}
}