# Purpose
Most of EF architectures such as [EF Code](https://github.com/dotnet/efcore) are designed for specific DB type and less functions than SQL. 

This DLL combines EF and SQL together, you can choose either one to use. And finally commit all of them in one commitment. 
# Environment
So far it only supports SQLite. But it's quite easy to support other RDBMS by replacement of DB inteface DLL.
# Example Code
Insert two rows. Fist one uses EF and second one uses SQL. Finally commit both on them.
```C#
List<SqlScript> sqls = new List<SqlScript>();
#region Entity Framework
FEEDER feeder = new FEEDER() { GRIPPERNO = 1,SCARATRAYNO=2 ,FEEDERNO=3};
sqls.AddRange(db.Create<FEEDER>(eInsertUpdateType.Normal,feeder));
#endregion
#region SQL Script
sqls.Add( new SqlScript("INSERT INTO FEEDER(GRIPPERNO,SCARATRAYNO,FEEDERNO) VALUES(4,5,6)"));
#endregion
db.commit(sqls);
```
