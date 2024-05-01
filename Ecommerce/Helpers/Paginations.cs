namespace Ecommerce.APIs.Helpers
{
	public class Paginations<T>
	{
		
        public MetaData metaData { get; set; }

		public Paginations(int pageSize, int pageIndex, int resCount ,int count, IReadOnlyList<T> data)
		{
			metaData = new MetaData
			{
				PageSize = pageSize,
				PageIndex = pageIndex,
				resultCount = resCount,
				
			};
			Count = count;
			Data = data;
		}

		public int Count { get; set; }
		public IReadOnlyList<T> Data { get; set; }

    }
}
