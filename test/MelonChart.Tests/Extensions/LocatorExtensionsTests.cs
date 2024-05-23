using FluentAssertions;

using MelonChart.Extensions;

namespace MelonChart.Tests.Extensions;

[TestClass]
public class LocatorExtensionsTests
{
    [DataTestMethod]
    [DataRow("2024-02-15", "2024-02-15")]
    [DataRow("02-15-2024", "2024-02-15")]
    [DataRow("2-15-2024", "2024-02-15")]
    [DataRow("2024.5.01", "2024-05-01")]
    [DataRow("5.01.2024", "2024-05-01")]
    [DataRow("2024/3/4", "2024-03-04")]
    [DataRow("05/31/2024", "2024-05-31")]
    [DataRow("5/31/2024", "2024-05-31")]
    [DataRow("6/5/2024", "2024-06-05")]
    public void Given_DateValue_When_Invoked_ToDateOnly_Then_It_Should_Return_Result(string value, string expected)
    {
        var result = value.ToDateOnly();

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("31/05/2024")]
    public void Given_DateValue_When_Invoked_ToDateOnly_Then_It_Should_Return_NULL(string value)
    {
        var result = value.ToDateOnly();

        result.Should().BeNull();
    }

    [DataTestMethod]
    [DataRow("2:15", "02:15:00")]
    public void Given_TimeValue_When_Invoked_ToDateOnly_Then_It_Should_Return_Result(string value, string expected)
    {
        var result = value.ToTimeOnly();

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("3.15")]
    [DataRow("3-30")]
    [DataRow("03,45")]
    public void Given_TimeValue_When_Invoked_ToDateOnly_Then_It_Should_Return_NULL(string value)
    {
        var result = value.ToTimeOnly();

        result.Should().BeNull();
    }
}