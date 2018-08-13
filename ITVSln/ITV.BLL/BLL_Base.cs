using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITV.BLL
{
    public class BLL_Base:MDL.ITVContext
    {
        public DataAccess.EF.EFDataAccessBase _efContext;
        public DataAccess.ADO.ADODataAccessBase _adoContext;

        public BLL_Base():base()
        {
            
            _efContext = new DataAccess.EF.EFDataAccessBase(base.Database.Connection.ConnectionString);
            _adoContext = new DataAccess.ADO.SqlDataAccess(base.Database.Connection.ConnectionString);
           
        }

    }
}
