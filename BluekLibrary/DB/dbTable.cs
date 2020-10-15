using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace BluekLibrary.DB {
    /// <summary>
    /// 테이블 공통
    /// </summary>
    /// <typeparam name="T">레코드 클레스</typeparam>
    public abstract class DbTable<T> where T : DbRowItem {

        protected IDataReader mDr;
        protected AllDbConnection mConn;
        protected List<T> mRows;
        protected DbTable<T> joinTable;

        public DbTable(AllDbConnection con) {
            mConn = con;
            this.InitObject();
        }
        private DbTable(IDataReader rd) {
            mDr = rd;
            this.InitObject();
        }

        /// <summary>
        /// 내부 컨트롤 추가
        /// </summary>
        private void InitObject() {
            mRows = new List<T>();
        }
        /// <summary>
        /// 레코드 수 취득
        /// </summary>
        public long Size => mRows.Count;
        /// <summary>
        /// 테이블 이름 추득
        /// </summary>
        /// <returns></returns>
        public String GetName() => this.GetType().Name;
        /// <summary>
        /// 검색레코드 취득
        /// </summary>
        public List<T> Rows => mRows;
        
        public DbTable<T> InnerJoin(DbTable<T> table) {
            joinTable = table;
            return joinTable;
        }

        /// <summary>
        /// 조건갱신
        /// </summary>
        /// <param name="setItem">갱신항목</param>
        /// <param name="cond">조건</param>
        /// <returns>갱신건수</returns>
        public int UpdateWhere(T setItem, T cond) {
            return this.UpdateWhere(setItem, cond, false);
        }
        /// <summary>
        /// 논리삭제
        /// </summary>
        /// <param name="cond">조건</param>
        public int DeleteFlagUpdate(T cond) {
            return this.DeleteFlagUpdate(cond,false);
        }
        /// <summary>
        /// 논리삭제
        /// </summary>
        /// <param name="cond">조건</param>
        /// <param name="lockFlag">락</param>
        public int DeleteFlagUpdate(T cond, bool lockFlag) {
            String where = (cond as ICreateSQL).CreateConditionSql();
            String item = " SET DeleteFlag = '1'";
            return this.UpdateWhere(item, where,null, lockFlag);
        }
        /// <summary>
        /// 조건갱신
        /// </summary>
        /// <param name="setItem">갱신항목</param>
        /// <param name="cond">조건</param>
        /// <param name="lockFlag">락</param>
        /// <returns>갱신건수</returns>
        public int UpdateWhere(T setItem, T cond, bool lockFlag) {
            String updateitem = (setItem as ICreateSQL).CreateUpdateSetItemSql();
            String where = (cond as ICreateSQL).CreateConditionSql();
            return this.UpdateWhere(updateitem, where, setItem, lockFlag);
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="row">동록 레코드</param>
        /// <returns>등록건수</returns>
        public int Insert(T row) {
            String items = (row as ICreateSQL).CreateInsertItemSql();
            String values = (row as ICreateSQL).CreateInsertValuesSql(mConn);
            return this.InsetSql(items, values,row, false);
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="row">동록 레코드</param>
        /// <param name="lockFlag">락</param>
        /// <returns>등록건수</returns>
        public int Insert(T row, bool lockFlag) {
            String items = (row as ICreateSQL).CreateInsertItemSql();
            String values = (row as ICreateSQL).CreateInsertValuesSql(mConn);
            return this.InsetSql(items, values,row, lockFlag);
        }

        /// <summary>
        /// 데이터 리드에서 데이터 값취득
        /// </summary>
        /// <typeparam name="T"> 데이터 형(DbRowItem 상속)</typeparam>
        /// <param name="rd"> 데이터리드</param>
        /// <returns></returns>
        protected List<T> ReadToDataReader() {
            T item = default(T);

            while (mDr.Read()) {
                item = (T)Activator.CreateInstance(typeof(T), mDr);
                Rows.Add(item);
            }
            return Rows;
        }
        /// <summary>
        /// 레코드 검색 
        /// </summary>
        /// <param name="cond">검색조간</param>
        /// <returns></returns>
        protected IDataReader selectWhere(T cond) {
            return selectWhere(cond,null);
        }
        /// <summary>
        /// 레코드 검색 
        /// </summary>
        /// <param name="cond">검색조간</param>
        /// <param name="orderby">정렬조건</param>
        /// <returns></returns>
        protected IDataReader selectWhere(T cond, OrderByClass orderby ) {
            String select = (cond as ICreateSQL).CreateSelectSql();
            String where = (cond as ICreateSQL).CreateConditionSql();
            String order = orderby!=null? "order by "+ orderby.CreateOrderByString():"";

            IDataReader dr = ExecuteSql(String.Format("SELECT {0} FROM {1} {2} {3}", select, this.GetName(), where, order));
            return dr;
        }

        protected IDataReader selectWhere(String cond,String orderby) {
            String where = " WHERE "+ cond;
            String order = "order by " + orderby;
            IDataReader dr = ExecuteSql(String.Format("SELECT * FROM {0} {1} {2}",  this.GetName(), where, order));
            return dr;
        }
        /// <summary>
        /// 테이블 전체 검색
        /// </summary>
        /// <returns></returns>
        protected IDataReader selectAll() {
            return selectAll(null);
        }
        /// <summary>
        /// 테이블 전체 검색
        /// </summary>
        /// <param name="orderby">정렬조건</param>
        /// <returns></returns>
        protected IDataReader selectAll(OrderByClass orderby) {
            String select = ((T)Activator.CreateInstance(typeof(T)) as ICreateSQL).CreateSelectSql();
            String order = orderby != null ? "order by " + orderby.CreateOrderByString() : "";

            IDataReader dr = ExecuteSql(String.Format("SELECT {0} FROM {1} {2} ", select, this.GetName(), order));
            return dr;
        }

        /// <summary>
        /// 조건갱신
        /// </summary>
        /// <param name="setItem">갱신항목</param>
        /// <param name="cond">조건</param>
        /// <returns>갱신건수</returns>
        protected int UpdateWhere(String setItem, String cond) {
            return UpdateWhere(setItem, cond, null, false);
        }

        /// <summary>
        /// 조건갱신
        /// </summary>
        /// <param name="setItem">갱신항목</param>
        /// <param name="cond">조건</param>
        /// <param name="lockFlag">락</param>
        /// <returns>갱신건수</returns>
        protected int UpdateWhere(String setItem, String cond, T row,bool lockFlag) {
            return this.UpdateExecuteSql(String.Format("update {0} {1} {2}", this.GetName(), setItem, cond), row,lockFlag) ;
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="intems">등록 항목</param>
        /// <param name="values">등록 값</param>
        /// <param name="lockFlag">락</param>
        /// <returns>갱신건수</returns>
        protected int InsetSql(String intems, String values , T row, bool lockFlag) {
            return this.UpdateExecuteSql(String.Format("insert {0} {1} {2}", this.GetName(), intems, values), row, lockFlag);
        }

        /// <summary>
        /// 취득 SQL실행
        /// </summary>
        /// <param name="sql">취득 SQL</param>
        /// <returns>취득 정보</returns>
        protected IDataReader ExecuteSql(String sql) {
            IDbCommand sqlCommand = mConn.CreateCommand();
            sqlCommand.CommandText = sql;
            return sqlCommand.ExecuteReader();
        }
        /// <summary>
        /// 갱신 SQL실행
        /// </summary>
        /// <param name="sql">취득 SQL</param>
        /// <param name="lockFlag">락</param>
        /// <returns>갱신건수</returns>
        protected int UpdateExecuteSql(String sql, T data, bool lockFlag) {
            List<IDataParameter> List=new List<IDataParameter>();
            if (data != null) {
                List.AddRange((data as ICreateSQL).GetParameter(mConn));
            }
            if (lockFlag) {
                lock (mConn.SqlLock) {
                    IDbCommand sqlCommand = mConn.CreateCommand();
                    sqlCommand.CommandText = sql;
                    foreach(IDataParameter item in List) {
                        sqlCommand.Parameters.Add(item);
                    }
                    return sqlCommand.ExecuteNonQuery();
                }
            } else {
                IDbCommand sqlCommand = mConn.CreateCommand();
                if (mConn.Transaction != null) {
                    sqlCommand.Transaction = mConn.Transaction;
                }
                foreach (IDataParameter item in List) {
                    sqlCommand.Parameters.Add(item);
                }
                sqlCommand.CommandText = sql;
                return sqlCommand.ExecuteNonQuery();
            }

        }
    }


}
