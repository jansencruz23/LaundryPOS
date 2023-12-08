using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.Views;
using LaundryPOS.Managers;
using LaundryPOS.Models;

namespace LaundryPOS.Helpers
{
    public class PaginationHelper<T>
        where T : Control
    {
        public event EventHandler<CartItemEventArgs> AddToCartClicked;
        private readonly FlowLayoutPanel panelItems;
        private readonly FlowLayoutPanel panelPage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;

        private const int ITEMS_PER_PAGE = 10;
        private List<ItemControl> itemsControls = new();
        private IEnumerable<Item> allItems;

        private int currentPage = 1;
        private int totalPages = 1;

        public PaginationHelper(FlowLayoutPanel itemsPanel, FlowLayoutPanel pagePanel, 
            IUnitOfWork unitOfWork, IStyleManager styleManager)
        {
            panelItems = itemsPanel;
            panelPage = pagePanel;
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
        }

        public async Task<IEnumerable<Item>> LoadData()
        {
            allItems ??= await _unitOfWork.ItemRepo.Get(includeProperties: "Category");

            itemsControls.Clear();
            itemsControls.AddRange(allItems
                .Select(item => new ItemControl(item, _styleManager))
                .ToList());

            totalPages = (int)Math.Ceiling((double)itemsControls.Count / ITEMS_PER_PAGE);
            DisplayCurrentPage();

            return allItems;
        }

        public void DisplayCurrentPage(IEnumerable<ItemControl> filteredItems = null)
        {
            panelItems.Controls.Clear();

            filteredItems = (filteredItems ?? itemsControls)
                .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();

            foreach (var control in filteredItems)
            {
                control.AddToCartClicked += AddToCartClicked!;
            }

            panelItems.Controls.AddRange(filteredItems.ToArray());

            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            panelPage.Controls.Clear();

            if (currentPage > 1)
            {
                AddPageButton("Previous", currentPage - 1);
            }

            AddPageButton("1", 1);

            var startPage = Math.Max(2, currentPage - 2);
            var endPage = Math.Min(startPage + 3, totalPages);

            if (startPage > 2)
            {
                AddEllipsisButton();
            }

            for (int i = startPage; i <= endPage; i++)
            {
                AddPageButton($"{i}", i);
            }

            if (endPage < totalPages)
            {
                AddEllipsisButton();
            }

            if (totalPages > 1)
            {
                AddPageButton($"Last", totalPages);
            }

            if (currentPage < totalPages)
            {
                AddPageButton("Next", currentPage + 1);
            }
        }

        private void AddPageButton(string text, int pageNum)
        {
            var pageButton = new Button();
            pageButton.Text = text;

            if (pageNum == currentPage)
            {
                pageButton.BackColor = Color.Blue;
                pageButton.ForeColor = Color.White;
            }

            pageButton.Click += (sender, e) =>
            {
                currentPage = pageNum;
                DisplayCurrentPage();
            };
            panelPage.Controls.Add(pageButton);
        }

        private void AddEllipsisButton()
        {
            var ellipsisButton = new Button();
            ellipsisButton.Text = "...";
            ellipsisButton.Enabled = false;
            panelPage.Controls.Add(ellipsisButton);
        }

        public void DisplayFilteredItems(IEnumerable<ItemControl> filteredItems = null)
        {
            var itemsToDisplay = filteredItems ?? itemsControls;
            panelItems.Controls.Clear();

            totalPages = (int)Math.Ceiling((double)itemsToDisplay.Count() / ITEMS_PER_PAGE);
            DisplayCurrentPage(itemsToDisplay);
        }
    }
}