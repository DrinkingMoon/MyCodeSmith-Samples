/****************************************************************************
* This utilty class is for sp,dal,bll.
****************************************************************************/

/************************************************************************************
***For trasaction.
*************************************************************************************/
#region Isolation Level
public enum TransactionIsolationLevelEnum
{
	ReadCommitted,
	ReadUncommitted,
	RepeatableRead,
	Serializable
}

public void GenerateSetTransactionIsolationLevelStatement(TransactionIsolationLevelEnum isolationLevel)
{
	Response.Write("SET TRANSACTION ISOLATION LEVEL ");
	
	switch (isolationLevel)
	{
		case TransactionIsolationLevelEnum.ReadUncommitted:
		{
			Response.WriteLine("READ UNCOMMITTED");
			break;
		}
		case TransactionIsolationLevelEnum.RepeatableRead:
		{
			Response.WriteLine("REPEATABLE READ");
			break;
		}
		case TransactionIsolationLevelEnum.Serializable:
		{
			Response.WriteLine("SERIALIZABLE");
			break;
		}
		default:
		{
			Response.WriteLine("READ COMMITTED");
			break;
		}
	}
}
#endregion

/************************************************************************************
***These methods are used for creating unified procedure name for the whole project.
***Please the name consecutive.
*************************************************************************************/
#region Procedure Naming
public string GetTableOwner()
{
	return GetTableOwner(true);
}

public string GetTableOwner(bool includeDot)
{
	if (TargetTable.Owner.Length > 0)
	{
		if (includeDot)
		{
			return "[" + TargetTable.Owner + "].";
		}
		else
		{
			return "[" + TargetTable.Owner + "]";
		}
	}
	else
	{
		return "";
	}
}

public string GetInsertProcedureName()
{
	return String.Format("{0}[{1}{2}_Insert]", GetTableOwner(), ProcedurePrefix, GetEntityName(false));
}

public string GetUpdateProcedureName()
{
	return String.Format("{0}[{1}{2}_Update]", GetTableOwner(), ProcedurePrefix, GetEntityName(false));
}

public string GetInsertUpdateProcedureName()
{
	return String.Format("{0}[{1}{2}_Save]", GetTableOwner(), ProcedurePrefix, GetEntityName(false));
}

public string GetDeleteProcedureName()
{
	return String.Format("{0}[{1}{2}_Delete]", GetTableOwner(), ProcedurePrefix, GetEntityName(false));
}

public string GetSelectProcedureName()
{
	return String.Format("{0}[{1}{2}_Select]", GetTableOwner(), ProcedurePrefix, GetEntityName(false));
}

public string GetSelectAllProcedureName()
{
	return String.Format("{0}[{1}{2}_SelectAll]", GetTableOwner(), ProcedurePrefix, GetEntityName(true));
}

public string GetSelectPagedProcedureName()
{
	return String.Format("{0}[{1}{2}_SelectPaged]", GetTableOwner(), ProcedurePrefix, GetEntityName(true));
}

public string GetSelectByProcedureName(ColumnSchemaCollection targetColumns)
{
	return String.Format("{0}[{1}{2}_SelectBy{3}]", GetTableOwner(), ProcedurePrefix, GetEntityName(true), GetBySuffix(targetColumns));
}

public string GetSelectByProcedureName(ColumnSchema column)
{
	return String.Format("{0}[{1}{2}_SelectBy{3}]", GetTableOwner(), ProcedurePrefix, GetEntityName(true), column.Name);
}

public string GetSelectDynamicProcedureName()
{
	return String.Format("{0}[{1}{2}_SelectDynamic]", GetTableOwner(), ProcedurePrefix, GetEntityName(true));
}

public string GetDeleteByProcedureName(ColumnSchemaCollection targetColumns)
{
	return String.Format("{0}[{1}{2}_DeleteBy{3}]", GetTableOwner(), ProcedurePrefix, GetEntityName(true), GetBySuffix(targetColumns));
}

