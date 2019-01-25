Public Class addcustomerinfo
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim fname As String = TextBox1.Text
        Dim mname As String = TextBox2.Text
        Dim lname As String = TextBox3.Text
        Dim street_purok As String = TextBox4.Text
        Dim brgy As String = TextBox5.Text
        Dim city As String = TextBox6.Text
        Dim province As String = TextBox7.Text
        Dim contactnumber As String = TextBox8.Text

        Try

            checkstate()
            dbconn.Open()
            With cmd
                .Connection = dbconn
                .CommandText = "INSERT INTO customers (fname,mname,lname,street_purok,barangay,city,province,contact_number) VALUES('" & fname & "','" & mname & "','" & lname & "','" & street_purok & "','" & brgy & "','" & city & "','" & province & "','" & contactnumber & "')"
                .ExecuteNonQuery()
                MessageBox.Show("Customer information has been added!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                TextBox1.Clear()
                TextBox2.Clear()
                TextBox3.Clear()
                TextBox4.Clear()
                TextBox5.Clear()
                TextBox6.Clear()
                TextBox7.Clear()
                TextBox8.Clear()

            End With
        Catch ex As Exception
            MessageBox.Show("Customer Information Not Added! " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dbconn.Close()
        dbconn.Dispose()

        loadcustomer()
    End Sub

    Private Sub addcustomerinfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class