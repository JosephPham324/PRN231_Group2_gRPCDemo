using Grpc.Core;
using GRPCService;
using GRPCService.Models;
using GRPCService.Protos;
using Microsoft.AspNetCore.Mvc;

namespace GRPCService.Services
{
    public class CustomerService : GrpcCustomer.GrpcCustomerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbcontext;

        public CustomerService(ApplicationDbContext db)
        {
            _dbcontext = db;
        }

        public override Task<CustomerList> GetAll(Empty request, ServerCallContext context)
        {
            CustomerList res = new CustomerList();
            var cusList = from cus in _dbcontext.Customers select new Protos.Customer
            {
                Id = cus.Id,
                Name = cus.Name,
                Address = cus.Address
            };

            res.Customers.AddRange(cusList);

            return Task.FromResult(res);
        }

        public override Task<Protos.Customer> GetCustomer(IDRequest request, ServerCallContext context)
        {
            try
            {
                var res = (from c in _dbcontext.Customers
                           where c.Id == request.Id
                           select new Protos.Customer
                           {
                               Id = c.Id,
                               Name = c.Name
                           }).FirstOrDefault();
                return Task.FromResult(res);

            }
            catch (Exception ex)
            {

                // Propagate the error to the client
                throw new RpcException(new Status(StatusCode.Unknown, "Exception was thrown by handler."));
            }
            
            return null;
        }
    }
}
