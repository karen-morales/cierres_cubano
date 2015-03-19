Imports MySql.Data.MySqlClient

Module mod_experts_db
    'Variables para la conexion local remota
    Public dbhostr As String = My.Settings.remote_db_host
    Public dbportr As String = My.Settings.remote_db_port
    Public dbnamer As String = My.Settings.remote_db_name
    Public dbuserr As String = My.Settings.remote_db_user
    Public dbpassr As String = My.Settings.remote_db_pass

    'Verifica si hay conexxion remota
    Public Function testConn() As Boolean
        Dim conn As New MySqlConnection

        Try
            'Abrimos la conexion a la base remota
            Dim sql As MySqlCommand = New MySqlCommand
            ' Creamos la conexion a la db
            conn.ConnectionString = "server=" & dbhostr & ";" _
                & "port=" & dbportr & ";" _
                & "uid=" & dbuserr & ";" _
                & "pwd=" & dbpassr & ";" _
                & "database=" & dbnamer & ";"
            conn.Open()
            conn.Ping()
            conn.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Ejecuta un qry local
    Public Function unused_executeQueryWDr(ByVal Query As String) As Double
        Dim conn As New MySqlConnection
        Dim sql As MySqlCommand = New MySqlCommand
        Dim res As Double

        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";"
        conn.Open()

        sql.Connection = conn
        sql.CommandText = Query
        sql.CommandType = CommandType.Text
        Try
            res = sql.ExecuteScalar()
        Catch ex As Exception
            res = 0
        End Try
        conn.Close()
        unused_executeQueryWDr = res
    End Function

    ' Abre una conexión
    Public Function mysql_conexion_up() As MySqlConnection
        Dim myconn As New MySqlConnection
        ' Creamos la conexion a la db
        myconn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        myconn.Open()
        Return myconn
    End Function

    ' ###########################################################################################################
    ' # Ejecuta una query regresando lista de diccionarios con los valores obtenidos, 
    ' # necesita como parámetro una conección abierta a la base de datos
    ' ###########################################################################################################
    Public Function mysql_execute(ByVal Query As String, ByRef myconn As MySqlConnection) As List(Of Dictionary(Of String, String))
        Dim sql As MySqlCommand = New MySqlCommand
        Dim rows As New List(Of Dictionary(Of String, String))
        Dim reader As MySqlDataReader = Nothing

        sql.Connection = myconn
        sql.CommandText = Query
        sql.CommandType = CommandType.Text
        Try
            reader = sql.ExecuteReader()
            While reader.Read()
                Dim i As Integer = 0
                'Dim row As New List(Of String)
                Dim dic As New Dictionary(Of String, String)
                While reader.FieldCount > i

                    If reader.IsDBNull(i) Then
                        dic.Add(reader.GetName(i), "")
                    Else
                        dic.Add(reader.GetName(i), reader.GetString(i))
                    End If
                    i += 1
                End While
                rows.Add(dic)
            End While
            reader.Close()
        Catch ex As Exception
            MsgBox("Ocurrió un error al ejecutar una consulta a la base de datos, inténtelo de nuevo y si el problema persiste consultelo con su adminsitrador.")
        End Try
        sql.Dispose()
        Return rows
    End Function

    ' ##################################################################################################################
    ' # Ejecuta una query con la funcion ExecuteReader, solo regresa el primer valor de la primer columna en modo texto
    ' ##################################################################################################################
    Public Function executeQueryTextr(ByVal Query As String) As String
        Dim conn As New MySqlConnection
        Dim sql As MySqlCommand = New MySqlCommand
        Dim res As String = ""

        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        conn.Open()

        sql.Connection = conn
        sql.CommandText = Query
        sql.CommandType = CommandType.Text
        Try
            Dim reader As MySqlDataReader = sql.ExecuteReader()
            While reader.Read()
                res = (reader.GetString(0))
            End While
        Catch ex As Exception
            res = ""
        End Try
        conn.Close()
        executeQueryTextr = res
    End Function

    ' #########################################################################################
    ' # Llena un combo box con el resultado de una query
    ' #########################################################################################
    Public Function fillComboByMySqlr(ByRef cmb As ComboBox, ByVal sql As String) As Boolean
        Dim conn As New MySqlConnection
        Dim ds As DataSet = New DataSet
        Dim da As New MySqlDataAdapter(sql, conn)
        Dim Tabla As New DataTable

        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        conn.Open()

        Dim cmd As New MySqlCommandBuilder(da)

        da.Fill(ds)
        cmb.DataSource = ds.Tables(0)
        cmb.ValueMember = ds.Tables(0).Columns(0).Caption.ToString
        cmb.DisplayMember = ds.Tables(0).Columns(1).Caption.ToString
        conn.Close()
        fillComboByMySqlr = True
    End Function

    ' #########################################################################################
    ' # Llena un GridControl de Devexpress con el resultado de una query
    ' #########################################################################################
    Public Function fillDataGridByMySqlrDX(ByRef dv As DevExpress.XtraGrid.GridControl, ByVal sql As String) As Boolean
        Dim conn As New MySqlConnection
        Dim ds As DataSet = New DataSet
        Dim da As New MySqlDataAdapter(sql, conn)
        Dim Tabla As New DataTable

        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        conn.Open()

        Dim cmd As New MySqlCommandBuilder(da)

        dv.DataSource = Nothing
        dv.RefreshDataSource()
        da.Fill(ds)
        dv.DataSource = ds.Tables(0)
        conn.Close()
        dv.RefreshDataSource()
        fillDataGridByMySqlrDX = True
    End Function

    ' #########################################################################################
    ' # Llena un GridView con el resultado de una query
    ' #########################################################################################
    Public Function fillDataGridByMySqlr(ByRef dv As DataGridView, ByVal sql As String) As Boolean
        Dim conn As New MySqlConnection
        Dim ds As DataSet = New DataSet
        Dim da As New MySqlDataAdapter(sql, conn)
        Dim Tabla As New DataTable

        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        conn.Open()

        Dim cmd As New MySqlCommandBuilder(da)

        da.Fill(ds)
        dv.DataSource = ds.Tables(0)
        conn.Close()
        fillDataGridByMySqlr = True
    End Function

    ' #######################################################################################################
    ' # Ejecuta una query con la funcion ExecuteScalar, solo regresa el primer valor de la primer columna
    ' #######################################################################################################
    Public Function executeQueryr(ByVal Query As String) As Double
        Dim conn As New MySqlConnection
        Dim sql As MySqlCommand = New MySqlCommand
        Dim res As Double

        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        conn.Open()

        sql.Connection = conn
        sql.CommandText = Query
        sql.CommandType = CommandType.Text
        Try
            res = sql.ExecuteScalar()
        Catch ex As Exception
            res = 0
        End Try
        conn.Close()
        executeQueryr = res
    End Function

    ' #########################################################################################
    ' # Llena un DataSer con el resultado de una query
    ' #########################################################################################
    Public Function fillDataSetByMySqlr(ByVal sql As String) As DataSet
        Dim conn As New MySqlConnection
        Dim ds As DataSet = New DataSet
        Dim da As New MySqlDataAdapter(sql, conn)
        Dim Tabla As New DataTable

        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        conn.Open()

        Dim cmd As New MySqlCommandBuilder(da)

        da.Fill(ds)
        conn.Close()
        fillDataSetByMySqlr = ds
    End Function
End Module
