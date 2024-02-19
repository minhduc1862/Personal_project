using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace personal_project.Models
{
    [Table("Images")]
    public class ItemImage
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Photo { get; set; }
    }
}
