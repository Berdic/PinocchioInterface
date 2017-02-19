using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PinocchioInterface.Validators
{
    public class FileExistsValidator : ValidationRule
    {
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Please enter a valid path.");

            string path = (string)value;

            if (path == "")
                return new ValidationResult(true, null);

            if (System.IO.File.Exists(path))
            {
                return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(false, "Please enter a valid path.");
        }
    }
}
