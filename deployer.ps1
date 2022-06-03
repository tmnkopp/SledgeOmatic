function Deploy { 
    [CmdletBinding()]
        param ( 
        [Parameter(Mandatory = $false, Position = 0)] 
        [string] $with 
    ) 
    if ($with -match ' release ' ){ 
        cd 'D:\repos\SledgeOMatic'; 
        dotnet build --configuration Debug ; dotnet build --configuration Release ;
        dotnet publish SledgeOMatic -p:PublishProfile=FolderProfile ; 
        Copy-Item -Path D:\repos\SledgeOMatic\SledgeOMatic\bin\publish\SOM.exe -Destination c:\_som\SOM.exe -Force 
     
        (Get-Content C:\_som\appsettings.json) `
        -replace '(Database|Server)=.*;', '' `
        -replace 'ConnectionStrings', 'ConnectionStrings' |
        Out-File C:\_som\appsettings.local

    }
    if ($with -match ' commit ' ){
        $m = -join ((65..90) + (97..122) | Get-Random -Count 2 | % {[char]$_ +''+ $_ }) 
        cd 'D:\repos\SledgeOMatic';  
        git add .; git commit -m ('refactor context fix:' + $m) ; git push; 
    }   
    explorer.exe C:\_som\
} 
cls; Deploy -with " release commit "    
 