function Deploy { 
    [CmdletBinding()]
        param ( 
        [Alias("r")][Parameter(Mandatory = $false, Position = 0)][string] $with ,
        [Alias("p")][Parameter(Mandatory = $false, Position = 1)][string] $Path
    ) 
    if ($with -match ' release ' ){ 
        cd $Path; 
        dotnet build --configuration Debug ; dotnet build --configuration Release ;
        dotnet publish SledgeOMatic -p:PublishProfile=FolderProfile ; 
        Copy-Item -Path ($Path + '\SledgeOMatic\bin\publish\SOM.exe') -Destination c:\_som\SOM.exe -Force 
     
        (Get-Content C:\_som\appsettings.json) `
        -replace '(Database|Server)=.*;', '' `
        -replace 'ConnectionStrings', 'ConnectionStrings' |
        Out-File C:\_som\appsettings.local

    }
  
    explorer.exe 'C:\_som\'
} 
cls; Deploy -with " release commit " -p 'C:\Users\timko\source\repos\SledgeOmatic'  
#   release
 