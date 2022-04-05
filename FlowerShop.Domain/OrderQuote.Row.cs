namespace FlowerShop.Domain;

public partial class OrderQuote
{
    public partial class Row
    {
        private readonly Dictionary<Bundle, BundleDetail> _bundles = new ();
        
        public Order.Row OrderRow { get; }
        public IReadOnlySet<BundleDetail> BundleDetails => _bundles.Values.ToHashSet();
        public int OrderQuantity => OrderRow.Quantity;
        public int QuotedQuantity => BundleDetails.Sum(bundleDetail => bundleDetail.TotalQuantity);
        public int MissingQuantity => OrderQuantity - QuotedQuantity;
        public bool IsFilled => OrderQuantity == QuotedQuantity;
        public decimal TotalPrice => BundleDetails.Sum(bundle => bundle.TotalPrice);

        public Row(Order.Row orderRow)
        {
            OrderRow = orderRow;
        }

        public void AddBundle(int count, Bundle bundle)
        {
            var newQuantity = QuotedQuantity + count * bundle.Quantity;
            
            if (newQuantity > OrderRow.Quantity)
                throw new OrderQuantityExceededException();

            var bundleDetail = GetBundleDetailFor(bundle);
            bundleDetail.Add(count);
        }

        private BundleDetail GetBundleDetailFor(Bundle bundle)
        {
            if (!_bundles.ContainsKey(bundle))
                _bundles.Add(bundle, new BundleDetail(bundle));

            return _bundles[bundle];
        }
    }
}