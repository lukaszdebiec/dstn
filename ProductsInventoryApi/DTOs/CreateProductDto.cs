using System.ComponentModel.DataAnnotations;

namespace ProductsInventoryApi.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [PriceValidation(ErrorMessage = "Price cannot be equal or less than 0")]
        public int Price { get; set; }
    }
}