using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms
{
    public partial class MainForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private List<ItemControl> items;

        public MainForm(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            items = new();
            InitializeComponent();
            DisplayServices();
        }

        private void ItemControl_AddToCartClicked(object sender, CartItemEventArgs e)
        {
            var cartItem = new CartControl(e.Service, e.Quantity);
            cartPanel.Controls.Add(cartItem);
        }

        private async void DisplayServices()
        {
            var services = await _unitOfWork.ServiceRepo.Get();

            items.AddRange(services
                .Select(service => new ItemControl(service)));

            items.ForEach(item =>
            {
                item.AddToCartClicked += ItemControl_AddToCartClicked!;
                itemsPanel.Controls.Add(item);
            });
        }
    }
}