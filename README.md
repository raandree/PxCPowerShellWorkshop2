# PxCPowerShellWorkshop2
PowerShell Workshop Content

> If you want to clone this repository to you machine, follow the instructions in [Clone and use a GitHub repository in Visual Studio Code
](https://docs.microsoft.com/en-us/azure/developer/javascript/how-to/with-visual-studio-code/clone-github-repository?tabs=create-repo-command-palette%2Cinitialize-repo-activity-bar%2Ccreate-branch-command-palette%2Ccommit-changes-command-palette%2Cpush-command-palette)

## Useful Links

- [PowerShell Array Guide: How to Use and Create](https://www.varonis.com/blog/powershell-array)
  This article also explains how to create strongly-typed arrays

  ```powershell
  $b = [int32[]]::new(3)
  ```

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

  -  Formatting a number as currency. The available formatters are documented in           [Standard numeric format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings).
    
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


