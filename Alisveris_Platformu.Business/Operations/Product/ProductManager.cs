using Alisveris_Platformu.Business.Operations.Product.Dtos;
using Alisveris_Platformu.Business.Types;
using Alisveris_Platformu.Data.Entities;
using Alisveris_Platformu.Data.Repositories;
using Alisveris_Platformu.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Business.Operations.Product
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<UserEntity> _userRepository;

        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository, IRepository<UserEntity> userRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = repository;
            _userRepository = userRepository;
        }

        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            var hasProduct = _productRepository.GetAll(x => x.Name.ToLower() == product.ProductName.ToLower()).Any();

            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu ürün zaten bulunuyor."
                };
            }

            var productEntity = new ProductEntity
            {
                Name = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,

            };  

            _productRepository.Add(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw new Exception("Ürün kaydı sırasında hata oluştu.");
            }



            return new ServiceMessage
            {
                IsSucceed = true
            };
        }
    }
}
