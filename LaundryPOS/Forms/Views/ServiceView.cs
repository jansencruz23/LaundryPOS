using LaundryPOS.Contracts;
using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms.Views
{
    public partial class ServiceView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceView(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
            InitializeAsync();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var service = new Service();
            service.Name = txtName.Text;

            double price;
            if (!double.TryParse(txtPrice.Text, out price))
            { 
                MessageBox.Show("Invalid Price");
            }
            else
            {
                service.Price = price;
                await CreateService(service);
                await RefreshData();
            }
        }

        public async Task CreateService(Service service)
        {
            _unitOfWork.ServiceRepo.Insert(service);
            await _unitOfWork.SaveAsync();
        }

        private async void InitializeAsync()
        {
            await DisplayServices();
        }

        private async Task DisplayServices()
        {
            IEnumerable<Service> serviceList = await _unitOfWork.ServiceRepo.Get();
            dgvService.DataSource = serviceList;

            dgvService.Columns[nameof(Service.ServiceId)].Visible = false;
            dgvService.Columns[nameof(Service.PicPath)].Visible = false;

            // For image
            // dgvService.Columns.Add(nameof(Service.Name), nameof(Service.Name));
        }

        private async Task RefreshData()
        {
            dgvService.DataSource = null;
            await DisplayServices();
        }
    }
}