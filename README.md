# PxCPowerShellWorkshop2
PowerShell Workshop Content

> If you want to clone this repository to you machine, follow the instructions in [Clone and use a GitHub repository in Visual Studio Code
](https://docs.microsoft.com/en-us/azure/developer/javascript/how-to/with-visual-studio-code/clone-github-repository?tabs=create-repo-command-palette%2Cinitialize-repo-activity-bar%2Ccreate-branch-command-palette%2Ccommit-changes-command-palette%2Cpush-command-palette). **In order to interact with Git repositories, download [git](https://git-scm.com/downloads)**.
***
&nbsp;
## Useful Links

- > **One of the bst knowledge resources in the PowerShell space: [Powershell: Everything you wanted to know about...](https://powershellexplained.com/sitemap/?utm_source=blog&utm_medium=blog&utm_content=recent)**
- > **[Advanced Functions Explained](https://github.com/raandree/PowerShellTraining): From a one-liner to a full featured advanced function and then to a module**
- [PowerShell Array Guide: How to Use and Create](https://www.varonis.com/blog/powershell-array)
  This article also explains how to create strongly-typed arrays

  ```powershell
  $b = [int32[]]::new(3)
  ```
- PowerShell Cheat Sheets
  - https://cdn.comparitech.com/wp-content/uploads/2018/08/Comparitech-Powershell-cheatsheet.pdf
  - https://ramblingcookiemonster.github.io/images/Cheat-Sheets/powershell-basic-cheat-sheet2.pdf
  - https://gitlab.com/JamesHedges/notes/-/wikis/Powershell/PowerShell-Cheat-Sheet
  - https://www.theochem.ru.nl/~pwormer/teachmat/PS_cheat_sheet.html

- When dealing with security information on NTFS volumes:
  - https://www.powershellgallery.com/packages/NTFSSecurity/4.2.6
  - https://github.com/raandree/NTFSSecurity

&nbsp;

## Generic Code Samples
- ### Get your local account and the group membership as SamAccountNames

  ```powershell
  [System.Security.Principal.WindowsIdentity]::GetCurrent()
  $id = [System.Security.Principal.WindowsIdentity]::GetCurrent())
  $id.Groups | ForEach-Object { $_.Translate([System.Security.Principal.NTAccount]) } 
  ```
  Output:

  ```
  Value
  -----
  Everyone
  BUILTIN\Users
  BUILTIN\Performance Log Users
  NT AUTHORITY\INTERACTIVE
  CONSOLE LOGON
  NT AUTHORITY\Authenticated Users
  NT AUTHORITY\This Organization
  MicrosoftAccount\r.andree@live.com
  NT AUTHORITY\Local account
  LOCAL
  NT AUTHORITY\Cloud Account Authentication
  ```

- ### String Concatenation / Formatting

  - Formatting a number as currency andin  many other ways.
    
    The available formatters are documented in           [Standard numeric format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings).
    
    ```powershell
    1..10 | ForEach-Object { 'There is {0,20:C} on my bank account' -f (Get-Random -Minimum 10 -Maximum 10000) }
    ```

  - Decimal number formatting for sorting

    This creates strings that cannot be sorted:

    ```powershell
    1..10 | ForEach-Object { "$_ Test User" }
    foreach ($i in $a) { "$i Test User" }
    ```

    In this sample each number has leasing zeros and makes sorting possible.

    ```powershell
    1..10 | ForEach-Object { "{0:D3} Test User" -f $_ } | Sort-Object
    ```

  - Using PowerShell variable interpolation and the format operator (-f, which points to ```[string]::Format()``` to get the same result.

    ```powershell
    $p = Get-Process -Id $PID
    "The current process with the ID $PID consumes $($p.WorkingSet) Bytes memory"

    "The current process with the ID $PID consumes $($p.WorkingSet / 1MB) MB memory"
    "The current process with the ID $PID consumes $([System.Math]::Round($p.WorkingSet / 1MB, 2)) MB memory"

    "The current process with the ID {0} consumes {1:N2} MB memory" -f $PID, ($p.WorkingSet / 1MB)
    ```

  - Get the 5 largest files, then get the top 5 extensions with the most files, then group all files by their creation time (only year and month)

    ```powershell
    dir -Recurse -File | Sort-Object -Property Length -Descending | Select-Object -First 5

    dir -Recurse -File |
    Group-Object -Property Extension |
    Sort-Object -Property Count -Descending |
    Select-Object -First 5

    dir -Recurse -File | Group-Object -Property { $_.CreationTime.ToString('yy MM') } | Sort-Object -Property Name -Descending
    ```

  - Adding properties and methods to existing can make data analysis way easier. Here we are getting data from a deeper level of the events and move them to the top. Things like grouping and filtering is easier now or makes it possible in the first place.

&nbsp;

- ### DefaultParameterValues: Describes how to set custom default values for cmdlet parameters and advanced functions.

  This sample is taken from the script [10 HyperV Full Lab with DSC and AzureDevOps.ps1](https://github.com/dsccommunity/DscWorkshop/blob/main/Lab/10%20HyperV%20Full%20Lab%20with%20DSC%20and%20AzureDevOps.ps1) and demonstrates, how to assign the following values to all machines without repeating it over and over again.  
  
  ```powershell  
  $PSDefaultParameterValues = @{
    'Add-LabMachineDefinition:Network'         = $labName
    'Add-LabMachineDefinition:ToolsPath'       = "$labSources\Tools"
    'Add-LabMachineDefinition:DomainName'      = 'contoso.com'
    'Add-LabMachineDefinition:DnsServer1'      = '192.168.111.10'
    'Add-LabMachineDefinition:OperatingSystem' = 'Windows Server 2022 Datacenter (Desktop Experience)'
    'Add-LabMachineDefinition:Gateway'         = '192.168.111.50'
  }
  ```

- ### A comparison of scriptblocks and functions

  ```powershell
  $cmd = {
    param(
        $Year
    )
    Get-Date -Year $Year
  }

  & $cmd 2000

  function Get-MyDate {
      param(
          $Year
      )
      Get-Date -Year $Year
  }

  Get-MyDate -Year 2000

  New-Item function:\Test1 -Value $cmd
  ```

- ### Dynamic ScriptBlocks
  Scriptblock can also be created dynamically by converting text into a scriptblock

  ```powershell
  $year = 2000
  $text = "Get-Date -Year $year"
  $cmd = [scriptblock]::Create($text)

  Invoke-LabCommand -ComputerName Lab2016DC1 -ScriptBlock $cmd -PassThru
  ```

- ### Injecting locally defined functions into a remote scope
  Usually functions can only be called remotely if available on the remote machine. The function[```Add-FunctionToPSSession```](https://github.com/AutomatedLab/AutomatedLab.Common/blob/develop/AutomatedLab.Common/Common/Public/Add-FunctionToPSSession.ps1) can injects functions into remote PowerShell sessions. There is also a cmdlet [```Add-VariableToPSSession```](https://github.com/AutomatedLab/AutomatedLab.Common/blob/develop/AutomatedLab.Common/Common/Public/Add-VariableToPSSession.ps1).

  ```powershell
  function Set-W32TimeServiceState {
      $s = Get-Service -Name W32Time
      if ($s.Status -ne 'Running') {
          Write-Host "Service '$($s.Name)' is not running on '$($env:COMPUTERNAME)', starting it..." -ForegroundColor Green
          $s | Start-Service
          Write-Host done -ForegroundColor Green
      }
      else {
          Write-Host "Service '$($s.Name)' is already on '$($env:COMPUTERNAME)' running" -ForegroundColor Green
      }
      Start-Sleep -Seconds 5
  }

  $vms = Get-LabVM
  $psSession = New-LabPSSession -ComputerName $vms
  Add-FunctionToPSSession -Session $psSession -FunctionInfo (Get-Command -Name Set-W32TimeServiceState)
  Invoke-LabCommand -ComputerName $vms -ScriptBlock { Set-W32TimeServiceState }
  $psSession | Remove-PSSession
  ```
- ### Comparing ```Select-Object``` with ```Where-Object```
  
  ```powershell
  #Horizontal "filtering" or rather selection of properties to return
  Get-Process | Select-Object -Property ID, Name, WorkingSet

  #returning the largest 5 processes considering the property WorkingSet, not exacly filtering
  Get-Process | Sort-Object -Property WorkingSet -Descending | Select-Object -First 5

  #really filtering giving an expression
  Get-Process | Where-Object { $_.WorkingSet -gt 500MB }
  Get-Process | Where-Object WorkingSet -gt 500MB
  Get-Process | Where-Object -Property WorkingSet -GT -Value 500MB

  #what Where-Object does internally
  Get-Process | ForEach-Object {
      if ($_.WorkingSet -gt 500MB) {
          $_
      }
  }

  #Some people expect the 1st code block to take less memory as for the selection of properties. In fact, it does not
  #but is much slower
  1..300 | ForEach-Object {
      $p = Get-Process | Select-Object -Property Name, ID, WorkingSet
  }
  1..300 | ForEach-Object {
      $p = Get-Process 
  }
  ```

&nbsp;

## Terms
- Class / Type
  
  Definition of what kind of object you can create. Remember, you cannot create objects of types / classes that are not defined. Classes are defined in the .net framework or additional libraries.
  
- Object / Instance
  
  Something you can actually touch, something created using the existing building plans (classes / types).
  > Note: Everything in .net and PowerShell is an object, even an int and bool
- Function
  
  Reusable unit of code that has a name and can be called.

  ```powershell
  function f1 {
    Get-Date
  }

  $a = f1
  ```

- Methods
  
  Similar like functions methods are units of code but available only on an existing object or class (static methods).

  > Calling a method always requires brackets at the end.

  ```powershell
  $a = Get-Date
  $a.ToString()
  ```

  - Properties

  Data that is defined on an object. Properties like methods can only be access on an object like this:

  ```powershell
  $a = Get-Date
  $a.Year
  ```
  - Member

  Is a collective term for everything a class or object provides. In PowerShell it usually referrers to methods and properties.

  - Scriptblock

    Scriptblock are units of codes like functions but without a name.

    ```powershell
    $sb = { Get-Date }
    $a = & $sb
    ```

&nbsp;

## When to use which kind of bracket?

- Normal brackets ()
  
    - To prioritize sections of a line like in math to prioritize additions before multiplications.

      ```powershell
      #This results in an error
      Get-Process.Count

      (Get-Process).Count
      ```

    - To call a method

- Curly brackets

    - To indicate scripts blocks. Used in If, ForEach, functions, etc.

    ```powershell
    if ($a -gt 5){
      'yes'
    }
    else {
      'no'
    }
    ```

- Square brackets
