using System;

namespace Second_example
{
	public static class MyLINQ
	{
		public static int Count<T>(IEnumerable<string> sequence)
		{
			int count = 0;
			foreach (var item in sequence)
				count += 1;

			return count;
		}
	}
}