using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DBConnect
    {
        public async Task<int> InsertLeg(DataTable csvFileData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(await GetConString()))
                {
                    foreach (DataRow dr in csvFileData.Rows)
                    {
                        using (SqlCommand cmd = new SqlCommand("dbo.InsertIntoLeg", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("LegId", SqlDbType.Int).Value = dr["LegId"];
                            cmd.Parameters.Add("AirlineCode", SqlDbType.Char).Value = dr["AirlineCode"];
                            cmd.Parameters.Add("AircraftRegistration", SqlDbType.Char).Value = dr["AircraftRegistration"];
                            cmd.Parameters.Add("FlightNumber", SqlDbType.Int).Value = dr["FlightNumber"];
                            cmd.Parameters.Add("Suffix", SqlDbType.Char).Value = dr["Suffix"];
                            cmd.Parameters.Add("DepartureStation", SqlDbType.Char).Value = dr["DepartureStation"];
                            cmd.Parameters.Add("ArrivalStation", SqlDbType.Char).Value = dr["ArrivalStation"];

                            if (dr["STDDate"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("STDDate", SqlDbType.DateTime).Value = dr["STDDate"];
                                cmd.Parameters.Add("STDTime", SqlDbType.Char).Value = dr["STDTime"];
                            }

                            if (dr["ATDDate"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("ATDDate", SqlDbType.DateTime).Value = dr["ATDDate"];
                                cmd.Parameters.Add("ATDTime", SqlDbType.Char).Value = dr["ATDTime"];
                            }

                            if (dr["STDDateLocal"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("STDDateLocal", SqlDbType.DateTime).Value = dr["STDDateLocal"];
                                cmd.Parameters.Add("STDTimeLocal", SqlDbType.Char).Value = dr["STDTimeLocal"];
                            }

                            if (dr["ATDDateLocal"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("ATDDateLocal", SqlDbType.DateTime).Value = dr["ATDDateLocal"];
                                cmd.Parameters.Add("ATDTimeLocal", SqlDbType.Char).Value = dr["ATDTimeLocal"];
                            }

                            if (dr["STADate"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("STADate", SqlDbType.DateTime).Value = dr["STADate"];
                                cmd.Parameters.Add("STATime", SqlDbType.Char).Value = dr["STATime"];
                            }

                            if (dr["ATADate"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("ATADate", SqlDbType.DateTime).Value = dr["ATADate"];
                                cmd.Parameters.Add("ATATime", SqlDbType.Char).Value = dr["ATATime"];
                            }

                            if (dr["STADateLocal"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("STADateLocal", SqlDbType.DateTime).Value = dr["STADateLocal"];
                                cmd.Parameters.Add("STATimeLocal", SqlDbType.Char).Value = dr["STATimeLocal"];
                            }

                            if (dr["ATADateLocal"].ToString() != "NULL")
                            {
                                cmd.Parameters.Add("ATADateLocal", SqlDbType.DateTime).Value = dr["ATADateLocal"];
                                cmd.Parameters.Add("ATATimeLocal", SqlDbType.Char).Value = dr["ATATimeLocal"];/**/
                            }

                            if (con.State == ConnectionState.Closed)
                                con.Open();

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return 1;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public async Task<DataTable> GetVisits(string filterBy, string aircraftReg, string depDate, string flightNum)
        {
            var dt = new DataTable();
            using (SqlConnection con = new SqlConnection(await GetConString()))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.GetVisits", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("FilterBy", SqlDbType.Char).Value = filterBy;
                    cmd.Parameters.Add("AircraftReg", SqlDbType.Char).Value = aircraftReg;
                    cmd.Parameters.Add("DepDate", SqlDbType.DateTime).Value = depDate;
                    cmd.Parameters.Add("FlightNum", SqlDbType.VarChar).Value = flightNum;

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    dt.Load(cmd.ExecuteReader());
                }
            }
            return dt;
        }

        private static async Task<string> GetConString()
        {
            return await Task.Run(()=>@"Data Source=SONY\SQLEXPRESS;Initial Catalog=AircraftVisits;Integrated Security=True;Integrated Security=SSPI;");
        }

    }
}
