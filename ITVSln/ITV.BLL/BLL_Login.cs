using ITV.MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITV.BLL
{
    public class BLL_Login:BLL_Base
    {
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
                var pwdEcy = COMMON.Encryption.Cipher.MD5Encrypt32(operPwd);
                var oper = this.OPERATORS.Where(x => x.OPER_CODE == operCode && x.OPER_PW == pwdEcy).FirstOrDefault();
                if(oper==null)
                {
                    result.Success = false;
                    result.Message = "用户名或者密码错误！";
                    return result;
                }

                oper.OPER_PW = string.Empty;
                var role = oper.ROLES.ToList();
                List<FUNCTIONS> funList = new List<FUNCTIONS>();
                role.ForEach(r => {
                    funList.AddRange(r.FUNCTIONS.ToList());
                });

                funList = funList.Distinct().ToList();
                result.Data = new KeyValuePair<OPERATORS, List<FUNCTIONS>>(oper,funList);
                result.Success = true;
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
