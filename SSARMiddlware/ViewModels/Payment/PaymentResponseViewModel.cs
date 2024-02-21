namespace SSARMiddlware.ViewModels.Payment
{
    internal class PaymentResponseViewModel
    {
        public string Id { get; set; }
        public long Price { get; set; }
        public bool IsPaid { get; set; }
    }
}
