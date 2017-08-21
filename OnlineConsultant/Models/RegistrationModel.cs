using System.ComponentModel.DataAnnotations;

namespace OnlineConsultant.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Не заполнено поле")]
        [Display(Name ="Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Не заполнено поле")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не заполнено поле")]
        [Display(Name = "Отчество")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не заполнено поле")]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не заполнено поле")]
        [Display(Name="Ваш вопрос спецалисту")]
        public string Question { get; set; }


        [Required(ErrorMessage = "Не заполнено поле")]
        [Keyword]
        [Display(Name = "Ключевое слово")]
        public string Keyword { get; set; }
    }
}