using LaundryPOS.Contracts;
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

        private async void DisplayServices()
        {
            var services = await _unitOfWork.ServiceRepo.Get();

            foreach (var service in services)
            {
                items.Add(new ItemControl(service));
            }

            foreach (var item in items)
            {
                itemsPanel.Controls.Add(item);
            }
        }
    }
}