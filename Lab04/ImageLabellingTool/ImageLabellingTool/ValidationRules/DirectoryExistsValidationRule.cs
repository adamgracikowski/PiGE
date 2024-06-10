using System.IO;
using System.Windows.Controls;

namespace ImageLabellingTool
{
    public class DirectoryExistsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "Directory path can't be empty.");
            else if (!Directory.Exists(value.ToString()))
                return new ValidationResult(false, "Given directory does not exist.");
            return ValidationResult.ValidResult;
        }
    }
}
