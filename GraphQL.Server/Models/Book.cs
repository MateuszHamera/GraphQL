using System.ComponentModel.DataAnnotations;

namespace GraphQL.Server.Models;

public class Book
{
    public int Id { get; set; }
   
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    [Required]
    [MaxLength(10)]
    public double Price { get; set; }
    public Author Author { get; set; }
}
