using System.ComponentModel;
using WebApplication2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication2.Models
{
    public class Klienci
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }



        [DisplayName("Imię")]
        [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź]+$", ErrorMessage = "Imię może zawierać tylko litery.")]
        [StringLength(50, ErrorMessage = "Imię może mieć maksymalnie 50 znaków.")]
        public string Name { get; set; } = "";

        [DisplayName("Nazwisko")]
        [RegularExpression(@"^[A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź]+$", ErrorMessage = "Nazwisko może zawierać tylko litery.")]
        [StringLength(50, ErrorMessage = "Nazwisko może mieć maksymalnie 50 znaków.")]

        public string Surname { get; set; } = "";

        [Required]
        [DisplayName("PESEL")]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "PESEL musi składać się z dokładnie 11 cyfr")]
        public string PESEL { get; set; } = "";

        [DisplayName("Rok Urodzenia")]
        public int BirthYear { get; set; }

        [DisplayName("Płeć")]
        public int Płec { get; set; }


    }
}
