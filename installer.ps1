 
function somInstaller {
	[CmdletBinding()]
	param(
		[Parameter()]
		[string] $Version,
		[Parameter()]
		[string] $InstallPath
	) 
    $v = $PSCmdlet.MyInvocation.BoundParameters["Verbose"].IsPresent

    $URL=''
    if($Version -notmatch '\d{1,3}\.\d{1,3}' ){
        $HTML = Invoke-RestMethod 'https://github.com/tmnkopp/SledgeOmatic/releases/latest'
        $HTML -match '(/tmnkopp/SledgeOmatic/releases/download/.*/som.exe)'
        $URL = "https://github.com" + $Matches[0] 
    }else{ 
        $URL="https://github.com" + '/tmnkopp/SledgeOmatic/releases/download/' + $Version + '/som.exe' 
    }  
    if($InstallPath -notmatch '\w{1}:.*\\' ){
        $InstallPath = 'c:\_som\' 
    }else{
        md -Force c:\_som\
    }

    if( $v ){
        Write-Host $URL 
        Write-Host $InstallPath
    }

    [System.Environment]::SetEnvironmentVariable('som', $InstallPath + 'som.exe',[System.EnvironmentVariableTarget]::User)
    $webClient = [System.Net.WebClient]::new() 
    try {
       $WebClient.DownloadFile( $URL , $InstallPath + "som.exe"  )
    }
    catch [System.Net.WebException],[System.IO.IOException] {
        "Unable to download som.exe"
    }   

}
somInstaller -Version '~'



