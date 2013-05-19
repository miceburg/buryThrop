Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.IO
Imports SHDocVw

Module Module1

    'LazarusSave.vb
    'Version 4
    '
    '01/23/13
    'tmoore82

    'Program to save all open files
    'Covers Microsoft Word 2010, Excel 2010, PowerPoint 2010, and IE 8

    'Declare variables
    Public strDMM As String
    Public strRebootBackup As String
    Public objPrj As Object 'point to the file
    Public strName As String 'name of the file
    Public strNum As String 'file number
    Public strVers As String 'version number
    Public strFilePath As String 'path of the file
    Public strDocNum As String 'document number
    Public strVersion As String 'version number
    Public strDocnums As String
    Public strVersions As String
    Public strLocalListText As String
    Public strURLarray As String
    Public strSaveName As String
    Public strDebug As String

    'Set up Regex
    Public RegEx As New Regex("[0-9]{9}_[0-9]\.")

    Public Sub Main()

        Console.WriteLine("Lazarus - Save v1.0")
        Console.WriteLine("Developed for Pillsbury Winthrop Shaw Pittman LLP, January 2013")
        Console.WriteLine()
        Console.WriteLine("Creating backup files and directories...")

        strLocalListText = ""
        strDocnums = ""
        strVersions = ""
        strDebug = ""

        'File Path for saving the files
        strDMM = "C:\dmm"

        'If the dmm directory doesn't exist, create it
        If Not My.Computer.FileSystem.DirectoryExists(strDMM) Then
            My.Computer.FileSystem.CreateDirectory(strDMM)
        End If

        'Create a folder in dmm for the backup files
        strRebootBackup = strDMM & "\Reboot_Backup\"

        'If the backup folder doesn't exist, create it
        If Not My.Computer.FileSystem.DirectoryExists(strRebootBackup) Then
            My.Computer.FileSystem.CreateDirectory(strRebootBackup)
        End If

        'Procedure calls
        SaveAllWord()
        SaveAllExcel()
        SaveAllPowerPoint()

        'Declare a path and name for the log of local files
        Dim strLocalListName As String
        strLocalListName = strRebootBackup & "Local_backup_files.csv"

        'Create the local log
        Dim fLocalList As System.IO.FileStream
        fLocalList = System.IO.File.Create(strLocalListName)
        fLocalList.Close()

        'write the file names to the local log so we can grab them later
        'as an array
        My.Computer.FileSystem.WriteAllText(strLocalListName, strLocalListText, False)

        'Declare a path and name for the log of FileSite files
        Dim strIMANdocnums As String
        strIMANdocnums = strRebootBackup & "IMAN_docnums.csv"

        'Create the FileSite log
        Dim fIMANdocnums As System.IO.FileStream
        fIMANdocnums = System.IO.File.Create(strIMANdocnums)
        fIMANdocnums.Close()

        'write the file names to the FileSite log so we can grab them later
        'as an array
        My.Computer.FileSystem.WriteAllText(strIMANdocnums, strDocnums, False)


        'Declare a path and name for the log of version numbers
        Dim strIMANversions As String
        strIMANversions = strRebootBackup & "IMAN_versions.csv"

        'Create the Version log
        Dim fIMANversions As System.IO.FileStream
        fIMANversions = System.IO.File.Create(strIMANversions)
        fIMANversions.Close()

        'write the file names to the version log so we can grab them later
        'as an array
        My.Computer.FileSystem.WriteAllText(strIMANversions, strVersions, False)

        'Create the URL log
        strURLarray = strRebootBackup & "urlArray.csv"
        Dim fURLarray As System.IO.FileStream
        fURLarray = System.IO.File.Create(strURLarray)
        fURLarray.Close()

        'Call the IE procedure
        SaveAllIE()

        Dim strDebugFile As String
        strDebugFile = strDMM & "\LS_Debug.txt"

        Dim fDebug As System.IO.FileStream
        fDebug = System.IO.File.Create(strDebugFile)
        fDebug.Close()

        My.Computer.FileSystem.WriteAllText(strDebugFile, strDebug, False)

        'Wrap up

        'Set all the variables to nothing
        fIMANdocnums = Nothing
        fIMANversions = Nothing
        fLocalList = Nothing
        objPrj = Nothing
        RegEx = Nothing

        Console.WriteLine()
        Console.WriteLine("Program complete.")

        System.Threading.Thread.Sleep(3000)

    End Sub

    '***FUNCTION TO SAVE ALL OPEN WORD FILES
    Public Sub SaveAllWord()

        strDebug = strDebug & "Word files: "

        Console.WriteLine()
        Console.WriteLine("Saving open Microsoft Word files...")

        'Start a counter
        Dim w As Integer
        w = 1

        Dim objWord As Object

        'Get any open instance of the app
        On Error Resume Next
        objWord = GetObject(, "Word.Application")

        If Err.Number = 0 Then

            'Until there are no open documents left...
            Do

                'Work with the active document
                objPrj = objWord.ActiveDocument

                'Get the file name
                strName = Path.GetFileName(objPrj.Name)
                'MsgBox("strName=" & strName)

                'RegEx test to see if the file is a FileSite file.
                'If so, save it to FileSite. Then set the string to
                'reflect the directory of the file to NRTEcho. 
                'This is where the file resides pre check-in. By opening
                'the Echo file later, we can simulate the check-out 
                'process.
                If RegEx.IsMatch(strName) = True Then
                    FileSiteSave(strName)
                ElseIf InStr(strName, ".doc") <> 0 Then
                    LocalSave(strName)
                Else
                    strSaveName = strRebootBackup & "Word_File_" & w & ".docx"
                    LocalSaveAs(strSaveName)
                End If

                If strName <> "" Then
                    Console.WriteLine(strName & " saved...")
                    strDebug = strDebug & strName & ", "
                End If

                'Close the current document
                objPrj.Close()

                'Increase the counter
                w = w + 1

            Loop Until objWord.Documents.Count = 0

            strDebug = strDebug & vbNewLine & "# Word Files: " & w - 1 & vbNewLine & vbNewLine

            'Quit the app
            objWord.Quit()
            System.Threading.Thread.Sleep(8000)

            objWord = Nothing

        Else

            AppNotOpen()
            strDebug = strDebug & "none" & vbNewLine & vbNewLine

        End If

        On Error GoTo 0

        strName = ""

        'End Word portion
    End Sub

    '***FUNCTION TO SAVE OPEN EXCEL FILES
    Public Sub SaveAllExcel()

        strDebug = strDebug & "Excel files: "

        Console.WriteLine()
        Console.WriteLine("Saving open Microsoft Excel files...")

        Dim x As Integer
        x = 1

        Dim objExcel As Object

        'Get any open instance of the app
        On Error Resume Next
        objExcel = GetObject(, "Excel.Application")

        If Err.Number = 0 Then

            'Until there are no open workbooks left...
            Do

                'Work with the active workbook
                objPrj = objExcel.ActiveWorkbook

                'Get the file name
                strName = Path.GetFileName(objPrj.Name)
                'MsgBox(strName)

                If RegEx.IsMatch(strName) = True Then
                    FileSiteSave(strName)
                ElseIf InStr(strName, ".xls") <> 0 Then
                    LocalSave(strName)
                Else
                    strSaveName = strRebootBackup & "Excel_File_" & x & ".xlsx"
                    LocalSaveAs(strSaveName)
                End If

                If strName <> "" Then
                    Console.WriteLine(strName & " saved...")
                    strDebug = strDebug & strName & ", "
                End If

                'Close the current workbook
                objPrj.Close()

                'Increase the counter
                x = x + 1

            Loop Until objExcel.WorkBooks.Count = 0

            strDebug = strDebug & vbNewLine & "# Excel Files: " & x - 1 & vbNewLine & vbNewLine

            'Quit the app
            objExcel.Quit()
            System.Threading.Thread.Sleep(8000)

            objExcel = Nothing

        Else

            AppNotOpen()
            strDebug = strDebug & "none" & vbNewLine & vbNewLine

        End If

        On Error GoTo 0

        strName = ""

        'End the Excel portion
    End Sub

    '***FUNCTION TO SAVE OPEN POWERPOINT FILES
    Public Sub SaveAllPowerPoint()

        strDebug = strDebug & "Powerpoint files: "

        Console.WriteLine()
        Console.WriteLine("Saving open Microsoft PowerPoint files...")

        Dim p As Integer
        p = 1

        Dim objPowerPoint As Object

        'Get any open instance of the app
        On Error Resume Next
        objPowerPoint = GetObject(, "PowerPoint.Application")
        If Err.Number = 0 Then

            'Until there are no open presentations left...
            Do

                'Work with the active presentation
                objPrj = objPowerPoint.ActivePresentation

                'Get the file name
                strName = Path.GetFileName(objPrj.Name)

                If RegEx.IsMatch(strName) = True Then
                    FileSiteSave(strName)
                ElseIf InStr(strName, ".ppt") <> 0 Then
                    LocalSave(strName)
                Else
                    strSaveName = strRebootBackup & "PowerPoint_File_" & p & ".pptx"
                    LocalSaveAs(strSaveName)
                End If

                If strName <> "" Then
                    Console.WriteLine(strName & " saved...")
                    strDebug = strDebug & strName & ", "
                End If

                'Close the current document
                objPrj.Close()

                'Increase the counter
                p = p + 1

            Loop Until objPowerPoint.Presentations.Count = 0

            strDebug = strDebug & vbNewLine & "# Powerpoint Files: " & p - 1 & vbNewLine & vbNewLine

            'Quit the app
            objPowerPoint.Quit()

            System.Threading.Thread.Sleep(8000)

            objPowerPoint = Nothing

        Else

            AppNotOpen()
            strDebug = strDebug & "none" & vbNewLine & vbNewLine

        End If

        On Error GoTo 0

        strName = ""

        'End PowerPoint portion
    End Sub


    '***FUNCTION TO SAVE THE URLs OF OPEN WEB PAGES
    Public Sub SaveAllIE()

        strDebug = strDebug & "IE URLs: "

        Console.WriteLine()
        Console.WriteLine("Saving web tabs open in Internet Explorer...")

        'Declare variables
        Dim strURL As String

        Dim SWs As New SHDocVw.ShellWindows
        Dim IE As SHDocVw.InternetExplorer

        strURL = ""

            ' Look at the URLs of any active Explorer windows 
            ' (this includes WINDOWS windows, not just IE)
            For Each IE In SWs

                ' Check the I.E. window to see if it's pointed 
                ' to a web location (http)
            If Left$(IE.LocationURL, 4) = "http" Then

                strURL = strURL & IE.LocationURL & ","
                Console.WriteLine(IE.LocationURL & " saved...")

            End If



            IE.Quit()

            Next IE

        If strURL = "" Then

            AppNotOpen()
            strDebug = strDebug & "none" & vbNewLine & vbNewLine

        Else

            strDebug = strDebug & strURL & ", "

        End If

            My.Computer.FileSystem.WriteAllText(strURLarray, strURL, False)

            IE = Nothing
            SWs = Nothing

            'End of IE portion
    End Sub


    Public Sub FileSiteSave(ByVal strName As String)

        'MsgBox("FileSite")
        strName = VB.Left(strName, InStr(strName, ".") - 1)
        strNum = VB.Left(strName, InStr(strName, "_") - 1)
        strVers = Mid(strName, InStr(strName, "_") + 1)
        strDocnums = strDocnums & strNum & ","
        strVersions = strVersions & strVers & ","
        objPrj.Save()

    End Sub

    Public Sub LocalSave(ByVal strName As String)

        'MsgBox("Local Save")
        objPrj.Save()
        strFilePath = objPrj.Path & "\" & strName
        strLocalListText = strLocalListText & strFilePath & ","

    End Sub

    Public Sub LocalSaveAs(ByVal strSaveName As String)

        objPrj.SaveAs(strSaveName)
        'MsgBox("Through the save command")
        strFilePath = strSaveName
        strLocalListText = strLocalListText & strFilePath & ","

    End Sub

    Public Sub AppNotOpen()

        Console.WriteLine("This application is not open.")

    End Sub

End Module
