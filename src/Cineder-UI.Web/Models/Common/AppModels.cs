using System.Runtime.Serialization;

namespace Cineder_UI.Web.Models.Common
{
    public enum SiteMode
    {
        None,
        Movie,
        Series
    }

    public enum SortOptions
    {
		[EnumMember(Value = "Sort")]
        None,
        [EnumMember(Value = "A - Z")]
        AlphaAsc,
		[EnumMember(Value = "Z - A")]
		AlphaDesc,
		[EnumMember(Value = "Year - Asc")]
		YearAsc,
		[EnumMember(Value = "Year - Desc")]
		YearDesc,
		[EnumMember(Value = "Ratings - Asc")]
		RatingsAsc,
		[EnumMember(Value = "Ratings - Desc")]
		RatingsDesc
	}

	public record FilterOption(string Name, int Value)
	{
        public FilterOption(): this(string.Empty, 0){}

		public static FilterOption FromSortOptions(SortOptions sortOptions)
		{
			return sortOptions switch
			{
				SortOptions.AlphaAsc => new("A - Z", (int)SortOptions.AlphaAsc),
				SortOptions.AlphaDesc => new("Z - A", (int)SortOptions.AlphaDesc),
				SortOptions.YearAsc => new("Year - Asc", (int) SortOptions.YearAsc),
				SortOptions.YearDesc => new("Year - Desc", (int)SortOptions.YearDesc),
				SortOptions.RatingsAsc => new("Ratings - Asc", (int) SortOptions.RatingsAsc),
				SortOptions.RatingsDesc => new("Ratings - Desc", (int) SortOptions.RatingsDesc),
				_ or SortOptions.None => new("Sort", 0)
			};
		}
    }
}
