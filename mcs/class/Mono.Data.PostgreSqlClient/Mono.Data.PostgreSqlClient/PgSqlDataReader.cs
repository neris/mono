//
// System.Data.SqlClient.SqlDataReader.cs
//
// Author:
//   Rodrigo Moya (rodrigo@ximian.com)
//   Daniel Morgan (danmorg@sc.rr.com)
//
// (C) Ximian, Inc 2002
// (C) Daniel Morgan 2002
//
// Credits:
//    SQL and concepts were used from libgda 0.8.190 (GNOME Data Access)
//    http://www.gnome-db.org/
//    with permission from the authors of the
//    PostgreSQL provider in libgda:
//        Michael Lausch <michael@lausch.at>
//        Rodrigo Moya <rodrigo@gnome-db.org>
//        Vivien Malerba <malerba@gnome-db.org>
//        Gonzalo Paniagua Javier <gonzalo@gnome-db.org>
//

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace System.Data.SqlClient {
	/// <summary>
	/// Provides a means of reading one or more forward-only streams
	/// of result sets obtained by executing a command 
	/// at a SQL database.
	/// </summary>
	//public sealed class SqlDataReader : MarshalByRefObject,
	//	IEnumerable, IDataReader, IDisposable, IDataRecord
	public sealed class SqlDataReader : IEnumerable, 
		IDataReader, IDataRecord {
		#region Fields

		private SqlCommand cmd;
		private DataTable table;

		private object[] fields;
		private string[] types; // PostgreSQL Type
		private bool[] isNull;
				
		private bool open = false;
		IntPtr pgResult; // PGresult
		private int rows;
		private int cols;

		private int currentRow = -1; // no Read() has been done yet

		#endregion // Fields

		#region Constructors

		internal SqlDataReader (SqlCommand sqlCmd, 
			DataTable dataTableSchema, IntPtr pg_result,
			int rowCount, int fieldCount, string[] pgtypes) {

			cmd = sqlCmd;
			table = dataTableSchema;
			pgResult = pg_result;
			rows = rowCount;
			cols = fieldCount;
			types = pgtypes;
			open = true;
		}

		#endregion

		#region Public Methods

		[MonoTODO]
		public void Close() {
			// close result set
			PostgresLibrary.PQclear (pgResult);
			open = false;
			// TODO: change busy state on SqlConnection to not busy
		}

		[MonoTODO]
		public DataTable GetSchemaTable() {
			return table;
		}

		[MonoTODO]
		public bool NextResult() {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public bool Read() {
			string value;
			fields = new object[cols]; // re-init row
			DbType dbType;

			if(currentRow < rows - 1)  {
				currentRow++;
				int c;
				for(c = 0; c < cols; c++) {

					// get data value
					value = PostgresLibrary.
						PQgetvalue(
						pgResult,
						currentRow, c);

					int columnIsNull;
					// is column NULL?
					columnIsNull = PostgresLibrary.
						PQgetisnull(pgResult,
						currentRow, c);

					int actualLength;
					// get Actual Length
					actualLength = PostgresLibrary.
						PQgetlength(pgResult,
						currentRow, c);
						
					dbType = PostgresHelper.
						TypnameToSqlDbType(types[c]);

					fields[c] = PostgresHelper.
						ConvertDbTypeToSystem (
							dbType,
							value);
				}
				return true;
			}
			return false; // EOF
		}

		[MonoTODO]
		public byte GetByte(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public long GetBytes(int i, long fieldOffset, 
			byte[] buffer, int bufferOffset, 
			int length) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public char GetChar(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public long GetChars(int i, long fieldOffset, 
			char[] buffer, int bufferOffset, 
			int length) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public IDataReader GetData(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public string GetDataTypeName(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public DateTime GetDateTime(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public decimal GetDecimal(int i) {
			return (decimal) fields[i];
		}

		[MonoTODO]
		public double GetDouble(int i) {
			return (double) fields[i];
		}

		[MonoTODO]
		public Type GetFieldType(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public float GetFloat(int i) {
			return (float) fields[i];
		}

		[MonoTODO]
		public Guid GetGuid(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public short GetInt16(int i) {
			return (short) fields[i];
		}

		[MonoTODO]
		public int GetInt32(int i) {
			return (int) fields[i];
		}

		[MonoTODO]
		public long GetInt64(int i) {
			return (long) fields[i];
		}

		[MonoTODO]
		public string GetName(int i) {
			return table.Columns[i].ColumnName;
		}

		[MonoTODO]
		public int GetOrdinal(string name) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public string GetString(int i) {
			return (string) fields[i];
		}

		[MonoTODO]
		public object GetValue(int i) {
			return fields[i];
		}

		[MonoTODO]
		public int GetValues(object[] values) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public bool IsDBNull(int i) {
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public bool GetBoolean(int i) {
			return (bool) fields[i];
		}

		[MonoTODO]
		public IEnumerator GetEnumerator() {
			throw new NotImplementedException ();
		}

		#endregion // Public Methods

		#region Destructors

		[MonoTODO]
		public void Dispose () {
		}

		[MonoTODO]
		~SqlDataReader() {
		}

		#endregion // Destructors

		#region Properties

		public int Depth {
			[MonoTODO]
			get { 
				throw new NotImplementedException (); 
			}
		}

		public bool IsClosed {
			[MonoTODO]
			get {
				if(open == false)
					return true;
				else
					return false;
			}
		}

		public int RecordsAffected {
			[MonoTODO]
			get { 
				throw new NotImplementedException (); 
			}
		}
	
		public int FieldCount {
			[MonoTODO]
			get { 
				return cols;
			}
		}

		public object this[string name] {
			[MonoTODO]
			get { 
				int i;
				for(i = 0; i < cols; i ++) {
					if(table.Columns[i].ColumnName.Equals(name)) {
						return fields[i];
					}

				}
	
				for(i = 0; i < cols; i++) {
					string ta;
					string n;
						
					ta = table.Columns[i].ColumnName.ToUpper();
					n = name.ToUpper();
						
					if(ta.Equals(n)) {
						return fields[i];
					}
				}
			
				throw new MissingFieldException("Missing field: " + name);
			}
		}

		public object this[int i] {
			[MonoTODO]
			get { 
				return fields[i];
			}
		}

		#endregion // Properties
	}
}
