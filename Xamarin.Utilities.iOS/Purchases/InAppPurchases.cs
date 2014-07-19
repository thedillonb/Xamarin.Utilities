using System;
using MonoTouch.StoreKit;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using Xamarin.Utilities.Core.Services;
using System.Collections.Generic;

namespace Xamarin.Utilities.Purchases
{
    public class InAppPurchases
    {
        private static readonly Lazy<InAppPurchases> _instance = new Lazy<InAppPurchases>(() => new InAppPurchases());
        private readonly TransactionObserver _observer;
        private readonly Dictionary<string, TaskCompletionSource<bool>> _completions = new Dictionary<string, TaskCompletionSource<bool>>();

        public event Action<SKPayment> PurchaseSuccess;
        public event Action<SKPayment, Exception> PurchaseError;

        public static InAppPurchases Instance
        {
            get { return _instance.Value; }
        }

        private void OnPurchaseError(SKPayment id, Exception e)
        {
            if (id != null && _completions.ContainsKey(id.ProductIdentifier))
            {
                _completions[id.ProductIdentifier].SetException(e);
                _completions.Remove(id.ProductIdentifier);
            }

            var handle = PurchaseError;
            if (handle != null)
                handle(id, e);
        }

        private void OnPurchaseSuccess(SKPayment id)
        {
            if (id != null && _completions.ContainsKey(id.ProductIdentifier))
            {
                _completions[id.ProductIdentifier].SetResult(true);
                _completions.Remove(id.ProductIdentifier);
            }

            var handle = PurchaseSuccess;
            if (handle != null)
                handle(id);
        }

        private InAppPurchases()
        {
            _observer = new TransactionObserver(this);
            SKPaymentQueue.DefaultQueue.AddTransactionObserver(_observer);
        }

        public static async Task<SKProductsResponse> RequestProductData (params string[] productIds)
        {
            var array = new NSString[productIds.Length];
            for (var i = 0; i < productIds.Length; i++)
                array[i] = new NSString(productIds[i]);

            var tcs = new TaskCompletionSource<SKProductsResponse>();
            var productIdentifiers = NSSet.MakeNSObjectSet<NSString>(array); //NSSet.MakeNSObjectSet<NSString>(array);​​​
            var productsRequest = new SKProductsRequest(productIdentifiers);
            productsRequest.ReceivedResponse += (sender, e) => tcs.SetResult(e.Response);
            productsRequest.RequestFailed += (sender, e) => tcs.SetException(new Exception(e.Error.LocalizedDescription));
            productsRequest.Start();
            var ret = await tcs.Task;
            productsRequest.Dispose();
            return ret;
        }

        public static bool CanMakePayments()
        {
            return SKPaymentQueue.CanMakePayments;        
        }

        public void Restore()
        {
            SKPaymentQueue.DefaultQueue.RestoreCompletedTransactions();                        
        }

        public async Task PurchaseProduct(SKProduct productId)
        {
            var completionSource = new TaskCompletionSource<bool>();
            SKPayment payment = SKPayment.PaymentWithProduct(productId);
            _completions.Add(payment.ProductIdentifier, completionSource);
            SKPaymentQueue.DefaultQueue.AddPayment (payment);
            await completionSource.Task;
        }

        private void CompleteTransaction (SKPaymentTransaction transaction)
        {
            Console.WriteLine ("CompleteTransaction " + transaction.TransactionIdentifier);
            OnPurchaseSuccess(transaction.Payment);
        }

        private void RestoreTransaction (SKPaymentTransaction transaction)
        {
            Console.WriteLine("RestoreTransaction " + transaction.TransactionIdentifier + "; OriginalTransaction " + transaction.OriginalTransaction.TransactionIdentifier);
            OnPurchaseSuccess(transaction.OriginalTransaction.Payment);
        }

        private void FailedTransaction (SKPaymentTransaction transaction)
        {
            var errorString = transaction.Error != null ? transaction.Error.LocalizedDescription : "Unable to process transaction!";
            OnPurchaseError(transaction.Payment, new Exception(errorString));
        }

        private class TransactionObserver : SKPaymentTransactionObserver
        {
            private readonly InAppPurchases _inAppPurchases;

            public TransactionObserver(InAppPurchases inAppPurchases)
            {
                _inAppPurchases = inAppPurchases;
            }

            public override void UpdatedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
            {
                foreach (SKPaymentTransaction transaction in transactions)
                {
                    try
                    {

                        switch (transaction.TransactionState)
                        {
                            case SKPaymentTransactionState.Purchased:
                                _inAppPurchases.CompleteTransaction(transaction);
                                SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                                break;
                            case SKPaymentTransactionState.Failed:
                                _inAppPurchases.FailedTransaction(transaction);
                                SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                                break;
                            case SKPaymentTransactionState.Restored:
                                _inAppPurchases.RestoreTransaction(transaction);
                                SKPaymentQueue.DefaultQueue.FinishTransaction(transaction);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        IoC.Resolve<IAlertDialogService>().Alert("Error", "Unable to process transaction: " + e.Message);
                    }
                }
            }

//            public override void RemovedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
//            {
//                foreach (var t in transactions)
//                {
//                    Console.WriteLine("Uh oh: " + t.TransactionState);
//
//                }
//            }

            public override void PaymentQueueRestoreCompletedTransactionsFinished (SKPaymentQueue queue)
            {
                Console.WriteLine(" ** RESTORE PaymentQueueRestoreCompletedTransactionsFinished ");
            }

            public override void RestoreCompletedTransactionsFailedWithError (SKPaymentQueue queue, NSError error)
            {
                Console.WriteLine(" ** RESTORE RestoreCompletedTransactionsFailedWithError " + error.LocalizedDescription);
            }
        }
    }

    public static class SKProductExtension 
    {
        public static string LocalizedPrice (this SKProduct product)
        {
            var formatter = new NSNumberFormatter ();
            formatter.FormatterBehavior = NSNumberFormatterBehavior.Version_10_4;  
            formatter.NumberStyle = NSNumberFormatterStyle.Currency;
            formatter.Locale = product.PriceLocale;
            var formattedString = formatter.StringFromNumber(product.Price);
            return formattedString;
        }
    }
}

