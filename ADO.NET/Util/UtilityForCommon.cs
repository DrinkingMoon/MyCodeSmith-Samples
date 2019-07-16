/****************************************************************************
* This utilty class is for aspx.
****************************************************************************/
//For aspx, aspx.cs
public void PrintHeader()
{
	Response.WriteLine("//============================================================");
	Response.WriteLine("// Producnt name:		RSoft ");
	Response.WriteLine("// Version: 			1.0");
	Response.WriteLine("// Coded by:			RodneyZhou");
	Response.WriteLine("// Auto generated at: 	{0}", DateTime.Now);
	Response.WriteLine("//============================================================");
	Response.WriteLine("// 1. Generated code. If the data table is multi-PK, please fix the error yourself.");
	Response.WriteLine("//============================================================");
	
	Response.WriteLine();
}

#region JS Utility

public void PrintHeaderForJS()
{
	Response.WriteLine("//============================================================");
	Response.WriteLine("// Producnt name:		RSoft ");
	Response.WriteLine("// Version: 			1.0");
	Response.WriteLine("// Coded by:			RodneyZhou");
	Response.WriteLine("// Auto generated at: 	{0}", DateTime.Now);
	Response.WriteLine("//============================================================");
	Response.WriteLine("// 1. Generated code. If the data table is multi-PK, please fix the error yourself.");
	Response.WriteLine("//============================================================");
	
	Response.WriteLine();
}





#endregion
///////////////////////////////////////////////////////////////
// CLASS NAME by Shen Bo
///////////////////////////////////////////////////////////////

public string GetModelMemberVarName()
{
	return GetModelParamName();
}
public string GetModelParamName()
{
	return MakeCamel(GetModelClassName());
}
public string GetModelClassName()
{
	return 	GetModelClassName(TargetTable);
}
public string GetModelClassName(TableSchema table)
{
	string result;
	if ( table.ExtendedProperties.Contains("ModelName") )
	{
		result = (string)table.ExtendedProperties["ModelName"].Value;	
		return MakePascal(result);
	}

	if (table.Name.EndsWith("s"))
	{
		//result = table.Name.Substring(0, table.Name.Length - 1);
		result = MakeSingle(table.Name);
	}
	else
	{
		result = table.Name;
	}

	return MakePascal(result);
}

///////////////////////////////////////////////////////////////
// PRIMARY KEY TYPE by Shen Bo
///////////////////////////////////////////////////////////////
public string GetPKPropertyType()
{
	return 	GetPKType(TargetTable);
}
public string GetPKType()
{
	return 	GetPKType(TargetTable);
}
public string GetPKType(TableSchema TargetTable)
{
	if (TargetTable.PrimaryKey != null)
	{
		if (TargetTable.PrimaryKey.MemberColumns.Count == 1)
		{
			return GetCSharpTypeFromDBFieldType(TargetTable.PrimaryKey.MemberColumns[0]);
		}
		else
		{
			return GetCSharpTypeFromDBFieldType(TargetTable.PrimaryKey.MemberColumns[0]);
		}
	}
	else
	{
		throw new ApplicationException("This template will only work on MyTables with a primary key.");
	}
}

///////////////////////////////////////////////////////////////
// PRIMARY KEY NAME by Shen Bo
///////////////////////////////////////////////////////////////
public string GetPKPropertyName()
{
	return MakePascal(GetPKName());
}
public string GetPKMemberVarName()
{
	return MakeCamel(GetPKName());	
}
public string GetPKParamName()
{
	return GetPKMemberVarName();	
}
public string GetPKName()
{
	return GetPKName(TargetTable);
}
public string GetPKName(TableSchema TargetTable)
{
	if (TargetTable.PrimaryKey != null)
	{
		if (TargetTable.PrimaryKey.MemberColumns.Count == 1)
		{
			return TargetTable.PrimaryKey.MemberColumns[0].Name;
		}
		else
		{
			return TargetTable.PrimaryKey.MemberColumns[0].Name;
		}
	}
	else
	{
		throw new ApplicationException("This template will only work on tables with a primary key.");
	}
}




///////////////////////////////////////////////////////////////
// FOREIGH KEY PROPERTY TYPE by Shen Bo
///////////////////////////////////////////////////////////////
public string GetFKPropertyType(TableKeySchema key)
{
	return MakePascal(GetFKPrimaryModelClassName(key));
}

///////////////////////////////////////////////////////////////
// FOREIGH KEY ID NAMEs by Shen Bo
///////////////////////////////////////////////////////////////
public string GetFKForeignIdName(TableKeySchema key)
{
	return key.ForeignKeyMemberColumns[0].Name;
}
public string GetFKPrimaryIdName(TableKeySchema key)
{
	return key.PrimaryKeyMemberColumns[0].Name;
}

///////////////////////////////////////////////////////////////
// FOREIGH KEY PROPERTY NAME by Shen Bo
///////////////////////////////////////////////////////////////
public string GetFKMemberVarName(TableKeySchema key)
{
//	return MakeCamel(GetFKName(key));
	string result = GetFKForeignIdName(key);
	if( result.ToLower().EndsWith("id") )
	{
		result = result.Substring(0, result.Length - 2);	
	}
	return MakeCamel(result);
}
public string GetFKPropertyName(TableKeySchema key)
{
	return MakePascal(GetFKMemberVarName(key));
}
public string GetFKPrimaryModelClassName(TableKeySchema key)
{
	return GetModelClassName(key.PrimaryKeyTable);
}

