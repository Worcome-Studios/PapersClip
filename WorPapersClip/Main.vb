Public Class Main
#Region "Var"
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Worcome_Studios\Commons\Apps\PapersClip"
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short
    Dim TeclasPulsadas As String = Nothing
    Dim parametros As String
    Dim MemoryList_Papers As New ArrayList
    Dim MemoryList_PapersCounter As Integer = -1
    Dim ListCounter As Integer = 1
#End Region

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.OfflineMode = False Then
            Try
                AppService.StartAppService(False, False, True, False, True)
            Catch ex As Exception
                MsgBox("ERROR CRITICO CON 'AppService'", MsgBoxStyle.Critical, "Worcome Security")
            End Try
        End If
        parametros = Microsoft.VisualBasic.Command
        If parametros = Nothing Then
            LoadIndexClipsFromFile()
        ElseIf parametros = "/FactoryReset" Then
            FactoryReset()
        End If
        If My.Computer.FileSystem.DirectoryExists(DIRCommons) = False Then
            My.Computer.FileSystem.CreateDirectory(DIRCommons)
        End If
        If My.Computer.FileSystem.DirectoryExists(DIRCommons & "\ClipsFiles") = False Then
            My.Computer.FileSystem.CreateDirectory(DIRCommons & "\ClipsFiles")
        End If
        If My.Computer.FileSystem.FileExists(DIRCommons & "\ClipsList.lst") = False Then
            My.Computer.FileSystem.WriteAllText(DIRCommons & "\ClipsList.lst", Nothing, False)
        End If
        If My.Computer.FileSystem.DirectoryExists(DIRCommons & "\ClipStory") = False Then
            My.Computer.FileSystem.CreateDirectory(DIRCommons & "\ClipStory")
        End If
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveIndexClipsToFile()
    End Sub

    Private Sub Main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Select Case Me.WindowState
            Case FormWindowState.Maximized
                Me.CenterToScreen()
                NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "???, No me Puedo Maximizar!", ToolTipIcon.Info)
                Me.WindowState = FormWindowState.Normal
            Case FormWindowState.Minimized
                Me.CenterToScreen()
                NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "Sigo ejecutandome, clickea el logo para mostrarme", ToolTipIcon.Info)
                Me.Hide()
            Case FormWindowState.Normal
                Me.CenterToScreen()
                Me.Show()
                Me.Focus()
        End Select
    End Sub

