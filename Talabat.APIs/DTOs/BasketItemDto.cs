using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
         public string ProductName { get; set; }
        [Range(.1, double.MaxValue, ErrorMessage ="Price must be greater then zero")]
        [Required]
        public decimal Price { get; set; }
        [Range(1,int.MaxValue, ErrorMessage ="Quantity must be one item at least!")]
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}