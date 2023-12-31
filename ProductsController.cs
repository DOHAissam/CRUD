using Azure.Messaging;
using CrudSystem.API.Data;
using CrudSystem.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CrudSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StorDbContext _dbContext;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(StorDbContext dbContext , ILogger<ProductsController> logger)
        {
            _dbContext = dbContext;
             _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Products>> GetAllProducts()
        {
            return _dbContext.products;

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Products?>> GetProducteById(int id)
        {
            return _dbContext.products.Where(P => P.Id == id).SingleOrDefault();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MessageRequester>> CreateProducte( Products products)
        {
            try
            { 
                    await _dbContext.products.AddAsync(products);
                    await _dbContext.SaveChangesAsync();
                    CreatedAtAction(nameof(Products), new { id = products.Name }, products);
                    return new MessageRequester() { successMsg = "Done" };
            }
            catch (Exception ex)
            {

                throw new Exception("Catch err", ex);
            }
       
        }
        [HttpPut]

        public async Task<ActionResult<MessageRequester>> Update(Products products)
        {
            try
            {
                _dbContext.products.Update(products);
                await _dbContext.SaveChangesAsync();
                return new MessageRequester() { successMsg= "Update is Done" };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("id")]

        public async Task<ActionResult<MessageRequester> > Delete(int id)
        {
            try
            {
                var ProductsGetByIdMetod = await GetProducteById(id);
                if (ProductsGetByIdMetod.Value is null)
                {
                    return NotFound();

                }

                _dbContext.Remove(ProductsGetByIdMetod.Value);
                await _dbContext.SaveChangesAsync();
                return new MessageRequester() { successMsg= "Done"};

            }   catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}

