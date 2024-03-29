using System;
using System.ComponentModel.DataAnnotations;

namespace MyApiBank.Models
{
    public class Transaction
    {
        [Key]
        public int TransId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string Trantype { get; set; }
    }
}
