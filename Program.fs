open FSharp.Configuration
open System.Text.Json
open System.Net.Http
open System.Collections.Generic
open Common
open Giraffe.ViewEngine
open Views
open System.IO

type Config = YamlConfig<"site_config.yml">

type ChartDto =
    { state: string
      state_full: string
      city: string
      volume: string
      airport_name: string
      military: string
      faa_ident: string
      icao_ident: string
      chart_seq: string
      chart_code: string
      chart_name: string
      pdf_name: string
      pdf_path: string }

type ChartsResponse = Dictionary<string, list<ChartDto>>

let getAllAirports (config: Config) =
    [ Seq.map (fun a -> { Id = a; Class = Bravo }) config.Airports.Bravo
      Seq.map (fun a -> { Id = a; Class = Charlie }) config.Airports.Charlie
      Seq.map (fun a -> { Id = a; Class = Delta }) config.Airports.Delta ]
    |> Seq.collect id

let makeUrl airports =
    let airportsQueryString = String.concat "," airports
    $"https://charts-api.oakartcc.org/v1/charts?apt={airportsQueryString}"

let fetchCharts (url: string) =
    task {
        let httpClient = new HttpClient()
        let! response = httpClient.GetAsync(url)
        let! stream = response.Content.ReadAsStreamAsync()
        return JsonSerializer.Deserialize<ChartsResponse>(stream)
    }

let parseChart (chart: ChartDto) =
    match parseChartType chart.chart_code with
    | Some t ->
        Some
            { Airport = chart.airport_name
              Name = chart.chart_name
              Type = t
              PdfPath = chart.pdf_path }
    | None -> None

let mapCharts (airports: Airport seq) (charts: ChartsResponse) =
    let sanitizeDtos = List.map parseChart >> List.choose id >> List.sortBy (chartToInt)

    let getChartsForId key =
        match charts.TryGetValue($"K{key}") with
        | true, dtos -> sanitizeDtos dtos
        | false, _ ->
            match charts.TryGetValue($"{key}") with
            | true, dtos -> sanitizeDtos dtos
            | false, _ -> []

    let mapped =
        (Map.empty, airports)
        ||> Seq.fold (fun map airport -> Map.add airport.Id (getChartsForId airport.Id) map)

    airports
    |> Seq.map (fun a -> (a, mapped.Item a.Id))
    |> Seq.filter (fun (_, c) -> not c.IsEmpty)


[<EntryPoint>]
let main args =
    let outputDir =
        match Array.toList args with
        | [] -> "./"
        | arg :: _ -> arg.TrimEnd([| '/'; '\\' |])

    let config = Config()
    let airports = getAllAirports config
    let chartsResponse = airports |> Seq.map (fun a -> a.Id) |> makeUrl |> fetchCharts

    let viewModel = mapCharts airports chartsResponse.Result

    match Directory.Exists(outputDir) with
    | true -> ()
    | false -> do Directory.CreateDirectory(outputDir) |> ignore

    renderPage config.Title viewModel
    |> RenderView.AsString.htmlDocument
    |> (fun s -> File.WriteAllText($"{outputDir}/index.html", s))

    0
