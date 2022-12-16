SELECT "Id" FROM
(
	(
		SELECT * FROM public."District"
	) tb1
	INNER JOIN 
	(
		SELECT "Id" as "City" FROM
		(
			SELECT * FROM public."City"
		) tb2
		INNER JOIN 
		(
			SELECT "Id" AS "Country" FROM public."Country" WHERE "Name" = 'Россия'
		) tb3
		ON tb2."Name" = 'Санкт-Петербург' and tb3."Country" = tb2."Country"
	) tb4
	ON tb1."Name" = 'Калининский' and tb1."City" = tb4."City"
)