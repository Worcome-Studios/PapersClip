Public Class HotKey

    Private Sub HotKey_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.HK_Diferencial_ONWORK.Contains("1_3") Then
            RadioButton1.Checked = True
            RadioButton2.Checked = False
            CheckBox3.Checked = True
            ComboBox1.Text = My.Settings.HK_Diferencial_ONWORK.Replace("1_3_", "")
        ElseIf My.Settings.HK_Diferencial_ONWORK.Contains("2_3") Then
            RadioButton1.Checked = False
            RadioButton2.Checked = True
            CheckBox3.Checked = True
            ComboBox1.Text = My.Settings.HK_Diferencial_ONWORK.Replace("2_3_", "")
        End If
    End Sub

    Sub SaveHotKeys()
        'Key codes https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8
        Try
            If ComboBox1.Text = "A" Then
                My.Settings.HK_3 = 65
            ElseIf ComboBox1.Text = "B" Then
                My.Settings.HK_3 = 66
            ElseIf ComboBox1.Text = "C" Then
                My.Settings.HK_3 = 67
            ElseIf ComboBox1.Text = "D" Then
                My.Settings.HK_3 = 68
            ElseIf ComboBox1.Text = "E" Then
                My.Settings.HK_3 = 69
            ElseIf ComboBox1.Text = "F" Then
                My.Settings.HK_3 = 70
            ElseIf ComboBox1.Text = "G" Then
                My.Settings.HK_3 = 71
            ElseIf ComboBox1.Text = "H" Then
                My.Settings.HK_3 = 72
            ElseIf ComboBox1.Text = "I" Then
                My.Settings.HK_3 = 73
            ElseIf ComboBox1.Text = "J" Then
                My.Settings.HK_3 = 74
            ElseIf ComboBox1.Text = "K" Then
                My.Settings.HK_3 = 75
            ElseIf ComboBox1.Text = "L" Then
                My.Settings.HK_3 = 76
            ElseIf ComboBox1.Text = "M" Then
                My.Settings.HK_3 = 77
            ElseIf ComboBox1.Text = "N" Then
                My.Settings.HK_3 = 78
            ElseIf ComboBox1.Text = "O" Then
                My.Settings.HK_3 = 79
            ElseIf ComboBox1.Text = "P" Then
                My.Settings.HK_3 = 80
            ElseIf ComboBox1.Text = "Q" Then
                My.Settings.HK_3 = 81
            ElseIf ComboBox1.Text = "R" Then
                My.Settings.HK_3 = 82
            ElseIf ComboBox1.Text = "S" Then
                My.Settings.HK_3 = 83
            ElseIf ComboBox1.Text = "T" Then
                My.Settings.HK_3 = 84
            ElseIf ComboBox1.Text = "U" Then
                My.Settings.HK_3 = 85
            ElseIf ComboBox1.Text = "V" Then
                My.Settings.HK_3 = 86
            ElseIf ComboBox1.Text = "W" Then
                My.Settings.HK_3 = 87
            ElseIf ComboBox1.Text = "X" Then
                My.Settings.HK_3 = 88
            ElseIf ComboBox1.Text = "Y" Then
                My.Settings.HK_3 = 89
            ElseIf ComboBox1.Text = "Z" Then
                My.Settings.HK_3 = 90
            Else
                MsgBox("Debe seleccionar una Letra", MsgBoxStyle.Critical, "Worcome Security")
                ComboBox1.Text = "C"
                Exit Sub
            End If
            If RadioButton1.Checked = True And RadioButton2.Checked = False And CheckBox3.Checked = True Then 'CTRL + Shift
                My.Settings.HK_1 = 17
                My.Settings.HK_2 = 16
                My.Settings.HK_Diferencial_ONWORK = "1_3_" & ComboBox1.Text
            ElseIf RadioButton1.Checked = False And RadioButton2.Checked = True And CheckBox3.Checked = True Then 'ALT + Shift
                My.Settings.HK_1 = 18 '164 LFT_ALT | 18 ALT
                My.Settings.HK_2 = 16
                My.Settings.HK_Diferencial_ONWORK = "2_3_" & ComboBox1.Text
            End If
            My.Settings.Save()
            My.Settings.Reload()
            Me.Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnSaveCounterHotkeys_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If RadioButton1.Checked = False And RadioButton2.Checked = False And CheckBox3.Checked = False Then
            MsgBox("Debe marcar por lo menos 1 tecla diferencial", MsgBoxStyle.Critical, "Worcome Security")
        ElseIf RadioButton1.Checked = False And RadioButton2.Checked = True And CheckBox3.Checked = False Then
            MsgBox("Debe haber mas de 1 tecla diferencial", MsgBoxStyle.Critical, "Worcome Security")
        ElseIf RadioButton1.Checked = True And RadioButton2.Checked = False And CheckBox3.Checked = False Then
            MsgBox("Debe haber mas de 1 tecla diferencial", MsgBoxStyle.Critical, "Worcome Security")
        Else
            If ComboBox1.Text = Nothing Or ComboBox1.Text.Contains(" ") Then
                MsgBox("Debe seleccionar una tecla del abecedario", MsgBoxStyle.Critical, "Worcome Security")
            Else
                SaveHotKeys()
            End If
        End If
    End Sub
End Class