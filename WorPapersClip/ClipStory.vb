Imports System.IO
Public Class ClipStory
    Dim ClipList As New ArrayList
    Dim Counterrr As Integer = -1

    Private Sub ClipStory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.ClipStory_IsEnabled = True Then
            CheckBox1.CheckState = CheckState.Checked
        Else
            CheckBox1.CheckState = CheckState.Unchecked
        End If
        IndexFilesToList()
    End Sub

    Sub IndexFilesToList()
        ListBox1.Items.Clear()
        ClipList.Clear()
        Try
            For Each Item As String In My.Computer.FileSystem.GetFiles(Main.DIRCommons & "\ClipStory", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
                ClipList.Add(Item)
                ListBox1.Items.Add(Counterrr + 1)
            Next
        Catch ex As Exception
            Console.WriteLine("[IndexFilesToList@ClipStory]Error: " & ex.Message)
        End Try
    End Sub

    Sub AddNewClip(ByVal NewText As String)
        Try
            Dim RandomString As String
            RandomString = CreateIdentification(10)
            If My.Computer.FileSystem.FileExists(Main.DIRCommons & "\ClipStory\" & RandomString & ".txt") = True Then
                My.Computer.FileSystem.DeleteFile(Main.DIRCommons & "\ClipStory\" & RandomString & ".txt")
            End If
            ClipList.Add(Main.DIRCommons & "\ClipStory\" & RandomString & ".txt")
            My.Computer.FileSystem.WriteAllText(Main.DIRCommons & "\ClipStory\" & RandomString & ".txt", NewText, False)
            Dim filePaths() As String = Directory.GetFiles(Main.DIRCommons & "\ClipStory", "*.txt")
            ListBox1.Items.Add(filePaths.Length + 1)
            IndexFilesToList()
        Catch ex As Exception
            Console.WriteLine("[AddNewClip@ClipStory]Error: " & ex.Message)
        End Try
    End Sub

    Function CreateIdentification(ByVal Length As Integer)
        Dim obj As New Random()
        Dim posibles As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        Dim longitud As Integer = posibles.Length
        Dim letra As Char
        Dim longitudnuevacadena As Integer = Length
        Dim nuevacadena As String = Nothing
        For i As Integer = 0 To longitudnuevacadena - 1
            letra = posibles(obj.[Next](longitud))
            nuevacadena += letra.ToString()
        Next
        Return nuevacadena
    End Function

    Private Sub ListBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseDoubleClick
        FastColoredTextBox1.Text = My.Computer.FileSystem.ReadAllText(ClipList(ListBox1.SelectedIndex))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.CheckState = CheckState.Checked Then
            My.Settings.ClipStory_IsEnabled = True
            ListBox1.Enabled = True
            FastColoredTextBox1.Enabled = True
        Else
            My.Settings.ClipStory_IsEnabled = False
            ListBox1.Enabled = False
            FastColoredTextBox1.Enabled = False
        End If
        My.Settings.Save()
        My.Settings.Reload()
    End Sub
End Class