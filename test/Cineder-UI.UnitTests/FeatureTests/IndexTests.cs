using Bunit;
using Cineder_UI.Web;
using Cineder_UI.Web.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Cineder_UI.UnitTests.FeatureTests
{
    public class IndexTests : TestContext
    {
        [Fact]
        public void Index_MovieSearch_StateShouldMatchUserInput()
        {
            var cut = RenderComponent<Web.Features.Home.Index>();

            var textField = cut.Find("#search-text");

            textField.Change("Movie Search");

            var actual = cut.Instance.Model;

            var expected = new LandingPageModel("Movie Search", 0);

            Assert.Equal(expected.SearchText, actual!.SearchText);
            Assert.Equal(expected.SearchType, actual!.SearchType);
        }

        [Fact]
        public void Index_SeriesSearch_StateShouldMatchUserInput()
        {
            var cut = RenderComponent<Web.Features.Home.Index>();

            var textField = cut.Find("#search-text");

            var seriesSearchOption = cut.Find("[name=search-type]");

            textField.Change("Series Search");

            seriesSearchOption.Change(1);  

            var actual = cut.Instance.Model;

            var expected = new LandingPageModel("Series Search", 1);

            Assert.Equal(expected.SearchText, actual!.SearchText);
            Assert.Equal(expected.SearchType, actual!.SearchType);
        }
    }
}
