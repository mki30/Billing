using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class TableID
{
    static readonly object LockObject = new object();

    public static string Save(string _TableName, Dictionary<string, object> Fields, DatabaseCE _db = null)
    {
        string[] ID = new[] { "ID" };
        return Save(_TableName, Fields, ID, _db);
    }

    public static string Save(string _TableName, Dictionary<string, object> Fields, string[] PK, DatabaseCE _db = null)
    {
        lock (LockObject)
        {
            if (!Fields.Keys.Contains("LUDate"))
                Fields.Add("LUDate", DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));

            string WhereCondition = "", Keys = "";

            // check if going for insert
            foreach (string s in PK)
            {
                WhereCondition += s + "=";

                if (Fields[s] == null)
                    return "Primary Keys can not be null";

                switch (Fields[s].GetType().Name.ToLower())
                {
                    case "string":
                        WhereCondition += "'" + Fields[s].ToString() + "' "; break;
                    case "datetime":
                        WhereCondition += "'" + Cmn.ToDate(Fields[s]).ToString("dd-MMM-yyyy HH:mm:ss") + "' ";
                        break;
                    default:
                        WhereCondition += Fields[s].ToString();
                        break;
                }

                WhereCondition += " and ";
                Keys += s + ",";
            }

            Keys = Keys.Substring(0, Keys.Length - 1);
            WhereCondition = WhereCondition.Substring(0, WhereCondition.Length - 5);

            string SQL = "";

            string Error = "", strVal = "", strType = "";
            DatabaseCE db = _db == null ? new DatabaseCE() : _db;

            try
            {

                //check if the record exits with the primary keys
                int count = db.GetCount(" select count(" + PK[0] + ") from [" + _TableName + "] where " + WhereCondition, ref Error);
                if (count == 0 || Convert.ToInt32(Fields[PK[0]]) == 0) // make it a insert condition
                    WhereCondition = "";


                if (WhereCondition.Length == 0) //its a insert
                {
                    if (Convert.ToInt32(Fields[PK[0]]) == 0 && PK[0] == "ID")
                    {
                        Fields[PK[0]] = db.GetMax(_TableName, PK[0], "", ref Error) + 1;
                    }

                    string strFields = "", strValues = "";

                    foreach (KeyValuePair<string, object> kpv in Fields)
                    {

                        strFields += "[" + kpv.Key + "],";
                        strVal = kpv.Value == null ? "" : kpv.Value.ToString();
                        strType = kpv.Value == null ? "string" : kpv.Value.GetType().Name.ToLower();

                        if (strType == "datetime")
                        {
                            //if (Convert.ToDateTime(strVal) < Cmn.MinDate)
                            //    strVal = Cmn.MinDate.ToString("dd-MMM-yyyy HH:mm");
                            //else
                            strVal = Cmn.ToDate(strVal).ToString("dd-MMM-yyyy HH:mm");
                        }

                        switch (strType)
                        {
                            case "string":
                            case "datetime":
                                strValues += "'" + strVal + "',";
                                break;
                            default:
                                strValues += strVal + ",";
                                break;
                        }

                    }
                    strFields = strFields.Substring(0, strFields.Length - 1);
                    strValues = strValues.Substring(0, strValues.Length - 1);

                    SQL = "insert into [" + _TableName + "] (" + strFields + ") VALUES (" + strValues + ")";
                }
                else
                {
                    foreach (KeyValuePair<string, object> kpv in Fields)
                    {
                        if (PK.Contains(kpv.Key))
                            continue;

                        strVal = kpv.Value == null ? "" : kpv.Value.ToString();
                        strType = kpv.Value == null ? "string" : kpv.Value.GetType().Name.ToLower();

                        if (strType == "datetime")
                        {
                            //if (Convert.ToDateTime(strVal) < Cmn.MinDate)
                            //    strVal = Cmn.MinDate.ToString("dd-MMM-yyyy HH:mm");
                            //else
                            strVal = Convert.ToDateTime(strVal).ToString("dd-MMM-yyyy HH:mm");
                        }

                        switch (strType)
                        {
                            case "string":
                            case "datetime":
                                SQL += "[" + kpv.Key + "]='" + strVal + "',";
                                break;
                            default:
                                SQL += "[" + kpv.Key + "]=" + strVal + ",";
                                break;
                        }
                    }
                    SQL = SQL.Substring(0, SQL.Length - 1); //remove last comma
                    SQL = "update [" + _TableName + "] set " + SQL + " where " + WhereCondition;
                }

                Error += db.RunQuery(SQL);
            }
            catch (Exception ex)
            {
                //Cmn.LogError(ex, "TableID_Save()");
                Error += ex.Message;
            }
            finally
            {
                if (_db == null)
                    db.Close();
            }
            return Error;
        }

    }
}

