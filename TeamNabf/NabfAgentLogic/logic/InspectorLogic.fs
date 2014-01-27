﻿namespace NabfAgentLogic

module InspectorLogic =

    open AgentTypes
    open AgentLogicLib

    //Run away from enemy saboteurs, preferring to go to an unexplored vertex
    let inspectorReact (s:State) (agents:Agent list * Agent list) =
        let enemySabs = List.filter (fun a -> (a.Role = Some Saboteur)) (fst agents)
        runAway s enemySabs

    //Inspect unknown enemies on your vertex or move to an adjacent vertex with an unknown enemy
    let inspect (s:State) =
        let agents = List.partition (fun a -> a.Node = s.Self.Node) s.NearbyAgents
        let unknownEnemies = List.filter (fun a -> (a.Role = None) && (a.Team <> s.Self.Team)) (fst agents)
        if not unknownEnemies.IsEmpty 
        then 
            tryDo (Inspect None) s 
        else
            let adjacentEnemies = List.filter (fun a -> (a.Role = None) && (a.Team <> s.Self.Team)) (snd agents)
            if not adjacentEnemies.IsEmpty
            then
                tryGo (s.World.[adjacentEnemies.Head.Node]) s
            else
                (false,None)