namespace System
open System.Reflection
open System.Runtime.InteropServices

[<assembly: AssemblyTitleAttribute("Fake.Persimmon")>]
[<assembly: AssemblyProductAttribute("FAKE.Persimmon")>]
[<assembly: GuidAttribute("547AB613-8059-4A80-BF6D-B3A19B220754")>]
[<assembly: AssemblyDescriptionAttribute("FAKE extension for Persimmon")>]
[<assembly: AssemblyVersionAttribute("1.0.2")>]
[<assembly: AssemblyFileVersionAttribute("1.0.2")>]
[<assembly: AssemblyInformationalVersionAttribute("1.0.2")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0.2"
