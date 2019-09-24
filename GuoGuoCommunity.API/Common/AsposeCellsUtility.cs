using Aspose.Cells;
using System.Data;

namespace GuoGuoCommunity.API.Common
{
    /// <summary>
    /// AsposeCells 帮助类
    /// </summary>
    public class AsposeCellsUtility
    {
        /// <summary>
        /// 导入表格，获取表格中的数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ImportExcel(string filePath)
        {
            Workbook workBook = new Workbook(filePath);
            Worksheet sheet = workBook.Worksheets[0];
            Cells cells = sheet.Cells;
           // DataTable dt = cells.ExportDataTable(0, 0, cells.MaxRow + 1, cells.MaxDataColumn + 1, true);
            DataTable dt = new DataTable();
            for (int i = 0; i < cells.MaxColumn + 1; i++)
            {
                DataColumn newcolumn = new DataColumn(cells[0, i].StringValue);
                dt.Columns.Add(newcolumn);
            }

            for (int j = 0; j < cells.MaxRow; j++)
            {
                DataRow newRow = dt.NewRow();
                for (int k = 0; k < cells.MaxColumn + 1; k++)
                {
                    newRow[k] = cells[j + 1, k].StringValue;
                }
                dt.Rows.Add(newRow);
            }

            DataView dv = new DataView(dt);

            string[] filterColumns = { "姓名", "省", "市", "区", "街道办", "社区", "小区", "楼宇", "单元", "层", "门牌号", "面积", "朝向", "生日", "性别", "手机号", "身份证号" };

            dt = dv.ToTable(true, filterColumns);

            return dt;
        }
    }
}