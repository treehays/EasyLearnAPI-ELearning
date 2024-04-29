using EasyLearn.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLearn.Models.Entities;

public class Wallet : AuditableEntity
{
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Debit { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Credit { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}
