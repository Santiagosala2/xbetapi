using DataStore;
using Microsoft.AspNetCore.Identity;
using AuthServer.Helpers.Hasher;
using Users.Models;
using Wallet.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Wallet.Manager
{
    public class WalletManager : IWalletManager
    {
        private DataStoreContext _context;

        public WalletManager(DataStoreContext context)
        {
            _context = context;
        }

        public async Task<(bool,decimal)> DepositAsync(User user , decimal deposit , string paymentProvider)
        {    
            Transaction transaction = new Transaction()
            {
                Date = DateTime.Now,
                Amount = deposit,
                SourceUserID = user.UserID,
                TargetUserID = user.UserID,
                TransactionType= "Deposit",
                PaymentProviderID = paymentProvider
            };

            IDbContextTransaction t = _context.Database.BeginTransaction();
            await _context.Transactions.AddAsync(transaction);
            user.Balance += deposit;
            int changesSaved = await _context.SaveChangesAsync();
            t.Commit();
            return (changesSaved == 2, user.Balance);
        }


    }
}
