using ITV.MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITV.BLL
{
    public class BLL_Login:BLL_Base 
    {
        public List<FUNCTIONS> GetFunctions(string operID)
        { 
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@" SELECT [FUNC_ID]
                                      ,[NODE_ID]
                                      ,[PARENT_ID]
                                      ,[MENU_TEXT]
                                      ,[MENU_VALUE]
                                      ,[MENU_IS_USE]
                                      ,[MENU_SORT]
                                      ,[MENU_LEVEL] from  FUNCTIONS where MENU_IS_Use = '1' AND exists
                                 (select 1 from ROLE_POWERS
                                 where ROLE_POWERS.FUNC_ID = FUNCTIONS.FUNC_ID
                                 and  exists(select 1 from ROLE_ASSIGN where ROLE_ASSIGN.ROLE_ID = ROLE_POWERS.ROLE_ID
                                             and exists(select 1 from OPERATORS where OPERATORS.OPER_ID = ROLE_ASSIGN.OPER_ID and OPER_ID = '{0}')
                                             )
                                  ) ", operID));

            return this._adoContext.DBExecuteAsIEnumerable<FUNCTIONS>(strSql.ToString(), null, System.Data.CommandType.Text).ToList<FUNCTIONS>();

        }
        /// <summary>
        /// 登录验证，并返回当前用户的信息和功能
        /// </summary>
        /// <param name="operCode"></param>
        /// <param name="operPwd"></param>
        /// <returns></returns>
        public ResultInfo<KeyValuePair<OPERATORS, List<FUNCTIONS>>> LoginValidate(string operCode, string operPwd)
        {
        
            ResultInfo<KeyValuePair<OPERATORS,List<FUNCTIONS>>> result = new ResultInfo<KeyValuePair<OPERATORS,List<FUNCTIONS>>>();
            try
            {
                #region 方法1 
                if (true)
                {
                    var pwdEcy = COMMON.Encryption.Cipher.MD5Encrypt32(operPwd);
                    var oper = this.OPERATORS.Where(x => x.OPER_CODE == operCode && x.OPER_PW == pwdEcy).FirstOrDefault();
                    if (oper == null)
                    {
                        result.Success = false;
                        result.Message = "用户名或者密码错误！";
                        return result;
                    }

                    oper.OPER_PW = string.Empty;
                    var role = oper.ROLES.ToList();
                    List<FUNCTIONS> funList = new List<FUNCTIONS>();
                    role.ForEach(r =>
                    {
                        funList.AddRange(r.FUNCTIONS.ToList());
                    });

                    funList = funList.Distinct().ToList();
                    result.Data = new KeyValuePair<OPERATORS, List<FUNCTIONS>>(oper, funList);
                    result.Success = true; 
                }
                #endregion

                #region 方法2
                
                #endregion
            }
            catch (Exception)
            {
                
                throw;
            }
            return result;
        }
    }
}
