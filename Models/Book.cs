using Amazon.DynamoDBv2.DataModel;

namespace book_app_api.Models;

[DynamoDBTable("Books")]
public class Book
{
    [DynamoDBHashKey("isbn")]
    public string? Isbn { get; set; }

    [DynamoDBProperty("title")]
    public string? Title { get; set; }
    
    [DynamoDBProperty("author")]
    public string? Author { get; set; }
    
    [DynamoDBProperty("description")]
    public string? Description { get; set; }
    
    [DynamoDBProperty("year")]
    public int? Year { get; set; }
    
    [DynamoDBProperty("pages")]
    public int? Pages { get; set; }
    
    [DynamoDBProperty("price")]
    public decimal? Price { get; set; }

    [DynamoDBProperty("qty")]
    public int? Qty { get; set; }
    
    [DynamoDBProperty("isFavorite")]
    public bool IsFavorite { get; set; }
}