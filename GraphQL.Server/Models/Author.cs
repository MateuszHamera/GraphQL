using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Server.Models;

public class Author
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [MaxLength(3)]
    public int Age { get; set; }

    public ICollection<Book> Books { get; set; } 
        = new List<Book>();
}
