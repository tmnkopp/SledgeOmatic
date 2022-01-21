function Deploy { 
    [CmdletBinding()]
        param ( 
        [Parameter(Mandatory = $false, Position = 0)] 
        [string] $with ,
        [Parameter(Mandatory = $false, Position = 0)] 
        [string] $message = 'refactor build '  
    )
    cd 'C:\Users\Tim\source\repos\SledgeOMatic';   
    if ($with -match ' release ' ){
        # taskkill /IM "SOM.exe" /F
        dotnet build --configuration Debug;
        dotnet build --configuration Release;
        dotnet publish SledgeOMatic -p:PublishProfile=FolderProfile   
        Copy-Item -Path C:\Users\Tim\source\repos\SledgeOMatic\SledgeOMatic\bin\publish\SOM.exe -Destination c:\_som\SOM.exe -Force  
        Remove-Item -Path C:\Users\Tim\source\repos\SledgeOMatic\SledgeOMatic\bin\publish\*.pdb -Force
        Remove-Item -Path C:\Users\Tim\source\repos\SledgeOMatic\SledgeOMatic\bin\publish\*.json -Force 
    }
    if ($with -match ' commit ' ){
        $m = -join ((65..90) + (97..122) | Get-Random -Count 2 | % {[char]$_ +''+ $_ })
        $message = $message + $m  
        $message = $message + $m
        cd 'C:\Users\Tim\source\repos\SledgeOMatic';  
        git add .; git commit -m ('config refactor' + $m) ; git push; 
    }  
    #explorer.exe C:\Users\Tim\source\repos\SledgeOMatic\SledgeOMatic\bin\publish\
    explorer.exe C:\_som\
} 
Deploy -with " release commit "    
#  som config
#  som compile -p IG -m Debug
#  som compile -p refactor -m Debug
#  som parse -p config -m Default