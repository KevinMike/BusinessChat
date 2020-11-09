using BusinessChat.Application.Common.Exceptions;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Infrastructure.Services;
using NUnit.Framework;

namespace BussinessChat.Application.IntegrationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Stooq_should_return_a_stock()
        {
            IStooqService stooqClient = new StooqService();

            var result = stooqClient.GetStock("aapl.us").Result;

            Assert.IsTrue(result.Succeeded,"Stock not found");
        }

        [Test]
        public void Stooq_should_not_return_a_stock()
        {
            IStooqService stooqClient = new StooqService();

            var result = stooqClient.GetStock("wrongcode").Result;

            Assert.IsFalse(result.Succeeded, "Stock found with wrong code");
        }

        [Test]
        public void Stooq_should_return_a_stock_for_aaplus()
        {
            IStooqService stooqClient = new StooqService();

            var result = stooqClient.GetStock("aapl.us").Result;

            if (!result.Succeeded) throw new NotFoundException();

            Assert.NotNull(result.Content,"Stock not found for aapl.us code");
        }
    }
}