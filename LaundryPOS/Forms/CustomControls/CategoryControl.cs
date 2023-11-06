using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.CustomControls
{
    public partial class CategoryControl : UserControl
    {
        public event EventHandler<CategoryEventArgs> CategoryClicked;
        private readonly Category _category;
        private readonly IStyleManager _styleManager;

        public CategoryControl(Category category,
            IStyleManager styleManager)
        {
            _category = category;
            _styleManager = styleManager;

            InitializeComponent();
            InitializeCategory();
            WireAllControls(this);
        }

        private void InitializeCategory()
        {
            lblName.Text = _category.Name;
            imgIcon.Image = GetImage();
        }

        private Image GetImage()
        {
            try
            {
                return _category.Image != null
                ? Image.FromFile(_category.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }

        private void WireAllControls(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                ctrl.Click += Control_Click!;
                if (ctrl.HasChildren)
                {
                    WireAllControls(ctrl);
                }
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, EventArgs.Empty);
            CategoryClicked?.Invoke(this,
                new CategoryEventArgs(_category));

            panelBg.FillColor = Color.FromArgb(48,48,48);
            lblName.ForeColor = Color.White;
        }

        public void ChangeToDefault()
        {
            panelBg.FillColor = Color.FromArgb(248, 248, 248);
            lblName.ForeColor = Color.FromArgb(24,24,24);
        }
    }
}