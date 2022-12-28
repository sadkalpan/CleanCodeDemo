using System;

namespace CleanCodeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProductManager productManager = new ProductManager(new CentralBankServiceAdapter());
            productManager.Sell(new Product { Id = 1, Name = "Shoe", Price = 1000 },
                new Customer{ Id=1, Name="Engin Demirog", IsStudent=false, IsOfficer=true }
                );
        }
    }
     
    class Customer : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStudent { get; set; }
        public bool IsOfficer { get; set; }
    }

    class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    internal interface IEntity
    {}

    class ProductManager : IProductService
    {
        IBankService _bankService;

        public ProductManager(IBankService bankService)
        {
            _bankService = bankService;
        }

        public void Sell(Product product, Customer customer)
        {
            decimal price = product.Price;
            if (customer.IsStudent)
            {
                price = price * (decimal)0.90;
            }
            else if (customer.IsOfficer)
            {
                price = price * (decimal)0.80;
            }
           price = _bankService.ConvertRate(new CurrencyRate { Currency=1, Price=1000});
            Console.WriteLine(price);
        }
    }
    interface IProductService
    {
        void Sell(Product product, Customer customer);
    }

    internal interface IBankService
    {
        decimal ConvertRate(CurrencyRate currenyRate);
    }
    class CentralBankServiceAdapter : IBankService
    {
        CentralBankService _centralBankService = new CentralBankService();

        public decimal ConvertRate(CurrencyRate currenyRate)
        {
            return _centralBankService.ConvertRate(currenyRate);
        }
    }
    class CentralBankService
    {
        public decimal ConvertRate(CurrencyRate currenyRate)
        {
            return currenyRate.Price / (decimal)5.30;
        }
    }
    class FakeBankService : IBankService
    {
        public decimal ConvertRate(CurrencyRate currenyRate)
        {
            return currenyRate.Price / (decimal)5.25;
        }
    }

    class CurrencyRate
    {
        public decimal Price { get; set; }
        public int Currency { get; set; }
    }
}
