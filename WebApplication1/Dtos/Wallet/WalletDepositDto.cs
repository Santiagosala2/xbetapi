using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.Dtos
{
    public class WalletDepositDto
    {
        [Column(TypeName = "decimal(18, 2)")]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "The deposit amount must be greater than 1")]
        [Required]
        public decimal Amount { get; set; }


        [Required]
        public string PaymentProvider { get; set; } = null!;
    }
}