*It's not a slicer.*
*It's not a dicer.*
*It's not chopper or a hopper.*
*What could it possibly be?* 
*It's Sledge-O-Matic.*

![alt text](https://the80port.com/cdn/logos/som75-2.png "som")
# Sledge-O-Matic
 
A code scaffolder, code generator, code refactor-er, task automator...Sledge-O-Matic is a C# extendable library of code generation utilities exposed through coder-friendly endpoints. Key value refactor substitutions can be configured through JSON, CSV, C# Dictionaries, SQL scripts, and more. Custom markup language (*soml*) can be used to refactor code inline. 


Compilation steps are made manageable using builder patterns. Compilations are debuggable, with compilation Modes including debug, verbose, cached and commit allowing the code to be inspected and tested prior to committing the compilation. Compilations may be configured and executed via command line using no code/ low code YAML configuration scripts. 

SOML: Compilations and code refactorings may be configured inline using SOML (**S**ledge-**O**-**M**atic/markup **L**anguage). SOML tags can be introduced into any codebase to execute inline code compile instructions. Angular frontend consuming an API is available for browser based code generation and refactoring. 

```   
    som!RegexReplacer  -p /p {"(nvarchar)":"string"}
        som!ModelCompile  -p /p aspnet_Membership /p .*
        /n /t /t, {1} {0}
        ModelCompile!som 
    RegexReplacer!som   

```
***

Sledge-O-Matic is a member of the "O-Matics" family, a suite of coding projects actively maintained by a busy but dedicated coder determined to unsuck sucky coding tasks.   

[SOM Angular Dashboard](https://github.com/tmnkopp/SOMDash) for GUI based Sledge-O-Matic-ing 

[Browse-O-matic](https://github.com/tmnkopp/BrowseOmatic) for automated testing your Sledge-O-Matic-ed code 

 
 
***
# Features

## Command Line Code Refactor
 
``` 
    > som compile -p refactorConfig -m Cache
    > som compile -p refactorConfig -m Debug    
    > som compile -p refactorConfig -m Commit       
    
```
## Compilations Config (ex. refactorConfig.yaml file)

``` YAML
    ContentCompilers: 
    - RegexReplacer:  '{"OriginalValue":"refac"}'    
    - NumericIncrementer:  [22421, 32421, '\d{5}'] 
    - NumericIncrementer:  [2410, 3521, '24[1-3]\d']  
    - NumericIncrementer:  [2310, 3310, '23[1-3]\d']  
    - NumericReplacer:  '{"2021":"2022"}'  
    FilenameCompilers:  
    - RegexReplacer:  '{"Original":"refac"}'    
    Compilations: 
    - FileFilter: '*.cs*'
      Source: 'C:\_som\source\'
      Dest: 'C:\_som\dest\csharp\'
    - FileFilter: '*.sql*'
      Source: 'C:\_som\source\'
      Dest: 'C:\_som\dest\sql\'    
```


# SOML **S**ledge-**O**-**M**atic/markup **L**anguage
 

Sledge-O-Matic supports in-line code compilation using SOML tags (som!  !som). 
 
`som!ModelCompile` for example will generate code based on a model and inject it directly into code. Formatting the injected code can be done inline or by using code templates. 

The following will inject all fields from the aspnet_Membership table into the code and format them as SQL parameters. 

```

        som!ModelCompile  -p /p aspnet_Membership /p .*
        /n /t /t, {1} {0}
        ModelCompile!som 

```
For more complicated formatting, simply reference a file

```

        som!ModelCompile  -p /p aspnet_Membership /p ^(?!.*User).*
        ~T\SQL\MyCustomTemplate_{1}.sql
        ModelCompile!som  

```

Support for nested tags to control order of execution 

```

    <!--som!NumericIncrementer  -p /p 1000 /p 5000 /p 10\d{2} -->
        1000
        1001
        1002
        1003
        1004
    <!--NumericIncrementer!som -->
 
    som!RegexReplacer  -p /p {"(nvarchar)":"string"}
        som!ModelCompile  -p /p aspnet_Membership /p ^(?!.*PK_|User).*
        /n /t /t, {1} {0}
        ModelCompile!som 
    RegexReplacer!som   

```


SOML integrates with any codebase.

####  PYTHON
``` python
    # python     
    import compiler
    import schema_normalizer
    class PyCompiler:
        def __init__(self, context):
            self._ctx = context 
            # som!ModelCompile  -p /p aspnet_Membership /p .*
            # ~T\PY\TRY_{1}.py 
            # ModelCompile!som 
    # som!ModelCompile -p /p aspnet_Membership /p .*
        self._ctx = context  
    # ModelCompile!som              
```

####  TypeScript
``` TypeScript 
    export class MembershipModel {
        constructor(
        // som!ModelCompile  -p /p aspnet_Membership /p .*
            public {0}?:{1}
        // ModelCompile!som                 
        ){}
    }//export class
    export class RolesModel {
      constructor(
        // som!ModelCompile  -p /p aspnet_Membership /p .*
            public {0}?:{1}
        // schema!ModelCompile    
      ){} 
    }//export class 
     
 
```

####  SQL
``` SQL 
    ALTER PROCEDURE [dbo].[aspnet_Membership_CREATE]
    -- som!ModelCompile  -p /p aspnet_Membership /p .*
        {0}
    -- ModelCompile!som
    AS
    BEGIN
    SELECT  @NewUserId = UserId FROM aspnet_Users WHERE LOWER(@UserName) = LoweredUserName 
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId OUTPUT
        SET @NewUserCreated = 1
    END
    -- som!ModelCompile  -p /p aspnet_Membership /p .*
    ~T\SQL\IF_NULL_ELSE_{1}.sql
    -- ModelCompile!som
    END
```
####  C# / RAZOR
``` CSHTML
    @* som!ModelCompile -p /p aspnet_Membership /p .*
        <div class="form-group">
                <label asp-for="{3}.{0}" class="control-label"></label>
                <input asp-for="{3}.{0}" class="form-control" />
                <span asp-validation-for="{3}.{0}" class="text-danger"></span>
        </div>
    @* ModelCompile!som *@
```
## Custom Compilers

Create custom Sledge-O-Matic compilers by extending `ICompilable`

```csharp

namespace SOM.Procedures
{
    public interface ICompilable
    {
         string Compile(string content);
    }
    public class MyCustomCompiler : ICompilable
    {
        public string Compile(string content)
        {
            // DO STUFF TO CODE 
            return content;
        }
    }
}

```

## Compilation Examples

```csharp
    
    Compiler compiler = new Compiler(); 
    compiler.Source = "c:\\_som\\_src";
    compiler.Dest = "c:\\_som\\_dest";
    compiler.CompileMode = CompileMode.Debug; 
    compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\pre-replace.json"));
    compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));  
    compiler.FileNameFormatter = (n) => (n.Replace("2020_Q4.sql", "2021_Q1.sql")); 
    compiler.FileFilter = "*my_sprocs*.sql*"; 
    compiler.Compile();  
```

Expressing in YAML

``` YAML

    ContentCompilers: 
    - NumericKeyReplacer:  'c:\_som\_src\_compile\keyval.sql'   
    - KeyValReplacer:  'c:\_som\_src\_compile\replace.json'
    FilenameCompilers:  
    - KeyValReplacer:  'c:\_som\_src\_compile\replace.json'    
    Compilations: 
    - FileFilter: '*my_sprocs*.sql*' 
      Source: 'c:\path\to\source'
      Dest: 'c:\path\to\compile\dest'
 
```

```csharp
    
    Compiler compiler = new Compiler(); 
    compiler.Source = "C:\Path\To\Source\Files";
    compiler.Dest = "C:\Path\To\Save\ReCompiled\Files";
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

## Compile + Build + Publish + Browser Test
```
    
    > som compile -m Commit -v -s c:\srcdir\ -d d:\destdir\ -b -t Run_Browser_Tests
    
```
 
OnCompiled executing an automated in-browser test routine using [Browse-O-matic](https://github.com/tmnkopp/BrowseOmatic) after the code is generated. 

```csharp 
            Compiler compiler = new Compiler();
            compiler.Source = "C:\Path\To\Source\Files";
            compiler.Dest = "C:\Path\To\Save\ReCompiled\Files";
            compiler.CompileMode = CompileMode.Commit;
            compiler.OnCompiled += (s, a) =>
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "dotnet build",
                    Arguments = "-runtime rhel.7.4-x64"
                };
                Process.Start(psi);
            };             
            compiler.OnCompiled += (s, a) =>
            {
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = System.Environment.GetEnvironmentVariable("bom");
                startinfo.UseShellExecute = true;
                startinfo.Arguments = @"exe -t Run_Browser_Tests";
                Process p = Process.Start(startinfo);
            };
            compiler.FileFilter = "*asp*";
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\pre-compile.json"));    
            compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\post-compile.json"));            
            compiler.FileNameFormatter = (n) => (n.Replace("2022", "2023")); 
            compiler.Compile();   

```

## Schema Compile + Build + Publish + Browser Test

```
    
    > som compile -m Commit -v -b -t Run_Automated_Tests
    
```

```csharp

           ISchemaProvider schema = new SchemaProvider("aspnet_Membership"); 

            Compiler compiler = new Compiler();
            compiler.Source = "C:\Path\To\Source\Files";
            compiler.Dest = "C:\Path\To\Save\ReCompiled\Files";
            compiler.CompileMode = CompileMode.Commit;
            compiler.FileFilter = "unittest.html";
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\pre-compile.json"));    
            compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\post-compile.json"));            
            compiler.FileNameFormatter = (n) => (n.Replace("unittest", "unittest_compiled"));
            compiler.ContentCompilers.Add(
                new SomSchemaInterpreter(schema)
                {
                    SchemaItemFilter = app => true,
                    SchemaItemProjector = (app) =>
                    {
                        app.StringFormatter = (i, f) => f.Replace("{0}", i.Name).Replace("{1}", i.DataType);
                        app.DataType = Regex.Replace(app.DataType, "(.*unique.*)", "int");
                        return app;
                    }
                });
            compiler.Compile(); 
            compiler.Source = compiler.Dest;
            compiler.OnCompiled += (s, a) =>
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "dotnet build" 
                };                
                Process.Start(psi);
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = System.Environment.GetEnvironmentVariable("bom");
                startinfo.UseShellExecute = true;
                startinfo.Arguments = @"exe -t Run_Automated_Tests";
                Process p = Process.Start(startinfo);
            };
            compiler.ContentCompilers.Clear(); 
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\pre-compile.json"));   
            compiler.ContentCompilers.Add(new Incrementer("(?<!\\d)\\d{5}(?!\\d)", 1250)); 
            compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\post-compile.json"));  
            compiler.ContentCompilers.Add(new Incrementer("(?<!\\d)\\d{3}(?!\\d)", 250));
            compiler.Compile();

```

 

[SOM Angular Dashboard](https://github.com/tmnkopp/SOMDash) for GUI based Sledge-O-Matic-ing 

[Browse-O-matic](https://github.com/tmnkopp/BrowseOmatic) for automated testing your Sledge-O-Matic-ed code 