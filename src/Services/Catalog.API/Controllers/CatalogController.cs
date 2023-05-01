using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : BaseController
    {
        IProductManager _productManager;

        public CatalogController(IProductManager productManager)
        {
            _productManager = productManager;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        [ResponseCache(Duration =10)]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _productManager.GetAll();
                return CustomResult("Data loaded Successfully.", products);
            }
            catch (Exception ex)
            { 
                return CustomResult(ex.Message,HttpStatusCode.BadRequest);
            } 
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult GetByCategory(string category)
        {
            try
            {
                var products = _productManager.GetByCategory(category);
                return CustomResult("Data load sucessfully.", products);
            }
            catch (Exception ex)
            {
                return CustomResult($"{ex.Message}", HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult GetById(string id)
        {
            try
            {
                var product = _productManager.GetById(id);
                return CustomResult("Data load sucessfully.", product);
            }
            catch (Exception ex)
            {
                return CustomResult($"{ex.Message}", HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = _productManager.Add(product);
                if (isSaved)
                {
                    return CustomResult("Product has been save successfully", product, HttpStatusCode.Created);
                }
                return CustomResult("Product saved failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return CustomResult($"{ex.Message}", HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("Data not found", HttpStatusCode.BadRequest);
                }

                bool isUpdated = _productManager.Update(product.Id, product);
                if (isUpdated)
                {
                    return CustomResult("Product has been modified successfully", product, HttpStatusCode.OK);
                }
                return CustomResult("Product modified failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return CustomResult($"{ex.Message}", HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public IActionResult DeleteProduct(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return CustomResult("Data not found", HttpStatusCode.BadRequest);
                }
                bool isDeleted = _productManager.Delete(id);
                if (isDeleted)
                {
                    return CustomResult("Product has been deleted successfully", HttpStatusCode.OK);
                }
                return CustomResult("Product deleted failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return CustomResult($"{ex.Message}", HttpStatusCode.BadRequest);
            }
        }


    }
}
