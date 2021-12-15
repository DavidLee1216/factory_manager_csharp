using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
//using OpenXml = DocumentFormat.OpenXml.Office.Excel;

using WinForms = System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace factory_manager
{
    public static class excelDataReader
    {
        public static String openExcelFile()
        {
            String filename = "";
            WinForms.OpenFileDialog openFileDialog = new WinForms.OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Browse Excel Files";
            openFileDialog.DefaultExt = "xlsx";
            openFileDialog.Filter = "excel files (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            WinForms.DialogResult result = openFileDialog.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                filename = openFileDialog.FileName;
            }
            return filename;
        }
        private static string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = "";
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                value = cell.CellValue.InnerText;
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        private static bool check_valid_excel_format(String[] defalut, String[] array)
        {
            return defalut.SequenceEqual<String>(array);
        }
        private static void insertData(DataGrid grid, String[] val)
        {
            MySqlConnection con = DatabaseConnection.getDBConnection();
            int id = DatabaseConnection.get_last_id_from_table("assets_info")+1;
            String asset_chinese_name = val[1];
            String asset_korean_name = val[2];
            String date = val[4];
            String count = val[5];
            String model = val[8];
            String price = val[10];
            String owner = val[11];
            if (count == "")
                count = "0";
            if (price == "")
                price = "0";
            String query = $"insert into assets_info (id, name, kor_name, in_date, count, model, price, owner, pay_state, exhaust, back) values('{id}', '{asset_chinese_name}'," +
                $"'{asset_korean_name}', '{date}','{count}', '{model}', '{price}', '{owner}', '0', '0', '0')";
            var cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();
            grid.Items.Add(new AssetInfo(id, asset_chinese_name, asset_korean_name, date, Convert.ToInt32(count), Convert.ToDouble(price), owner, model, false));
            int place_id = DatabaseConnection.get_last_id_from_table("placement_info")+1;
            String place = val[7];
            String remark = val[14];
            String placeCount = val[9];
            query = $"insert into placement_info (id, asset_id, place, count, date, remark) values('{place_id}', '{id}', '{place}', '{placeCount}', '{date}', '{remark}')";
            cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();
//             grid.Items.Add(new AssetInfo(id, asset_chinese_name, asset_korean_name, date, int.Parse(placeCount), double.Parse(price), owner, model, false));
        }
        public static void readExcelData(DataGrid grid, String filename)
        {
            String[] header = { "序号", "品名", "품명", "Name", "入库日期", "数量", "数量合计", "摆放工程", "规格/型号", "数量", "单价", "归属厂", "付款情况", "确认日期", "备注" };
            String[] data_header_strings = { };
            List<string> str_list = new List<string>(data_header_strings);

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filename, false))
            {

                //create the object for workbook part  
                WorkbookPart wbPart = doc.WorkbookPart;

                //statement to get the count of the worksheet  
                int worksheetcount = doc.WorkbookPart.Workbook.Sheets.Count();

                //statement to get the sheet object  
                Sheet mysheet = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.GetItem(0);

                //statement to get the worksheet object by using the sheet id  
                Worksheet Worksheet = ((WorksheetPart)wbPart.GetPartById(mysheet.Id)).Worksheet;

                IEnumerable<Row> rows = Worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                int counter = 0;
                int rowHeaderId = 1;
                foreach (Row row in rows)
                {
                    counter = counter + 1;
                    if (row.Descendants<Cell>().Count() == 1)
                    {
                        rowHeaderId = counter + 1;
                        continue;
                    }
                    if (counter == rowHeaderId)
                    {
                        bool title = false;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            var colunmName = GetCellValue(doc, cell);
                            if (colunmName == "")
                            {
                                rowHeaderId = counter + 1;
                                title = true;
                                break;
                            }
                            str_list.Add(colunmName);
                        }
                        if (title)
                            continue;
                        str_list.RemoveAt(0);
                        data_header_strings = str_list.ToArray();
                        if (check_valid_excel_format(header, data_header_strings) == false)
                        {
                            common.show_message("不合适的文件。", "파일형식이 일치하지 않습니다.");
//                             return;
                        }
                    }
                    else
                    {
                        int id = 0;
                        str_list.Clear();
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            String value = GetCellValue(doc, cell);
                            if (value == "" && cell.CellValue!=null)
                                value = cell.CellValue.InnerText;
                            id++;
                            str_list.Add(value);
                        }
                        String[] strArray = str_list.ToArray();
                        insertData(grid, strArray);
                    }
                }

                    //Note: worksheet has 8 children and the first child[1] = sheetviewdimension,....child[4]=sheetdata  
                    //                 int wkschildno = 4;
                    // 
                    // 
                    //                 //statement to get the sheetdata which contains the rows and cell in table  
                    //                 SheetData Rows = (SheetData)Worksheet.ChildElements.GetItem(wkschildno);
                    // 
                    // 
                    //                 //getting the row as per the specified index of getitem method  
                    //                 Row currentrow = (Row)Rows.ChildElements.GetItem(1);
                    // 
                    //                 //getting the cell as per the specified index of getitem method  
                    //                 Cell currentcell = (Cell)currentrow.ChildElements.GetItem(1);
                    // 
                    //                 //statement to take the integer value  
                    //                 string currentcellvalue = currentcell.InnerText;
                    // 
                }
        }
    }
}