public string GetDeleteByProcedureName(ColumnSchema column)
{
	return String.Format("{0}[{1}{2}_DeleteBy{3}]", GetTableOwner(), ProcedurePrefix, GetEntityName(true), column.Name);
}


public string GetDeleteDynamicProcedureName()
{
	return String.Format("{0}[{1}{2}_DeleteDynamic]", GetTableOwner(), ProcedurePrefix, GetEntityName(true));
}

public string AddDot(int index, int count)
{
	if(index != count -1)
	{
		return ",";
	}
	return string.Empty;
}

public string GetEntityName(bool plural)
{
	string entityName = TargetTable.Name;
	
	if (plural)
	{
		entityName = StringUtil.ToPlural(entityName);
	}
	else
	{
		entityName = StringUtil.ToSingular(entityName);
	}
	
	return entityName;
}

public string GetBySuffix(ColumnSchemaCollection columns)
{
    System.Text.StringBuilder bySuffix = new System.Text.StringBuilder();
	for (int i = 0; i < columns.Count; i++)
	{
	    if (i > 0) bySuffix.Append("And");
	    bySuffix.Append(columns[i].Name);
	}
			
	return bySuffix.ToString();
}
#endregion

#region String Tool

//用于生成正确的参数的注释
public string GenerateCommentForParameter(string parameterName)
{
	string str = "/// <param name=\"{0}\"></param>";
	return String.Format(str, parameterName);
}


#endregion 

#region DB Tools
public string GetCSharpTypeFromDBFieldType(ColumnSchema column)
{
	if (column.Name.EndsWith("TypeCode")) return column.Name;
	
	switch (column.DataType)
	{
		case DbType.AnsiString: return "string";
		case DbType.AnsiStringFixedLength: return "string";
		case DbType.Binary: return "byte[]";
		case DbType.Boolean: return "bool?";
		case DbType.Byte: return "byte?";
		case DbType.Currency: return "decimal?";
		case DbType.Date: return "DateTime?";
		case DbType.DateTime: return "DateTime?";
		case DbType.Decimal: return "decimal?";
		case DbType.Double: return "double?";
		case DbType.Guid: return "Guid?";
		case DbType.Int16: return "short?";
		case DbType.Int32: return "int?";
		case DbType.Int64: return "long?";
		case DbType.Object: return "object";
		case DbType.SByte: return "sbyte?";
		case DbType.Single: return "float?";
		case DbType.String: return "string";
		case DbType.StringFixedLength: return "string";
		case DbType.Time: return "TimeSpan?";
		case DbType.UInt16: return "ushort?";
		case DbType.UInt32: return "uint?";
		case DbType.UInt64: return "ulong?";
		case DbType.VarNumeric: return "decimal?";
		default:
		{
			return "__UNKNOWN__" + column.NativeType;
		}
	}
}
#endregion


public void PrintHeader()
{
	Response.WriteLine("//============================================================");
	Response.WriteLine("// Producnt name:		RSoft Dal");
	Response.WriteLine("// Version: 			1.0");
	Response.WriteLine("// Coded by:			RodneyZhou");
	Response.WriteLine("// Auto generated at: 	{0}", DateTime.Now);
	Response.WriteLine("//============================================================");
	Response.WriteLine("// 1. Please take care the nocount property in storedprocedure! If the property is set to yes,");
	Response.WriteLine("//    the return affected row number should alway be -1.");
	Response.WriteLine("// 2. In the case of primary keys, you need to modify the break build yourself.");
	Response.WriteLine("//============================================================");
	
	Response.WriteLine();
}

public void PrintCatchFinallyByBLL()
{
	Response.WriteLine("	        catch (Exception e)");
	Response.WriteLine("            {");
	Response.WriteLine("                throw e;");
	Response.WriteLine("            }");
	Response.WriteLine("            finally");
	Response.WriteLine("            {");
	Response.WriteLine("                if (con != null)");
	Response.WriteLine("                {");
	Response.WriteLine("                    con.Close();");
	Response.WriteLine("                }");
	Response.WriteLine("            }");
	Response.WriteLine();
}