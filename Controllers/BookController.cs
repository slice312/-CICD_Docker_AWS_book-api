using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;

namespace book_app_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IDynamoDBContext _dynamoDbContext;
    private readonly IAmazonDynamoDB _amazonDynamoDb;

    public BookController(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb)
    {
        _dynamoDbContext = dynamoDbContext;
        _amazonDynamoDb = amazonDynamoDb;
    }

    [HttpGet]
    [Route("get/{category}")]
    public async Task<IActionResult> Get(string category)
    {
        // LoadAsync is used to load single item
        var product = await _dynamoDbContext.LoadAsync<Book>(category);
        return base.Ok(product);
    }

    [HttpPost]
    [Route("save")]
    public async Task<IActionResult> Save(Book book)
    {
        // SaveAsync is used to put an item in DynamoDB, it will overwrite if an item with the same primary key already exists
        await _dynamoDbContext.SaveAsync(book);
        return base.Ok();
    }
    
    [HttpDelete]
    [Route("delete/{category}")]
    public async Task<IActionResult> Delete(string category)
    {
        // DeleteAsync is used to delete an item from DynamoDB
        await _dynamoDbContext.DeleteAsync<Book>(category);
        return base.Ok();
    }
    
    [HttpGet]
    [Route("search/{category}")]
    public async Task<IActionResult> Search(string category, string? bookTitle = null, decimal? price = null)
    {
        // Note: You can only query the tables that have a composite primary key (partition key and sort key).

        // 1. Construct QueryFilter
        var queryFilter = new QueryFilter("category", QueryOperator.Equal, category);

        if (!string.IsNullOrEmpty(bookTitle))
        {
            queryFilter.AddCondition("title", ScanOperator.Equal, bookTitle);
        }

        if (price.HasValue)
        {
            queryFilter.AddCondition("price", ScanOperator.LessThanOrEqual, price);
        }

        // 2. Construct QueryOperationConfig
        var queryOperationConfig = new QueryOperationConfig
        {
            Filter = queryFilter
        };

        // 3. Create async search object
        var search = _dynamoDbContext.FromQueryAsync<Book>(queryOperationConfig);

        // 4. Finally get all the data in a singleshot
        List<Book> searchResponse = await search.GetRemainingAsync();

        // Return it
        return Ok(searchResponse);
    }
}