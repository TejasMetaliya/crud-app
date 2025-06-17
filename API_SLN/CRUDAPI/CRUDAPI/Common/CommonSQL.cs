namespace CRUDAPI.Common
{
	public class CommonSQL
	{
		public static string USERSSelectSql = @"SELECT		ID, 
															NAME,
															PASSWORD
												FROM		USERS
												ORDER BY	ID";

		public static string USERSSelectCountSql = @"	SELECT		COUNT(1)
														FROM		USERS
														WHERE		NAME = '{0}'
														AND			PASSWORD = '{1}' ";
		//"SELECT COUNT(1) FROM USERS WHERE NAME = :username AND PASSWORD = :password";

		public static string M_PRODUCTSelectSql = @"SELECT		PROID,
																PROCODE,
																PRONAME,
																PRODESCRIPTION,
																PROPRICE,
																PROCATEGORY,
																PROQUANTITY,
																PROINVENTORYSTATUS
													FROM		M_PRODUCT
													ORDER BY	PROID";

	}
}
