#region

using DataStorage.HelperClasses;

#endregion

namespace BierBock.Tests;

public class UnitTestUnitConverter
{
    [Theory]
    [InlineData("3000ml", "3000 ml")]
    [InlineData("300 cl", "3000 ml")]
    [InlineData("1.5 l", "1500 ml")]
    [InlineData("2 gal", "7570,82 ml")]
    [InlineData("1 tsp", "5 ml")]
    [InlineData("1 tbsp", "14,7868 ml")]
    [InlineData("1 fl oz", "29,5735 ml")]
    [InlineData("1 cup", "236,588 ml")]
    [InlineData("1 pt", "568,26 ml")]
    [InlineData("1 qt", "946,353 ml")]
    public void GivenQuantity_ShouldParseToMl(string input, string expectedOutput)
    {
        // Act
        var result = UnitNormalizer.NormalizeQuantity(input);

        // Assert
        Assert.Equal(expectedOutput, result);
    }
}