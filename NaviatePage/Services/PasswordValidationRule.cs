using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NaviatePage.Services
{
    public class PasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = value as string;

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return new ValidationResult(false, "Mật khẩu phải dài ít nhất 6 ký tự.");
            }

            return ValidationResult.ValidResult;
        }
    }
}