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
		public string Name { get; set; } = "";

		[DisplayName("Nazwisko")]
		public string Surname { get; set; } = "";

		[DisplayName("PESEL")]
		public string PESEL { get; set; } = "";

		[DisplayName("Rok Urodzenia")]
		public int BirthYear { get; set; }

		[DisplayName("Płeć")]
		public int Płec { get; set; }


	}
}
