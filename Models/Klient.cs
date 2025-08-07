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

       

		[DisplayName("Name")]
		public string Name { get; set; } = "";

		[DisplayName("Surname")]
		public string Surname { get; set; } = "";

		[DisplayName("PESEL")]
		public string PESEL { get; set; } = "";

		[DisplayName("BirthYear")]
		public int BirthYear { get; set; }

		[DisplayName("Płec")]
		public int Płec { get; set; }


	}
}
