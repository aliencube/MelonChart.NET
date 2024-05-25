using FluentAssertions;

using MelonChart.Extensions;

namespace MelonChart.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [DataTestMethod]
    [DataRow("2024-02-05", "2024-02-05")]
    [DataRow("2024-2-5", "2024-02-05")]
    [DataRow("02-05-2024", "2024-02-05")]
    [DataRow("2-5-2024", "2024-02-05")]
    [DataRow("2024.05.01", "2024-05-01")]
    [DataRow("2024.5.1", "2024-05-01")]
    [DataRow("05.01.2024", "2024-05-01")]
    [DataRow("5.1.2024", "2024-05-01")]
    [DataRow("2024/03/04", "2024-03-04")]
    [DataRow("2024/3/4", "2024-03-04")]
    [DataRow("05/06/2024", "2024-05-06")]
    [DataRow("5/6/2024", "2024-05-06")]
    [DataRow("31/05/2024", null)]
    public async Task Given_DateValue_When_Invoked_ToDateOnly_Then_It_Should_Return_Result(string value, string expected)
    {
        var instance = Task.FromResult((string?)value);

        var result = await instance.ToDateOnly().ConfigureAwait(false);

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("02:05:09", "02:05:09")]
    [DataRow("2:5:9", "02:05:09")]
    [DataRow("02:09", "02:09:00")]
    [DataRow("2:9", "02:09:00")]
    [DataRow("03.05", "03:05:00")]
    [DataRow("3.5", "03:05:00")]
    [DataRow("3-30", null)]
    [DataRow("03,45", null)]
    public async Task Given_TimeValue_When_Invoked_ToDateOnly_Then_It_Should_Return_Result(string value, string expected)
    {
        var instance = Task.FromResult((string?)value);

        var result = await instance.ToTimeOnly().ConfigureAwait(false);

        result.Should().Be(expected);
    }
}