using System.ComponentModel.DataAnnotations;

namespace Practical_8.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "Price must be zero or greater.")]
        public decimal Price { get; set; }
    }
}
