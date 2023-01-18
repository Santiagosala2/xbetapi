using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Users.Models;

namespace Wallet.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public DateTime Date { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "The deposit amount must be greater than 1")]
        public decimal Amount { get; set; }

        public int? SourceUserID { get; set; }
        public int TargetUserID { get; set; }

        public string TransactionType { get; set; } = null!;
        public string PaymentProviderID { get; set; } = null!;

        public virtual User SourceUser { get; set; } = null!;
        public virtual User TargetUser { get; set; } = null!;

    }
}
