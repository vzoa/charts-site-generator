module Views

open Giraffe.ViewEngine
open Common

let classes = String.concat " "

let tabButton airport airportClass =
    let baseCssCls = "tablink p-1 mx-1 hover:bg-gray-800"

    let airportCssCls =
        match airportClass with
        | Bravo -> "text-orange-600"
        | Charlie -> "text-sky-600"
        | Delta -> "text-gray-300"

    let cls = classes [ baseCssCls; airportCssCls ]
    let onclick = $"openpage('{airport}', this)"

    button [ _class cls; _onclick onclick ] [ str airport ]

let navBar (airports: Airport seq) =
    let buttons = airports |> Seq.map (fun a -> tabButton a.Id a.Class) |> Seq.toList

    nav [ _class "bg-gray-950 flex items-center p-1" ] buttons

let chartButton (chart: Chart) =
    let baseCssCls =
        "chartbutton hover:bg-gray-700 border p-1 m-1 cursor-pointer text-xs w-32 h-12"

    let chartCssCls =
        match chart.Type with
        | APD -> "border-green-500"
        | MIN -> "border-orange-500"
        | LAH -> "border-yellow-500"
        | HOT -> "border-red-500"
        | STAR -> "border-sky-500"
        | IAP -> "border-violet-500"
        | DP -> "border-pink-500"
        | DAU -> "border-slate-500"

    let cls = classes [ baseCssCls; chartCssCls ]
    let chartId = $"{chart.Airport} {chart.Name}"
    let onclick = $"openpdf('{chart.PdfPath}', this)"

    div [ _id chartId; _class cls; _data "url" chart.PdfPath; _onclick onclick ] [ str chart.Name ]

let tabContent airportId (charts: Chart seq) =
    let cls = "tabcontent flex flex-row flex-wrap justify-start items-start hidden"

    div [ _id airportId; _class cls ] (Seq.map chartButton charts |> Seq.toList)

let pdfViewer =
    let baseCssCls = "hidden w-full h-full"
    let __data = attr "data"

    object
        [ _id "pdfViewer"
          _class baseCssCls
          _type "application/pdf"
          _width "100%"
          _height "100%"
          __data "" ]
        []

let renderPage (viewModel: (Airport * Chart list) seq) =

    let navContentSection = viewModel |> Seq.map fst |> navBar

    let tabContentSection =
        viewModel
        |> Seq.map (fun (airport, charts) -> tabContent airport.Id charts)
        |> Seq.toList

    html
        [ _class "font-mono bg-gray-800 h-full" ]
        [ head
              []
              [ title [] [ str "Charts" ]
                link [ _rel "stylesheet"; _href "style.css" ]
                script [ _src "script.js" ] [] ]
          body
              [ _class "text-gray-300 h-full" ]
              [ div [] [ navContentSection ]
                div
                    [ _class "flex flex-row h-full" ]
                    [ div [ _class "w-1/3" ] tabContentSection
                      div [ _class "w-2/3" ] [ pdfViewer ] ] ]
          footer [] [] ]
