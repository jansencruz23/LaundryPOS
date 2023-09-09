using LaundryPOS.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace LaundryPOS
{
    public partial class Form1 : Form
    {
        private readonly IUnitOfWork _unitOfWork;

        public Form1(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
        }
    }
}