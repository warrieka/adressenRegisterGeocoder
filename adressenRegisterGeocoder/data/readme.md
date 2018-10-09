Put shapefile of wegenregister here 
===================================

Download from: https://download.vlaanderen.be/Producten/Detail?id=4277

Disolve and simplify on LSTRNMID using spatialite.  
It shoult be name wr.shp and all field except streetnameid should be removed.

In  SQL: 

    CREATE Table wr AS
    SELECT CastToMultiLinestring( Simplify( st_unaryunion(st_collect(wr0.geom)), 1 ) AS Geom, wr0.LSTRNMID as ID 

    FROM wegenregister as wr0

    GROUP BY  wr0.LSTRNMID 