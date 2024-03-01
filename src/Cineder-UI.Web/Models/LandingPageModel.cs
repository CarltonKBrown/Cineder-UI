namespace Cineder_UI.Web;

public class LandingPageModel
{
    public LandingPageModel(string searchText, int searchType)
    {
        SearchText = searchText;
        SearchType = searchType;
    }

    public LandingPageModel():this(string.Empty, 0)
    {
        
    }

    public string SearchText { get; set; } = string.Empty;
    public int SearchType { get; set; } = 0;
}

public class SearchType(string name, int value)
{
    public SearchType(): this(string.Empty, 0) {}

    public string Name { get; set; } = name;
    public int Value { get; set; } = value;
}