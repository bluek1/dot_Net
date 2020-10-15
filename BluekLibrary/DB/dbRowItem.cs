using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace BluekLibrary.DB {
    public abstract class  DbRowItem : ICreateSQL {

        public DbRowItem() {
            foreach (PropertyInfo pinfo in this.GetType().GetProperties()) {
                AttriDbValue attri = pinfo.GetCustomAttribute<AttriDbValue>(false);
                if(attri != null) {
                    DbValue val = new DbValue(attri );
                    val.SetName(pinfo.Name);
                    pinfo.SetValue(this, val);
                } else if (pinfo.PropertyType == typeof(DbValue)) {
                    DbValue val = (DbValue)pinfo.GetValue(this);
                    val.SetName(pinfo.Name);
                }
            }
        }

        public DbRowItem(IDataReader rd) {
            foreach (String str in (this as ICreateSQL).GetFiledListItem()) {
                if (this.GetType().GetProperty(str).PropertyType == typeof(DbValue)) {
                    object val = this.GetType().GetProperty(str).GetValue(this);
                    if (val is DbValue) {
                        if ((val as DbValue).SelectItemVisible) {
                            this.GetType().GetProperty(str).SetValue(this, new DbValue(rd[str.ToUpper()].ToString()));
                        }
                    }
                } else {
                    this.GetType().GetProperty(str).SetValue(this, rd[str.ToUpper()].ToString());
                }
            }
        }



        /// <summary>
        /// 데이터 리드에서 데이터 값취득
        /// </summary>
        /// <typeparam name="T"> 데이터 형(DbRowItem 상속)</typeparam>
        /// <param name="rd"> 데이터리드</param>
        /// <returns></returns>
        public static T ReadToDataReader<T>(IDataReader rd) where T : ICreateSQL {
            T rtn = (T)Activator.CreateInstance(typeof(T));
            foreach (String str in rtn.GetFiledListItem()) {
                typeof(T).GetProperty(str).SetValue(rtn,rd[str.ToUpper()]);
            }
            return rtn;
        }
        /// <summary>
        /// 구성항목 취득
        /// </summary>
        /// <returns></returns>
        IEnumerable<String> ICreateSQL.GetFiledListItem() {
            PropertyInfo[] Props = this.GetType().GetProperties();
            foreach(PropertyInfo p in Props) {
                yield return p.Name;
            }
        }
        /// <summary>
        /// Select문 작성
        /// </summary>
        /// <returns>Select문</returns>
        String ICreateSQL.CreateSelectSql() {
            StringBuilder select = new StringBuilder("");
            foreach (String str in (this as ICreateSQL).GetFiledListItem()) {
                object val = this.GetType().GetProperty(str).GetValue(this);
                if (val is DbValue) {
                    if ((val as DbValue).SelectItemVisible) {
                        select.AppendFormat(" {0} AS {1} ,", str, str);
                    } 
                }
                
            }
            select.Remove(select.Length - 1,1);
            return select.ToString();
        }

        /// <summary>
        /// 조건문 작성
        /// </summary>
        /// <returns>조건문</returns>
        String ICreateSQL.CreateConditionSql() {
            StringBuilder select = new StringBuilder("WHERE ");
            foreach (String str in (this as ICreateSQL).GetFiledListItem()) {
                Object val = this.GetType().GetProperty(str).GetValue(this) ;
                if (!(val as DbValue ).IsNull) { 
                    select.AppendFormat(" {0} = '{1}' AND", str, val);
                }
            }
            select.Remove(select.Length - 3,3);
            return select.ToString();
        }

        /// <summary>
        /// Update문 작성
        /// </summary>
        /// <returns>UPDATE문</returns>
        String ICreateSQL.CreateUpdateSetItemSql() {
            StringBuilder select = new StringBuilder("SET ");
            foreach (String str in (this as ICreateSQL).GetFiledListItem()) {
                object val = this.GetType().GetProperty(str).GetValue(this);
                if (val is DbValue) {
                    if ((val as DbValue).IsParameter&& !(val as DbValue).IsNull) {
                        select.AppendFormat(" {0} = {1} ,", str, val);
                    } else {
                        if (!(val as DbValue).IsNull) {
                            select.AppendFormat(" {0} = '{1}' ,", str, val);
                        }
                    }
                }
            }
            select.Remove(select.Length - 1, 1);
            return select.ToString();
        }

        /// <summary>
        /// INSERT문작성
        /// </summary>
        /// <param name="con">커넥션</param>
        /// <returns>INSERT문</returns>
        String ICreateSQL.CreateInsertValuesSql(AllDbConnection con) {
            StringBuilder values = new StringBuilder("VALUES (");
            foreach (String str in (this as ICreateSQL).GetFiledListItem()) {
                if(str.ToLower() == "deleteflag" || str.ToLower() == "updatetime") continue;
                object val = this.GetType().GetProperty(str).GetValue(this);
                if (val is DbValue) {
                    if ((val as DbValue).IsParameter&& !(val as DbValue).IsNull) {
                        values.AppendFormat("{0} ,", val);
                    } else {
                        if (!(val as DbValue).IsNull) {
                            values.AppendFormat("'{0}' ,", val);
                        }
                        
                    }
                }
            }
            values.AppendFormat("'0' , {0})", con.GetKeyWord.getSystemDate());
            return values.ToString();
        }

        /// <summary>
        /// INSERT문 항목부분작성
        /// </summary>
        /// <returns>INSERT문</returns>
        String ICreateSQL.CreateInsertItemSql() {
            StringBuilder values = new StringBuilder("(");
            foreach (String str in (this as ICreateSQL).GetFiledListItem()) {
                if (str.ToLower() == "deleteflag" || str.ToLower() == "updatetime") continue;
                object val = this.GetType().GetProperty(str).GetValue(this);
                if (val is DbValue) {
                    if (!(val as DbValue).IsNull) {
                        values.AppendFormat("{0} ,", str);
                    }
                } else {
                    values.AppendFormat("{0} ,", str);
                }
            }
            values.Append(" DeleteFlag ,UpdateTime)");
            return values.ToString();
        }

        /// <summary>
        /// SQL파라미터 취득
        /// </summary>
        /// <param name="con">커넥션</param>
        /// <returns>SQL파라미터</returns>
        IDataParameter[] ICreateSQL.GetParameter(AllDbConnection con) {
            List<IDataParameter> list= new List<IDataParameter>();
            foreach (String str in (this as ICreateSQL).GetFiledListItem()) {
                object val = this.GetType().GetProperty(str).GetValue(this);
                if (val is DbValue) {
                    if ((val as DbValue).IsParameter && !(val as DbValue).IsNull) {
                        list.Add((val as DbValue).GetParameter(con.ConnectStringBuilder));
                    }
                }
                
            }
            return list.ToArray();
        }

    }

    public interface ICreateSQL {
        IEnumerable<String> GetFiledListItem();
        /// <summary>
        /// 업데이트 SQL작성
        /// </summary>
        /// <returns></returns>
        String CreateUpdateSetItemSql();
        /// <summary>
        /// SELECT문에 조건 선택
        /// </summary>
        /// <returns></returns>
        String CreateConditionSql();
        /// <summary>
        /// SELECT문 적성
        /// </summary>
        /// <returns></returns>
        String CreateSelectSql();
        /// <summary>
        /// INSERT문의 VALUES작성
        /// </summary>
        /// <returns></returns>
        String CreateInsertValuesSql(AllDbConnection con);
        /// <summary>
        /// INSERT문의 테이블 정의 부분작성
        /// </summary>
        /// <returns></returns>
        String CreateInsertItemSql();
        /// <summary>
        /// 파라미터 취득
        /// </summary>
        /// <returns></returns>
        IDataParameter[] GetParameter(AllDbConnection con);


    }
}
