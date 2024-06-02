using Banking.App.Controllers;
using Banking.App.Data.Entities;
using Banking.App.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBanking.Test.Account
{
    [TestClass]
    public class AccountsControllerTests
    {
        [TestMethod]
        public async Task CreateAccount_ShouldReturnCreatedAccount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new AccountsController(context);
                var account = new Banking.App.Data.Entities.Account { AccountNumber = "123456789", AccountHolderName = "John Doe", Balance = 0 };

                var result = await controller.CreateAccount(account) as CreatedAtActionResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(account.AccountHolderName, ((Banking.App.Data.Entities.Account)result.Value).AccountHolderName);
            }
        }
    }
}
