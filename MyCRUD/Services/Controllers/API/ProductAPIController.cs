using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Models;
using Services.Models.ViewModels;

namespace Services.Controllers.API
{
    [Route("api/Product/{action}")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly NorthWindContext _northWind;

        public ProductAPIController(NorthWindContext northWind)
        {
            _northWind = northWind;
        }
        [HttpGet]
        public List<ProductViewModel> GetProductInfo()
        {
            return _northWind.MyProducts.Select(p => new ProductViewModel()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.UnitPrice,
                Stock = p.UnitsInStock,
                LaunchDate = p.LaunchDate.ToString("G")
            }).ToList();
        }
        [HttpPost]
        public bool CreateProduct(CreatePorductViewModel NewProduct)
        {
            var item = new MyProduct()
            {
                ProductName = NewProduct.ProductName,
                UnitPrice = NewProduct.Price,
                UnitsInStock = NewProduct.Stock,
                LaunchDate = DateTime.Now,
            };
            _northWind.Add(item);
            try
            {
                _northWind.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
