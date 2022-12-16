INSERT INTO public."Parameter" ("IEValue", "Request") (
	SELECT * FROM
	(
		SELECT "Id" AS "IEValue" FROM
		(
			(
				SELECT * FROM public."InfrastructureElementValue"
			) tb4 
			INNER JOIN
			(
				SELECT * FROM
				(
					SELECT "Id" AS "InfrastructureElement" FROM public."InfrastructureElement" WHERE "Name" = 'Церкви'
				) tb1
				CROSS JOIN
				(
					SELECT "Id" AS "Value" FROM public."Value" WHERE "Name" = 'Важно'
				) tb2
			) tb3
			ON tb4."InfrastructureElement" = tb3."InfrastructureElement" and tb4."Value" = tb3."Value"
		) 
	) tb7
	CROSS JOIN
	(
		SELECT "Id" AS "Request" FROM public."Request" WHERE "Account" = 3
	) tb6
)