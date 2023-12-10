using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Forms.CustomControls;
using LaundryPOS.Models;

namespace LaundryPOS.Forms
{
    public partial class PaymentForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;
        private readonly Order _orders;
        private readonly decimal _total;
        private readonly int _transactionId;

        public PaymentForm(Order orders, decimal total,
            IUnitOfWork unitOfWork, Employee employee,
            IStyleManager styleManager,
            int transactionId = default)
        {
            _orders = orders;
            _total = total;
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _employee = employee;
            _transactionId = transactionId;

            InitializeComponent();
            InitializeOrders();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private async void PaymentForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private void InitializeOrders()
        {
            _orders.Items.ForEach(order =>
                orderPanel.Controls
                    .Add(new OrderPaymentControl(order)));

            lblTotal.Text = $"₱ {_total:#,###.00}";
        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                if (amount >= _total)
                {
                    try
                    {
                        var transaction = await _unitOfWork.TransactionRepo
                            .GetByID(_transactionId);

                        if (transaction != null)
                        {
                            UpdateTransaction(transaction, amount);
                            _unitOfWork.TransactionRepo.Update(transaction);
                        }
                        else
                        {
                            transaction = await CreateTransaction(amount);
                            _unitOfWork.TransactionRepo.Insert(transaction);
                        }

                        await _unitOfWork.SaveAsync();
                        var receiptForm = new ReceiptForm(_unitOfWork, transaction);
                        receiptForm.FormClosed += (s, args) => Close();
                        receiptForm.ShowDialog();
                }
                    catch (Exception ex)
                {
                        MessageDialog.Show(this, "An error occurred: " + ex.Message, "Payment Failed",
                        MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                }
            }
                else
                {
                    MessageDialog.Show(this, "Insufficient Payment", "Payment Failed",
                        MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                }
            }
            else
            {
                MessageDialog.Show(this, "Please make sure you enter valid input", "Payment Failed", 
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
            }
        }

        private async Task<Transaction> CreateTransaction(decimal amount)
        {
            var transaction = new Transaction
            {
                EmployeeId = _employee.Id,
                TransactionDate = DateTime.Now,
                TotalAmount = _total,
                AmountPaid = amount,
                Change = amount - _total,
                IsCompleted = true,
                Items = new List<TransactionItem>()
            };

            await SubtractStocks(transaction.Items);
            return transaction;
        }

        private async Task SubtractStocks(ICollection<TransactionItem> transaction)
        {
            foreach (var order in _orders.Items)
            {
                var item = await _unitOfWork.ItemRepo.GetByID(order.Item.Id);

                if (item != null)
                {
                    var soldQuantity = order.Quantity;
                    var availableQuantity = item.Stock;

                    if (soldQuantity <= availableQuantity)
                    {
                        var transactionItem = new TransactionItem
                        {
                            ItemId = order.Item.Id,
                            Quantity = soldQuantity,
                            SubTotal = order.SubTotal
                        };

                        transaction.Add(transactionItem);
                        item.Stock -= soldQuantity;
                    }
                    else
                    {
                        throw new Exception("Item quantity cannot exceed item stocks. Please check your order again.");
                    }
                }
                else
                {
                    MessageDialog.Show(this, "Transaction failed.", "Transaction Failed",
                        MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                }
            }
        }

        private void UpdateTransaction(Transaction transaction, decimal amount)
        {
            transaction.TransactionDate = DateTime.Now;
            transaction.AmountPaid = amount;
            transaction.Change = amount - _total;
            transaction.IsCompleted = true;
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            var button = sender as Guna2Button;
            var number = button!.Text;

            if (number.Equals(".") && txtAmount.Text.Contains('.'))
            {
                return;
            }

            txtAmount.Text += number;
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToButton(btnPay);
            await _styleManager.Theme.ApplyThemeToPanel(panelBg);
        }

        private void lblCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtAmount.Text.Length > 0)
            {
                txtAmount.Text = txtAmount.Text[..^1];
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                lblChange.Text = $"{amount - _total}";
            }
        }
    }
}