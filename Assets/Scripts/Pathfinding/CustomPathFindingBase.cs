using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MonoBehaviours.Pathfinding
{
    public abstract class CustomPathFindingBase:PathfindingBase
    {
        protected List<Nodes> Map;
        protected Vector2Int Mapsize;
        
        public bool IsDynamicPathFinding;
        [Tooltip("The UpdateRate is in Millisecond")]
        [SerializeField] protected int UpdateRate; 
        [SerializeField]protected Transform Transform;
        protected Vector3 target;

        protected List<Vector3> Path = new List<Vector3>();

        protected List<Vector3> obstaclePositions;
        protected List<Vector3> wallPositions;
        
        public virtual void Initialize(Transform transform,List<Nodes> map, Vector2Int mapSize)
        {
            Transform = transform;
            Map = map;
            Mapsize = mapSize;
            
            obstaclePositions = new List<Vector3>();
            wallPositions = new List<Vector3>();

            // Get all the obstacles in the scene.
            Obstacle.Obstacle[] obstacles = MonoBehaviour.FindObjectsOfType<Obstacle.Obstacle>();

            // Store the positions of the active obstacles and walls.
            foreach (Obstacle.Obstacle obstacle in obstacles)
            {
                if (obstacle.isActive)
                {
                    if (obstacle.type == ObstacleType.Wall)
                    {
                        wallPositions.Add(obstacle.transform.position);
                    }
                    else
                    {
                        obstaclePositions.Add(obstacle.transform.position);
                    }
                }
            }
        }

        public virtual async void Update()
        {
            if(target == default)
                return;
            
            Path = FindPath(Transform.position, target);
            
            await Task.Delay(UpdateRate);
        }

        public void SetCurrentTarget(Vector3 _currentTransform)
        {
            target = _currentTransform;
        }

        public abstract override List<Vector3> FindPath(Vector3 start, Vector3 dest);
    }
}