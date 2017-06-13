Imports System
Imports System.IO
Imports System.Threading.Tasks
Imports ITHit.WebDAV.Server

Namespace CalDav

    ''' <summary>
    ''' Folder that contains user folders which contain calendars.
    ''' Instances of this class correspond to the following path: [DAVLocation]/calendars/
    ''' </summary>
    ''' <example>
    ''' [DAVLocation]
    '''  |-- ...
    '''  |-- calendars  -- this class
    '''      |-- [User1]
    '''      |-- ...
    '''      |-- [UserX]
    ''' </example>
    Public Class CalendarsRootFolder
        Inherits DavFolder

        ''' <summary>
        ''' This folder name.
        ''' </summary>
        Private Shared ReadOnly calendarsRootFolderName As String = "calendars"

        ''' <summary>
        ''' Path to this folder.
        ''' </summary>
        Public Shared CalendarsRootFolderPath As String = String.Format("{0}{1}/", DavLocationFolder.DavLocationFolderPath, calendarsRootFolderName)

        ''' <summary>
        ''' Returns calendars root folder that corresponds to path or null if path does not correspond to calendars root folder.
        ''' </summary>
        ''' <param name="context">Instance of <see cref="DavContext"/></param>
        ''' <param name="path">Encoded path relative to WebDAV root.</param>
        ''' <returns>CalendarsRootFolder instance or null if path does not correspond to this folder.</returns>
        Public Shared Function GetCalendarsRootFolder(context As DavContext, path As String) As CalendarsRootFolder
            If Not path.Equals(CalendarsRootFolderPath, StringComparison.InvariantCultureIgnoreCase) Then Return Nothing
            Dim folder As DirectoryInfo = New DirectoryInfo(context.MapPath(path))
            If Not folder.Exists Then Return Nothing
            Return New CalendarsRootFolder(folder, context, path)
        End Function

        Private Sub New(directory As DirectoryInfo, context As DavContext, path As String)
            MyBase.New(directory, context, path)
        End Sub

        'If required you can appy some rules, for example prohibit creating files in this folder
        '
        '/// <summary>
        '/// Prohibit creating files in this folder.
        '/// </summary>
        'override public async Task<IFileAsync> CreateFileAsync(string name)
        '{
        'throw new DavException("Creating files in this folder is not implemented.", DavStatus.NOT_IMPLEMENTED);
        '}
        '
        '/// <summary>
        '/// Prohibit creating folders via WebDAV in this folder.
        '/// </summary>
        '/// <remarks>
        '/// New user folders are created during first log-in.
        '/// </remarks>
        'override public async Task CreateFolderAsync(string name)
        '{
        'throw new DavException("Creating sub-folders in this folder is not implemented.", DavStatus.NOT_IMPLEMENTED);
        '}
        '
        '/// <summary>
        '/// Prohibit copying this folder.
        '/// </summary>
        'override public async Task CopyToAsync(IItemCollection destFolder, string destName, bool deep, MultistatusException multistatus)
        '{
        'throw new DavException("Copying this folder is not allowed.", DavStatus.NOT_ALLOWED);
        '}
        ''' <summary>
        ''' Prohibit moving or renaming this folder
        ''' </summary>        
        Overrides Public Async Function MoveToAsync(destFolder As IItemCollectionAsync, destName As String, multistatus As MultistatusException) As Task
            Throw New DavException("Moving or renaming this folder is not allowed.", DavStatus.NOT_ALLOWED)
        End Function

        ''' <summary>
        ''' Prohibit deleting this folder.
        ''' </summary>        
        Overrides Public Async Function DeleteAsync(multistatus As MultistatusException) As Task
            Throw New DavException("Deleting this folder is not allowed.", DavStatus.NOT_ALLOWED)
        End Function
    End Class
End Namespace