//So dirty function! -- reviewed by shenbo
public string MakeCamel(string value)
{
	return value.Substring(0, 1).ToLower() + value.Substring(1);
}

//I will be dirty too! -- coded by shenbo
public string MakePascal(string value)
{
	return value.Substring(0, 1).ToUpper() + value.Substring(1);
}

public string MakePlural(string name)
{
	Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
	Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
	Regex plural3 = new Regex("(?<keep>[sxzh])$");
	Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

	if(plural1.IsMatch(name))
		return plural1.Replace(name, "${keep}ies");
	else if(plural2.IsMatch(name))
		return plural2.Replace(name, "${keep}s");
	else if(plural3.IsMatch(name))
		return plural3.Replace(name, "${keep}es");
	else if(plural4.IsMatch(name))
		return plural4.Replace(name, "${keep}s");

	return name;
}

public string MakeSingle(string name)
{
	Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
	Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
	Regex plural3 = new Regex("(?<keep>[sxzh])es$");
	Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

	if(plural1.IsMatch(name))
		return plural1.Replace(name, "${keep}y");
	else if(plural2.IsMatch(name))
		return plural2.Replace(name, "${keep}");
	else if(plural3.IsMatch(name))
		return plural3.Replace(name, "${keep}");
	else if(plural4.IsMatch(name))
		return plural4.Replace(name, "${keep}");

	return name;
}

///////////////////////////////////////////////////////////////
// PROPERTY TYPE by Shen Bo
///////////////////////////////////////////////////////////////
public string GetPropertyType(ColumnSchema column)
{
	return GetCSharpTypeFromDBFieldType(column);
}
public string GetMemberVarType(ColumnSchema column)
{
	return GetCSharpTypeFromDBFieldType(column);
}
public string GetParamType(ColumnSchema column)
{
	return GetCSharpTypeFromDBFieldType(column);
}

#region PK
//For pk parameters
public string GenerateParametersForPK()
{
	if(TargetTable.PrimaryKey.MemberColumns.Count == 1)
	{
		return string.Format(" {0} {1}", GetPKPropertyType(), MakeCamel(GetPKName())); 
	}
	else
	{
		string str = "";
		for(int i = 0; i < TargetTable.PrimaryKey.MemberColumns.Count; i ++)
		{
			if(i != TargetTable.PrimaryKey.MemberColumns.Count -1)
			{
				str = str + string.Format(" {0} {1},", GetPKPropertyType(),  MakeCamel(TargetTable.PrimaryKey.MemberColumns[i].Name) ); 
			}
			else
			{
				str = str + string.Format(" {0} {1}", GetPKPropertyType(),  MakeCamel(TargetTable.PrimaryKey.MemberColumns[i].Name)); 
			}
		}
		
		return str;
	}
}

//For pk value
public string GenerateValuesForPK()
{
	if(TargetTable.PrimaryKeys.Count == 1)
	{
		return string.Format("{0}", MakeCamel(GetPKName())).Trim(); 
	}
	else
	{
		string str = "";
		for(int i = 0; i < TargetTable.PrimaryKey.MemberColumns.Count; i ++)
		{
			if(i != TargetTable.PrimaryKey.MemberColumns.Count -1)
			{
				str = str + string.Format("{0} ,", MakeCamel(TargetTable.PrimaryKey.MemberColumns[i].Name) ); 
			}
			else
			{
				str = str + string.Format("{0}", MakeCamel(TargetTable.PrimaryKey.MemberColumns[i].Name)); 
			}
		}
		
		return str;
	}
}
#endregion


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

public string GetCSharpTypeFromDBFieldTypeWithoutNonable(ColumnSchema column)
{
	if (column.Name.EndsWith("TypeCode")) return column.Name;
	
	switch (column.DataType)
	{
		case DbType.AnsiString: return "string";
		case DbType.AnsiStringFixedLength: return "string";
		case DbType.Binary: return "byte[]";
		case DbType.Boolean: return "bool";
		case DbType.Byte: return "byte";
		case DbType.Currency: return "decimal";
		case DbType.Date: return "DateTime";
		case DbType.DateTime: return "DateTime";
		case DbType.Decimal: return "decimal";
		case DbType.Double: return "double";
		case DbType.Guid: return "Guid";
		case DbType.Int16: return "short";
		case DbType.Int32: return "int";
		case DbType.Int64: return "long";
		case DbType.Object: return "object";
		case DbType.SByte: return "sbyte";
		case DbType.Single: return "float";
		case DbType.String: return "string";
		case DbType.StringFixedLength: return "string";
		case DbType.Time: return "TimeSpan";
		case DbType.UInt16: return "ushort";
		case DbType.UInt32: return "uint";
		case DbType.UInt64: return "ulong";
		case DbType.VarNumeric: return "decimal";
		default:
		{
			return "__UNKNOWN__" + column.NativeType;
		}
	}
}

#region overrides
[Category("Options")]
[FileDialog(FileDialogType.Save, Title="Select Output File", Filter="CSharp Files (*.cs)|*.cs|All Files (*.*)|*.*", DefaultExtension=".cs")]
public override string OutputFile
{
	get {return base.OutputFile;}
	set {base.OutputFile = value;}
}
#endregion