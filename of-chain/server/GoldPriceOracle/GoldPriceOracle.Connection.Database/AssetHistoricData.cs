using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldPriceOracle.Connection.Database
{
    public class AssetHistoricData
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        public short AssetId { get; set; }

        public short FiatCurrencyId { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual FiatCurrency FiatCurrency { get; set; }
    }
}