using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPi.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}
