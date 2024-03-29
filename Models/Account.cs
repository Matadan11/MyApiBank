using System.ComponentModel.DataAnnotations;

namespace MyApiBank.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public int Transaction_list { get; set; }

        public int Deposits { get; set; }

        public int Withdrawals { get; set; }

        public decimal accrued_interest { get; set;}
    }
}