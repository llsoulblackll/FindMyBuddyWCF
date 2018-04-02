using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FindMyBuddyWCF.DataAccess
{
    public class PetDao : EntityDao<Pet>
    {
        private const string USP_PET_INSERT = "usp_Pet_Insert";
        private const string USP_PET_UPDATE = "usp_Pet_Update";
        private const string USP_PET_DELETE = "usp_Pet_Delete";
        private const string USP_PET_FIND = "usp_Pet_Find";
        private const string USP_PET_FINDALL = "usp_Pet_FindAll";

        private const string ID_COLUMN = "Id";
        private const string NAME_COLUMN = "Name";
        private const string AGE_COLUMN = "Age";
        private const string DESCRIPTION_COLUMN = "Description";
        private const string IMAGE_PATH_COLUMN = "ImagePath";
        private const string LOST_LAT_COLUMN = "LostLat";
        private const string LOST_LON_COLUMN = "LostLon";
        private const string RACE_COLUMN = "Race";
        private const string SIZE_COLUMN = "Size";

        private const string ID_PARAM = "@id";
        private const string NAME_PARAM = "@name";
        private const string AGE_PARAM = "@age";
        private const string DESCRIPTION_PARAM = "@description";
        private const string IMAGE_PATH_PARAM = "@imagePath";
        private const string LOST_LAT_PARAM = "@lostLat";
        private const string LOST_LON_PARAM = "@lostLon";
        private const string RACE_PARAM = "@race";
        private const string SIZE_PARAM = "@size";

        private const string CONNECTION_STRING = "Data Source=.;Initial Catalog=FindMyBuddy;Integrated Security=True";

        private IDbConnection connection;
        private IDbCommand command;
        private IDataReader dataReader;


        public int Insert(Pet toInsert)
        {
            using (connection = GetOpenConnection(CONNECTION_STRING))
            using (command = GetStoredProcedureCommand(USP_PET_INSERT, connection))
            {
                command.Parameters.Add(GetParameter(NAME_PARAM, toInsert.Name));
                command.Parameters.Add(GetParameter(AGE_PARAM, toInsert.Age));
                command.Parameters.Add(GetParameter(DESCRIPTION_PARAM, toInsert.Description));
                command.Parameters.Add(GetParameter(IMAGE_PATH_PARAM, toInsert.ImagePath));
                command.Parameters.Add(GetParameter(LOST_LAT_PARAM, toInsert.LostLat));
                command.Parameters.Add(GetParameter(LOST_LON_PARAM, toInsert.LostLon));
                command.Parameters.Add(GetParameter(RACE_PARAM, toInsert.Race));
                command.Parameters.Add(GetParameter(SIZE_PARAM, toInsert.Size));
                //ADO.NET CANNOT INFERE PARAMETERS WITHOUT VALUE IN THIS CASE AN OUTPUT ONE, SO WE NEED TO SPECIFY IT
                //PROVIDE ONLY DBTYPE SINCE WE ARE NOT DEALING WITH VARCHAR OR CHAR INT HAS NOT DEFAULT VALUE OF 0
                command.Parameters.Add(GetOutputParameter(ID_PARAM, SqlDbType.Int));

                command.ExecuteNonQuery();

                int genId = (int)((IDataParameter)command.Parameters[ID_PARAM]).Value;

                return genId;
            }
        }

        public bool Update(Pet toUpdate)
        {
            using (connection = GetOpenConnection(CONNECTION_STRING))
            using (command = GetStoredProcedureCommand(USP_PET_UPDATE, connection))
            {
                command.Parameters.Add(GetParameter(ID_PARAM, toUpdate.Id));
                command.Parameters.Add(GetParameter(NAME_PARAM, toUpdate.Name));
                command.Parameters.Add(GetParameter(AGE_PARAM, toUpdate.Age));
                command.Parameters.Add(GetParameter(DESCRIPTION_PARAM, toUpdate.Description));
                command.Parameters.Add(GetParameter(IMAGE_PATH_PARAM, toUpdate.ImagePath));
                command.Parameters.Add(GetParameter(LOST_LAT_PARAM, toUpdate.LostLat));
                command.Parameters.Add(GetParameter(LOST_LON_PARAM, toUpdate.LostLon));
                command.Parameters.Add(GetParameter(RACE_PARAM, toUpdate.Race));
                command.Parameters.Add(GetParameter(SIZE_PARAM, toUpdate.Size));

                return command.ExecuteNonQuery() > 1;
            }
        }

        public bool Delete(object id)
        {
            using (connection = GetOpenConnection(CONNECTION_STRING))
            using (command = GetStoredProcedureCommand(USP_PET_DELETE, connection))
            {
                command.Parameters.Add(GetParameter(ID_PARAM, id));
                return command.ExecuteNonQuery() > 1;
            }
        }

        public Pet Find(object id)
        {
            using (connection = GetOpenConnection(CONNECTION_STRING))
            using (command = GetStoredProcedureCommand(USP_PET_FIND, connection))
            {
                var param = command.CreateParameter();
                param.ParameterName = "@id";
                param.Value = id;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                using (dataReader = command.ExecuteReader())
                {
                    int ID_INDEX = dataReader.GetOrdinal(ID_COLUMN);
                    int NAME_INDEX = dataReader.GetOrdinal(NAME_COLUMN);
                    int AGE_INDEX = dataReader.GetOrdinal(AGE_COLUMN);
                    int DESCRIPTION_INDEX = dataReader.GetOrdinal(DESCRIPTION_COLUMN);
                    int IMAGE_PATH_INDEX = dataReader.GetOrdinal(IMAGE_PATH_COLUMN);
                    int LOST_LAT_INDEX = dataReader.GetOrdinal(LOST_LAT_COLUMN);
                    int LOST_LON_INDEX = dataReader.GetOrdinal(LOST_LON_COLUMN);
                    int RACE_INDEX = dataReader.GetOrdinal(RACE_COLUMN);
                    int SIZE_INDEX = dataReader.GetOrdinal(SIZE_COLUMN);

                    Pet p = null;
                    if (dataReader.Read())
                    {
                        p = new Pet
                        {
                            Id = dataReader.IsDBNull(ID_INDEX) ? default(int) : dataReader.GetInt32(ID_INDEX),
                            Name = dataReader.IsDBNull(NAME_INDEX) ? default(string) : dataReader.GetString(NAME_INDEX),
                            Age = dataReader.IsDBNull(AGE_INDEX) ? default(int) : dataReader.GetInt32(AGE_INDEX),
                            Description = dataReader.IsDBNull(DESCRIPTION_INDEX) ? default(string) : dataReader.GetString(DESCRIPTION_INDEX),
                            ImagePath = dataReader.IsDBNull(IMAGE_PATH_INDEX) ? default(string) : dataReader.GetString(IMAGE_PATH_INDEX),
                            LostLat = dataReader.IsDBNull(LOST_LAT_INDEX) ? default(double) : dataReader.GetDouble(LOST_LAT_INDEX),
                            LostLon = dataReader.IsDBNull(LOST_LON_INDEX) ? default(double) : dataReader.GetDouble(LOST_LON_INDEX),
                            Race = dataReader.IsDBNull(RACE_INDEX) ? default(string) : dataReader.GetString(RACE_INDEX),
                            Size = dataReader.IsDBNull(SIZE_INDEX) ? default(string) : dataReader.GetString(SIZE_INDEX)
                        };
                    }

                    return p;
                }
            }
        }

        public List<Pet> FindAll()
        {
            using (connection = GetOpenConnection(CONNECTION_STRING))
            using (command = GetStoredProcedureCommand(USP_PET_FINDALL, connection))
            using (dataReader = command.ExecuteReader())
            {
                int ID_INDEX = dataReader.GetOrdinal(ID_COLUMN);
                int NAME_INDEX = dataReader.GetOrdinal(NAME_COLUMN);
                int AGE_INDEX = dataReader.GetOrdinal(AGE_COLUMN);
                int DESCRIPTION_INDEX = dataReader.GetOrdinal(DESCRIPTION_COLUMN);
                int IMAGE_PATH_INDEX = dataReader.GetOrdinal(IMAGE_PATH_COLUMN);
                int LOST_LAT_INDEX = dataReader.GetOrdinal(LOST_LAT_COLUMN);
                int LOST_LON_INDEX = dataReader.GetOrdinal(LOST_LON_COLUMN);
                int RACE_INDEX = dataReader.GetOrdinal(RACE_COLUMN);
                int SIZE_INDEX = dataReader.GetOrdinal(SIZE_COLUMN);

                List<Pet> lPet = new List<Pet>();
                Pet p = null;
                while (dataReader.Read())
                {
                    p = new Pet
                    {
                        Id = dataReader.IsDBNull(ID_INDEX) ? default(int) : dataReader.GetInt32(ID_INDEX),
                        Name = dataReader.IsDBNull(NAME_INDEX) ? default(string) : dataReader.GetString(NAME_INDEX),
                        Age = dataReader.IsDBNull(AGE_INDEX) ? default(int) : dataReader.GetInt32(AGE_INDEX),
                        Description = dataReader.IsDBNull(DESCRIPTION_INDEX) ? default(string) : dataReader.GetString(DESCRIPTION_INDEX),
                        ImagePath = dataReader.IsDBNull(IMAGE_PATH_INDEX) ? default(string) : dataReader.GetString(IMAGE_PATH_INDEX),
                        LostLat = dataReader.IsDBNull(LOST_LAT_INDEX) ? default(double) : dataReader.GetDouble(LOST_LAT_INDEX),
                        LostLon = dataReader.IsDBNull(LOST_LON_INDEX) ? default(double) : dataReader.GetDouble(LOST_LON_INDEX),
                        Race = dataReader.IsDBNull(RACE_INDEX) ? default(string) : dataReader.GetString(RACE_INDEX),
                        Size = dataReader.IsDBNull(SIZE_INDEX) ? default(string) : dataReader.GetString(SIZE_INDEX)
                    };
                    lPet.Add(p);
                }

                return lPet;
            }
        }

        private IDbConnection GetOpenConnection(string connString)
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            return conn;
        }

        private IDbCommand GetStoredProcedureCommand(string procedure, IDbConnection conn)
        {
            var cmd = conn.CreateCommand();//new SqlCommand(procedure, (SqlConnection)conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedure;
            return cmd;
        }

        private IDataParameter GetParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }

        private IDataParameter GetOutputParameter(string name, SqlDbType type, int size = 0)
        {
            //ASK FOR DBTYPE AND SIZE OTHER SIZE CANNOT BE 0 EXCEPTION IS THROWN
            var param = new SqlParameter();
            param.ParameterName = name;
            param.Direction = ParameterDirection.Output;
            param.SqlDbType = type;
            if (size > 0)
                param.Size = size;
            return param;
        }
        /*private T GetDefaultValueIfDBNull<T>(int columnIndex, IDataReader reader)
        {
            return reader.IsDBNull(columnIndex) ? default(T) : reader.Get;
        }*/

    }
}