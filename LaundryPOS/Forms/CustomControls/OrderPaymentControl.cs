using LaundryPOS.Forms.Views;

namespace LaundryPOS.Forms.CustomControls
{
    public partial class OrderPaymentControl : UserControl
    {
        private readonly CartItem _cartItem;
        public OrderPaymentControl(CartItem cartItem)
        {
            _cartItem = cartItem;
            InitializeComponent();
            InitializeOrder();
        }

        private void InitializeOrder()
        {
            lblName.Text = _cartItem.Item.Name;
            lblQuantity.Text = _cartItem.Quantity.ToString();
            lblPrice.Text = $"{_cartItem.Item.Price:#,###.00}";
            lblSubTotal.Text = $"{_cartItem.SubTotal:#,###.00}";
        }
    }
}