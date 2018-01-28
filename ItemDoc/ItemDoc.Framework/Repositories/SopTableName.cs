using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class SopTableName : TableNameAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public SopTableName(string tableName) :
            base(tableName)
        {

        }
       

    }



}
