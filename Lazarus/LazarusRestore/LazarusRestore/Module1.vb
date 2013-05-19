Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Security

Module Module1

    'LazarusRestore.vb
    'Version 3
    '
    '01/18/13
    'tmoore82

    'Program to restore files that were open before forced reboot
    'Covers Microsoft Word 2010, Excel 2010, PowerPoint 2010, and IE 8

    'Set up Regex
    Public RegEx As New Regex("[0-9]{9}_[0-9]\.")

    'Declare variables
    Public strReadArray As String
    Public BuildArray() As String
    Public strFileCheck As String
    Public strNotOpen As String
    Public strDMM As String
    Public strRebootBackup As String
    Public strLocalListName As String
    Public strIMANdocnums As String
    Public strIMANversions As String
    Public strFileName As String
    Public strFileInDMM As String
    Public strNoTab As String
    Public strBackupListName As String
    Public strURLarray As String

    Sub Main()

        Console.WriteLine("Lazarus - Save v1.0")
        Console.WriteLine("Developed for Pillsbury Winthrop Shaw Pittman LLP, January 2013")
        Console.WriteLine()
        Console.WriteLine("Search for saved files...")

        'File Path for saving the files
        strDMM = "C:\dmm"

        'Create a folder in dmm for the backup
        strRebootBackup = strDMM & "\Reboot_Backup\"

        If Not Directory.Exists(strRebootBackup) Then
            Console.WriteLine("Reboot_Backup directory does not exist. Terminating program...")
            System.Threading.Thread.Sleep(5000)
            Exit Sub
        End If
        'Declare a path and name for the log of local files
        strLocalListName = strRebootBackup & "Local_backup_files.csv"

        'Declare a path and name for the log of FileSite files
        strIMANdocnums = strRebootBackup & "IMAN_docnums.csv"

        'Declare a path and name for the log of version numbers
        strIMANversions = strRebootBackup & "IMAN_versions.csv"

        strURLarray = strRebootBackup & "urlArray.csv"

        'Variable for storing the names of files that couldn't be opened
        strNotOpen = ""

        'Declare variables to store names.
        'This will allow the program to warn 
        'users when files have been saved to
        'the backup location because they weren't
        'saved before.
        strFileInDMM = ""

        'Declare and initiate variable to alert that
        'some web tabs couldn't be restored.
        strNoTab = ""

        'Run the function to open local office files
        RestoreLocalFiles()

        'Run the function to open FileSite office files
        RestoreFileSite()

        'Run the function to restore web tabs
        RestoreWebTabs()

        System.Threading.Thread.Sleep(2000)

        'Alert what files, if any, could not be opened automatically
        If strNotOpen <> "" Then
            Dim strNOalert As String
            strNOalert = "Your system has been updated and rebooted.  The following files were properly saved prior to reboot, but they could not be opened automatically:" & vbNewLine & vbNewLine & strNotOpen
            MsgBox(strNOalert)
        End If

        System.Threading.Thread.Sleep(2000)

        If strNoTab <> "" Then
            MsgBox(strNoTab)
        End If

        System.Threading.Thread.Sleep(2000)

        'If there are files identified as 
        'being from the reboot backup folder,
        'list those files in a message to alert
        'the user that they should be saved 
        'to another location.
        'Otherwise, delete the folder.
        If strFileInDMM <> "" Then
            Dim strErrorMsg As String
            strErrorMsg = "Your system has been updated and rebooted. The following files were saved to a backup location. Please save them to another location." & vbNewLine & vbNewLine & strFileInDMM
            MsgBox(strErrorMsg)
        Else
            On Error Resume Next
            My.Computer.FileSystem.DeleteDirectory("c:\dmm\Reboot_Backup", FileIO.DeleteDirectoryOption.DeleteAllContents)
            If Err.Number <> 0 Then
                Console.WriteLine()
                Console.WriteLine("Directory still in use.")
                Console.WriteLine("Program terminating...")
                System.Threading.Thread.Sleep(5000)
                Exit Sub
            End If
            On Error GoTo 0
        End If

        'Alert that the restoration is complete
        Console.WriteLine()
        Console.WriteLine("Program terminating...")
        System.Threading.Thread.Sleep(5000)

        'End program
    End Sub


    '***FUNCTION TO RESTORE OFFICE FILES
    Public Sub RestoreLocalFiles()

        Console.WriteLine()
        Console.WriteLine("Attempting to open local files...")
        Console.WriteLine()

        'ArrayFile = objFSO.OpenTextFile(strBackupListName, ForReading)

        'Read the file
        strReadArray = My.Computer.FileSystem.ReadAllText(strLocalListName)

        'If there's nothing in the file, exit the sub.
        If strReadArray = "" Then
            Exit Sub
        End If

        'Build the array of file names
        BuildArray = Split(strReadArray, ",")

        'Check the file to see if it contains spaces.
        'If it does, enclose it in spaces, then run.
        'Otherwise, just run the file.
        '(The Windows shell can't recognize file names
        'with spaces unless they are in double qoutes.)
        For n = 0 To UBound(BuildArray) - 1
            strFileCheck = BuildArray(n)

            On Error Resume Next

            Console.WriteLine("Opening " & strFileCheck & "...")

            If InStr(strFileCheck, " ") = 0 Then
                Process.Start(strFileCheck)
                If Err.Number <> 0 Then
                    strNotOpen = strNotOpen & strFileCheck & vbNewLine
                End If
            Else
                Process.Start(Chr(34) & strFileCheck & Chr(34))
                If Err.Number <> 0 Then
                    strNotOpen = strNotOpen & strFileCheck & vbNewLine
                End If
            End If

            On Error GoTo 0

            'If the file name contains "Reboot_Backup", it was saved
            'to the backup folder. Create a string that lists these 
            'names.
            strFileName = BuildArray(n)
            If InStr(strFileName, "Reboot_Backup") Then
                strFileInDMM = vbNewLine & strFileInDMM & strFileName
            End If

            'Sleep for a couple seconds between opening files. 
            'This should help improve the user experience.
            System.Threading.Thread.Sleep(2000)

        Next

        'End Restore Office files function
    End Sub

    '***FUNCTION TO RESTORE FILESITE FILES THAT WERE OPEN AT REBOOT
    Public Sub RestoreFileSite()

        Console.WriteLine()
        Console.WriteLine("Attempting to open files from FileSite...")

        'iwl:dms=(Servername)&lib=(Databasename)&num=(docnumber)&ver=(version)&command=opencmd

        'for example
        'iwl:dms=NTGATEWAY&lib=US_NW&num=704015082&ver=1&command=opencmd

        'Dim strGateway As String
        'strGateway = "GATEWAY" 'program seems to work no matter what is here, possibly because we are constantly connected

        Dim strlib As String
        Dim strDocNum As String
        Dim strVers As String

        Dim nIMAN As Integer
        nIMAN = 0

        Dim strReadArrayDN As String
        Dim strReadArrayVR As String

        strReadArrayDN = My.Computer.FileSystem.ReadAllText(strIMANdocnums)
        strReadArrayVR = My.Computer.FileSystem.ReadAllText(strIMANversions)

        Dim BuildArrayDN() As String
        Dim BuildArrayVR() As String

        BuildArrayDN = Split(strReadArrayDN, ",")
        BuildArrayVR = Split(strReadArrayVR, ",")

        Dim strDBnum As String
        Dim strPW As SecureString = New SecureString()
        strPW.Clear()
        strPW.AppendChar("")

        For nIMAN = 0 To UBound(BuildArrayDN) - 1

            strDBnum = Left(BuildArrayDN(nIMAN), 1)

            If strDBnum = "7" Then

                strlib = "US_NW"

            ElseIf strDBnum = "6" Then

                strlib = "US_SW"

            ElseIf strDBnum = "5" Then

                strlib = "US_NE"

            ElseIf strDBnum = "4" Then

                strlib = "US_SE"

            Else

                strNotOpen = strNotOpen & BuildArrayDN(nIMAN) & "v" & BuildArrayVR(nIMAN) & vbNewLine
                Continue For

            End If

            strDocNum = BuildArrayDN(nIMAN)
            strVers = BuildArrayVR(nIMAN)

            'iwl:lib=(Databasename)&num=(docnumber)&ver=(version)&command=opencmd

            'for example
            'iwl:lib=Live&num=12345&ver=2&command=opencmd

            Dim iwlurl As String
            iwlurl = "iwl:lib=" & strlib & "&num=" & strDocNum & "&ver=" & strVers & "&command=opencmd"

            Console.WriteLine()
            Console.WriteLine("Opening " & strDocNum & "v" & strVers & "...")

            Process.Start(iwlurl) ', "", "", strPW, 1)

            nIMAN = nIMAN + 1

            System.Threading.Thread.Sleep(2000)

        Next

    End Sub
    '***FUNCTION TO RESTORE WEB TABS THAT WERE OPEN AT REBOOT
    Public Sub RestoreWebTabs()


        Console.WriteLine()
        Console.WriteLine("Attempting to open web pages...")
        Console.WriteLine()

        'Set and read the array file
        'ArrayFile = objFSO.OpenTextFile(strRebootBackup & "urlArray.csv", ForReading)
        'strReadArray = ArrayFile.ReadLine

        strReadArray = My.Computer.FileSystem.ReadAllText(strURLarray)

        'If there's nothing in the file, exit the sub.
        If strReadArray = "" Then
            Exit Sub
        End If

        'Build the array of urls
        BuildArray = Split(strReadArray, ",")

        'Open each URL in a new tab
        For n = 0 To UBound(BuildArray) - 1
            If InStr(BuildArray(n), "http") Then
                Console.WriteLine("Opening " & BuildArray(n) & "...")
                Process.Start(BuildArray(n))
                System.Threading.Thread.Sleep(2000)

            Else

                Console.WriteLine("Error opening URL, attempting the next...")

            End If

        Next

        'End function
    End Sub


End Module
