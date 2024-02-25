using ClipUp.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClipUp.Shared.Tools.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueValueValidationAttribute : ValidationAttribute
    {
        private string GetErrorMessage(object value) => $"Значение {value} занято"; 
        private string _field { get; set; }
        private string _table { get; set; }
        private string _errorMessage { get; set; }

        public UniqueValueValidationAttribute(string field, string table, string errorMessage = null!)
        {
            _field = field;
            _table = table;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            ApplicationContext? applicationContext = validationContext
                .GetService<ApplicationContext>();
            string sql = $"SELECT \"{_field}\" FROM \"{_table}\"";
            Console.WriteLine(sql);
            string? result = applicationContext!.Database
                .SqlQueryRaw<string>(sql).ToList().Where(email => email == value!.ToString()).FirstOrDefault();
            if (result != null)
            {
                string? message = _errorMessage == null ? GetErrorMessage(value!) : _errorMessage;
                return new ValidationResult(message);
            }
            return ValidationResult.Success;
        }
    }
}
