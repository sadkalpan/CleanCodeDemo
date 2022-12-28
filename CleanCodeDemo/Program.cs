using CleanCodeDemo;
using System;

namespace CleanCodeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProductManager productManager = new ProductManager(new CentralBankServiceAdapter());
            productManager.Sell(new Product { Id = 1, Name = "Shoe", Price = 1000 },
                new Customer{ Id=1, Name="Engin Demirog" }
                );
        }
    }

    class Officer : IEntity, IPerson
    {
        public Officer()
        {
            CampaignHandler = new OfficerCampaignHandler();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IPersonCampaignHandler CampaignHandler { get; set; }
    }
    class Student : IEntity, IPerson
    {
        public Student()
        {
            CampaignHandler = new StudentCampaignHandler();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IPersonCampaignHandler CampaignHandler { get; set; }
    }
    class Customer : IEntity, IPerson
    {
        public Customer()
        {
            CampaignHandler = new CustomerCampaignHandler();
        }   
        public int Id { get; set; }
        public string Name { get; set; }
        public IPersonCampaignHandler CampaignHandler { get; set; }
    }

    class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    internal interface IEntity
    {}
    internal interface IPerson
    {
        int Id { get; set; }
        string Name { get; set; }
        IPersonCampaignHandler CampaignHandler { get; set; }
    }
    class ProductManager : IProductService
    {
        private readonly IBankService _bankService;
        public ProductManager(IBankService bankService)
        {
            _bankService = bankService;
        }
        public void Sell(Product product, IPerson person)
        {
            /*decimal price = product.Price;
            if (customer.IsStudent)
            {
                price = price * (decimal)0.90;
            }
            else if (customer.IsOfficer)
            {
                price = price * (decimal)0.80;
            }*/
            var price = person.CampaignHandler.Calculate(product);
            var exchangePrice = _bankService.ConvertRate(new CurrencyRate
            {
                Currency = 1,
                Price = price
            });
            Console.WriteLine(person.Name + " İsimli Müşterimiz için indirim Karşılığı:" + price.ToString("#.##"));
            Console.WriteLine("Ürünün Döviz Karşılığı:" + exchangePrice.ToString("#.##"));
            Console.WriteLine("#####################################################");
        }
    }
    interface IProductService
    {
        void Sell(Product product, IPerson person);
    }
    internal interface IPersonCampaignHandler
    {
        decimal Calculate(Product product);
    }
    class StudentCampaignHandler : IPersonCampaignHandler
    {
        public decimal Calculate(Product product)
        {
            return product.Price * (decimal)0.90;
        }
    }
    class CustomerCampaignHandler : IPersonCampaignHandler
    {
        public decimal Calculate(Product product)
        {
            return product.Price;
        }
    }
    class OfficerCampaignHandler : IPersonCampaignHandler
    {
        public decimal Calculate(Product product)
        {
            return product.Price * (decimal)0.80;
        }
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
