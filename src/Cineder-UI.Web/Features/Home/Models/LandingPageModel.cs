namespace Features.Home.Models;

public class LandingPageModel
{
    public LandingPageModel(string searchText, int searchType)
    {
        SearchText = searchText;
        SearchType = searchType;
    }

    public LandingPageModel() : this(string.Empty, 0){}

    public string SearchText { get; set; }
    public int SearchType { get; set; }
}