*It's not a slicer.*
*It's not a dicer.*
*It's not chopper or a hopper.*
*What in the hell could it possibly be?* 
*It's Sledge-O-Matic.*

![alt text](https://the80port.com/cdn/logos/som75-2.png "som")
# Sledge-O-Matic?

SOM takes sucky repetitive code tasks and un-sucks them. 

A code scaffolder, code generator, code compiler, code refactor-er, repetitive task automator...Sledge-O-Matic is a C# extendable library of code generation utilities exposed through coder-friendly endpoints. SOM adheres to fluent developer friendly endpoints. Key value refactor substitutions, for instance, can be easily expressed through JSON, CSV, C# Dictionaries, or SQL generated tables. Custom markup language refactors code inline. 

Compilation steps are made manageable using builder patterns. Compilations are debuggable, with compilation Modes including debug, verbose, cached and commit allowing the code to be inspected and tested prior to committing the compilation. Compilations may be configured and executed via command line using no code/ low code YAML configuration scripts. 

SOML: Compilations and code refactorings may be configured inline using SOML (SOMarkup Language). SOML tags can be introduced into any codebase to execute inline code compile instructions. Angular frontend consuming an API is available for browser based code generation and refactoring. 

Sledge-O-Matic is actively maintained by an overworked but dedicated coder determined to unsuck coding tasks. 

## Command Line Code Compilation
```
    
    > som compile -m Commit -v -s c:\srcdir\ -d d:\destdir\
    
```
## YAML config Compilations

``` YAML
    ContentCompilers:
    - NumericKeyReplacer:  ['c:\_som\_src\_compile\keyval.sql']
    - KeyValReplacer:  ['c:\_som\_src\_compile\replace.json'] 
    Source: 'c:\_som\_src\_compile'
    Dest: 'c:\_som\_src\_compile\_compiled'
    FileFilter: '*asp*' 
    FilenameCompilers: 
    - KeyValReplacer:  ['c:\_som\_src\_compile\replace.json'] 
    Compile:
```

## SOML (SOMarkup Language)

####  PYTHON
``` 
    # python

    # som!schema -m aspnet_Roles -f {0}

    # schema!som
    
    # som!schema -t ~T\PY\TRY_{1}.py 
    
    # schema!som 
```

####  SQL
```  
    ALTER PROCEDURE [dbo].[aspnet_Membership_CREATE]
    -- som!schema -m aspnet_Membership -f @{0} {1}({2})

    -- schema!som
    AS
    BEGIN
    SELECT  @NewUserId = UserId FROM aspnet_Users WHERE LOWER(@UserName) = LoweredUserName 
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId OUTPUT
        SET @NewUserCreated = 1
    END
    -- som!schema -t ~T\SQL\IF_NULL_ELSE_{1}.sql
    
    -- schema!som
    END
```
####  C# / RAZOR
```
    @* som!schema -t ~T\CS\FORM_ELEMENT_{1}.cshtml *@
        <div class="form-group">
                <label asp-for="{3}.{0}" class="control-label"></label>
                <input asp-for="{3}.{0}" class="form-control" />
                <span asp-validation-for="{3}.{0}" class="text-danger"></span>
        </div>
    @* schema!som *@
```
## C# Compilation
```csharp
    
    Compiler compiler = new Compiler(); 
    compiler.Source = "c:\\_som\\_src";
    compiler.Dest = "c:\\_som\\_dest";
    compiler.CompileMode = CompileMode.Commit; 
    compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\pre-replace.json"));
    compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));  
    compiler.FileNameFormatter = (n) => (n.Replace("2020_Q4.sql", "2021_Q1.sql")); 
    compiler.FileFilter = "*my_sprocs*.sql*"; 
    compiler.Compile();  
```

```csharp
    
    Compiler compiler = new Compiler(); 
    compiler.Source = "c:\\_som\\_src\\_compile";
    compiler.Dest = "c:\\_som\\_src\\_compile\\_compiled";
    compiler.CompileMode = CompileMode.Cache; 
    compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\pre-replace.json"));
    compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));
    compiler.ContentCompilers.Add(new Incrementer($",\d{4},", 500));
    compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\post-replace.json"));
    compiler.FileNameFormatter = (n) => (n.Replace("Q4", "Q1"));
    compiler.FileNameFormatter = (n) => (n.Replace("2020", "2021"));
    compiler.FileNameFormatter = (n) => (n.Replace("CFO", "CIO"));
    compiler.FileFilter = "*DB_Version_Update*.sql";
    compiler.Compile();   
    Cache.Inspect();
    compiler.FileFilter = "*db_sprocs*.sql";
    compiler.Compile();
    Cache.Inspect();
    compiler.FileFilter = "*asp*"; 
    compiler.Compile(); 
    Cache.Inspect();
 
```




