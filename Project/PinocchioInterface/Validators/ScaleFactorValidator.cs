using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PinocchioInterface.Validators
{
    public class ScaleFactorValidator : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Field needs to be a decimal number.");

            string factor = (string)value;

            if (factor == "")
                return new ValidationResult(false, "Field needs to be a decimal number.");

            if (System.Text.RegularExpressions.Regex.IsMatch(factor, "^[0-9]+([.,][0-9]*)?$"))
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(factor, "^0+[.,]*0*$")) 
                    return new ValidationResult(false, "Scale factor can't be 0.");
                else
                    return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(false, "Field needs to be a decimal number.");
        }
    }
}
