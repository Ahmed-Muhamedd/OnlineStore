using Microsoft.EntityFrameworkCore;
using OnlineStore.Common;
using OnlineStore.Core.Models;
using OnlineStore.Data;
using OnlineStore.Dtos;
using OnlineStore.Interfaces;
using System.Collections.Generic;

namespace OnlineStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext Context)
        {
            this._context = Context;
        }
        public async Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderDto CreateOrder, string customerId)
        {
            
            var ids = CreateOrder.OrderDetials.Select(pk => pk.ProductID).ToList();
            var products = await _context.Products.Where(p => ids.Contains(p.ID)).ToListAsync();

            if(products.Count != ids.Count)
            {
                var foundIds = products.Select(p => p.ID).ToList();
                var missingIds = ids.Except(foundIds).ToList();
                return Result<OrderResponseDto>.NotFound( $"Product id not found {string.Join(", ", missingIds)}");
            }

            var totalOrderAmount = 0m;
            foreach (var prd in CreateOrder.OrderDetials)
            {
                var product = products.First(p => p.ID == prd.ProductID);
                if (prd.Quantity > product.QuantityInStock)
                    return Result<OrderResponseDto>.NotFound($"Product quantity more than in stock");


                totalOrderAmount += product.Price * prd.Quantity;

            }

            var Order = new Order();
            Order.CreatedAt = DateTime.UtcNow;
            Order.TotalAmount = totalOrderAmount;
            Order.Status = 2;
            Order.CustomerID = customerId;


            var orderItemList = CreateOrder.OrderDetials.Select(detail =>
            {
                var product = products.First(p => p.ID == detail.ProductID);
                return new OrderItem
                {
                    ProductID = detail.ProductID,
                    Price = product.Price,
                    Quantity = detail.Quantity,
                    TotalItemsPrice = product.Price * detail.Quantity,
                    OrderID = Order.ID
                };
            }).ToList();

            #region Other Solution
            //var orderItemList = new List<OrderItem>();
            //foreach (var product in Products)
            //{
            //    var productQuantity = CreateOrder.OrderDetials.First(p => p.ProductID == product.ID).Quantity;
            //    orderItemList.Add(new OrderItem
            //    {
            //        ProductID = product.ID,
            //        Price = product.Price,
            //        Quantity = productQuantity,
            //        TotalItemsPrice = product.Price * productQuantity,
            //        OrderID = Order.ID
            //    });

            //}
            #endregion

            var payment = new Payment();
            payment.OrderID = Order.ID;
            payment.Amount = totalOrderAmount;
            payment.PaymentMethod = CreateOrder.PaymentMethod;
            payment.TransactionDate = DateTime.UtcNow;


            var shipping = new Shipping();
            shipping.OrderID= Order.ID;
            shipping.CarrierName = "AmazonTest";
            shipping.TrackingNumber = Guid.NewGuid().ToString();
            shipping.ShippingStatus = 2;
            shipping.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(3);
            shipping.ActualDeliveryDate = null;

            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Orders.AddAsync(Order);

                await _context.OrderItems.AddRangeAsync(orderItemList);

                foreach (var item in orderItemList)
                {
                    var product = products.First(p => p.ID ==  item.ProductID);
                    product.QuantityInStock -= item.Quantity;
                }

                await _context.Payments.AddAsync(payment);

                await _context.Shippings.AddAsync(shipping);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Result<OrderResponseDto>.Success( new OrderResponseDto
                {
                    OrderID = Order.ID,
                    EstimatedDeliveryDate = shipping.EstimatedDeliveryDate,
                    TotalAmount = totalOrderAmount
                });


            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return Result<OrderResponseDto>.ServerError($"Error occured in adding order: {ex.Message}");

            }
        }

        public async Task<Result<IEnumerable<OrderDetailsDto>>> GetCustomerOrdersAsync(string customerId)
        {
            var order = await _context.Orders.Where(order =>  order.CustomerID == customerId)
               .Select(order => new OrderDetailsDto
               {
                   OrderID = order.ID,
                   OrderDate = order.CreatedAt,
                   TotalAmount = order.TotalAmount,
                   Items = order.OrderItems!.Select(item => new ItemsDto
                   {
                       ProductName = item.Product!.Name,
                       Price = item.Price,
                       Quantity = item.Quantity
                   }).ToList()
               }).ToListAsync();

            if (order == null) return Result<IEnumerable<OrderDetailsDto>>.NotFound($"Order not found");

            return Result<IEnumerable<OrderDetailsDto>>.Success(order);
        }

        public async Task<Result<OrderDetailsDto>> GetOrderByIdAsync(int orderId, string customerId)
        {
            var order = await _context.Orders.Where(order => order.ID == orderId && order.CustomerID == customerId)
                .Select(order => new OrderDetailsDto
                {
                    OrderID = order.ID,
                    OrderDate = order.CreatedAt,
                    TotalAmount = order.TotalAmount,
                    Items = order.OrderItems!.Select(item => new ItemsDto
                    {
                        ProductName = item.Product!.Name,
                        Price = item.Price,
                        Quantity = item.Quantity
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (order == null) return Result<OrderDetailsDto>.NotFound($"Order not found with id: {orderId}");

            return Result<OrderDetailsDto>.Success(order);
        }
    }
}
