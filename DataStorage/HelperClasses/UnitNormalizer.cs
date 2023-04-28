#region

using System.Text.RegularExpressions;
using UnitsNet;
using UnitsNet.Units;

#endregion

namespace DataStorage.HelperClasses;

public class UnitNormalizer
{
    private static readonly Regex Regex = new(@"([0-9]*)(\D*)", RegexOptions.Compiled);

    private static readonly Dictionary<string, double> _unitWithFactor = new()
    {
        { "gal", 3785.41 },
        { "tbsp", 14.7868 },
        { "floz", 29.5735 },
        { "cup", 236.588 },
        { "pt", 568.26 },
        { "qt", 946.353 }
    };

    public static string NormalizeQuantity(string? quantityString)
    {
        quantityString = quantityString?.ToLower().Replace(" ", "").Replace('.', ',') ?? string.Empty;

        if (Quantity.TryParse(typeof(Volume), quantityString, out var quantityValue))
        {
            var ml = quantityValue.ToUnit(VolumeUnit.Milliliter).ToString();
            return ml?.Replace(".", "") ?? quantityString;
        }

        var match = Regex.Match(quantityString);

        if (match.Success && match.Groups.Count > 1)
        {
            var quantOld = int.Parse(match.Groups[1].Value);
            var unitOld = match.Groups[2].Value;

            if (!_unitWithFactor.ContainsKey(unitOld)) return quantityString;

            var quantNew = quantOld * _unitWithFactor[unitOld];

            return quantNew + " ml";
        }

        return quantityString;
    }
}