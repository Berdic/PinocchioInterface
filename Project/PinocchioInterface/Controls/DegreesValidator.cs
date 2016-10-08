using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace PinocchioInterface
{
    public class DegreesValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, null);

            string degrees = (string)value;

            if (degrees == "")
                return new ValidationResult(false, "Field must be a number between 0 and 359.");

            if (System.Text.RegularExpressions.Regex.IsMatch(degrees, "^[0-9]*$"))
            {
                int degreesInt = Convert.ToInt16(degrees);
                if (degreesInt > -1 && degreesInt < 360)
                    return new ValidationResult(true, null);
                else
                    return new ValidationResult(false, "Field must be a number between 0 and 359.");
            }
            else
                return new ValidationResult(false, "Field must be a number between 0 and 359.");
        }
    }

    public class ScaleFactorValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Field needs to be a decimal number.");

            string factor = (string)value;

            if (factor == "")
                return new ValidationResult(false, "Field needs to be a decimal number.");

            if (System.Text.RegularExpressions.Regex.IsMatch(factor, "^[0-9]*([.,][0-9]*)?$"))
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(factor, "^0+$"))
                    return new ValidationResult(false, "Scale factor can't be 0.");
                else
                    return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(false, "Field needs to be a decimal number.");
        }
    }

    public class FileExistsValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Please enter a valid path.");

            string path = (string)value;

            if (path == "")
                return new ValidationResult(false, "Please enter a valid path.");

            if (System.IO.File.Exists(path))
            {
                return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(false, "Please enter a valid path.");
        }
    }
}
