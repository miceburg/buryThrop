Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.IO

Module Module1

    'LazarusSave.vb
    'Version 3
    '
    '01/17/13
    'tmoore82

    'Program to save all open files
    'Covers Microsoft Word 2010, Excel 2010, PowerPoint 2010, and IE 8

    'Declare variables
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
    Public strRebootBackup As String
    'Set up Regex
    Public RegEx As New Regex("[0-9]{9}_[0-9]\.")

    Public Sub Main()

        'Create a Shell object
        'Dim objShell As Object
        'objShell = CreateObject("Shell.Application")

        strLocalListText = ""
        strDocnums = ""
        strVersions = ""

        'Procedure calls
        SaveAllWord()
        SaveAllExcel()
        SaveAllPowerPoint()
        SaveAllIE()

        'File Path for saving the files
        Dim strDmm As String
        strDmm = "C:\dmm"

        'If the dmm directory doesn't exist, create it
        If Not My.Computer.FileSystem.DirectoryExists(strDmm) Then
            My.Computer.FileSystem.CreateDirectory(strDmm)
        End If

        'Create a folder in dmm for the backup files
        Dim strRebootBackup As String
        strRebootBackup = strDmm & "\Reboot_Backup\"

        'If the backup folder doesn't exist, create it
        If Not My.Computer.FileSystem.DirectoryExists(strRebootBackup) Then
            My.Computer.FileSystem.CreateDirectory(strRebootBackup)
        End If

        'Assign a variable the Echo directory
        Dim strEcho As String
        strEcho = "C:\NRTEcho"

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

        'Wrap up

        'Set all the variables to nothing
        'fLog = Nothing
        fIMANdocnums = Nothing
        fIMANversions = Nothing
        fLocalList = Nothing
        'objShell = Nothing
        'objFSO = Nothing
        objPrj = Nothing
        'fBackupList = Nothing
        RegEx = Nothing

        'Delver a message to the user that the script is complete.
        '(For testing only.)
        MsgBox("All Done")

        'End the program
    End Sub

    '***FUNCTION TO SAVE ALL OPEN WORD FILES
    Public Sub SaveAllWord()

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
                    'MsgBox ("True - Word")
                    'strFilePath = objPrj.ActiveWindow.Caption
                    strName = VB.Left(strName, InStr(strName, ".") - 1)
                    strNum = VB.Left(strName, InStr(strName, "_") - 1)
                    strVers = Mid(strName, InStr(strName, "_") + 1)
                    strDocnums = strDocnums & strNum & ","
                    strVersions = strVersions & strVers & ","
                    objPrj.iManAutoMacro.AutoMacro.FileSaveBinding()
                ElseIf InStr(strName, ".doc") <> 0 Then
                    objPrj.Save()
                    strFilePath = Path.GetFullPath(strName)
                    strLocalListText = strLocalListText & strFilePath & ","
                Else
                    objPrj.SaveAs(strRebootBackup & "\Word_File_" & w & ".docx")
                    strName = Path.GetFileName(objPrj.Name)
                    strFilePath = strRebootBackup & strName
                    strLocalListText = strLocalListText & strFilePath & ","
                End If

                'Close the current document
                objPrj.Close()

                'Increase the counter
                w = w + 1

            Loop Until objWord.Documents.Count = 0

            'Quit the app
            objWord.Quit()
            System.Threading.Thread.Sleep(10000)

            objWord = Nothing

        End If

        On Error GoTo 0

        'End Word portion
    End Sub

    '***FUNCTION TO SAVE OPEN EXCEL FILES
    Public Sub SaveAllExcel()

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
                    'MsgBox ("True - Word")
                    'strFilePath = objPrj.ActiveWindow.Caption
                    strName = VB.Left(strName, InStr(strName, ".") - 1)
                    strNum = VB.Left(strName, InStr(strName, "_") - 1)
                    strVers = Mid(strName, InStr(strName, "_") + 1)
                    strDocnums = strDocnums & strNum & ","
                    strVersions = strVersions & strVers & ","
                    objPrj.iManAutoMacro.AutoMacro.FileSaveBinding()
                ElseIf InStr(strName, ".xls") <> 0 Then
                    'MsgBox ("xls route")
                    objPrj.Save()
                    strFilePath = Path.GetFullPath(strName)
                    strLocalListText = strLocalListText & strFilePath & ","
                    'MsgBox (strLocalListText)
                Else
                    objPrj.SaveAs(strRebootBackup & "\Excel_File_" & x & ".xlsx")
                    strName = Path.GetFileName(objPrj.Name)
                    'MsgBox(strName)
                    strFilePath = strRebootBackup & strName
                    strLocalListText = strLocalListText & strFilePath & ","
                    'MsgBox (strLocalListText)
                End If

                'Close the current workbook
                objPrj.Close()

                'Increase the counter
                x = x + 1

            Loop Until objExcel.WorkBooks.Count = 0

            'Quit the app
            objExcel.Quit()
            System.Threading.Thread.Sleep(10000)

            objExcel = Nothing

        End If

        On Error GoTo 0

        'End the Excel portion
    End Sub

    '***FUNCTION TO SAVE OPEN POWERPOINT FILES
    Public Sub SaveAllPowerPoint()

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
                    'MsgBox ("True - Word")
                    'strFilePath = objPrj.ActiveWindow.Caption
                    strName = VB.Left(strName, InStr(strName, ".") - 1)
                    strNum = VB.Left(strName, InStr(strName, "_") - 1)
                    strVers = Mid(strName, InStr(strName, "_") + 1)
                    strDocnums = strDocnums & strNum & ","
                    strVersions = strVersions & strVers & ","
                    objPrj.iManAutoMacro.AutoMacro.FileSaveBinding()
                ElseIf InStr(strName, ".ppt") <> 0 Then
                    objPrj.Save()
                    strFilePath = Path.GetFullPath(strName)
                    strLocalListText = strLocalListText & strFilePath & ","
                Else
                    objPrj.SaveAs(strRebootBackup & "\PowerPoint_File_" & p & ".pptx")
                    strName = Path.GetFileName(objPrj.Name)
                    strFilePath = strRebootBackup & strName
                    strLocalListText = strLocalListText & strFilePath & ","
                End If

                'Close the current document
                objPrj.Close()

                'Increase the counter
                p = p + 1

            Loop Until objPowerPoint.Presentations.Count = 0

            'Quit the app
            objPowerPoint.Quit()
            System.Threading.Thread.Sleep(10000)

            objPowerPoint = Nothing

        End If

        On Error GoTo 0

        'End PowerPoint portion
    End Sub


    '***FUNCTION TO SAVE THE URLs OF OPEN WEB PAGES
    Public Sub SaveAllIE()

        'Declare variables
        Dim strURL As String

        'create a text file to store web addresses.
        'These have to be run differently, so it is easier 
        'to pull them from another file.	
        Dim fURLarray As System.IO.FileStream
        fURLarray = System.IO.File.Create(strRebootBackup & "urlArray.csv")
        fURLarray.Close()

        'Initiate URL variable
        strURL = ""

        'Declare and initiate a shell application
        Dim objShellApp As Object
        objShellApp = CreateObject("Shell.Application")

        'Open a Windows App
        Dim objShellWindows As Object
        objShellWindows = objShellApp.Windows

        'Declare variables
        Dim objIE As Object
        Dim strCheckHTTP As String

        'Get the URL for each open window,
        'but add it to the URL string only if it 
        'is a web address.
        Dim n As Integer
        n = 0

        For n = 0 To objShellWindows.Count - 1
            objIE = objShellWindows.Item(n)

            strCheckHTTP = objIE.LocationURL

            If InStr(strCheckHTTP, "http") <> 0 Then

                strURL = strURL & objIE.LocationURL & ","

            End If

        Next

        'Write the URL string
        My.Computer.FileSystem.WriteAllText(strRebootBackup & "urlArray.csv", strURL, False)

        'set variables to nothing
        fURLarray = Nothing
        objIE = Nothing
        objShellWindows = Nothing

        'End of IE portion
    End Sub


End Module
