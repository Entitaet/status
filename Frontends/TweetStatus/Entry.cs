using System;

namespace TweetStatus
{
    public class Entry
    {
        public int EntityCount { get; private set; }

        public Entry(int entityCount)
		{
            EntityCount=entityCount;
		}

		public Entry(string lineFromFile)
		{
			string[] parts=lineFromFile.Split(new char[]{'\t'}, StringSplitOptions.None);
            EntityCount=Convert.ToInt32(parts[0]);
		}

		public static bool operator==(Entry a, Entry b)
		{
            if(a.EntityCount==b.EntityCount)
            {
                return true;
            }

			return false;
		} 

		public static bool operator!=(Entry a, Entry b)
		{
			return !(a==b);
		} 

		public override string ToString()
		{
            return String.Format("{0}", EntityCount);
		}
    }
}

