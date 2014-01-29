﻿namespace NabfAgentLogic
module CommonLogic =

    open AgentTypes
    open Graphing.Graph
    open PathFinding
    open SaboteurLogic
    open RepairerLogic
    open SentinelLogic
    open ExplorerLogic
    open InspectorLogic
    open AgentLogicLib
    open Logging

    let reactToEnemyAgent (s:State) =
        let agents = List.partition (fun a -> (a.Node = s.Self.Node) && (a.Team <> s.Self.Team)) s.NearbyAgents
        if not (fst agents).IsEmpty then
            match s.Self.Role.Value with
            | Saboteur -> saboteurReact s agents
            | Repairer -> repairerReact s agents
            | Sentinel -> sentinelReact s agents
            | Explorer -> explorerReact s agents
            | Inspector -> inspectorReact s agents
        else
            (false,None)

    let exploreLocalGraph (s:State) =
        let rank = rankByType s
        let unexplored = (pathsToNearestNUnexplored rank s.Self s.World)
        if unexplored = []
        then 
            (false,None) 
        else
            let index = rank % unexplored.Length      
            tryGo s.World.[unexplored.[index].Head] s
            
    let idle (s:State) = recharge

