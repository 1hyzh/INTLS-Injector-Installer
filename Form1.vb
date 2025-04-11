Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.IO




Public Class Form1


    ' Download the files based on JSON when Guna2Button2 is clicked
    Private Async Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        ' Check if path is empty
        If Guna2TextBox1.Text = "" Then
            MessageBox.Show("You must indicate the install path!", "INTLS", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If


        ' Define the URL for the JSON that contains download URLs
        Dim jsonUrl As String = "https://raw.githubusercontent.com/1hyzh/intls_ver/refs/heads/main/version.json" ' Replace with the actual URL to your JSON

        ' Fetch and parse the JSON data
        Dim client As New HttpClient()
        Try
            ' Get the JSON string from the remote URL
            Dim jsonString As String = Await client.GetStringAsync(jsonUrl)

            ' Parse the JSON string into a dynamic object
            Dim jsonObject As Object = JsonConvert.DeserializeObject(jsonString)

            ' Extract the URLs from the JSON

            Dim injectorEXEurl As String = jsonObject("injectorEXEurl")
            Dim zlibURL As String = jsonObject("zlibURL")
            Dim libcurlURL As String = jsonObject("libcurlURL")

            ' Define the destination folder path
            Dim destinationFolder As String = Guna2TextBox1.Text

            ' Check if the folder exists, if not, create it
            If Not Directory.Exists(destinationFolder) Then
                Directory.CreateDirectory(destinationFolder)
            End If

            ' Download the files

            Await DownloadFileAsync(client, injectorEXEurl, destinationFolder)
            Await DownloadFileAsync(client, zlibURL, destinationFolder)
            Await DownloadFileAsync(client, libcurlURL, destinationFolder)

            If Guna2CheckBox1.Checked = True Then
                Process.Start(destinationFolder & "\INTLS.Injector.exe")
            End If
        Catch ex As Exception
            ' Handle any errors (e.g., JSON fetch errors, download errors)
            MessageBox.Show("Error: " & ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Helper function to download a file
    Private Async Function DownloadFileAsync(client As HttpClient, fileUrl As String, destinationFolder As String) As Task
        Try
            ' Get the file name from the URL (e.g., file1.dll)
            Dim fileName As String = Path.GetFileName(fileUrl)
            Dim destinationPath As String = Path.Combine(destinationFolder, fileName)

            ' Download the file asynchronously
            Dim fileBytes As Byte() = Await client.GetByteArrayAsync(fileUrl)

            ' Save the file to the specified path
            Await File.WriteAllBytesAsync(destinationPath, fileBytes)


        Catch ex As Exception
            ' Handle errors specific to file download
            MessageBox.Show("Error downloading " & Path.GetFileName(fileUrl) & ": " & ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        End
    End Sub
End Class
