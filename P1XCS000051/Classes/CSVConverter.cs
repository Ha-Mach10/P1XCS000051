using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Microsoft.VisualBasic.FileIO;

namespace P1XCS000051
{
    class CSVConverter
    {
        /// <summary>
        /// csvデータをDataTableへ格納
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(string path, Encoding encoding)
        {
            using (TextFieldParser tfp = new TextFieldParser(path, encoding))
            {
                tfp.TextFieldType = FieldType.Delimited;
                tfp.Delimiters = new string[] { "," };
                tfp.HasFieldsEnclosedInQuotes = true;       // デリミタで指定した形式のcsvに対応
                tfp.TrimWhiteSpace = true;                  // 区切り文字前後の空白スペースを削除

                string[] headers = tfp.ReadFields();
                int fieldCount = headers.Length;

                DataTable dt = new DataTable();
                DataColumn dc;
                DataRow dr;

                for (int i = 0; i < fieldCount; i++)
                {
                    dc = new DataColumn(headers[i], typeof(string));
                    dt.Columns.Add(dc);
                }
                while (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();

                    dr = dt.NewRow();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        dr[headers[i]] = fields[i];
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }
    }
}
