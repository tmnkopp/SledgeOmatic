function Deploy { 
    [CmdletBinding()]
        param ( 
        [Parameter(Mandatory = $false, Position = 0)] 
        [string] $with 
    ) 
    if ($with -match ' release ' ){ 
        cd 'C:\Users\Tim\source\repos\SledgeOMatic'; 
        dotnet build --configuration Debug ; dotnet build --configuration Release ;
        dotnet publish SledgeOMatic -p:PublishProfile=FolderProfile ; 
        Copy-Item -Path C:\Users\Tim\source\repos\SledgeOMatic\SledgeOMatic\bin\publish\SOM.exe -Destination c:\_som\SOM.exe -Force  
    }
    if ($with -match ' commit ' ){
        $m = -join ((65..90) + (97..122) | Get-Random -Count 2 | % {[char]$_ +''+ $_ }) 
        cd 'C:\Users\Tim\source\repos\SledgeOMatic';  
        git add .; git commit -m ('refactor context ' + $m) ; git push; 
    }   
    explorer.exe C:\_som\
} 
cls; Deploy -with " release commit "    
 