using OnlineConsultant.Models;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace OnlineConsultant
{
    public class KeywordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var form = (RegistrationModel)validationContext.ObjectInstance;
            var word = ConfigurationManager.AppSettings["keyword"].ToUpper();

            if(form.Keyword != null && form.Keyword.ToUpper() == word)
                return ValidationResult.Success;

            return new ValidationResult("Не верно указано ключевое слово");
        }
    }
}