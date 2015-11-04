[<AutoOpen>]
module Fake.PersimmonHelper

open System
open System.IO
open System.Text

type OutputDestination =
  | Console
  | PlaneTextFile of path: string
  | XmlFile of path: string

type ErrorOutputDestination =
  | Console
  | File of path: string

type PersimmonParams = {
  ToolPath: string
  NoProgress: bool
  Parallel: bool
  Output: OutputDestination
  Error: ErrorOutputDestination
  TimeOut : TimeSpan
  ErrorLevel: TestRunnerErrorLevel
}

let PersimmonDefaults = {
  ToolPath = findToolInSubPath "Persimmon.Console.exe" (currentDirectory @@ "tools" @@ "Persimmon.Console")
  NoProgress = false
  Parallel = false
  Output = OutputDestination.Console
  Error = Console
  TimeOut = TimeSpan.FromMinutes 5.
  ErrorLevel = Error
}

let buildPersimmonArgs parameters assemblies =
  let output, outputText =
    match parameters.Output with
    | OutputDestination.Console -> false, ""
    | PlaneTextFile path -> (true, sprintf "--output:%s" <| path.TrimEnd Path.DirectorySeparatorChar)
    | XmlFile path -> (true, sprintf "\"--output:%s\" --format:xml" <| path.TrimEnd Path.DirectorySeparatorChar)
  let error, errorText =
    match parameters.Error with
    | Console -> false, ""
    | File path -> (true, sprintf "--error:%s" <| path.TrimEnd Path.DirectorySeparatorChar)
  StringBuilder()
  |> appendIfTrueWithoutQuotes output outputText
  |> appendIfTrue error errorText
  |> appendIfTrue parameters.NoProgress "--no-progress"
  |> appendIfTrue parameters.Parallel "--parallel"
  |> appendFileNamesIfNotNull assemblies
  |> toText

let Persimmon setParams assemblies =
  let details = separated ", " assemblies
  traceStartTask "Persimmon" details
  let parameters = setParams PersimmonDefaults
  let args = buildPersimmonArgs parameters assemblies
  trace (parameters.ToolPath + " " + args)
  if 0 <> ExecProcess (fun info -> 
    info.FileName <- parameters.ToolPath
    info.Arguments <- args) parameters.TimeOut
  then 
    sprintf "Persimmon test failed on %s." details
    |> match parameters.ErrorLevel with
       | Error | FailOnFirstError -> failwith
       | DontFailBuild -> traceImportant
  traceEndTask "Persimmon" details
