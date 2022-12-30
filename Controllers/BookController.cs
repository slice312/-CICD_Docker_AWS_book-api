using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using book_app_api.Models;

namespace book_app_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IDynamoDBContext _dynamoDbContext;
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly ILogger<WeatherForecastController> _logger; // TODO: решить типы


    public BookController(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb,
        ILogger<WeatherForecastController> logger)
    {
        _dynamoDbContext = dynamoDbContext;
        _amazonDynamoDb = amazonDynamoDb;
        _logger = logger;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetAll()
    {
        List<Book> allBooks = await _dynamoDbContext.ScanAsync<Book>(new List<ScanCondition>())
            .GetRemainingAsync();
        return base.Ok(allBooks);
    }

    [HttpGet]
    [Route("{isbn}")]
    public async Task<IActionResult> Get(string isbn)
    {
        var book = await _dynamoDbContext.LoadAsync<Book>(isbn);
        return base.Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Book book)
    {
        await _dynamoDbContext.SaveAsync<Book>(book);
        return base.Ok(book);
    }

    [HttpDelete]
    [Route("{isbn}")]
    public async Task<IActionResult> Delete(string isbn)
    {
        // DeleteAsync is used to delete an item from DynamoDB
        await _dynamoDbContext.DeleteAsync<Book>(isbn);
        return base.Ok();
    }

    [HttpPut]
    [Route("{isbn}")]
    public async Task<IActionResult> Update(string isbn, Book book)
    {
        var bookInBase = await _dynamoDbContext.LoadAsync<Book>(isbn);
        if (bookInBase is null)
            return base.NotFound();
        await _dynamoDbContext.SaveAsync<Book>(book);
        return base.Ok("book");
    }
    
    [HttpPatch]
    [Route("{isbn}")]
    public async Task<IActionResult> ToggleFavorite(string isbn)
    {
        var bookInBase = await _dynamoDbContext.LoadAsync<Book>(isbn);
        if (bookInBase is null)
            return base.NotFound();
        bookInBase.IsFavorite = !bookInBase.IsFavorite;
        await _dynamoDbContext.SaveAsync<Book>(bookInBase);
        return base.Ok(new {IsFavorite = bookInBase.IsFavorite});
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