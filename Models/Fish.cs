using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pescheria.Models
{
    public class Fish
    {
        public int FishId { get; set; }

        

        public string Name { get; set; }

        [Display(Name = "Di Mare")]
        public bool IsSeaFish { get; set; }
        public int Price { get; set; }

        public DateTime? DeletedAt { get; set; } = null;
        

    }
}
