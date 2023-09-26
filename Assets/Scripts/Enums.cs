
public enum ObstacleType { Wall, Dynamic, Static }


public enum NPCStateNameEum
{
    Idle,Patrolling, Chasing, Investigating,Lost
}

public enum PathfindingAlgorithmEnum
{
    DFS,BFS,AStar,NavMesh
}

public enum SensorName
{
    FovSensor,SoundSensor
}

public enum SensorEventName
{
    OnNearDetect,OnFarDetect,OnLostPlayer,OnSoundDetect
}

public enum AnimationEventName
{
    OnFootStep
}