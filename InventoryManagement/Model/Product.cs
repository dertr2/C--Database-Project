namespace InvencotryManagement.Model;

public class Product
{
    public int Id { get; set; } 
    public string Name {get; set;} = string.Empty;
    public decimal Price {get; set;}
    public int Quantity {get; set;}
    public bool InStock {get; set;}
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedDate {get; set;}
}