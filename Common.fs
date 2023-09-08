module Common

type AirportClass = Bravo | Charlie | Delta
type ChartType = APD | MIN | LAH | HOT | STAR | IAP | DP | DAU

type Chart = {
    Airport: string
    Name: string
    Type: ChartType
    PdfPath: string
}

type Airport = {
    Id: string
    Class: AirportClass
}

let parseChartType (s: string) =
    match s.ToUpper() with
    | "APD" -> Some APD
    | "MIN" -> Some MIN
    | "LAH" -> Some LAH
    | "HOT" -> Some HOT
    | "STAR" -> Some STAR
    | "IAP" -> Some IAP
    | "DP" -> Some DP
    | "DAU" -> Some DAU
    | _ -> None

let parseAirportClass (s: string) =
    match s.ToUpper() with
    | "BRAVO" -> Some Bravo
    | "CHARLIE" -> Some Charlie
    | "DELTA" -> Some Delta
    | _ -> None