using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Models;
using Services.Models.ViewModels;
using System.Xml.Linq;

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
        [HttpGet]
        [Route("{ProductId}")]
        public bool EditProductInfo(int ProductId)
        {
            var EditItem = _northWind.MyProducts.Where(p => p.ProductId == ProductId).Select(p => new EditInfoViewModel()
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price = p.UnitPrice,
                Stock = p.UnitsInStock
            }).FirstOrDefault();
            HttpContext.Session.SetString("EditProductData", JsonConvert.SerializeObject(EditItem));

            return false;
        }
        [HttpGet]
        [Route("{ProductId}")]
        public EditInfoViewModel GetEditProductInfo(int ProductId)
        {
            return _northWind.MyProducts.Where(p => p.ProductId == ProductId).Select(p => new EditInfoViewModel()
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price = p.UnitPrice,
                Stock = p.UnitsInStock
            }).First();
        }
        [HttpPut]
        public bool EditProduct(EditInfoViewModel EditProduct)
        {
            var EditItem = _northWind.MyProducts.FirstOrDefault(p => p.ProductId == EditProduct.Id);
            EditItem.ProductName = EditProduct.Name;
            EditItem.UnitPrice = EditProduct.Price;
            EditItem.UnitsInStock = EditProduct.Stock;
            EditItem.LaunchDate = DateTime.Now;
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
        [HttpDelete]
        [Route("{ProductId}")]
        public bool DeleteProductInfo(int ProductId)
        {
            var DeleteItem = _northWind.MyProducts.FirstOrDefault(p => p.ProductId == ProductId);
            _northWind.Remove(DeleteItem);
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
