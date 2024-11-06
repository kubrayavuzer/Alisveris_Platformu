using Alisveris_Platformu.Business.Operations.Order.Dtos;
using Alisveris_Platformu.Business.Types;
using Alisveris_Platformu.Data.Entities;
using Alisveris_Platformu.Data.Repositories;
using Alisveris_Platformu.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;

        public OrderManager(IUnitOfWork unitOfWork, IRepository<OrderEntity> orderRepository,
            IRepository<OrderProductEntity> orderProductRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;

        }

        public async Task<ServiceMessage> AddOrder(AddOrderDto order)
        {
            var hasOrder = _orderRepository.GetAll(x => x.Name.ToLower() ==
            order.OrderName.ToLower()).Any();

            if(hasOrder)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu sipariş zaten mevcut."
                };
            }

            //süreçle ilgili sorunlar için(sipariş var ürün yok? gibi)

            await _unitOfWork.BeginTransaction();

            var orderEntity = new OrderEntity
            {
                Name = order.OrderName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId

            };

            _orderRepository.Add(orderEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception)
            {
                throw new Exception("Kayıt sırasında hata oluştu."); 
            }

            foreach(var productId in order.ProductIds)
            {
                var orderProduct = new OrderProductEntity
                {
                    OrderId = orderEntity.Id,
                    ProductId = productId,
                };
                //(1-3), (1-5), (1-8)

                _orderProductRepository.Add(orderProduct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch(Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Ürünler eklenirken bir hata oluştu, süreç tekrar başlıyor.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };

        }

        public async Task<ServiceMessage> DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);

            if(order is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Silinmek istenen sipariş bulunamadı."
                };
            }

            _orderRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw new Exception("Silme işlemi sırasında hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
            };

        }

        public async Task<OrderDto> GetOrder(int id)
        {
            var order = await _orderRepository.GetAll(x => x.Id == id)
            .Select(x => new OrderDto
            {
                Id = x.Id,
                Name = x.Name,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount,
                Products = x.OrderProducts.Select(f => new OrderProductDto
                {
                    Id = f.Id,
                    Name = f.Product.Name
                }).ToList()
            }).FirstOrDefaultAsync();

            return order;
        }

        public async Task<List<OrderDto>> GetOrders()
        {
            var orders = await _orderRepository.GetAll()
           .Select(x => new OrderDto
           {
               Id = x.Id,
               Name = x.Name,
               OrderDate = x.OrderDate,
               TotalAmount = x.TotalAmount,
               Products = x.OrderProducts.Select(f => new OrderProductDto
               {
                   Id = f.Id,
                   Name = f.Product.Name
               }).ToList()
           }).ToListAsync();

            return orders;
        }

        public async Task<ServiceMessage> UpdateOrder(UpdateOrderDto order)
        {
            var orderEntity = _orderRepository.GetById(order.Id);

            if(orderEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Sipariş bulunamadı."
                };
            }

            await _unitOfWork.BeginTransaction();

            orderEntity.OrderDate = order.OrderDate;
            orderEntity.TotalAmount = order.TotalAmount;
            orderEntity.Name = order.Name;
            orderEntity.CustomerId = order.CustomerId;

            _orderRepository.Update(orderEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Sipariş bilgileri güncellenirken hata oldu.");
            }

            var orderProducts = _orderProductRepository.GetAll(x => x.OrderId == x.OrderId).ToList();

            foreach(var orderProduct in orderProducts)
            {
                _orderProductRepository.Delete(orderProduct, false); //hard delete
            }
            
            foreach(var productId in order.ProductIds)
            {
                var orderProduct = new OrderProductEntity
                {
                    OrderId = orderEntity.Id,
                    ProductId = productId,
                };

                _orderProductRepository.Add(orderProduct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch(Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Sipariş bilgileri oluştururken hata oluştu, işlemler geri alınıyor");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
            };

        }
    }
}
