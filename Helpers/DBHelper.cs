using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace SampleApp.Helpers
{
    public class DBHelper:IDisposable
    {
        protected string _connString = null;
        protected SqlConnection _conn = null;
        protected SqlTransaction _trans = null;
        protected bool _disposed = false;
        protected int _commandTimeout = 0;
               
        public SqlTransaction Transaction { get { return _trans; } }
                
        public DBHelper(string connStringName)
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings[connStringName];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new System.Exception("Fatal error: missing connecting string (" + connStringName + ") in web.config file");
            _connString = mySetting.ConnectionString;
            Connect();
        }
               
        public DBHelper()
        {           
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["ConnString"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new System.Exception("Fatal error: missing connecting string ");
            _connString = mySetting.ConnectionString;
            Connect();

        }        
        protected void Connect()
        {
            _conn = new SqlConnection(_connString);
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
        }
        protected void DisConnect()
        {            
            if (_conn != null && _conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }
       
        public SqlCommand CreateCommand(string qry, CommandType type, params object[] args)
        {
            SqlCommand cmd = new SqlCommand(qry, _conn);
            cmd.CommandTimeout = _commandTimeout;

            // Associate with current transaction, if any
            if (_trans != null)
                cmd.Transaction = _trans;

            // Set command type
            cmd.CommandType = type;

            // Construct SQL parameters
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string && i < (args.Length - 1))
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = (string)args[i];
                    parm.Value = args[++i];
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
                else if (args[i] is SqlParameter)
                {
                    cmd.Parameters.Add((SqlParameter)args[i]);
                }
                else throw new ArgumentException("Invalid number or type of arguments supplied");
            }
            return cmd;
        }        
        
        public int ExecNonQuery(string qry, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
        public int ExecNonQueryProc(string proc, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(proc, CommandType.StoredProcedure, args))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        public object ExecScalar(string qry, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    return cmd.ExecuteScalar();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        public object ExecScalarProc(string qry, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    return cmd.ExecuteScalar();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
        
        public SqlDataReader ExecDataReader(string qry, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    return cmd.ExecuteReader();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        public SqlDataReader ExecDataReaderProc(string qry, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    return cmd.ExecuteReader();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        public DataSet ExecDataSet(string qry, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
        public DataSet ExecDataSetQuery(string qry)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        public DataSet GetDataSetProc(string qry)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
        
        public DataSet ExecDataSetProc(string qry, params object[] args)
        {
            try
            {
                Connect();
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        public SqlTransaction BeginTransaction()
        {
            Rollback();
            _trans = _conn.BeginTransaction();
            return Transaction;
        }
       
        public void Commit()
        {
            if (_trans != null)
            {
                _trans.Commit();
                _trans = null;
            }
        }

        public void Rollback()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                _trans = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Need to dispose managed resources if being called manually
                if (disposing)
                {
                    if (_conn != null)
                    {
                        Rollback();
                        _conn.Dispose();
                        _conn = null;
                    }
                }
                _disposed = true;
            }
        }

    }
}
    
