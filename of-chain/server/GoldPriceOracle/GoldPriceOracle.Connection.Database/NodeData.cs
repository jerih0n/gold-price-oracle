using System.ComponentModel.DataAnnotations;

namespace GoldPriceOracle.Connection.Database
{
    public class NodeData
    {
        [Key]
        public long Id { get; set; }

        [Required]      
        public string Password { get; set; }

        [Required]
        public string PrivateKeyEncrypted { get; set; }

        [Required]
        public string ActiveAddress { get; set; }
    }
}
