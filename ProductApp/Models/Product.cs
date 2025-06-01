using System.ComponentModel.DataAnnotations;

namespace ProductApp.Models
{
    /// <summary>
    /// Reprezentuje model produktu dostępnego w aplikacji.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}
