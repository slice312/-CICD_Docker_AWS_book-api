using Amazon.DynamoDBv2.DataModel;

namespace book_app_api;

[DynamoDBTable("books")]
public class Book
{
    [DynamoDBHashKey("category")]
    public string? Category { get; set; }

    [DynamoDBRangeKey("title")]
    public string? Title { get; set; }
    
    [DynamoDBProperty("description")]
    public string? Description { get; set; }
    
    [DynamoDBProperty("price")]
    public decimal Price { get; set; }
}