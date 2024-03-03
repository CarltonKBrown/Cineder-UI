﻿using Xunit;
using Bunit;
using Cineder_UI.Web.Components;
namespace Cineder_UI.UnitTests.ComponentTests
{

    public class MainNavBarTests : TestContext
    {
        [Fact]
        public void MainNavBar_ShouldRenderImageCorectly()
        {
            // Act
            var cut = RenderComponent<MainNavBar>();

            var actual = cut.Find("#navbar-image");

            var expected = @"<img id=""navbar-image"" src=""img/favicon.png"" />";

            // Assert
            actual.MarkupMatches(expected);

        }

        [Fact]
        public void MainNavBar_ShouldRenderTextCorectly()
        {
            // Act
            var cut = RenderComponent<MainNavBar>();

            var actual = cut.Find("#navbar-text");

            var expected = @"<span id=""navbar-text"" class=""text-white"">Cineder</span>";

            // Assert
            actual.MarkupMatches(expected);

        }

        [Fact]
        public void MainNavBar_ShouldRenderLinkCorectly()
        {
            // Act
            var cut = RenderComponent<MainNavBar>();

            var actual = cut.Find("#navbar-link");

            var expected = @"<a id=""navbar-link"" class=""navbar-brand"" href=""#"">
                <img id=""navbar-image"" src=""img/favicon.png"" />
                <span id=""navbar-text"" class=""text-white"">Cineder</span>
            </a>";

            // Assert
            actual.MarkupMatches(expected);

        }
    }
}
