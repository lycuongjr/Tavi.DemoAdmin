using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcContrib.UI.Grid;
namespace Tavi.Core.HtmlEx
{
    public class CustomHtmlTableGridRenderer<T> : HtmlTableGridRenderer<T> where T : class
    {
        private int _dataCount = 10;
        private int _index;
        private bool _isRenderEmptyRow = false;

        public CustomHtmlTableGridRenderer(int dataCount)
        {
            _dataCount = dataCount;
        }

        protected override bool ShouldRenderHeader()
        {
            return true;
        }

        protected override void RenderItems()
        {
            if (DataSource == null || DataSource.Count() == 0)
            {
                _isRenderEmptyRow = true;
            }

            if (!_isRenderEmptyRow)
            {
                base.RenderItems();
            }

            RenderEmptyRows();
        }

        protected void RenderEmptyRows()
        {
            int remain = _dataCount - _index;

            if (remain > 0)
            {
                for (int i = 0; i < remain; i++)
                {
                    GridRowViewData<T> grid;

                    if (_index % 2 == 0)
                    {
                        grid = new GridRowViewData<T>(default(T), false);
                    }
                    else
                    {
                        grid = new GridRowViewData<T>(default(T), true);
                    }

                    RenderItem(grid);
                }
            }
        }

        protected override void RenderItem(GridRowViewData<T> rowData)
        {
            _index++;

            base.RenderItem(rowData);
        }

        protected override void RenderCellValue(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            if (rowData.Item == null)
            {
                this.Writer.Write("&nbsp;");
                return;
            }

            base.RenderCellValue(column, rowData);
        }
    }
}
