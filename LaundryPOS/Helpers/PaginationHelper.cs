﻿using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Forms.Views;

namespace LaundryPOS.Helpers
{
    public class PaginationHelper<T>
        where T : Control
    {
        private const int ITEMS_PER_PAGE = 16;
        private int totalPages = 1;

        public event EventHandler<EventArgs> PageClicked;
        private readonly FlowLayoutPanel panelPage;
        private readonly IStyleManager _styleManager;

        public int CurrentPage { get; set; } = 1;

        public PaginationHelper(FlowLayoutPanel pagePanel, IStyleManager styleManager)
        {
            panelPage = pagePanel;
            _styleManager = styleManager;
        }

        public IEnumerable<ItemControl> GetPagedItems(IEnumerable<ItemControl> filteredItems = null)
        {
            totalPages = (int)Math.Ceiling((double)filteredItems.Count() / ITEMS_PER_PAGE);

            filteredItems = (filteredItems)
                .Skip((CurrentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();

            UpdateNavigationButtons();

            return filteredItems;
        }

        private void UpdateNavigationButtons()
        {
             panelPage.Controls.Clear();

            if (CurrentPage > 1)
            {
                AddPageButton("Previous", CurrentPage - 1);
            }

            AddPageButton("1", 1);

            var startPage = Math.Max(2, CurrentPage - 2);
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

            if (CurrentPage < totalPages)
            {
                AddPageButton("Next", CurrentPage + 1);
            }
        }

        private void AddPageButton(string text, int pageNum)
        {
            var pageButton = new Guna2Button()
            {
                Text = text,
                FillColor = Color.White,
                ForeColor = Color.FromArgb(48, 48, 48),
                Size = new Size(90, 30),
                BorderRadius = 5,
                BorderThickness = 1,
                Font = _styleManager.Font.Helvetica(10f),
            };

            if (pageNum == CurrentPage)
            {
                pageButton.FillColor = Color.FromArgb(48, 48, 48);
                pageButton.ForeColor = Color.White;
            }

            pageButton.Click += (sender, e) =>
            {
                CurrentPage = pageNum;
                PageClicked?.Invoke(this, EventArgs.Empty);
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
    }
}