using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudSystem.API.Model
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
       
        [Column("Producte_name")]
        [Required]
        [MaxLength(10)]
        
        public string Name { get; set; }
        [Column("Discription")]
        [Required]
        public string Description { get; set; }
        [Column("Qoantat")]
        [Required]
        public int Quantity { get; set; }
        [Column("Category")]
        public string Category { get; set; }
    }
}
