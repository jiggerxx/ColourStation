Public Class deleteproduct



    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim selectedproductkey As String
        Dim selectedproductval As String

        Try
            selectedproductkey = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key
            selectedproductval = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Value
        Catch ex As Exception
            selectedproductkey = ""
            selectedproductval = ""
        End Try

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,units.*,type.* FROM products LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.unitID = units.unitID WHERE products.prodcode = '" & Label19.Text & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    Label9.Text = dr.Item("barcode")
                    Label10.Text = dr.Item("prodname")
                    Label11.Text = dr.Item("type")
                    Label12.Text = dr.Item("unit")
                    Label13.Text = dr.Item("proddesc")
                    Label14.Text = dr.Item("srp")
                    Label15.Text = dr.Item("stock")

                    If dr.Item("hchem") = "1" Then
                        Label16.Text = "Yes"
                    Else
                        Label16.Text = "No"
                    End If

                End While
            End With

        Catch ex As Exception

        End Try

        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub deleteproduct_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'loadproducts()
        'ComboBox1.SelectedIndex = Convert.ToInt32(getTemporary(editproduct.ComboBox4.SelectedIndex))
        'Label9.Text = "------------"
        'Label10.Text = "------------"
        'Label11.Text = "------------"
        'Label12.Text = "------------"
        'Label13.Text = "------------"
        'Label14.Text = "------------"
        'Label15.Text = "------------"
        'Label16.Text = "------------"

        Try
            checkstate()
            dbconn.Open()

            With cmd
                .Connection = dbconn
                .CommandText = "SELECT products.*,units.*,type.* FROM products LEFT JOIN units ON products.unitID = units.unitID LEFT JOIN type ON products.unitID = units.unitID WHERE products.prodcode = '" & Label19.Text & "'"
                dr = cmd.ExecuteReader

                While dr.Read
                    Label9.Text = dr.Item("barcode")
                    Label10.Text = dr.Item("prodname")
                    Label11.Text = dr.Item("type")
                    Label12.Text = dr.Item("unit")
                    Label13.Text = dr.Item("proddesc")
                    Label14.Text = dr.Item("srp")
                    Label15.Text = dr.Item("stock")

                    If dr.Item("hchem") = "1" Then
                        Label16.Text = "Yes"
                    Else
                        Label16.Text = "No"
                    End If

                End While
            End With

        Catch ex As Exception

        End Try

        dbconn.Close()
        dbconn.Dispose()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim delproducts As String = DirectCast(ComboBox1.SelectedItem, KeyValuePair(Of String, String)).Key

        Try

            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "DELETE FROM csdb.products WHERE prodcode='" & Label19.Text & "'"
                .ExecuteNonQuery()
                MessageBox.Show("Product " & Label10.Text & " is deleted!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                loadproducts()
                Label9.Text = "------------"
                Label10.Text = "------------"
                Label11.Text = "------------"
                Label12.Text = "------------"
                Label13.Text = "------------"
                Label14.Text = "------------"
                Label15.Text = "------------"
                Label16.Text = "------------"

            End With
        Catch ex As Exception
            MessageBox.Show("Product not found!" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        dbconn.Close()
        dbconn.Dispose()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
    End Sub

   
    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class