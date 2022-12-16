INSERT INTO public."Account" (
"Mail", "Login", "Password", "IsAdmin") VALUES (
'ne hehe'::character varying, 'kuks'::character varying, '312312'::character varying, false::boolean)
 returning "Id";