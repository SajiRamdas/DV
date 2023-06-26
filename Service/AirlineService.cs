using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Service
{
    public class AirlineService
    {
        DBConnect dbConnect = new DBConnect();

        public async Task<int> UploadLegData(string filePath)
        {
            var dataFromCSV = GetDataTabletFromCSVFile(filePath);
            dataFromCSV.DefaultView.Sort = "LegId ASC";
            return await dbConnect.InsertLeg(dataFromCSV);
        }

        private static DataTable GetDataTabletFromCSVFile(string filePath)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(filePath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        var field = csvReader.ReadFields();
                        string[] fieldData = field[0].ToString().Split(',');
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return csvData;
        }

        public async Task<DataTable> GetVisits(string filterBy, string aircraftReg, string depDate, string flightNum)
        {
            return await dbConnect.GetVisits(filterBy, aircraftReg, depDate, flightNum);
        }
    }
}
