using AbundantLife_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Data
{
    public class DBCommands
    {

        static List<string> _ParamName = new List<string>();
        static List<object> _ParamValue = new List<object>();
        public readonly static string _Connection = @"Data Source=DESKTOP-O4M7D2C\CHARLESSERVER;Initial Catalog=abundantlife;Integrated Security=True";

        internal static void PopulateParams(string paramName, object paramValue)
        {
            _ParamName.Add(paramName);
            _ParamValue.Add(paramValue);
        }

        static void EmptyParams()
        {
            _ParamName.Clear();
            _ParamValue.Clear();
        }

        internal static object DataReader(string sprocName, ObjectTypes objectType)
        {
            object obj = ObjectFactory(objectType);

            try
            {
                using (SqlConnection con = new SqlConnection(_Connection))
                {
                    SqlCommand cmd = new SqlCommand(sprocName, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int index = 0; index < _ParamName.Count; index++)
                        cmd.Parameters.AddWithValue(_ParamName[index], _ParamValue[index]);

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                prop.SetValue(obj, rdr[prop.Name]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RecordError(ex);
            }
            finally
            {
                EmptyParams();
            }

            return obj;
        }

        internal static string ExecuteScalar(string sprocName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_Connection))
                {
                    SqlCommand cmd = new SqlCommand(sprocName, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int index = 0; index < _ParamName.Count; index++)
                        cmd.Parameters.AddWithValue(_ParamName[index], _ParamValue[index]);

                    con.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        return result.ToString();
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return null;
            }
            finally
            {
                EmptyParams();
            }
        }

        internal static bool ExecuteNonQuery(string sprocName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_Connection))
                {
                    SqlCommand cmd = new SqlCommand(sprocName, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int index = 0; index < _ParamName.Count; index++)
                        cmd.Parameters.AddWithValue(_ParamName[index], _ParamValue[index]);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
            finally
            {
                EmptyParams();
            }
        }

        internal static DataTable AdapterFill(string sprocName)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(_Connection))
                {
                    SqlCommand cmd = new SqlCommand(sprocName, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int index = 0; index < _ParamName.Count; index++)
                        cmd.Parameters.AddWithValue(_ParamName[index], _ParamValue[index]);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
            }
            finally
            {
                EmptyParams();
            }

            return dataTable;
        }

        internal enum ObjectTypes
        {
            UserInfo
        }

        static object ObjectFactory(ObjectTypes objectType)
        {
            if (objectType == ObjectTypes.UserInfo)
            {
                return new UserInfo();
            }
            //else if (objectType == ObjectTypes.Player)
            //{
            //    return new Player();
            //}
            //else if (objectType == ObjectTypes.Team)
            //{
            //    return new Team();
            //}
            //else if (objectType == ObjectTypes.City)
            //{
            //    return new City();
            //}
            //else
            throw new Exception("");
        }

        public static void RecordError(Exception exception)
        {
            try
            {
                //save to database
            }
            catch (Exception ex)
            {
                //email
                throw;
            }
        }
    }
}
