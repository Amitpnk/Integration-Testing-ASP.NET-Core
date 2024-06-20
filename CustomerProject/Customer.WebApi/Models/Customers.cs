using System.ComponentModel.DataAnnotations;

namespace CustomerProject.WebApi.Models;

public class Customers
{
    [Key]
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
}

