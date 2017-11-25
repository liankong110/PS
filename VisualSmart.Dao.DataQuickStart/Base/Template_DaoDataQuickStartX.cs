using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Dao.DataQuickStart
{
    public class TableDetail
    {
        public string tableName { get; set; }
        public string tableNameDes { get; set; }
        public string columName { get; set; }
        public string IsIdentity { get; set; }
        public string IsIdentity_PK { get; set; }
        public string TYPE { get; set; }
        public string length { get; set; }
        public string IsNULL { get; set; }
        public string defaultValue { get; set; }
        public string columNameDes { get; set; }

    }

    public partial class Template_DaoDataQuickStart
    {
        public void IniData(string tableName)
        {
            var addSql = new StringBuilder();
            var addSqlParameters = new StringBuilder();
            var updateSql = new StringBuilder();
            var selectSql = new StringBuilder();
            GetTableDetail(tableName);
            foreach (var model in models)
            {
                addSql.AppendFormat("{0},", model.columName);
                addSqlParameters.AppendFormat("@{0},", model.columName);
                updateSql.AppendFormat("{0}=@{0},", model.columName);
                selectSql.AppendFormat("{0},", model.columName);
            }
            AddSql = addSql.ToString().Trim(',');
            AddSqlParameters = addSqlParameters.ToString().Trim(',');
            UpdateSql = updateSql.ToString().Trim(',');
            SelectSql = selectSql.ToString().Trim(',');
        }
        public string AddSql { get; set; }

        public string AddSqlParameters { get; set; }

        public string UpdateSql { get; set; }


        public string SelectSql { get; set; }

        List<TableDetail> models = new List<TableDetail>();
        

        public void GetTableDetail(string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT
    tableName       = case when a.colorder=1 then d.name else '' end,
    tableNameDes     = case when a.colorder=1 then isnull(f.value,'') else '' end, 
    columName     = a.name,
    IsIdentity       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '1'else '0' end,
    IsIdentity_PK       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then '1' else '0' end,
    TYPE       = b.name,
    length = a.length,
    长度       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
    小数位数   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
    IsNULL     = case when a.isnullable=1 then '1'else '0' end,
    defaultValue     = isnull(e.text,''),
    columNameDes   = isnull(g.[value],'')
FROM
    syscolumns a
left join
    systypes b
on
    a.xusertype=b.xusertype
inner join
    sysobjects d
on
    a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join
    syscomments e
on
    a.cdefault=e.id
left join
sys.extended_properties   g
on
    a.id=G.major_id and a.colid=g.minor_id 
left join

sys.extended_properties f
on
    d.id=f.major_id and f.minor_id=0
where
    d.name='{0}'   --如果只查询指定表,加上此红色where条件，tablename是要查询的表名；去除红色where条件查询说有的表信息
order by
    a.id,a.colorder", tableName);

            string connectionString = "Data Source=.;Initial Catalog=VisualSmart;Persist Security Info=True;User ID=sa;Password=Fengjian1234.;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand sqlCom = new SqlCommand(sb.ToString());
                using (SqlDataReader objReader = sqlCom.ExecuteReader())
                {
                    if (objReader.Read())
                    {
                        TableDetail model = new TableDetail();
                        model.columName = objReader["columName"].ToString();
                        model.tableName = objReader["tableName"].ToString();
                        model.tableNameDes = objReader["tableNameDes"].ToString();
                        model.IsIdentity = objReader["IsIdentity"].ToString();
                        model.IsIdentity_PK = objReader["IsIdentity_PK"].ToString();
                        model.TYPE = objReader["TYPE"].ToString();
                        model.length = objReader["length"].ToString();
                        model.IsNULL = objReader["IsNULL"].ToString();
                        model.defaultValue = objReader["defaultValue"].ToString();
                        model.columNameDes = objReader["columNameDes"].ToString();
                        models.Add(model);
                    }
                }
                conn.Close();
            }
        }



    }
}
