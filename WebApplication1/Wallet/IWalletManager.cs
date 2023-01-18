using Users.Models;

namespace Wallet.Manager
{
    public interface IWalletManager
    {
        Task<(bool, decimal)> DepositAsync(User user, decimal deposit, string paymentProvider);
    }
}