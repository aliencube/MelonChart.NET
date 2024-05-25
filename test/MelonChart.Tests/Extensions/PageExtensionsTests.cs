using FluentAssertions;

using MelonChart.Extensions;

using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace MelonChart.Tests.Extensions;

[TestClass]
public class PageExtensionsTests : PageTest
{
    [TestInitialize]
    public async Task TestInitialize()
    {
        var html = await File.ReadAllTextAsync("./playwright.dev.dotnet.html");
        this.Page.SetDefaultTimeout(1000);
        await this.Page.SetContentAsync(html);
    }

    [TestMethod]
    public async Task Given_Null_When_Invoked_GetAttributeOfElementAsync_Then_It_Should_Throw_Exception()
    {
        var page = default(IPage);

        Func<Task> func = async () => await page.GetAttributeOfElementAsync("attribute", "selector");

        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task Given_Page_And_No_Selector_When_Invoked_GetAttributeOfElementAsync_Then_It_Should_Throw_Exception()
    {
        Func<Task> func = async () => await this.Page.GetAttributeOfElementAsync("attribute");

        await func.Should().ThrowAsync<ArgumentException>();
    }

    [DataTestMethod]
    [DataRow("false", "aria-expanded", "div[class='navbar__inner']", "div[class='navbar__items']", "button")]
    [DataRow(null, "aria-extended", "div[class='navbar__inner']", "div[class='navbar__items']", "button")]
    public async Task Given_Attribute_And_Selectors_When_Invoked_GetAttributeOfElementAsync_Then_It_Should_Return_Result(string expected, string attribute, params string[] selectors)
    {
        var result = await this.Page.GetAttributeOfElementAsync(attribute, selectors);

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("aria-expanded", "div[class='navbar__inner']", "div[class='navbar__items']", "article")]
    public async Task Given_Attribute_And_NonExisting_Selectors_When_Invoked_GetAttributeOfElementAsync_Then_It_Should_Throw_Exception(string attribute, params string[] selectors)
    {
        Func<Task> func = async () => await this.Page.GetAttributeOfElementAsync(attribute, selectors);

        await func.Should().ThrowAsync<TimeoutException>();
    }

    [TestMethod]
    public async Task Given_Null_When_Invoked_GetAttributeOfNthElementAsync_Then_It_Should_Throw_Exception()
    {
        var page = default(IPage);

        Func<Task> func = async () => await page.GetAttributeOfNthElementAsync("attribute", 0, "selector");

        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task Given_Page_And_No_Selector_When_Invoked_GetAttributeOfNthElementAsync_Then_It_Should_Throw_Exception()
    {
        Func<Task> func = async () => await this.Page.GetAttributeOfNthElementAsync("attribute", 0);

        await func.Should().ThrowAsync<ArgumentException>();
    }

    [DataTestMethod]
    [DataRow("/dotnet/", "href", 0, "div[class='navbar__inner']", "div[class='navbar__items']", "a")]
    [DataRow(null, "title", 0, "div[class='navbar__inner']", "div[class='navbar__items']", "button")]
    public async Task Given_Attribute_And_Index_And_Selectors_When_Invoked_GetAttributeOfElementAsync_Then_It_Should_Return_Result(string expected, string attribute, int index, params string[] selectors)
    {
        var result = await this.Page.GetAttributeOfNthElementAsync(attribute, index, selectors);

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("aria-expanded", 0, "div[class='navbar__inner']", "div[class='navbar__items']", "article")]
    public async Task Given_Attribute_And_Index_And_NonExisting_Selectors_When_Invoked_GetAttributeOfElementAsync_Then_It_Should_Throw_Exception(string attribute, int index, params string[] selectors)
    {
        Func<Task> func = async () => await this.Page.GetAttributeOfNthElementAsync(attribute, index, selectors);

        await func.Should().ThrowAsync<TimeoutException>();
    }

    [TestMethod]
    public async Task Given_Null_When_Invoked_GetTextOfElementAsync_Then_It_Should_Throw_Exception()
    {
        var page = default(IPage);

        Func<Task> func = async () => await page.GetTextOfElementAsync("selector");

        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task Given_Page_And_No_Selector_When_Invoked_GetTextOfElementAsync_Then_It_Should_Throw_Exception()
    {
        Func<Task> func = async () => await this.Page.GetTextOfElementAsync().ConfigureAwait(false);

        await func.Should().ThrowAsync<ArgumentException>();
    }

    [DataTestMethod]
    [DataRow("Playwright for .NET", "div[class='navbar__inner']", "div[class='navbar__items']", "b")]
    [DataRow("", "div[class='navbar__inner']", "div[class='navbar__items']", "path")]
    public async Task Given_Attribute_And_Selectors_When_Invoked_GetTextOfElementAsync_Then_It_Should_Return_Result(string expected, params string[] selectors)
    {
        var result = await this.Page.GetTextOfElementAsync(selectors);

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("div[class='navbar__inner']", "div[class='navbar__items']", "article")]
    public async Task Given_Attribute_And_NonExisting_Selectors_When_Invoked_GetTextOfElementAsync_Then_It_Should_Throw_Exception(params string[] selectors)
    {
        Func<Task> func = async () => await this.Page.GetTextOfElementAsync(selectors);

        await func.Should().ThrowAsync<TimeoutException>();
    }

    [TestMethod]
    public async Task Given_Null_When_Invoked_GetTextOfNthElementAsync_Then_It_Should_Throw_Exception()
    {
        var page = default(IPage);

        Func<Task> func = async () => await page.GetTextOfNthElementAsync(0, "selector");

        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task Given_Page_And_No_Selector_When_Invoked_GetTextOfNthElementAsync_Then_It_Should_Throw_Exception()
    {
        Func<Task> func = async () => await this.Page.GetTextOfNthElementAsync(0);

        await func.Should().ThrowAsync<ArgumentException>();
    }

    [DataTestMethod]
    [DataRow("Docs", 0, "div[class='navbar__inner']", "div[class='navbar__items']", "a[class='navbar__item navbar__link']")]
    public async Task Given_Attribute_And_Index_And_Selectors_When_Invoked_GetTextOfNthElementAsync_Then_It_Should_Return_Result(string expected, int index, params string[] selectors)
    {
        var result = await this.Page.GetTextOfNthElementAsync(index, selectors);

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow(0, "div[class='navbar__inner']", "div[class='navbar__items']", "article")]
    public async Task Given_Attribute_And_Index_And_NonExisting_Selectors_When_Invoked_GetTextOfNthElementAsync_Then_It_Should_Throw_Exception(int index, params string[] selectors)
    {
        Func<Task> func = async () => await this.Page.GetTextOfNthElementAsync(index, selectors);

        await func.Should().ThrowAsync<TimeoutException>();
    }
}