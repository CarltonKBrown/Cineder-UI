using Bunit;
using Cineder_UI.Web.Components;
using Xunit;


namespace Cineder_UI.UnitTests.ComponentTests
{
    public class MainFooterTests : TestContext
    {
        [Fact]
        public void MainFooter_ShouldRenderAuthorText_Correctly()
        {
            // Act
            var cut = RenderComponent<MainFooter>();

            var actual = cut.Find("#footer-author");

            var expected = @"<p id=""footer-author"" class=""lh-sm mb-0"">
							<small>2024 © Carlton K. Brown</small>
						</p>";

            // Assert
            actual.MarkupMatches(expected);
        }

        [Fact]
        public void MainFooter_ShouldRenderApiLogo_Correctly()
        {
            // Act
            var cut = RenderComponent<MainFooter>();

            var actual = cut.Find("#footer-api-logo");

            var expected = @"<p id=""footer-api-logo"" class=""lh-sm footer-text mb-0"">
							<small>
								Powered by:
								<a href=""https://www.themoviedb.org/"" target=""_blank"">
									<img src=""img/themoviedblogo.svg"" style=""height: 40px; width: 40px;"" alt="""">
								</a>
							</small>
						</p>";

            // Assert
            actual.MarkupMatches(expected);
        }

        [Fact]
        public void MainFooter_SouldRenderDisclaimer_Correctly()
        {
            // Act
            var cut = RenderComponent<MainFooter>();

            var actual = cut.Find("#footer-disclaimer");

            var expected = @"<p id=""footer-disclaimer"" class=""lh-sm footer-text mb-0 text-center"">
							<small>This product uses the TMDb API but is not endorsed or certified by TMDb.</small>
						</p>";

            // Assert
            actual.MarkupMatches(expected);
        }
    }
}
