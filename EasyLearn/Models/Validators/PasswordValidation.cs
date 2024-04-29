using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EasyLearn.Models.Validators;

public class PasswordValidation : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var password = value as string;
        if (string.IsNullOrWhiteSpace(password)) return false;
        if (password.Length < 8) return false;
        if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")) return false;
        return true;
    }

}
