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
            foreach (var control in cartPanel.Controls)
            {
                if (control is CartControl existingCartItem &&
                    existingCartItem.Service.ItemId == e.Service.ItemId)
                {
                    existingCartItem.Quantity += e.Quantity;
                    existingCartItem.InitializeCartItem();

                    return;
                }
            }

            var cartItem = new CartControl(e.Service, e.Quantity);
            cartItem.RemoveFromCartClicked += CartControl_RemoveFromCartClicked!;
            cartPanel.Controls.Add(cartItem);
        }

        private void CartControl_RemoveFromCartClicked(object sender, EventArgs e)
        {
            cartPanel.Controls.Remove(sender as CartControl);
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