#Region "Controls"
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.Focus()
    End Sub
    Private Sub lblAbout_Click(sender As Object, e As EventArgs) Handles Label2.Click
        About.Show()
    End Sub
    Private Sub listPapersClips_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        MemoryList_PapersCounter = ListBox1.SelectedIndex
        ShowPaper(False)
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            My.Computer.FileSystem.WriteAllText(MemoryList_Papers(MemoryList_PapersCounter), FastColoredTextBox1.Text, False)
        Catch ex As Exception
            Console.WriteLine("[Main@btnSave_Click]Error: " & ex.Message)
        End Try
    End Sub
    Private Sub btnOpenFromFile_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim OpenDialog As New OpenFileDialog
            OpenDialog.Title = "Abrir archivo..."
            OpenDialog.CheckFileExists = True
            OpenDialog.FileName = Nothing
            OpenDialog.InitialDirectory = "C:\"
            OpenDialog.Multiselect = False
            OpenDialog.Filter = "All file types|*.*"
            If OpenDialog.ShowDialog = DialogResult.OK Then
                FastColoredTextBox1.Text = My.Computer.FileSystem.ReadAllText(OpenDialog.FileName)
            End If
        Catch ex As Exception
            Console.WriteLine("[Main@btnOpenFromFile_Click]Error: " & ex.Message)
        End Try
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles Button3.Click
        FastColoredTextBox1.Clear()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles Button4.Click
        FastColoredTextBox1.Clear()
        GroupBox1.Visible = False
        MemoryList_PapersCounter = -1
    End Sub
    Private Sub btnNewClip_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim Dialog_TextBox = InputBox("Ingrese un nombre para el Clip", "Worcome Security")
        If Dialog_TextBox = Nothing Then
        Else
            My.Computer.FileSystem.WriteAllText(DIRCommons & "\ClipsFiles\" & Dialog_TextBox & ".clip", Nothing, False)
            MemoryList_Papers.Add(DIRCommons & "\ClipsFiles\" & Dialog_TextBox & ".clip")
            ListBox1.Items.Add(ListCounter & ") " & Dialog_TextBox)
            MemoryList_PapersCounter = ListBox1.SelectedIndex
            ShowPaper(False)
            ListCounter = ListCounter + 1
        End If
    End Sub
    Private Sub btnRemoveClip_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If MessageBox.Show("¿Seguro que quiere eliminar el Clip '" & ListBox1.SelectedItem & "'?", "Worcome Security", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
            Try
                If ListBox1.SelectedItem = Nothing Then
                Else
                    MemoryList_PapersCounter = ListBox1.SelectedIndex
                    My.Computer.FileSystem.DeleteFile(MemoryList_Papers(MemoryList_PapersCounter))
                    MemoryList_Papers.RemoveAt(ListBox1.SelectedIndex)
                    ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
                    FastColoredTextBox1.Clear()
                    GroupBox1.Visible = False
                    MemoryList_PapersCounter = -1
                    ListCounter = ListCounter - 1
                End If
            Catch ex As Exception
                Console.WriteLine("[Main@btnRemoveClip_Click]Error: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub TriggerTimerVKeys_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            'Default(CTRL + Shift + C + {1, 2, 3, 4, 5})
            If (GetAsyncKeyState(My.Settings.HK_1)) And (GetAsyncKeyState(My.Settings.HK_2)) And (GetAsyncKeyState(My.Settings.HK_3)) And (GetAsyncKeyState(49)) Then 'Clip 1
                Console.WriteLine("Combinacion registrada CLIP1")
                NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "Se ha copiado al Portapapeles el texto que contiene el Clip '" & ListBox1.SelectedItem & "'", ToolTipIcon.Info)
                MemoryList_PapersCounter = 0
                ListBox1.SelectedIndex = 0
                ShowPaper(True)
            ElseIf (GetAsyncKeyState(My.Settings.HK_1)) And (GetAsyncKeyState(My.Settings.HK_2)) And (GetAsyncKeyState(My.Settings.HK_3)) And (GetAsyncKeyState(50)) Then 'Clip 2
                Console.WriteLine("Combinacion registrada CLIP2")
                NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "Se ha copiado al Portapapeles el texto que contiene el Clip '" & ListBox1.SelectedItem & "'", ToolTipIcon.Info)
                MemoryList_PapersCounter = 1
                ListBox1.SelectedIndex = 1
                ShowPaper(True)
            ElseIf (GetAsyncKeyState(My.Settings.HK_1)) And (GetAsyncKeyState(My.Settings.HK_2)) And (GetAsyncKeyState(My.Settings.HK_3)) And (GetAsyncKeyState(51)) Then 'Clip 3
                Console.WriteLine("Combinacion registrada CLIP3")
                NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "Se ha copiado al Portapapeles el texto que contiene el Clip '" & ListBox1.SelectedItem & "'", ToolTipIcon.Info)
                MemoryList_PapersCounter = 2
                ListBox1.SelectedIndex = 2
                ShowPaper(True)
            ElseIf (GetAsyncKeyState(My.Settings.HK_1)) And (GetAsyncKeyState(My.Settings.HK_2)) And (GetAsyncKeyState(My.Settings.HK_3)) And (GetAsyncKeyState(52)) Then 'Clip 4
                Console.WriteLine("Combinacion registrada CLIP4")
                NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "Se ha copiado al Portapapeles el texto que contiene el Clip '" & ListBox1.SelectedItem & "'", ToolTipIcon.Info)
                MemoryList_PapersCounter = 3
                ListBox1.SelectedIndex = 3
                ShowPaper(True)
            ElseIf (GetAsyncKeyState(My.Settings.HK_1)) And (GetAsyncKeyState(My.Settings.HK_2)) And (GetAsyncKeyState(My.Settings.HK_3)) And (GetAsyncKeyState(53)) Then 'Clip 5
                Console.WriteLine("Combinacion registrada CLIP5")
                NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "Se ha copiado al Portapapeles el texto que contiene el Clip '" & ListBox1.SelectedItem & "'", ToolTipIcon.Info)
                MemoryList_PapersCounter = 4
                ListBox1.SelectedIndex = 4
                ShowPaper(True)
            ElseIf (GetAsyncKeyState(44)) Then 'ScreenShot
                If My.Settings.Screenshots_IsEnabled = True Then
                    Console.WriteLine("Combinacion registrada Screenshot")
                    NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "Se a hecho un pantallazo", ToolTipIcon.Info)
                    ScreenShots.SaveTheImage()
                End If
            ElseIf (GetAsyncKeyState(17)) And (GetAsyncKeyState(67)) Then 'ClipStory
                If My.Settings.ClipStory_IsEnabled = True Then
                    Console.WriteLine("Combinacion registrada Copiar")
                    ClipStory.AddNewClip(My.Computer.Clipboard.GetText)
                End If
            End If
            'EL TIMER ESTA TAN RAPIDO QUE DETECTA MUCHAS PULSACIONES
        Catch ex As Exception
            Console.WriteLine("[Main@TriggerTimerVKeys_Tick]Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnHotKeys_Click(sender As Object, e As EventArgs) Handles Button7.Click
        HotKey.ShowDialog()
    End Sub

    Private Sub ListBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseDoubleClick
        ShowPaper(True)
    End Sub
#End Region

#Region "Subs"
    Sub ShowPaper(ByVal Flag As Boolean)
        Try
            FastColoredTextBox1.Text = My.Computer.FileSystem.ReadAllText(MemoryList_Papers(MemoryList_PapersCounter))
            GroupBox1.Text = "Clip: " & ListBox1.SelectedItem
            GroupBox1.Visible = True
            Button6.Enabled = True
            If Flag = True Then
                My.Computer.Clipboard.SetText(FastColoredTextBox1.Text)
            End If
        Catch ex As Exception
            Console.WriteLine("[Main@ShowPaper]Error: " & ex.Message)
        End Try
    End Sub
    Sub LoadIndexClipsFromFile()
        Try
            MemoryList_Papers.Clear()
            ListBox1.Items.Clear()
            For Each Item As String In IO.File.ReadLines(DIRCommons & "\ClipsList.lst")
                MemoryList_Papers.Add(Item)
                Item = Item.Replace(".clip", "")
                Item = Item.Remove(0, Item.LastIndexOf("\") + 1)
                ListBox1.Items.Add(ListCounter & ") " & Item)
                ListCounter = ListCounter + 1
            Next
        Catch ex As Exception
            Console.WriteLine("[Main@LoadIndexClipsFromFile]Error: " & ex.Message)
        End Try
    End Sub
    Sub SaveIndexClipsToFile()
        Try
            ListCounter = 1
            If My.Computer.FileSystem.FileExists(DIRCommons & "\ClipsList.lst") = True Then
                My.Computer.FileSystem.DeleteFile(DIRCommons & "\ClipsList.lst")
            End If
            Dim tempString As String = Nothing
            For Each Item As Object In MemoryList_Papers
                tempString = tempString & Item & vbCrLf
            Next
            My.Computer.FileSystem.WriteAllText(DIRCommons & "\ClipsList.lst", tempString, False)
            LoadIndexClipsFromFile()
        Catch ex As Exception
            Console.WriteLine("[Main@SaveIndexClipsToFile]Error: " & ex.Message)
        End Try
    End Sub
    Sub FactoryReset()
        NotifyIcon1.ShowBalloonTip(1, "Wor: PapersClip", "PapersClip fue iniciado bajo el parametro: /FactoryReset", ToolTipIcon.Warning)
        If MessageBox.Show("¿Realmente quieres hacer un FactoryReset a la Aplicacion?" & vbCrLf & "Do you really want to make a FactoryReset to the Application?" & vbCrLf & "Todo sera eliminado" & vbCrLf & "Everything will be eliminated", "Worcome Security", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
            If My.Computer.FileSystem.DirectoryExists(DIRCommons) = True Then
                My.Computer.FileSystem.DeleteDirectory(DIRCommons, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
            MsgBox("Aplicacion Vuelta a la Vercion de Fabrica" & vbCrLf & "Application Return to the Factory Version" & vbCrLf & "Vuelva a Iniciar la Aplicacion" & vbCrLf & "Restart the Application", MsgBoxStyle.Information, "Worcome Security")
            End
        Else
            LoadIndexClipsFromFile()
        End If
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        ScreenShots.Show()
        ScreenShots.Focus()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ClipStory.Show()
        ClipStory.Focus()
    End Sub
#End Region
End Class