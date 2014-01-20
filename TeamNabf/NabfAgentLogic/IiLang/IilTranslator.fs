namespace NabfAgentLogic.IiLang
    module IilTranslator = 
        open Graphing.Graph
        open IiLangDefinitions
        open NabfAgentLogic.AgentTypes

        exception InvalidIilException of string * Element

        let parseIilEdge iilData =
            match iilData with
            | ParameterList [Numeral weight; Identifier vert1; Identifier vert2] ->
                (Some (int weight), vert1, vert2)
            | ParameterList [Identifier vert1; Identifier vert2] ->
                (None, vert1, vert2)
            | _ -> raise <| InvalidIilException ("Edge", iilData)

        let parseIilVertex iilData = 
            match iilData with
            | Function (name, [Numeral value])
                -> { Identifier = name; Value = Some (int value); Edges = Set.empty }
            | Function (name, [])
                -> { Identifier = name; Value = None; Edges = Set.empty } 
            | _ -> raise <| InvalidIilException ("Vertex", iilData)

        let parseIilRole iilData = 
            match iilData with
            | Identifier "Saboteur"  -> Some Saboteur
            | Identifier "Explorer"  -> Some Explorer
            | Identifier "Repairer"  -> Some Repairer
            | Identifier "Inspector" -> Some Inspector
            | Identifier "Sentinel"  -> Some Sentinel
            | Identifier ""          -> None
            | _ -> raise <| InvalidIilException ("Role", iilData)
        
        let parseIilAgent iilData =
            match iilData with
            | Function (name, 
                        [ Numeral energy
                        ; Numeral health
                        ; Numeral maxEnergy
                        ; Numeral maxHealth
                        ; Identifier node
                        ; role
                        ; Numeral strength
                        ; Identifier team
                        ; Numeral visionRange
                        ])
                -> { Energy      = int <| energy
                   ; Health      = int <| health
                   ; MaxEnergy   = int <| maxEnergy
                   ; MaxHealth   = int <| maxHealth     
                   ; Name        = name
                   ; Node        = node
                   ; Role        = parseIilRole role
                   ; Strength    = int <| strength
                   ; Team        = team
                   ; VisionRange = int <| visionRange
                   }
            | _ -> raise <| InvalidIilException ("Agent", iilData)

//        let parseIilActionRequest iilData =
//            match iilData with
//            | [ Function ("id", [Numeral id])
//              ; Function ("deadline", [Numeral deadline])
//              ; Function ("timestamp", [Numeral timestamp])
//              ] -> 

//        let parseIilPercept iilPercept =
//            match iilPercept with
//            | Percept (name, data) -> 
//                match name with
//                | "actionRequest"     -> 
//                | "inspectedEntities" -> 
//                | "probedVertices"    ->    
//                | "self"              -> 
//                | "step"              ->    
//                | "surveyedEdges"     ->        
//                | "team"              ->    
//                | "visibleEdges"      ->    
//                | "visibleEntities"   ->    
//                | "visibleVertices"   ->    
//            | _ -> failwith "no"            

