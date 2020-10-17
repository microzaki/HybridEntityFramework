using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HybridEntityFramework;
namespace HybridEntityFramework.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DBCtrl db = new DBCtrl();
            #region Insert
            Console.WriteLine("Key Enter to start INSERT demostration");
            Console.ReadLine();

            db.commit(new List<SqlScript>() {new SqlScript( "DELETE FROM FEEDER") });
            List<SqlScript> sqls = new List<SqlScript>();
            #region Entity Framework
            FEEDER feeder = new FEEDER() { GRIPPERNO = 1,SCARATRAYNO=2 ,FEEDERNO=3};
            sqls.AddRange(db.Create<FEEDER>(eInsertUpdateType.Normal,feeder));
            #endregion
            #region SQL Script
            sqls.Add( new SqlScript("INSERT INTO FEEDER(GRIPPERNO,SCARATRAYNO,FEEDERNO) VALUES(4,5,6)"));
            #endregion
            db.commit(sqls);
            Console.WriteLine("Insert two rows to FEEDER table by EF & SQL successfully");
            #endregion

            #region Update
            Console.WriteLine("Key Enter to start UPDATE demostration");
            Console.ReadLine();
            sqls = new List<SqlScript>();
            #region Entity Framework
            feeder.SCARATRAYNO = 100;
            sqls.AddRange(db.Update<FEEDER>(feeder));
            #endregion
            db.commit(sqls);
            Console.WriteLine("Update FEEDER by EF successfully and key Enter to stop");
            Console.ReadLine();
            #endregion
        }
    }
}
